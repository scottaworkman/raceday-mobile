using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RaceDay.Helpers;
using RaceDay.Model;
using Xamarin.Forms;

namespace RaceDay.Services
{
    public class RaceDayClient
    {
        private const string API_ENDPOINT = "https://app.workmanfamily.com/raceday/api/";
        private const string COMMAND_LOGIN = "login";
        private const string COMMAND_EVENT = "event";
        private const string COMMAND_ATTEND = "attend";
        private const string COMMAND_USER = "mfuser";

        /// <summary>
        /// Makes sure the current user is in the server API
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        /// 
        public static async Task AddUser(string id, string name, string firstname, string lastname, string email)
        {
            var token = await AppToken();
            if (token == null)
                return;

            JsonUser user = new JsonUser
            {
                UserId = id,
                Name = name,
                FirstName = firstname,
                LastName = lastname,
                Email = email,
                LastUpdate = DateTime.Now
            };
            if (string.IsNullOrEmpty(firstname) && string.IsNullOrEmpty(lastname))
            {
                string[] nameparts = name.Split(' ');
                if (nameparts.Length >= 2)
                {
                    user.FirstName = nameparts[0];
                    user.LastName = nameparts[1];
                    for (int i = 2; i < nameparts.Length; i++)
                    {
                        user.LastName += " " + nameparts[i];
                    }
                }
            }

            RestClient client = new RestClient(API_ENDPOINT);
            client.AddHeader("Authorization", "Bearer " + token.Token);
            await client.PostApi<string>(COMMAND_USER, user, HttpStatusCode.Created);
            if (client.StatusCode == HttpStatusCode.Unauthorized)
            {
                token = await Authorize();
                if (token != null)
                {
                    client.ClearHeaders();
                    client.AddHeader("Authorization", "Bearer " + token.Token);
                    await client.PostApi<string>(COMMAND_USER, user, HttpStatusCode.Created);
                }
            }
        }

        /// <summary>
        /// Get the list of all upcoming events
        /// </summary>
        /// <returns></returns>
        public static async Task<IEnumerable<Event>> GetEvents()
        {
            var token = await AppToken();
            if (token == null)
                return null;

            RestClient client = new RestClient(API_ENDPOINT);
            client.AddHeader("Authorization", "Bearer " + token.Token);
            var events = await client.GetApi<IEnumerable<Event>>(COMMAND_EVENT);
            if (events == null && client.StatusCode == HttpStatusCode.Unauthorized)
            {
                token = await Authorize();
                if (token != null)
                {
                    client.ClearHeaders();
                    client.AddHeader("Authorization", "Bearer " + token.Token);
                    events = await client.GetApi<IEnumerable<Event>>(COMMAND_EVENT);
                }
            }
            return events;
        }

        /// <summary>
        /// Get details about the event including the participants
        /// </summary>
        /// <param name="eventid"></param>
        /// <returns></returns>
        /// 
        public static async Task<IEnumerable<Participant>> GetEventParticipants(int eventid)
        {
            var token = await AppToken();
            if (token == null)
                return null;

            RestClient client = new RestClient(API_ENDPOINT);
            client.AddHeader("Authorization", "Bearer " + token.Token);
            var info = await client.GetApi<EventInfo>(COMMAND_EVENT + "/" + eventid);
            if (info == null && client.StatusCode == HttpStatusCode.Unauthorized)
            {
                token = await Authorize();
                if (token != null)
                {
                    client.ClearHeaders();
                    client.AddHeader("Authorization", "Bearer " + token.Token);
                    info = await client.GetApi<EventInfo>(COMMAND_EVENT + "/" + eventid);
                }
            }

            if (info != null)
                return info.attendees;

            return null;
        }

        /// <summary>
        /// Marks the currently authorized user as either attending or not attending the event
        /// </summary>
        /// <param name="eventid"></param>
        /// <param name="attend"></param>
        /// <returns></returns>
        /// 
        public static async Task<bool> Attending(int eventid, bool attend)
        {
            var token = await AppToken();
            if (token == null)
                return false;

            RestClient client = new RestClient(API_ENDPOINT);
            client.AddHeader("Authorization", "Bearer " + token.Token);
            await client.SimpleApi(COMMAND_ATTEND + "/" + eventid, (attend ? "PUT" : "DELETE"), (attend ? attend.ToString() : null));
            if (client.StatusCode == HttpStatusCode.Unauthorized)
            {
                token = await Authorize();
                if (token != null)
                {
                    client.ClearHeaders();
                    client.AddHeader("Authorization", "Bearer " + token.Token);
                    await client.SimpleApi(COMMAND_ATTEND + "/" + eventid, (attend ? "PUT" : "DELETE"), (attend ? attend.ToString(): null));
                }
            }

            return (client.StatusCode == HttpStatusCode.OK);
        }

