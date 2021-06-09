using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SmartTracker.Models.DTOs;
using SmartTracker.Models.DAL;
using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;

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
            var sql_statement= new SQLStatement<UserModel>(_table: "UserInfo").ReadSTMT(where_column: "EmailAddress");
            
            var model_for_query = new UserModel(){
                EmailAddress = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value
            };

            List<UserModel> queries = SQLDispatcher<UserModel>.Read(model_for_query, sql_statement);
            
            if (queries.Count > 1) 
                return View("Error");
            else 
                return View(queries[0]);
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
