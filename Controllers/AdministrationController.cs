using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAttendance.Areas.Identity.Data;
using WebAttendance.Models.ViewModels;

namespace WebAttendance.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministrationController : Controller
    {
        private RoleManager<ApplicationUserRole> roleManger;
        private UserManager<ApplicationUser> userManager;
        public AdministrationController(RoleManager <ApplicationUserRole> _roleManager , UserManager<ApplicationUser> _userManager)
        {
            roleManger = _roleManager;
            userManager = _userManager;
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task <IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUserRole identityRole = new ApplicationUserRole
                {
                    Name = model.Name
                };
                IdentityResult result = await roleManger.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }

                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
        }

            
            return View(model);
        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = roleManger.Roles;
            return View(roles);
        }

        [HttpGet]
        public async Task< IActionResult> EditRole(string id)
        {
            
            var role = await roleManger.FindByIdAsync(id);
            var model = new EditRoleViewModel
            {
                Id = role.Id,
                Name = role.Name
            };

            var someCustomerList = userManager.Users.ToList();
            foreach (var user in someCustomerList)
            {
                
                if (await userManager.IsInRoleAsync(user , role.Name))
                {
                   model.Users.Add(user.UserName);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var stringedRoleId = model.Id.ToString();
            var role = await roleManger.FindByIdAsync(stringedRoleId);

            role.Name = model.Name;
            var result = await roleManger.UpdateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("ListRoles");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;
            //var stringedRoleId = Int32.Parse(roleId);
            var role = await roleManger.FindByIdAsync(roleId);
            var model = new List<UserRoleViewModel>();
            var someCustomerList = userManager.Users.ToList();
            foreach (var user in someCustomerList)
            {
                var userRoleViewModel = new UserRoleViewModel 
                { 
                    UserId = user.Id.ToString(),
                    UserName =user.UserName
                };


                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }
                model.Add(userRoleViewModel);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            //var stringedRoleId = roleId.ToString();
            var role = await roleManger.FindByIdAsync(roleId);
            var someCustomerList = userManager.Users.ToList();
            var someRolesList = roleManger.Roles.ToList();
            for (int i=0;i<model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);
                IdentityResult result = null;
                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }else if(!(model[i].IsSelected) && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }else
                {
                    continue;
                }
                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("EditRole", new { Id = roleId });
                }
            }
            return RedirectToAction("EditRole", new { Id = roleId });
            
        }


        [HttpGet]
        public async Task<IActionResult> DeleteRoleGet(string id)
        {
            var role = await roleManger.FindByIdAsync(id);
            var model = new EditRoleViewModel
            {
                Id = role.Id,
                Name = role.Name
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await roleManger.FindByIdAsync(id);

            if(role == null)
            {
                return RedirectToAction("ListRoles");
            } else
            {
                var result = await roleManger.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);

                    }
                    return View("ListRoles");
                }
            }
        }

        public IActionResult Index()
        {
            return View();
        }



    }
}
