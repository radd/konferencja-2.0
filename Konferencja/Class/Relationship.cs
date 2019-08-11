using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konferencja.Class
{
    public class Relationship
    {
        public int ID { get; }
        public int confID { get; set; }
        public int speakerID { get; set; }
        public Relationship(int ID, int confID, int speakerID)
        {
            this.ID = ID;
            this.confID = confID;
            this.speakerID = speakerID;
        }
    }
}
