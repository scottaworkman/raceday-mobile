using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RaceDay.Services
{
    public class RestClient
    {
        private readonly Uri baseUrl;
        private List<KeyValuePair<string, string>> headers;

        public HttpStatusCode StatusCode { get; set; }

        public RestClient(string url)
        {
            baseUrl = new Uri(url);
            headers = new List<KeyValuePair<string, string>>();
        }

        public void AddHeader(string name, string value)
        {
            headers.Add(new KeyValuePair<string, string>(name, value));
        }

        public void ClearHeaders()
        {
            headers.Clear();
        }

        public async Task<T> PostApi<T>(string api, object value, HttpStatusCode success) where T : class
        {
            try
            {
                // Create Http Client
                var client = HttpWebRequest.Create(new Uri(baseUrl, api));
                client.Method = "POST";
                client.ContentType = "application/json";

                // Add any headers
                //
                foreach (var header in headers)
                    client.Headers[header.Key] = header.Value;

                // Write Post body to request
                if (value != null)
                {
                    var content = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value));
                    Stream dataStream = await client.GetRequestStreamAsync();
                    dataStream.Write(content, 0, content.Length);
                }

                // Obtain Response and parse the return
                //
                HttpWebResponse response = await client.GetResponseAsync() as HttpWebResponse;
                StatusCode = response.StatusCode;
                if (StatusCode == success)
                {
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(receiveStream, Encoding.UTF8);
                    string responseBody = await reader.ReadToEndAsync();

                    return JsonConvert.DeserializeObject<T>(responseBody);
                }
            }
            catch(WebException ex)
            {
                var response = ex.Response as HttpWebResponse;
                if (response != null)
                {
                    StatusCode = response.StatusCode;
                }
                else
                {
                    StatusCode = HttpStatusCode.GatewayTimeout;
                }
            }
            catch(Exception)
            { }
            return null;
        }

        public async Task<T> GetApi<T>(string api) where T : class
        {
            try
            {
                // Create Http Client
                var client = HttpWebRequest.Create(new Uri(baseUrl, api));
                client.Method = "GET";
                client.ContentType = "application/json";

                // Add any headers
                //
                foreach (var header in headers)
                    client.Headers[header.Key] = header.Value;

                // Obtain Response and parse the return
                //
                HttpWebResponse response = await client.GetResponseAsync() as HttpWebResponse;
                StatusCode = response.StatusCode;

                if (StatusCode == HttpStatusCode.OK)
                {
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(receiveStream, Encoding.UTF8);
                    string responseBody = await reader.ReadToEndAsync();

                    return JsonConvert.DeserializeObject<T>(responseBody);
                }
            }
            catch (WebException ex)
            {
                var response = ex.Response as HttpWebResponse;
                if (response != null)
                {
                    StatusCode = response.StatusCode;
                }
                else
                {
                    StatusCode = HttpStatusCode.GatewayTimeout;
                }
            }
            catch (Exception)
            { }
            return null;
        }

        /// <summary>
        /// Simple PUT REST request with no request/response body
        /// </summary>
        /// <param name="api"></param>
        /// <returns></returns>
        /// 
        public async Task PutApi(string api)
        {
            await SimpleApi(api, "PUT", null);
            return;
        }

        /// <summary>
        /// Simple PUT DELETE request with no request/response body
        /// </summary>
        /// <param name="api"></param>
        /// <returns></returns>
        /// 
        public async Task DeleteApi(string api)
        {
            await SimpleApi(api, "DELETE", null);
            return;
        }

        /// <summary>
        /// Common method for making a REST call with an option request body and no response value
        /// </summary>
        /// <param name="api"></param>
        /// <param name="method"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// 
        public async Task SimpleApi(string api, string method, object value)
        {
            try { 
                // Create Http Client
                var client = HttpWebRequest.Create(new Uri(baseUrl, api));
                client.Method = method;
                client.ContentType = "application/json";

                // Add any headers
                //
                foreach (var header in headers)
                    client.Headers[header.Key] = header.Value;

                // Write body to request, if any
                if (value != null)
                {
                    var content = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value));
                    Stream dataStream = await client.GetRequestStreamAsync();
                    dataStream.Write(content, 0, content.Length);
                }
                else
                {
                    client.ContentLength = 0;
                }

                // Obtain Response, no return expected
                //
                HttpWebResponse response = await client.GetResponseAsync() as HttpWebResponse;
                StatusCode = response.StatusCode;
            }
            catch (WebException ex)
            {
                var response = ex.Response as HttpWebResponse;
                if (response != null)
                {
                    StatusCode = response.StatusCode;
                }
                else
                {
                    StatusCode = HttpStatusCode.GatewayTimeout;
                }
            }
            catch (Exception)
            { }

            return;
        }
    }
}
