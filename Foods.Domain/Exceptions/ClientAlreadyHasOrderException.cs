namespace Foods.Domain.Exceptions
{
    public class ClientAlreadyHasOrderException : Exception
    {
        public ClientAlreadyHasOrderException(string message) : base(message)
        {
        }
    }
}
