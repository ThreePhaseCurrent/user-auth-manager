using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using UserAuthManager.API.Models;
using UserAuthManager.Core.Repositories.Interfaces;

namespace UserAuthManager.API.Services.Interfaces
{
    public interface IUserService: IUserRepository
    {
        Task<SignInResult> UserSingIn(Login login);
        Task<IdentityResult> SetClientRole(string email);
    }
}