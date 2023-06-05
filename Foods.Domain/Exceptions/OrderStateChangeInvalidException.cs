namespace Foods.Domain.Exceptions
{
    public class OrderStateChangeInvalidException : Exception
    {
        public OrderStateChangeInvalidException(string msg) : base(msg) { } 
    }
}
