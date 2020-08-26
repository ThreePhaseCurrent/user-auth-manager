using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserAuthManager.API.Models;
using UserAuthManager.API.Services.Interfaces;
using UserAuthManager.API.ViewModels;

namespace UserAuthManager.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        
        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all users with short data
        /// </summary>
        /// <returns></returns>
        [Route("users")]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAll();
            var shortUsers = _mapper.Map<List<ShortUserInfo>>(users);

            return Ok(shortUsers);
        }
    }
}