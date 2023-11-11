using DemoIdentty.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DemoIdentty.Areas.Admin.Pages.Role
{
    public class AddUserRoleModel : PageModel
    {
        readonly RoleManager<IdentityRole> roleManager;
        readonly UserManager<AppUser> userManager;

        [TempData]
        public string? StatusMessage { set; get; }

        public class InputModel
        {
            [Required]
            public string? Id { get; set; }
            public string? Name { get; set; }
            public string[]? RoleNames { get; set; }
        }

        [BindProperty]
        public InputModel Input { set; get; }

        [BindProperty]
        public bool IsConfirm { set; get; }

        public AddUserRoleModel(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public SelectList AllRoles { set; get; }

        public IActionResult OnGet() => NotFound("Page Not Found.");

        public async Task<IActionResult> OnPost()
        {
            var user = await userManager.FindByIdAsync(Input.Id);
            if (user == null)
            {
                return NotFound("User Not Found.");
            }
            Input.Name = user.UserName;

            var userRs = await userManager.GetRolesAsync(user);

            var allRs = await roleManager.Roles.ToListAsync();
            var allRoleNames = allRs.Select(x => x.Name).ToList();
            AllRoles = new SelectList(allRoleNames);

            if (!IsConfirm)
            {
                IsConfirm = true;
                Input.RoleNames = userRs.ToArray();
                StatusMessage = "";
                ModelState.Clear();
            }
            else
            {
                StatusMessage = "User updated";
                
                if (Input.RoleNames == null)
                {
                    Input.RoleNames = new string[] { };
                }

                // thêm role mới
                foreach(var item in Input.RoleNames)
                {
                    if (!userRs.Contains(item)) 
                    {
                        await userManager.AddToRoleAsync(user, item);
                    }
                }

                // xóa role cũ, role không được chọn trong Input.RoleNames, nhưng có trong userRs
                foreach (var item in userRs)
                {
                    if (!Input.RoleNames.Contains(item))
                    {
                        await userManager.RemoveFromRoleAsync(user, item);
                    }
                }
            }

            return Page();
        }
    }
}
