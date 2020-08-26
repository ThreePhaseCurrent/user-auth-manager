using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using UserAuthManager.Core.Models;

namespace UserAuthManager.Core.Repositories.Interfaces
{
    public interface IUserRepository
    {
        IQueryable<ApplicationUser> Users { get; }
        Task<List<ApplicationUser>> GetAll();
        Task<IdentityResult> CreateUser(ApplicationUser user, string password);
    }
}