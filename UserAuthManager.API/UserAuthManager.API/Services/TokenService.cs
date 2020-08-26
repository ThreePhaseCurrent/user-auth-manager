using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UserAuthManager.API.Models;
using UserAuthManager.API.Services.Interfaces;
using UserAuthManager.Core.Models;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace UserAuthManager.API.Services
{
    public class TokenService : ITokenService
    {
        private readonly IOptions<AuthOptions> _authOptions;
        private readonly UserManager<ApplicationUser> _userManager;

        public TokenService(IOptions<AuthOptions> authOptions, UserManager<ApplicationUser> userManager)
        {
            _authOptions = authOptions;
            _userManager = userManager;
        }

        /// <summary>
        /// Generate token for user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<string> GetToken(ApplicationUser user)
        {
            var authParams = _authOptions.Value;

            var securityKey = authParams.GetSymmetricSecurityKey();
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim("role", role));
            }
            
            var token = new JwtSecurityToken(authParams.Issuer, authParams.Audience, claims,
                expires: DateTime.Now.AddSeconds(authParams.TokenLifeTime),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Task<string> RefreshToken(ApplicationUser user)
        {
            throw new System.NotImplementedException();
        }
    }
}