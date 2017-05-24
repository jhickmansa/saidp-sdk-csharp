using System.Net;
using System.Net.Http;
using System.Text;

namespace SecureAuth.Sdk
{
    public class ApiClient : IApiClient
    {
        #region Private Members
        public Configuration Configuration { get; set; }
        public  HttpClient ApiHttpClient { get; set; }
        #endregion

        #region Properties
        public string AppId
        {
            get { return this.Configuration.AppId; }
        }

        public string AppKey
        {
            get { return this.Configuration.AppKey; }
        }

        public string SecureAuthRealmUrl
        {
            get { return this.Configuration.SecureAuthRealmUrl; }
        }
        #endregion

        #region Constructors
        internal ApiClient(Configuration configuration)
        {
            this.Configuration = configuration;
            this.ApiHttpClient = new HttpClient(new HmacSigningHandler(this.AppId, this.AppKey));

            if (configuration.BypassCertValidation)
            {
                ServicePointManager.ServerCertificateValidationCallback = new
                System.Net.Security.RemoteCertificateValidationCallback
                (
                   delegate { return true; }
                );
            }
        }
        #endregion

        #region Methods
        public T Get<T>(string apiEndpoint) where T : BaseResponse
        {
            string rawResult = string.Empty;
            HttpStatusCode statusCode;
            string requestUrl = string.Concat(this.SecureAuthRealmUrl, apiEndpoint);

            // Process HTTP request
            using (HttpClient client = ApiHttpClient)
            {
                var response = client.GetAsync(requestUrl).Result;
                statusCode = response.StatusCode;
                rawResult = response.Content.ReadAsStringAsync().Result;
            }

            // Deserialize the response
            var result = JsonSerializer.Deserialize<T>(rawResult);

            // Set HTTP status code and return
            ((BaseResponse) result).StatusCode = statusCode;
            ((BaseResponse) result).RawJson = rawResult;
            return result;
        }

        public T Post<T>(string apiEndpoint, BaseRequest request = null) 
            where T : BaseResponse
        {
            string rawResult = string.Empty;
            string rawRequest = string.Empty;
            HttpStatusCode statusCode;
            string requestUrl = string.Concat(this.SecureAuthRealmUrl, apiEndpoint);

            // Process HTTP request
            using (HttpClient client = ApiHttpClient)
            {
                if (request != null)
                {
                    rawRequest = JsonSerializer.Serialize(request);
                }
                HttpContent content = new StringContent(rawRequest, Encoding.UTF8, "application/json");
                var response = client.PostAsync(requestUrl, content).Result;
                statusCode = response.StatusCode;

                rawResult = response.Content.ReadAsStringAsync().Result;
            }

            // Deserialize the response
            var result = JsonSerializer.Deserialize<T>(rawResult);

            // Set HTTP status code and return
            ((BaseResponse)result).StatusCode = statusCode;
            ((BaseResponse)result).RawJson = rawResult;
            ((BaseResponse)result).RawRequestJson = rawRequest;
            return result;
        }

        public T Put<T>(string apiEndpoint, BaseRequest request = null)
            where T : BaseResponse
        {
            string rawResult = string.Empty;
            string rawRequest = string.Empty;
            HttpStatusCode statusCode;
            string requestUrl = string.Concat(this.SecureAuthRealmUrl, apiEndpoint);

            using (HttpClient client = ApiHttpClient)
            {
                if (request != null)
                {
                    rawRequest = JsonSerializer.Serialize(request);
                }
                HttpContent content = new StringContent(rawRequest, Encoding.UTF8, "application/json");
                var response = client.PutAsync(requestUrl, content).Result;
                statusCode = response.StatusCode;

                rawResult = response.Content.ReadAsStringAsync().Result;
            }

            // Deserialize the response
            var result = JsonSerializer.Deserialize<T>(rawResult);

            // Set HTTP status code and return
            ((BaseResponse)result).StatusCode = statusCode;
            ((BaseResponse)result).RawJson = rawResult;
            ((BaseResponse)result).RawRequestJson = rawRequest;
            return result;
        }
        #endregion
    }
}
