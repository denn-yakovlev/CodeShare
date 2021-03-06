﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Threading.Tasks;
using CodeShare.Services.DatabaseInteractor;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeShare.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        [Required]
        [MinLength(3, ErrorMessage = "Password should have at least 3 characters")]
        public string UserName { get; set; }

        [BindProperty]
        [Required]
        [MinLength(8, ErrorMessage = "Password should have at least 8 characters")]
        public string Password { get; set; }

        public IDatabaseInteractor DbInteractor { get; }

        public LoginModel(IDatabaseInteractor dbInteractor)
        {
            DbInteractor = dbInteractor;
        }

        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnPostAsync(string returnRoute)
        {
            if (ModelState.IsValid)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, UserName),
                    new Claim("Password", Password)
                };
                var identity = new ClaimsIdentity(claims, "Cookies");
                var user = DbInteractor.Users.Filter(user => user.UserName == UserName).SingleOrDefault();
                if (user != null)
                    await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(identity));
                else
                    return Redirect("/error");
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