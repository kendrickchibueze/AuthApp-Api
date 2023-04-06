using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AuthApp_Api.Services.Interface
{
    public interface ISecurityService
    {

        void SecureToken(Claim[] claim, out JwtSecurityToken token, out string tokenAstring);
    }
}
