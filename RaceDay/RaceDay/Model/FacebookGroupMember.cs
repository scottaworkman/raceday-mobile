using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceDay.Model
{
    public class FacebookGroupMemberList
    {
        public List<FacebookGroupMember> data { get; set; }
    }

    public class FacebookGroupMember
    {
        public string id { get; set; }
        public string name { get; set; }
        public Boolean administrator { get; set; }
    }
}
