using Contentful.Essential.Sample.Models;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Contentful.Essential.Sample.Http
{
    public class GenericClient : IHttpClient, IDisposable
    {
        private const int DEFAULT_TIMEOUT = 15;
        private HttpClient _client;
        /// <summary>
        /// Will create a custom http client with the JSON request header
        /// </summary>
        /// <param name="baseUrl">The base URL of the service</param>
        public GenericClient(string baseUrl) : this(baseUrl, new MediaTypeWithQualityHeaderValue("application/json")) { }

        /// <summary>
        /// Will create a custom http client with the supplied requestheaders
        /// </summary>
        /// <param name="baseUrl">The base URL of the service</param>
        /// <param name="mediaTypes">The media types to allow</param>
        public GenericClient(string baseUrl, params MediaTypeWithQualityHeaderValue[] mediaTypes)
        {
            //ServicePointManager.ServerCertificateValidationCallback += CertCallback;
            _client = new HttpClient();
            _client.BaseAddress = new Uri(baseUrl);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.Timeout = TimeSpan.FromSeconds(DEFAULT_TIMEOUT);
            foreach (var mType in mediaTypes)
            {
                _client.DefaultRequestHeaders.Accept.Add(mType);
            }
        }

        public GenericClient(string baseUrl, string token, params MediaTypeWithQualityHeaderValue[] mediaTypes)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(baseUrl);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", token);
            _client.Timeout = TimeSpan.FromSeconds(DEFAULT_TIMEOUT);
            foreach (var mType in mediaTypes)
            {
                _client.DefaultRequestHeaders.Accept.Add(mType);
            }
        }

        public GenericClient(string baseUrl, string userName, string password, params MediaTypeWithQualityHeaderValue[] mediaTypes)
        {
            //ServicePointManager.ServerCertificateValidationCallback += CertCallback;
            _client = new HttpClient();
            _client.BaseAddress = new Uri(baseUrl);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Authorization = GetBasicAuthHeader(userName, password);
            _client.Timeout = TimeSpan.FromSeconds(DEFAULT_TIMEOUT);
            foreach (var mType in mediaTypes)
            {
                _client.DefaultRequestHeaders.Accept.Add(mType);
            }
        }
        /// <summary>
        /// Will return a Basic authentication header
        /// </summary>
        /// <param name="userName">the username</param>
        /// <param name="pwd">the password</param>
        /// <returns></returns>
        protected virtual AuthenticationHeaderValue GetBasicAuthHeader(string userName, string pwd)
        {
            string format = string.Format("{0}:{1}", userName, pwd);
            string authString = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(format));
            return new AuthenticationHeaderValue("Basic", authString);
        }

        ///// <summary>
        ///// Callback for all certification here we can check if it OK or not...
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="cert"></param>
        ///// <param name="chain"></param>
        ///// <param name="sslPolicyErrors"></param>
        ///// <returns></returns>
        //private bool CertCallback(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        //{
        //    //do some magic here...
        //    return true;
        //}

        public virtual void UpdateTimeout(int timeout)
        {
            _client.Timeout = TimeSpan.FromSeconds(timeout);
        }

        /// <summary>
        /// Sets OAuth Access Token
        /// Currently only handles grant types ClientCredentials and Password
        /// </summary>
        /// <param name="tokenUriPath"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="grantType"></param>
        public virtual int SetAccessToken(string baseUrl, string tokenUriPath, string clientId, string clientSecret, string username = "", string password = "", OAuthGrantType grantType = OAuthGrantType.ClientCredentials)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);

            if (!string.IsNullOrWhiteSpace(clientId) && !string.IsNullOrWhiteSpace(clientSecret))
                client.DefaultRequestHeaders.Authorization = GetBasicAuthHeader(clientId, clientSecret);

            HttpResponseMessage response;
            try
            {
                if (grantType == OAuthGrantType.Password)
                {
                    response = client.PostAsync(tokenUriPath,
                                                  new StringContent(string.Format("grant_type=password&username={0}&password={1}",
                                                  HttpUtility.UrlEncode(username),
                                                  HttpUtility.UrlEncode(password)), Encoding.UTF8,
                                                  "application/x-www-form-urlencoded")).Result;
                }
                else
                {
                    response = client.PostAsync(tokenUriPath, new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded")).Result;
                }

                string resultJSON = response.Content.ReadAsStringAsync().Result;
                client.Dispose();


                APIToken result = JsonConvert.DeserializeObject<APIToken>(resultJSON);

                if (response.IsSuccessStatusCode)
                {
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(result.TokenType.ToLower()), result.AccessToken);
                }
                else
                {
                    // SystemLog.Log(this, string.Format("Unable to get access token for {0}; Reason: {1}", baseUrl, result.Error), Level.Error);
                }
                return result.ExpiresIn;
            }
            catch (Exception ex)
            {
                // SystemLog.Log(this, string.Format("Unable to get access token for {0}; Reason: {1}", baseUrl, ex.Message), Level.Error, ex);
                return 0;
            }
        }

        /// <summary>
        /// Will make a request to a REST service and return the response
        /// </summary>
        /// <typeparam name="T">The type to deserialize the result as</typeparam>
        /// <param name="path">the local path of the request ex: "/products/find" </param>
        /// <param name="query">Query params to send with the call. ex: ?q=bacon&page=1&size=10</param>
        /// <param name="onError">A callback to get the exception form the request</param>
        /// <returns>Returns <typeparamref name="T"/></returns>
        public T Get<T>(string path, object query = null, Action<Exception> onError = null) where T : class
        {
            string queryPath = path;
            if (query != null)
            {
                var queryString = query.GetType().GetProperties().Select(p => string.Format("{0}={1}", p.Name, p.GetValue(query)));
                queryPath = string.Format("{0}?{1}", path.TrimEnd('/'), string.Join("&", queryString));
            }

            return Get<T, Exception>(queryPath, onError).Result;
        }

        /// <summary>
        /// Will make a request to a REST service and return the response
        /// </summary>
        /// <typeparam name="T">The type to deserialize the result as</typeparam>
        /// <param name="queryPath">the fully qualified path, with query. ex: /products/find/?q=bacon&page=1&size=10</param>
        /// <param name="onError">A callback to get the exception form the request</param>
        /// <returns>Returns <typeparamref name="T"/></returns>
        private async Task<T> Get<T, TEx>(string queryPath, Action<TEx> onerror)
            where T : class
            where TEx : Exception
        {
            try
            {
                //"api/stores/locate/?Lat=41.6281940&Lng=-87.628194&Page=1&PageSize=10&WithinMiles=50";
                HttpResponseMessage response = await _client.GetAsync(queryPath).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<T>().Result;
                }
                else
                {
                    // SystemLog.Log(this, string.Format("{0}: Status {1}, {2}", queryPath, response.StatusCode, response.ReasonPhrase), Level.Error);
                    return default(T);
                }
            }

            catch (TaskCanceledException timeoutEx)
            {
                // SystemLog.Log(this, string.Format("{0}", queryPath), Level.Error, timeoutEx);
                return default(T);
            }

            catch (TEx ex)
            {
                // SystemLog.Log(this, string.Format("{0}", queryPath), Level.Error, ex);

                if (onerror != null)
                {
                    onerror(ex);
                }

                throw;
            }
        }

        /// <summary>
        /// Will make a request to a REST service and return the response
        /// </summary>
        /// <typeparam name="T">The type to deserialize the result as</typeparam>
        /// <param name="path">the local path of the request ex: "/products/find" </param>
        /// <param name="query">Query params to send with the call. ex: ?q=bacon&page=1&size=10</param>
        /// <param name="onError">A callback to get the exception form the request</param>
        /// <returns>Returns <typeparamref name="T"/> if the response was unsuccessful.  Returns null on success.</returns>
        public T Post<T>(string path, object query = null, Action<Exception> onError = null) where T : class
        {
            return Post<T, Exception>(path, query, onError).Result;
        }

        /// <summary>
        /// Will make a request to a REST service and return the response
        /// </summary>
        /// <typeparam name="T">The type to deserialize the result as</typeparam>
        /// <param name="queryPath">the fully qualified path, with query. ex: /products/find/?q=bacon&page=1&size=10</param>
        /// <param name="onError">A callback to get the exception form the request</param>
        /// <returns>Returns <typeparamref name="T"/> if the response was unsuccessful.  Returns null on success.</returns>
        private async Task<T> Post<T, TEx>(string queryPath, object query, Action<TEx> onerror)
            where T : class
            where TEx : Exception
        {
            try
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync(queryPath, query).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    // SystemLog.Log(this, string.Format("{0}: Status {1}, {2}", queryPath, response.StatusCode, response.ReasonPhrase), Level.Error);
                    T result = response.Content.ReadAsAsync<T>().Result;
                    return (T)Convert.ChangeType(result, typeof(T));
                }
                else
                {
                    T result = response.Content.ReadAsAsync<T>().Result;
                    if (result == null)
                    {
                        return default(T);
                    }
                    return (T)Convert.ChangeType(result, typeof(T));
                }
            }

            catch (TaskCanceledException timeoutEx)
            {
                // SystemLog.Log(this, string.Format("{0}", queryPath), Level.Error, timeoutEx);
                return default(T);
            }

            catch (TEx ex)
            {
                // SystemLog.Log(this, string.Format("{0}", queryPath), Level.Error, ex);

                if (onerror != null)
                {
                    onerror(ex);
                }

                throw;
            }
        }


        public void Dispose()
        {
            _client.Dispose();
            //ServicePointManager.ServerCertificateValidationCallback -= CertCallback;
        }
    }
}
