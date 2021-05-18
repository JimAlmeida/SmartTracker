using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SmartTracker.Models.DTOs;
using System.Linq;
using System.Security.Claims;

namespace SmartTracker.Controllers
{
    public class AccountController : Controller
    {
        public async Task Login(string returnUrl = "/Home/Home")
        {
            await HttpContext.ChallengeAsync("Auth0", new AuthenticationProperties() { RedirectUri = returnUrl });
        }

        [Authorize]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync("Auth0", new AuthenticationProperties
            {
                RedirectUri = Url.Action("Index", "Home")
            });
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Profile()
        {   
            /*
             1 - Create a more comprehensive model
             2 - Create the table in the Azure DB
             3 - Implement the derived DAL interface for the model
             4 - With the User Claims info, query the DB accordingly
             5 - Map the query to the model
             6 - Pass the model to the view
             */
            return View(new UserModel()
            {
                Name = User.Identity.Name,
                EmailAddress = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
                ProfileImage = User.Claims.FirstOrDefault(c => c.Type == "picture")?.Value
            });
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Claims()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
