using DemoIdentty.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DemoIdentty.Areas.Admin.Pages.Role
{
    public class UserModel : PageModel
    {
        readonly RoleManager<IdentityRole> roleManager;
        readonly UserManager<AppUser> userManager;

        [TempData]
        public string StatusMessage { set; get; }

        public UserModel(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public class UserAndRole: AppUser
        {
            public string? ListRoles { set; get; }
        }

        public List<UserAndRole> Users { set; get; }

        public IActionResult OnPost() => NotFound("Not Found");

        public async Task<IActionResult> OnGet()
        {
            Users = await userManager.Users
                .Select(u=>new UserAndRole { Id = u.Id, UserName = u.UserName})
                .ToListAsync();
            
            foreach(var user in Users)
            {
                var roles = await userManager.GetRolesAsync(user);
                user.ListRoles = string.Join(",", roles);
            }

            return Page();
        }
    }
}
