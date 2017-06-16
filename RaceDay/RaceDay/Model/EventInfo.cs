using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceDay.Model
{
    public class EventInfo
    {
        public Event eventinfo { get; set; }
        public IEnumerable<Participant> attendees { get; set; }
    }
}
