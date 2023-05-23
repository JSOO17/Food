namespace Foods.Domain.HttpClients.Interfaces
{
    public interface IUserMicroHttpClient
    {
        Task<HttpResponseMessage> GetAsync(string route);
        Task<HttpResponseMessage> PostAsync(string route, HttpContent content);
        void PutAuthorization(string scheme, string auth);
    }
}
