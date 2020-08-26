using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using UserAuthManager.API.Data;
using UserAuthManager.API.Models;
using UserAuthManager.API.Services.Interfaces;
using UserAuthManager.Core.Models;
using UserAuthManager.Core.Repositories;

namespace UserAuthManager.API.Services
{
    public class UserService: UserRepository, IUserService
    {
        private SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;
        
        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : base(userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public UserService(UserManager<ApplicationUser> userManager, UserManagerDbContext context, SignInManager<ApplicationUser> signInManager) : base(userManager, context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public async Task<SignInResult> UserSingIn(Login login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);
            return  await _signInManager.CheckPasswordSignInAsync(user, 
                login.Password, false);
        }

        /// <summary>
        /// Set user to client role
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<IdentityResult> SetClientRole(string email)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);
            var result = await _userManager.AddToRoleAsync(user, AuthorizationConstants.Roles.CLIENT);

            return result;
        }
    }
}