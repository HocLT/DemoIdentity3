using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DemoIdentty.Areas.Admin.Pages.Role
{
    [Authorize("CanView")]
    public class IndexModel : PageModel
    {
        readonly RoleManager<IdentityRole> roleManager;

        [TempData]
        public string StatusMessage { set; get; }

        public IndexModel(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        public List<IdentityRole>? Roles { get; set; } = default;

        public async Task<IActionResult> OnGetAsync()
        {
            Roles = await roleManager.Roles.ToListAsync();
            return Page();
        }
    }
}
