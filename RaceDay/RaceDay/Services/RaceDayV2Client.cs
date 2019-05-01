using RaceDay.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

using RaceDay.Services.Models;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace RaceDay.Services
{
    public class RaceDayV2Client
    {
        private const string COMMAND_LOGIN = "login";
        private const string COMMAND_MFUSER = "mfuser";
        private const string COMMAND_EVENT = "event";
        private const string COMMAND_ATTEND = "attend";

        #region Login

        public static async Task<AuthResult> Login(string userEmail, string userPassword)
        {
            // Create the input model
            //
            var loginAuth = new LoginAuth
            {
                groupid = Settings.GroupCode,
                email = userEmail,
                password = userPassword,
                apikey = Settings.GroupApi
            };

            // Configure the REST client
            //
            var client = new RestClient(Settings.APIUrl);
            var loginResult = await client.PostApi<AuthResult>(COMMAND_LOGIN, loginAuth, HttpStatusCode.OK);

            return loginResult;
        }

        public static async Task<HttpStatusCode> ForgotPassword(string email)
        {
            // Prepare the API input as URL parameters
            //
            var requestParms = $"groupid={HttpUtility.UrlEncode(Settings.GroupCode)}&apikey={HttpUtility.UrlEncode(Settings.GroupApi)}&email={HttpUtility.UrlEncode(email)}";

            // Configure the REST client
            //
            var client = new RestClient(Settings.APIUrl);
            await client.GetApi<APIResult>(COMMAND_LOGIN + "?" + requestParms);

            return client.StatusCode;
        }

        public static async Task<HttpStatusCode> UpdatePassword(string Email, string Password)
        {
            var parms = new LoginAuth
            {
                groupid = Settings.GroupCode,
                email = Email,
                password = Password,
                apikey = Settings.GroupApi,
            };

            var client = new RestClient(Settings.APIUrl);
            await client.SimpleApi(COMMAND_LOGIN, "PUT", parms);

            return client.StatusCode;
        }
        #endregion

        #region Events

        public static async Task<List<EventAttending>> GetAllEventsForCurrentUser()
        {
            var token = await AppToken();
            if (token == null)
                return null;

            // Setup GET Rest Client
            //
            var client = new RestClient(Settings.APIUrl);
            client.AddHeader("Authorization", $"Bearer {token}");

            // Get the event list
            //
            var events = await client.GetApi<List<EventAttending>>(COMMAND_EVENT);
            if (client.StatusCode == HttpStatusCode.Unauthorized)
            {
                token = await Authorize();
                if (token != null)
                {
                    client.ClearHeaders();
                    client.AddHeader("Authorization", $"Bearer {token}");
                    events = await client.GetApi<List<EventAttending>>(COMMAND_EVENT);
                }
            }

            return events;
        }

        public static async Task<EventDetail> GetEventDetail(int eventId)
        {
            var token = await AppToken();
            if (token == null)
                return null;

            // Setup GET Rest Client
            //
            var client = new RestClient(Settings.APIUrl);
            client.AddHeader("Authorization", $"Bearer {token}");

            // Get the event details
            //
            var eventDetail = await client.GetApi<EventDetail>($"{COMMAND_EVENT}/{eventId}");
            if (client.StatusCode == HttpStatusCode.Unauthorized)
            {
                token = await Authorize();
                if (token != null)
                {
                    client.ClearHeaders();
                    client.AddHeader("Authorization", $"Bearer {token}");
                    eventDetail = await client.GetApi<EventDetail>($"{COMMAND_EVENT}/{eventId}");
                }
            }

            return eventDetail;
        }

        public static async Task<Boolean> AddUserToEvent(int eventId)
        {
            var token = await AppToken();
            if (token == null)
                return false;

            var client = new RestClient(Settings.APIUrl);
            client.AddHeader("Authorization", $"Bearer {token}");
            await client.PutApi($"{COMMAND_ATTEND}/{eventId}");
            if (client.StatusCode == HttpStatusCode.Unauthorized)
            {
                token = await Authorize();
                if (token != null)
                {
                    client.ClearHeaders();
                    client.AddHeader("Authorization", $"Bearer {token}");
                    await client.PutApi($"{COMMAND_ATTEND}/{eventId}");
                }
            }

            return (client.StatusCode == HttpStatusCode.OK);
        }

        public static async Task<Boolean> RemoveUserFromEvent(int eventId)
        {
            var token = await AppToken();
            if (token == null)
                return false;

            var client = new RestClient(Settings.APIUrl);
            client.AddHeader("Authorization", $"Bearer {token}");
            await client.DeleteApi($"{COMMAND_ATTEND}/{eventId}");
            if (client.StatusCode == HttpStatusCode.Unauthorized)
            {
                token = await Authorize();
                if (token != null)
                {
                    client.ClearHeaders();
                    client.AddHeader("Authorization", $"Bearer {token}");
                    await client.DeleteApi($"{COMMAND_ATTEND}/{eventId}");
                }
            }

            return (client.StatusCode == HttpStatusCode.OK);
        }

        public static async Task<EventDetail> AddEvent(string name, DateTime date, string url, string location, string description, string creatorid)
        {
            var token = await AppToken();
            if (token == null)
                return null;

            var client = new RestClient(Settings.APIUrl);
            client.AddHeader("Authorization", $"Bearer {token}");

            var eventInfo = new JsonEvent
            {
                Name = name,
                Date = date,
                Url = url,
                Location = location,
                Description = description,
                CreatorId = creatorid
            };

            var result = await client.PostApi<EventDetail>(COMMAND_EVENT, eventInfo, HttpStatusCode.Created);
            if (client.StatusCode == HttpStatusCode.Unauthorized)
            {
                token = await Authorize();
                if (token != null)
                {
                    client.ClearHeaders();
                    client.AddHeader("Authorization", $"Bearer {token}");
                    result = await client.PostApi<EventDetail>(COMMAND_EVENT, eventInfo, HttpStatusCode.Created);
                }
            }
            return result;
        }

        public static async Task<Boolean> DeleteEvent(int eventId)
        {
            var token = await AppToken();
            if (token == null)
                return false;

            var client = new RestClient(Settings.APIUrl);
            client.AddHeader("Authorization", $"Bearer {token}");

            await client.SimpleApi($"{COMMAND_EVENT}/{eventId}", "DELETE", null);
            return (client.StatusCode == HttpStatusCode.OK);
        }

        public static async Task<Boolean> UpdateEvent(int eventID, string name, DateTime date, string url, string location, string description, string creatorid)
        {
            var token = await AppToken();
            if (token == null)
                return false;

            var client = new RestClient(Settings.APIUrl);
            client.AddHeader("Authorization", $"Bearer {token}");

            var jsonEvent = new JsonEvent
            {
                EventId = eventID,
                Name = name,
                Date = date,
                Url = url,
                Location = location,
                Description = description,
                CreatorId = creatorid
            };

            await client.SimpleApi($"{COMMAND_EVENT}/{eventID}", "PUT", jsonEvent);
            return (client.StatusCode == HttpStatusCode.OK);
        }
        #endregion

        #region mfuser

        public static async Task<HttpStatusCode> UserRegister(string groupcode, string firstName, string lastName, string email, string password)
        {
            return await UserRegister(groupcode, $"{firstName} {lastName}", firstName, lastName, email, password);
        }
        public static async Task<HttpStatusCode> UserRegister(string groupcode, string name, string firstName, string lastName, string email, string password)
        {
            // Create input
            //
            var newUser = new JsonUser
            {
                FirstName = firstName,
                LastName = lastName,
                Name = name,
                Email = email,
                Password = password,
                UserId = string.Empty
            };

            // Post to REST client and get response
            //
            var client = new RestClient(Settings.APIUrl);
            var userResult = await client.PostApi<string>($"{COMMAND_MFUSER}?code={groupcode}", newUser, HttpStatusCode.Created);

            return client.StatusCode;
        }

        public static async Task<HttpStatusCode> EditUser(string id, JsonUser user)
        {
            var token = await AppToken();
            if (token == null)
                return HttpStatusCode.Forbidden;

            
            id = Uri.EscapeDataString(id);

            var client = new RestClient(Settings.APIUrl);
            client.AddHeader("Authorization", $"Bearer {token}");

            await client.SimpleApi($"{COMMAND_MFUSER}/{id}/", "PUT", user);
            if (client.StatusCode == HttpStatusCode.Unauthorized)
            {
                token = await Authorize();
                if (token != null)
                {
                    client.ClearHeaders();
                    client.AddHeader("Authorization", $"Bearer {token}");
                    await client.SimpleApi($"{COMMAND_MFUSER}/{id}/", "PUT", user);
                }
            }
            return client.StatusCode;
        }

        #endregion

        #region Helpers

        public static async Task<string> AppToken()
        {
            return (Settings.Token != null ? Settings.Token.Token : await Authorize());
        }

        public static async Task<string> Authorize()
        {
            var auth = new LoginAuth
            {
                groupid = Settings.GroupCode,
                email = Settings.UserEmail,
                password = Settings.UserPassword,
                apikey = Settings.GroupApi
            };

            var client = new RestClient(Settings.APIUrl);
            var loginResult = await client.PostApi<AuthResult>(COMMAND_LOGIN, auth, HttpStatusCode.OK);

            if (loginResult != null)
            {
                Settings.Token = new Model.AccessToken
                {
                    Token = loginResult.token,
                    Expiration = loginResult.expiration,
                    Role = loginResult.role,
                };
            }

            return (Settings.Token != null ? Settings.Token?.Token : null);
        }

        #endregion
    }
}
