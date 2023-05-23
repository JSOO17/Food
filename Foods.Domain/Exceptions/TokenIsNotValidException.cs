namespace Foods.Domain.Exceptions
{
    public class TokenIsNotValidException : Exception
    {   
        public TokenIsNotValidException(string message) : base(message)
        {
        }
}
}
