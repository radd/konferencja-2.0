using Konferencja.Class;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konferencja.Query
{
    class SpeakerQuery
    {
        private readonly string PATH = Database.DBPATH + Database.SPEAKERS;

        public SpeakerQuery() { }

        public Speaker getSpeaker(int ID)
        {
            return getSpeakersBy(new int[] { ID })[0];
        }

        public List<Speaker> getSpeakers()
        {
            List<Speaker> speakers = new List<Speaker>();

            try
            {
                using (StreamReader sr = new StreamReader(PATH, Encoding.GetEncoding("windows-1250")))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null && line != "")
                    {
                        line = encodeString(line);
                        string[] cols = line.Split(';');
                        speakers.Add(new Speaker(int.Parse(cols[0]), cols[1], cols[2], formatTime(cols[3]), formatTime(cols[4])));
                    }
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); }

            speakers = speakers.OrderBy(Speaker => Speaker.From).ToList();
            return speakers;
        }

        public List<Speaker> getSpeakersBy(int[] IDs)
        {
            List<Speaker> allSpeakers = getSpeakers();
            List<Speaker> speakers = new List<Speaker>();
            try
            {
                speakers = allSpeakers.Where(Speaker => (IDs == null || IDs.Contains(Speaker.ID)) ).ToList();
            }
            catch (Exception e) { Console.WriteLine(e.Message); }

            return speakers;
        }

        public bool setSpeaker(Speaker setSpeaker, int confID)
        {
            bool ret = false;

            List<Speaker> speakers = getSpeakers();
            Speaker speaker = speakers.Where(Speaker => Speaker.ID == setSpeaker.ID).First();

            if (speaker == null)
                throw new Exception("Nie znaleziono ID prelegenta");

            speaker.Name = decodeString(setSpeaker.Name);
            speaker.Title = decodeString(setSpeaker.Title);
            speaker.From = setSpeaker.From;
            speaker.To = setSpeaker.To;

            if (!CheckCorrectTime(speaker.From, speaker.To))
                throw new Exception("Godziny są nieprawidłowe");
            if (!CheckTimeSpeakerSet(speaker.From, speaker.To, speaker.ID, confID))
                throw new Exception("Ta godzina jest już zajęta");

            try
            {
                using (StreamWriter sr = new StreamWriter(new FileStream(PATH, FileMode.Create, FileAccess.Write), Encoding.GetEncoding("windows-1250")))
                {
                    foreach (Speaker s in speakers)
                    {
                        sr.WriteLine("{0};{1};{2};{3};{4}", s.ID, s.Name, s.Title, s.From.ToString("HH:mm"), s.To.ToString("HH:mm"));
                    }
                    ret = true;
                }
            }
            catch { }

            return ret;
        }

        public int addSpeaker(int confID, string name, string title, DateTime from, DateTime to)
        {
            bool ret = false;

            if (!CheckCorrectTime(from, to))
                throw new Exception("Godziny są nieprawidłowe");
            if (!CheckTimeSpeakerAdd(from, to, confID))
                throw new Exception("Ta godzina jest już zajęta");

            List<Speaker> speakers = getSpeakers();
            Speaker last;
            int newID = 1;
            if (speakers.Count != 0)
            {
                last = speakers.Last();
                newID = last.ID + 1;
            }

            decodeStrings(ref name, ref title);

            try
            {
                using (StreamWriter sr = new StreamWriter(new FileStream(PATH, FileMode.Append, FileAccess.Write), Encoding.GetEncoding("windows-1250")))
                {
                    sr.WriteLine("{0};{1};{2};{3};{4}", newID, name, title, from.ToString("HH:mm"), to.ToString("HH:mm"));
                    ret = true;
                }
            }
            catch { }

            return (ret) ? newID : 0;
        }

        public void Delete(int speakerID)
        {
            List<Speaker> speakers = getSpeakers();
            Speaker speaker = speakers.Where(Speaker => Speaker.ID == speakerID).First();
            if (speaker != null)
            {
                RelationshipQuery relaQuery = new RelationshipQuery();
                relaQuery.DeleteSpeaker(speaker.ID);

                speakers.Remove(speaker);
                try
                {
                    using (StreamWriter sr = new StreamWriter(new FileStream(PATH, FileMode.Create, FileAccess.Write), Encoding.GetEncoding("windows-1250")))
                    {
                        foreach (Speaker s in speakers)
                        {
                            sr.WriteLine("{0};{1};{2};{3};{4}", s.ID, s.Name, s.Title, s.From.ToString("HH:mm"), s.To.ToString("HH:mm"));
                        }
                    }
                }
                catch { }

            }


        }

        private DateTime formatTime(string time)
        {
            if (time == null)
                throw new Exception("Nieprawidłowy format godziny");
            DateTime dt;
            if (DateTime.TryParse(time, out dt))
                return dt;
            else
                throw new Exception("Nieprawidłowy format godziny");
        }

        private void decodeStrings(ref string name, ref string title)
        {
            name = decodeString(name);
            title = decodeString(title);
        }

        private string decodeString(string str)
        {
            str = str.Trim();
            str = str.Replace(";", "");
            return str;
        }

        private string encodeString(string str)
        {
            str = str.Trim();
            return str;
        }


        private bool CheckCorrectTime(DateTime f, DateTime t)
        {
            TimeSpan from = new TimeSpan(f.Hour, f.Minute, 0);
            TimeSpan to = new TimeSpan(t.Hour, t.Minute, 0);

            return TimeSpan.Compare(from, to) == -1 ? true : false;
        }
        private bool CheckTimeSpeakerSet(DateTime f, DateTime t, int speakerID, int confID)
        {
            RelationshipQuery relaQuery = new RelationshipQuery();
            List<Speaker> speakers = relaQuery.getSpeakers(confID);
            speakers = speakers.Where(Speaker => (Speaker.ID != speakerID)).ToList();
            return CheckTime(f, t, speakers, confID);
        }

        private bool CheckTimeSpeakerAdd(DateTime f, DateTime t, int confID)
        {
            RelationshipQuery relaQuery = new RelationshipQuery();
            List<Speaker> speakers = relaQuery.getSpeakers(confID);
            return CheckTime(f, t, speakers, confID);
        }

        private bool CheckTime(DateTime f, DateTime t, List<Speaker> speakers, int confID)
        {
            bool check = true;
            TimeSpan from = new TimeSpan(f.Hour, f.Minute, 0);
            TimeSpan to = new TimeSpan(t.Hour, t.Minute, 0);
            TimeSpan fromCheck;
            TimeSpan toCheck;

            foreach (Speaker s in speakers)
            {
                fromCheck = new TimeSpan(s.From.Hour, s.From.Minute, 0);
                toCheck = new TimeSpan(s.To.Hour, s.To.Minute, 0);
                if (!(TimeSpan.Compare(from, toCheck) >= 0 || TimeSpan.Compare(to, fromCheck) <= 0))
                {
                    check = false;
                    break;
                }

            }
            return check;
        }

    }
}
