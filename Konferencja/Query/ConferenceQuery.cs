using Konferencja.Class;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konferencja.Query
{
    public class ConferenceQuery
    {
        private readonly string PATH = Database.DBPATH + Database.CONFERENCES;

        public ConferenceQuery() { }

        public Conference getConference(int ID)
        {
            return getConferencesBy(new int[] { ID })[0];
        }
        public List<Conference> getConferences()
        {
            List<Conference> confs = new List<Conference>();
            try
            {
                using (StreamReader sr = new StreamReader(PATH, Encoding.GetEncoding("windows-1250")))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null && line != "")
                    {
                        line = encodeString(line);
                        string[] cols = line.Split(';');
                        confs.Add(new Conference(int.Parse(cols[0]), cols[1], formatDate(cols[2]), formatTime(cols[3]), formatTime(cols[4]), cols[5], cols[6]));
                    }
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); }

            confs = confs.OrderByDescending(Conference => Conference.Date).ToList();
            return confs;
        }

        public List<Conference> getConferencesBy(int[] IDs)
        {
            List<Conference> allconfs = getConferences();
            List<Conference> confs = new List<Conference>();
            try
            {
                confs = allconfs.Where(Conference => (IDs == null || IDs.Contains(Conference.ID))).ToList();
            }
            catch (Exception e) { Console.WriteLine(e.Message); }

            return confs;
        }

        public bool setConference(Conference setConf)
        {
            bool ret = false;
            List<Conference> confs = getConferences();
            Conference conf = confs.Where(Conference => Conference.ID == setConf.ID).First();
            if (conf == null)
                throw new Exception("Nie znaleziono ID konferencji");

            conf.Name = decodeString(setConf.Name);
            conf.Date = setConf.Date;
            conf.From = setConf.From;
            conf.To = setConf.To;
            conf.Venue = decodeString(setConf.Venue);
            conf.Logo = decodeString(setConf.Logo);

            if (!(CheckCorrectTime(conf.From, conf.To)))
                throw new Exception("Godziny są nieprawidłowe");

            try
            {
                using (StreamWriter sr = new StreamWriter(new FileStream(PATH, FileMode.Create, FileAccess.Write), Encoding.GetEncoding("windows-1250")))
                {
                    foreach (Conference c in confs)
                    {
                        sr.WriteLine("{0};{1};{2};{3};{4};{5};{6}", c.ID, c.Name, c.Date.ToString("yyyy-MM-dd"), c.From.ToString("HH:mm"), c.To.ToString("HH:mm"), c.Venue, c.Logo);
                    }
                    ret = true;
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); }

            return ret;
        }

        public int addConference(string name, DateTime date, DateTime from, DateTime to, string venue, string logo)
        {
            bool ret = false;

            if (!CheckCorrectTime(from, to))
                throw new Exception("Godziny są nieprawidłowe");

            List<Conference> confs = getConferences();
            Conference last;
            int newID = 1;
            if (confs.Count != 0)
            {
                last = confs.Last();
                newID = last.ID + 1;
            }

            decodeStrings(ref name, ref venue, ref logo);

            try
            {
                using (StreamWriter sr = new StreamWriter(new FileStream(PATH, FileMode.Append, FileAccess.Write), Encoding.GetEncoding("windows-1250")))
                {
                    sr.WriteLine("{0};{1};{2};{3};{4};{5};{6}", newID, name, date.ToString("yyyy-MM-dd"), from.ToString("HH:mm"), to.ToString("HH:mm"), venue, logo);
                    ret = true;
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); }

            return (ret) ? newID : 0;
        }

        public void Delete(int confID)
        {
            List<Conference> confs = getConferences();
            Conference conf = confs.Where(Conference => Conference.ID == confID).First();

            if (conf != null)
            {
                RelationshipQuery relaQuery = new RelationshipQuery();
                relaQuery.DeleteConference(conf.ID);

                confs.Remove(conf);
                try
                {
                    using (StreamWriter sr = new StreamWriter(new FileStream(PATH, FileMode.Create, FileAccess.Write), Encoding.GetEncoding("windows-1250")))
                    {
                        foreach (Conference c in confs)
                        {
                            sr.WriteLine("{0};{1};{2};{3};{4};{5};{6}", c.ID, c.Name, c.Date.ToString("yyyy-MM-dd"), c.From.ToString("HH:mm"), c.To.ToString("HH:mm"), c.Venue, c.Logo);
                        }
                    }
                }
                catch { }

            }


        }

        private DateTime formatDate(string date)
        {
            if (date == null)
                throw new Exception("Nieprawidłowy format daty");
            DateTime dt;
            if (DateTime.TryParse(date, out dt))
                return dt;
            else
                throw new Exception("Nieprawidłowy format daty");
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

        private void decodeStrings(ref string name, ref string venue, ref string logo)
        {
            name = decodeString(name);
            venue = decodeString(venue);
            logo = decodeString(logo);
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

    }
}
