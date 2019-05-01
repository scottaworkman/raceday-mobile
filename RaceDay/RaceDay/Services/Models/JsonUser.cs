using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaceDay.Services.Models
{
    public class JsonUser
    {
        public string UserId { get; set; }

        public string Name { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public DateTime LastUpdate { get; set; }
    }
}