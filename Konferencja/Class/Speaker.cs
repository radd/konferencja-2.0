using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konferencja.Class
{
    public class Speaker
    {
        public int ID { get; }
        public string Name { get; set; }
        public string Title { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public Speaker() { }
        public Speaker(int ID, string name, string title, DateTime from, DateTime to)
        {
            this.ID = ID;
            this.Name = name;
            this.Title = title;
            this.From = from;
            this.To = to;
        }

        public string GetFromTo
        {
            get
            {
                return String.Format("{0} - {1}", From.ToString("H:mm"), To.ToString("H:mm"));
            }
            set { }
        }
    }
}
