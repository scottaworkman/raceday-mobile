using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaceDay.Services.Models
{
    public class EventDetail
    {
        public EventAttending eventinfo { get; set; }
        public List<JsonUser> attendees { get; set; }
    }

}