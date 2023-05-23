namespace Foods.Infrastructure.API.Security
{
    public class Payload
    {
        public int RoleId { get; set; } 
        public string Email { get; set; }
        public long UserId { get; set; }
    }
}
