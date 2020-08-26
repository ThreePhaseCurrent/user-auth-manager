using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace UserAuthManager.Core.Models
{
    public class UserManagerDbContext: IdentityDbContext<ApplicationUser>
    {
        public UserManagerDbContext(DbContextOptions<UserManagerDbContext> options)
            : base(options)
        {
        }
    }
}