using Konferencja.Class;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konferencja.Query
{
    public class RelationshipQuery
    {
        private readonly string PATH = Database.DBPATH + Database.RELATIONSHIPS;

        public RelationshipQuery() { }

        public List<Speaker> getSpeakers(int ID)
        {
            SpeakerQuery speakerQuery = new SpeakerQuery();
            List<int> IDs = getSpeakersID(ID);
            List<Speaker> speakers = speakerQuery.getSpeakersBy(IDs.ToArray());

            return speakers;
        }

        private List<int> getSpeakersID(int ID)
        {
            List<int> IDs = new List<int>();
            try
            {
                using (StreamReader sr = new StreamReader(PATH, Encoding.GetEncoding("windows-1250")))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] cols = line.Split(';');
                        if (int.Parse(cols[1]) == ID)
                            IDs.Add(int.Parse(cols[2]));
                    }
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); }

            return IDs;
        }

        private List<Relationship> getRelationships()
        {
            List<Relationship> rela = new List<Relationship>();
            try
            {
                using (StreamReader sr = new StreamReader(PATH))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] cols = line.Split(';');
                        rela.Add(new Relationship(int.Parse(cols[0]), int.Parse(cols[1]), int.Parse(cols[2])));
                    }
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); }

            return rela;
        }

        public int addRelationship(int confID, int spekearID)
        {
            bool ret = false;
            List<Relationship> rela = getRelationships();
            int newID = 1;
            if (rela.Count != 0)
            {
                Relationship last = rela.Last();
                newID = last.ID + 1;
            }
            try
            {
                using (StreamWriter sr = new StreamWriter(new FileStream(PATH, FileMode.Append, FileAccess.Write), Encoding.GetEncoding("windows-1250")))
                {
                    sr.WriteLine("{0};{1};{2}", newID, confID, spekearID);
                    ret = true;
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); }

            return (ret) ? newID : 0;
        }

        public void DeleteSpeaker(int speakerID)
        {
            List<Relationship> rela = getRelationships();
            List<Relationship> relaToDelete = rela.Where(Relationship => Relationship.speakerID == speakerID).ToList();

            foreach (Relationship r in relaToDelete)
                rela.Remove(r);

            try
            {
                using (StreamWriter sr = new StreamWriter(new FileStream(PATH, FileMode.Create, FileAccess.Write), Encoding.GetEncoding("windows-1250")))
                {
                    foreach (Relationship r in rela)
                    {
                        sr.WriteLine("{0};{1};{2}", r.ID, r.confID, r.speakerID);
                    }
                }
            }
            catch { }

        }

        public void DeleteConference(int confID)
        {
            List<Relationship> rela = getRelationships();
            List<Relationship> relaToDelete = rela.Where(Relationship => Relationship.confID == confID).ToList();

            SpeakerQuery speakerQuery = new SpeakerQuery();

            foreach (Relationship r in relaToDelete)
            {
                speakerQuery.Delete(r.speakerID);
                rela.Remove(r);
            }


            try
            {
                using (StreamWriter sr = new StreamWriter(new FileStream(PATH, FileMode.Create, FileAccess.Write), Encoding.GetEncoding("windows-1250")))
                {
                    foreach (Relationship r in rela)
                    {
                        sr.WriteLine("{0};{1};{2}", r.ID, r.confID, r.speakerID);
                    }
                }
            }
            catch { }

        }
    }
}
