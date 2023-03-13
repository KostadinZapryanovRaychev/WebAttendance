using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebAttendance.Areas.Identity.Data;
using WebAttendance.Data;
using WebAttendance.Models;
using WebAttendance.Models.ViewModels;

namespace WebAttendance.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        private readonly UserManager<ApplicationUser> userManager;
        public AdminController(UserManager<ApplicationUser> usrMgr)
        {
            userManager = usrMgr;
        }
        public IActionResult Index()
        {
            return View(userManager.Users);
        }


        public async Task<IActionResult> Update(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user != null)
                return View(user);
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, string email, string officialname, string Description)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                if (!string.IsNullOrEmpty(email))
                    user.Email = email;
                else
                    ModelState.AddModelError("", "Email cannot be empty");

                if (!string.IsNullOrEmpty(officialname))
                    user.OfficialName = officialname;
                else
                    ModelState.AddModelError("", "Cannot be Empty");


                if (!string.IsNullOrEmpty(Description))
                    user.Description = Description;
                else
                    ModelState.AddModelError("", "Not Empty");

                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(officialname))
                {
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return RedirectToAction("Index");
                    else
                        Errors(result);
                }
                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(Description))
                {
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return RedirectToAction("Index");
                    else
                        Errors(result);
                }
            }
            else
                ModelState.AddModelError("", "User Not Found");
            return View(user);
        }

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    Errors(result);
            }
            else
                ModelState.AddModelError("", "User Not Found");
            return View("Index", userManager.Users);
        }

    }
}
