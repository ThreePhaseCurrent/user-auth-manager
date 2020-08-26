using System.Threading.Tasks;
using UserAuthManager.Core.Models;

namespace UserAuthManager.API.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> GetToken(ApplicationUser user);
        Task<string> RefreshToken(ApplicationUser user);
    }
}