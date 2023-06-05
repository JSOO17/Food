using Foods.Domain.HttpClients.Interfaces;
using Foods.Domain.Models;
using Newtonsoft.Json;
using System.Text;

namespace Foods.Domain.HttpClients
{
    public class MessengerHttpClient : IMessengerHttpClient
    {

        private readonly HttpClient _httpClient;

        public MessengerHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task SendMessageAsync(MessageModel messageModel)
        {
            var message = JsonConvert.SerializeObject(messageModel);

            var content = new StringContent(message, Encoding.UTF8, "application/json");

            await _httpClient.PostAsync("api/Message", content);
        }
    }
}
