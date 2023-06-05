using Foods.Domain.Models;

namespace Foods.Domain.HttpClients.Interfaces
{
    public interface IMessengerHttpClient
    {
        Task SendMessageAsync(MessageModel message);
    }
}
