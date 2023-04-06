using AuthApp_Api.Services.Interface;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthApp_Api.Services
{
    public class JWTSecurityService:ISecurityService

    {

        public JWTSecurityService(IConfiguration configuration)
        {

            Configuration = configuration;

        }


        public IConfiguration Configuration { get; }

        public void SecureToken(Claim[] claims, out JwtSecurityToken token, out string tokenAstring)
        {
            try
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Authentication:Key"]));
                token = new JwtSecurityToken
                (

                  issuer: Configuration["Authentication:Issuer"],
                  audience: Configuration["Authentication:Audience"],
                  claims: claims,
                  expires: DateTime.Now.AddHours(1),
                  signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                  );


                tokenAstring = new JwtSecurityTokenHandler().WriteToken(token);


                 
                    
                

            }
            catch (Exception)
            {
                throw;
            }
        }

      

    }
}
