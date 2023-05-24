namespace Foods.Domain.Exceptions
{
    public class UserIsNotAEmployeeException : Exception
    {
        public UserIsNotAEmployeeException(string? message) : base(message)
        {
        }
    }
}
