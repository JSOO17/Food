using Foods.Domain.Exceptions;
using Foods.Domain.HttpClients.Interfaces;
using Foods.Domain.Utils;
using Foods.Infrastructure.API.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Foods.Infrastructure.API.Controllers
{
    public class BaseController : ControllerBase
    {
        private readonly IUserMicroHttpClient _userHttpClient;

        protected const string MessageForbbiden = "forbbiden. Errors: {0}";
        protected const string MessageUnauthorized = "Unauthorized. Errors: {0}";
        protected const string MessageBadRequest = "bad request. Errors: {0}";

        public BaseController(IUserMicroHttpClient userMicroHttpClient)
        {
            _userHttpClient = userMicroHttpClient;
        }

        public Payload GetPayload()
        {
            var auth = Request.Headers.TryGetValue("Authorization", out var value) ? value.ToString() : string.Empty;

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(auth.Replace(JwtBearerDefaults.AuthenticationScheme + " ", string.Empty));

            IEnumerable<Claim> claims = jwtToken.Claims;

            return new Payload
            {
                RoleId = int.Parse(GetClaim(ClaimTypes.Role, claims).Value),
                UserId = int.Parse(GetClaim(ClaimTypes.NameIdentifier, claims).Value),
                Cellphone = GetClaim(ClaimTypes.MobilePhone, claims).Value,
                Email = GetClaim(ClaimTypes.Email, claims).Value
            };
        }

        protected Claim GetClaim(string claimType, IEnumerable<Claim> claims)
        {
            if (claims != null)
            {
                return claims.FirstOrDefault(x => x.Type == claimType) ?? new Claim(string.Empty, string.Empty);
            }

            return new Claim(string.Empty, string.Empty);
        }

        public async Task ValidateToken()
        {
            var auth = Request.Headers.TryGetValue("Authorization", out var value) ? value.ToString() : string.Empty;

            _userHttpClient.PutAuthorization(JwtBearerDefaults.AuthenticationScheme, auth);

            var contenido = new StringContent(string.Empty, Encoding.UTF8, "application/json");
            var result = await _userHttpClient.PostAsync("api/User/ValidateToken", contenido);

            if (!result.IsSuccessStatusCode)
            {
                throw new TokenIsNotValidException("The token is not valid");
            }
        }

        public void IsOwner(int roleId)
        {
            if (roleId != Roles.Owner)
            {
                throw new RoleHasNotPermissionException("You dont have permissions");
            }
        }
    }
}
