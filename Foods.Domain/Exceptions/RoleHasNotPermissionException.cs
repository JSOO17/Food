namespace Foods.Domain.Exceptions
{
    public class RoleHasNotPermissionException : Exception
    {
        public RoleHasNotPermissionException(string msg) : base(msg) { }
    }
}
