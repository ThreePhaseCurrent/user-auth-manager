using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserAuthManager.API.Models;
using UserAuthManager.API.Services.Interfaces;
using UserAuthManager.API.Validators;
using UserAuthManager.API.ViewModels;
using UserAuthManager.Core.Models;

namespace UserAuthManager.API.Controllers
{
    [ApiController]
    [Route("")]
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        
        public AuthController(ILogger<AuthController> logger, IUserService userService, IMapper mapper, 
            ITokenService tokenService)
        {
            _logger = logger;
            _userService = userService;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] Login request)
        {
            var validResult = await new LoginValidator().ValidateAsync(request);

            if (!validResult.IsValid)
            {
                _logger.Log(LogLevel.Information, "User credentials is wrong!");
                
                foreach (var error in validResult.Errors)
                {
                    ModelState.AddModelError(error.ErrorCode, error.ErrorMessage);
                }
                
                return BadRequest(ModelState);
            }

            var signInResult = await _userService.UserSingIn(request);

            if (signInResult.Succeeded)
            {
                var user = await _userService.Users.FirstOrDefaultAsync(x => 
                    x.Email == request.Email);
                var token = await _tokenService.GetToken(user);
                
                _logger.Log(LogLevel.Information, "User was signin!");
            
                return Ok(new SuccessLoginViewModel()
                {
                    AccessToken = token
                });
            }
            
            return Unauthorized();
        }
        
        /// <summary>
        /// Check username exist
        /// </summary>
        /// <param name="username">User name</param>
        /// <returns>Returns True if username is not exist</returns>
        [Route("username-check/{username}")]
        [HttpGet]
        public async Task<bool> UserNameCheck(string username)
        {
            var user = await _userService.Users
                .FirstOrDefaultAsync(u => u.UserName == username);

            return user == null;
        }

        /// <summary>
        /// Register user
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] Register register)
        {
            var validResult = await new RegisterValidator().ValidateAsync(register);
            
            if (!validResult.IsValid)
            {
                _logger.Log(LogLevel.Information, "User data is not valid!");

                foreach (var error in validResult.Errors)
                {
                    ModelState.AddModelError(error.ErrorCode, error.ErrorMessage);
                }

                return BadRequest(ModelState);
            }

            var user = _mapper.Map<ApplicationUser>(register);

            var result = await _userService.CreateUser(user, register.Password);
            if (result.Succeeded)
            {
                _logger.Log(LogLevel.Information, "User was register!");

                await _userService.SetClientRole(user.Email);
                return Ok();
            }

            return BadRequest();
        }
    }
}