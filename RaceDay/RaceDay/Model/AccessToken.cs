using System;

namespace RaceDay.Model
{
    public class AccessToken
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public int Role { get; set; }
        public string Name { get; set; }
    }
}
