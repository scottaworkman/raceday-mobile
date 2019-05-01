using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaceDay.Services.Models
{
    public class JsonEvent
    {
        public int EventId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Url { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string CreatorId { get; set; }
    }
}