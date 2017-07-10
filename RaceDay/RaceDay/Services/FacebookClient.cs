using RaceDay.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RaceDay.Services
{
    /// <summary>
    /// Facebook Graph API using Restclient for communications.
    /// </summary>
    /// 
    public class FacebookClient
    {
        private string GRAPH_ENDPOINT = "https://graph.facebook.com/v2.9/";
        public const string RELATION_URL = "{0}/{1}?access_token={2}";
        private string AccessToken = string.Empty;

        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        /// <summary>
        /// AccessToken should have been retrieved previously using a FB login process
        /// </summary>
        /// <param name="accesstoken"></param>
        /// 
        public FacebookClient(string accesstoken)
        {
            AccessToken = accesstoken;
        }

        /// <summary>
        /// Get basic user information such as Id, Name, Email
        /// </summary>
        /// <returns></returns>
        /// 
        public async Task GetUserProfile()
        {
            try
            {
                RestClient client = new RestClient(GRAPH_ENDPOINT);
                var me = await client.GetApi<FacebookMe>("me?fields=id,name,email&access_token=" + WebUtility.UrlEncode(AccessToken));
                if (me != null)
                {
                    Id = me.id;
                    Name = me.name;
                    Email = me.email;
                }
            }
            catch (WebException)
            {
                Id = string.Empty;
                Name = string.Empty;
                Email = string.Empty;
            }

            return;
        }

        /// <summary>
        /// Check that the user is a member of the identified group
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        /// 
        public async Task<Boolean> UserInGroup(string groupId)
        {
            try
            {
                RestClient client = new RestClient(GRAPH_ENDPOINT);
                var groupMembers = await client.GetApi<FacebookGroupMemberList>(String.Format(RELATION_URL, groupId, "members", WebUtility.UrlEncode(AccessToken)) + "&limit=200");
                foreach (FacebookGroupMember member in groupMembers.data)
                {
                    if (member.id == Id)
                        return true;
                }
            }
            catch (WebException)
            {
                return false;
            }

            return false;
        }
    }
}
