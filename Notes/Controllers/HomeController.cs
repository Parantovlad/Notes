using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notes.User;

namespace Notes.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUserTracker _userTracker;

        public HomeController(IUserTracker userTracker)
        {
            _userTracker = userTracker;
        }

        public IActionResult Index()
        {
            var user = HttpContext.User?.Identity as ClaimsIdentity;
            if (user != null && user.IsAuthenticated)
            {
                _userTracker.AddUser(user.FindFirst("sid").Value, user.Name);
            }

            return View();
        }

        public IActionResult Error()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }
    }
}
