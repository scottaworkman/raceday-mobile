using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceDay.Model
{
    public class Event
    {
        public int EventId { get; set; }
        public int GroupId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Url { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string CreatorId { get; set; }
        public string UserId { get; set; }
        public bool Attending { get; set; }
    }
}
