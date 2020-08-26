using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserAuthManager.Core.Models;

namespace UserAuthManager.API.Data
{
    public class InitData
    {
        public static void SeedData(IServiceProvider services)
        {
            var context = services.GetService<UserManagerDbContext>();
            
            string[] roles = { AuthorizationConstants.Roles.CLIENT };

            foreach (string role in roles)
            {
                var roleStore = new RoleStore<IdentityRole>(context);
            
                if (!context.Roles.Any(r => r.Name == role))
                {
                    var nr = new IdentityRole(role) {NormalizedName = role.ToUpper()};
                    roleStore.CreateAsync(nr);
                }
            }
            
            context.SaveChangesAsync();
        }
    }
}