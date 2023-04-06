using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthApp_Api.Data
{
    public class AuthenticationDbContext:IdentityDbContext<User>
    {

        public AuthenticationDbContext(DbContextOptions options):base(options)
        {

        }

       
    }
}
