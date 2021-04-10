using Ecommerce.Application.System;
using Ecommerce.ViewModel.System.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromForm] LoginRequest request)
        {
            var resultToken = await _userService.Authencate(request);
            if (string.IsNullOrEmpty(resultToken))
            {
                return BadRequest("Username or password is incorrect");
            }
            return Ok(new { token = resultToken });
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromForm] RegisterRequest request)
        {
            var result = await _userService.Register(request);
            if (result == false)
            {
                return BadRequest("Register is unsuccessful");
            }
            return Ok(new { token = result });
        }
    }
}   