        /// <summary>
        /// Adds a new event to the list.  Return the added event so we have the event id
        /// </summary>
        /// <param name="newEvent"></param>
        /// <returns></returns>
        /// 
        public static async Task<EventInfo> AddEvent(Event newEvent)
        {
            var token = await AppToken();
            if (token == null)
                return null;

            RestClient client = new RestClient(API_ENDPOINT);
            client.AddHeader("Authorization", "Bearer " + token.Token);
            var addedEvent = await client.PostApi<EventInfo>(COMMAND_EVENT, newEvent, HttpStatusCode.Created);
            if (client.StatusCode == HttpStatusCode.Unauthorized)
            {
                token = await Authorize();
                if (token != null)
                {
                    client.ClearHeaders();
                    client.AddHeader("Authorization", "Bearer " + token.Token);
                    addedEvent = await client.PostApi<EventInfo>(COMMAND_EVENT, newEvent, HttpStatusCode.Created);
                }
            }

            return addedEvent;
        }

        /// <summary>
        /// Requests the deletion of an event through the API
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        /// 
        public static async Task<bool> DeleteEvent(int eventId)
        {
            var token = await AppToken();
            if (token == null)
                return false;

            RestClient client = new RestClient(API_ENDPOINT);
            client.AddHeader("Authorization", "Bearer " + token.Token);
            await client.SimpleApi(COMMAND_EVENT + "/" + eventId, "DELETE", null);
            if (client.StatusCode == HttpStatusCode.Unauthorized)
            {
                token = await Authorize();
                if (token != null)
                {
                    client.ClearHeaders();
                    client.AddHeader("Authorization", "Bearer " + token.Token);
                    await client.SimpleApi(COMMAND_EVENT + "/" + eventId, "DELETE", null);
                }
            }

            return (client.StatusCode == HttpStatusCode.OK);
        }

        /// <summary>
        /// Requests the update of event information through the API
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        /// 
        public static async Task<bool> UpdateEvent(Event _event)
        {
            var token = await AppToken();
            if (token == null)
                return false;

            RestClient client = new RestClient(API_ENDPOINT);
            client.AddHeader("Authorization", "Bearer " + token.Token);
            await client.SimpleApi(COMMAND_EVENT + "/" + _event.EventId, "PUT", _event);
            if (client.StatusCode == HttpStatusCode.Unauthorized)
            {
                token = await Authorize();
                if (token != null)
                {
                    client.ClearHeaders();
                    client.AddHeader("Authorization", "Bearer " + token.Token);
                    await client.SimpleApi(COMMAND_EVENT + "/" + _event.EventId, "PUT", _event);
                }
            }

            return (client.StatusCode == HttpStatusCode.OK);
        }

        /// <summary>
        /// Obtain an Access Token used for authenticating this application as a REST client.
        /// Access Tokens are stored in mobile persistent storage, so if not exist we must obtain a new one
        /// </summary>
        /// <returns></returns>
        /// 
        public static async Task<AccessToken> AppToken()
        {
            return Settings.Token ?? await Authorize();
        }

        /// <summary>
        /// Call the Authorize REST API to gain an access token for the pending call. 
        /// If the user is not yet part of the server app, we will add them since Facebook authorization will have
        /// been completed at this point.
        /// </summary>
        /// <returns></returns>
        /// 
        public static async Task<AccessToken> Authorize()
        {
            // Send an authorization request
            //
            AccessRequest requestBody = new AccessRequest
            {
                GroupId = Settings.GroupCode,
                UserId = Settings.UserId,
                ApiKey = Settings.GroupApi
            };

            RestClient client = new RestClient(API_ENDPOINT);
            var token = await client.PostApi<AccessToken>(COMMAND_LOGIN, requestBody, HttpStatusCode.OK);
            if (token != null)
            {
                Settings.Token = token;
                return token;
            }

            return null;
        }
    }
}
