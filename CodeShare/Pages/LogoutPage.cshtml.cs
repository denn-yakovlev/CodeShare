using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeShare.Pages
{
    public class LogoutPageModel : PageModel
    {
        public async Task<IActionResult> OnGet(string returnRoute)
        {
            await HttpContext.SignOutAsync("Cookies");
            if (Url.IsLocalUrl(returnRoute))
                return Redirect(returnRoute);
            return RedirectToPage("_Host");
        }
    }
}