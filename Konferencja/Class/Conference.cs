using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konferencja.Class
{
    public class Conference
    {
        public int ID { get; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Venue { get; set; }
        public string Logo { get; set; }
        private List<Speaker> speakers;


        public Conference() { }
        public Conference(int ID, string name, DateTime date, DateTime from, DateTime to, string venue, string logo)
        {
            this.ID = ID;
            this.Name = name;
            this.Date = date;
            this.From = from;
            this.To = to;
            this.Venue = venue;
            this.Logo = logo;
        }

        public void setSpeakers(List<Speaker> speakers)
        {
            this.speakers = speakers;
        }

        public List<Speaker> getSpeakers()
        {
            return (speakers != null) ? speakers : new List<Speaker>();
        }
        public int getSpeakersCount()
        {
            return speakers.Count;
        }

        public string GetDate
        {
            get
            {
                return Date.ToString("dd-MM-yyyy");
            }
            set { }
        }


    }
}
