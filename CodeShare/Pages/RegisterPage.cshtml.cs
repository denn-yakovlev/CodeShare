using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CodeShare.Services.DatabaseInteractor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using CodeShare.Model.DTOs;

namespace CodeShare.Pages
{
    public class RegisterPageModel : PageModel
    {

        [BindProperty]
        [Required]
        [MinLength(3, ErrorMessage = "Password should have at least 3 characters")]
        public string UserName { get; set; }

        [BindProperty]
        [Required]
        [MinLength(8, ErrorMessage = "Password should have at least 8 characters")]
        public string Password { get; set; }

        [BindProperty]
        [Required]
        [MinLength(8, ErrorMessage = "Password should have at least 8 characters")]
        public string RepeatedPassword { get; set; }

        public IDatabaseInteractor DbInteractor { get; }

        public RegisterPageModel(IDatabaseInteractor dbInteractor)
        {
            DbInteractor = dbInteractor;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync(string returnRoute)
        {
            if (ModelState.IsValid && Password == RepeatedPassword)
            {
                var user = (await DbInteractor.Users.FilterAsync(user => user.UserName == UserName)).SingleOrDefault();
                System.Threading.Tasks.Task registrationTask;
                if (user == null)
                {
                    var newUser = new User { UserName = UserName, Password = Password };
                    registrationTask = DbInteractor.Users.CreateAsync(newUser);
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, UserName),
                        new Claim("Password", Password)
                    };
                    var identity = new ClaimsIdentity(claims, "Cookies");
                    await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(identity));
                    await registrationTask;
                }
                else
                {
                    return Redirect("/Login");
                }
                if (Url.IsLocalUrl(returnRoute))
                    return Redirect(returnRoute);
                return RedirectToPage("_Host");
            }
            else
            {
                return Page();
            }
        }
    }
}