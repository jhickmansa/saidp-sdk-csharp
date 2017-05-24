using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace SecureAuth.Sdk
{
    public interface IApiClient
    {
        Configuration Configuration { get; set; }
        HttpClient ApiHttpClient { get; set; }
        string AppId { get; }
        string AppKey { get; }
        string SecureAuthRealmUrl { get; }

        T Get<T>(string apiEndpoint) where T : BaseResponse;
        T Post<T>(string apiEndpoint, BaseRequest request = null) where T : BaseResponse;
        T Put<T>(string apiEndpoint, BaseRequest request = null) where T : BaseResponse;
    }
}
