using Foods.Domain.HttpClients.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<HttpResponseMessage> GetAsync(string ruta)
        {
            return await _httpClient.GetAsync(ruta);
        }

        public async Task<HttpResponseMessage> PostAsync(string ruta, HttpContent contenido)
        {
            return await _httpClient.PostAsync(ruta, contenido);
        }
    }
}
