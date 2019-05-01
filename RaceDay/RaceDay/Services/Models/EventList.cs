using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaceDay.Services.Models
{
    // Model returned through the REST client for listing events alonng with Attending indicator for current user
    //
    public partial class EventAttending
    {
        public int EventId { get; set; }
        public int GroupId { get; set; }
        public string Name { get; set; }
        public System.DateTime Date { get; set; }
        public string Url { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string CreatorId { get; set; }
        public string UserId { get; set; }
        public int Attending { get; set; }
        public Nullable<int> AttendanceCount { get; set; }
    }
}