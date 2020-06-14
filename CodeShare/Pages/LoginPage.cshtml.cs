using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeShare.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string UserName { get; set; }

        [BindProperty]
        public string Password { get; set; }

        //public string ReturnRoute { get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync(string returnRoute)
        {
            //ReturnRoute = returnRoute;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, UserName),
                new Claim("Password", Password)
            };
            var identity = new ClaimsIdentity(claims, "Cookies");
            //var authResult = await HttpContext.AuthenticateAsync("Cookies");
            //AuthenticateResult.Success(authResult.Ticket);
            await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(identity));
            if (Url.IsLocalUrl(returnRoute))
                return Redirect(returnRoute);
            return RedirectToPage("_Host");
        }
    }
}