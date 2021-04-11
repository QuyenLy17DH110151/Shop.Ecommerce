using Ecommerce.AdminApp.Service;
using Ecommerce.ViewModel.System.Users;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.AdminApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserAPIClient _userAPIClient;

        public UserController(IUserAPIClient userAPIClient)
        {
            _userAPIClient = userAPIClient;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var token = await _userAPIClient.Authenticate(request);
            return View(token);
        }
    }
}
