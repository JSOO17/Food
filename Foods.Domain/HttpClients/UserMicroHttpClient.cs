using Foods.Domain.HttpClients.Interfaces;
using System.Net.Http.Headers;

namespace Foods.Domain.HttpClients
{
    public class UserMicroHttpClient : IUserMicroHttpClient
    {
        private readonly HttpClient _httpClient;

        public UserMicroHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void PutAuthorization(string scheme, string auth)
        {
            auth = auth.Replace(scheme + " ", string.Empty);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme, auth);
        }

        public async Task<HttpResponseMessage> GetAsync(string route)
        {
            return await _httpClient.GetAsync(route);
        }

        public async Task<HttpResponseMessage> PostAsync(string route, HttpContent content)
        {
            return await _httpClient.PostAsync(route, content);
        }
    }
}
