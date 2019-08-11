using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konferencja
{
    class Database
    {
        public static readonly string CONFERENCES = "conferences.txt";
        public static readonly string SPEAKERS = "speakers.txt";
        public static readonly string RELATIONSHIPS = "relationship.txt";
        public static readonly string DBPATH = "database\\";
        public static readonly string IMGPATH = "images\\";

        public Database()
        {
            checkFolder(DBPATH);
            checkFolder(IMGPATH);
            checkFile(CONFERENCES);
            checkFile(SPEAKERS);
            checkFile(RELATIONSHIPS);
        }

        private void checkFolder(string dbPath)
        {
            bool exists = Directory.Exists(dbPath);
            if (!exists)
                Directory.CreateDirectory(dbPath);
        }
        private void checkFile(string file)
        {

            if (!File.Exists(DBPATH + file))
            {
                TextWriter tw = new StreamWriter(DBPATH + file, true);
                tw.Close();
            }
        }



    }
}
