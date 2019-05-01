using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaceDay.Services.Models
{
    public class LoginAuth
    {
        public string groupid { get; set; }
        public string userid { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string apikey { get; set; }
    }

    public class AuthResult
    {
        public string token { get; set; }
        public DateTime expiration { get; set; }
        public int role { get; set; }
        public string name { get; set; }
        public string userid { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string message { get; set; }
    }
}