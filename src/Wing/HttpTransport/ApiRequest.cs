using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Wing.Converter;
using Wing.Exceptions;

namespace Wing.HttpTransport
{
    public class ApiRequest : IRequest
    {
        private readonly IJson _json;
        private readonly HttpClient _client;
        private readonly string _mediaTyype;

        public ApiRequest(IJson json, string contentType = "application/json")
        {
            _json = json;
            _client = new HttpClient();
            _mediaTyype = contentType;
        }

        public ApiRequest(IJson json, HttpClient client, string contentType = "application/json")
        {
            _json = json;
            _client = client;
            _mediaTyype = contentType;
        }

        public async Task<bool> Delete(string uri)
        {
            var response = await _client.DeleteAsync(uri);
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return true;
            }

            throw new ApiRequestException(uri, response.StatusCode);
        }

        public async Task<T> Get<T>(string uri)
        {
            var response = await _client.GetAsync(uri);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return _json.Deserialize<T>(await response.Content.ReadAsStringAsync());
            }

            throw new ApiRequestException(uri, response.StatusCode);
        }

        public Task<T> Patch<T>(string uri, object data = null)
        {
            return Request<T>(
                uri,
                content =>
            {
                var request = new HttpRequestMessage(new HttpMethod("Patch"), uri)
                {
                    Content = content
                };
                return _client.SendAsync(request);
            },
                data);
        }

        public Task<T> Post<T>(string uri, object data = null)
        {
            return Request<T>(uri, content => { return _client.PostAsync(uri, content); }, data);
        }

        public Task<T> Put<T>(string uri, object data = null)
        {
            return Request<T>(uri, content => { return _client.PutAsync(uri, content); }, data);
        }

        protected virtual async Task<T> Request<T>(string uri, Func<StringContent, Task<HttpResponseMessage>> callback, object data = null)
        {
            StringContent content = null;
            string reqJson = string.Empty;
            if (data != null)
            {
                reqJson = _json.Serialize(data);
                content = new StringContent(reqJson, Encoding.UTF8, _mediaTyype);
            }

            var response = await callback(content);
            var result = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
            {
                return _json.Deserialize<T>(result);
            }

            throw new ApiRequestException(uri, response.StatusCode, reqJson, result);
        }
    }
}
