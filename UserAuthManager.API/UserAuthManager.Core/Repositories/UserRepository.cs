using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserAuthManager.Core.Models;
using UserAuthManager.Core.Repositories.Interfaces;

namespace UserAuthManager.Core.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly UserManagerDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        public IQueryable<ApplicationUser> Users => _context.Users;
        
        public UserRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        } 
        
        public UserRepository(UserManager<ApplicationUser> userManager, UserManagerDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        
        public async Task<List<ApplicationUser>> GetAll()
        {
            return await _context.Set<ApplicationUser>().ToListAsync();
        }
        
        public async Task<IdentityResult> CreateUser(ApplicationUser user, string password) =>
            await _userManager.CreateAsync(user, password);
    }
}