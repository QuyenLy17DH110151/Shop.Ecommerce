using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.AdminApp.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = HttpContext.Session.GetString("Token");
            if (session == null)
            {
                RedirectToAction("Index", "Login",null);
            }
            base.OnActionExecuting(context);
        }
    }
}
