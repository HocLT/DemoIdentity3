using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace DemoIdentty.Areas.Admin.Pages.Role
{
    public class DeleteModel : PageModel
    {
        readonly RoleManager<IdentityRole> roleManager;

        [TempData]
        public string? StatusMessage { set; get; }

        public class InputModel
        {
            [Required]
            public string? Id { set; get; }
            public string? Name { set; get; }
        }

        [BindProperty]
        public InputModel Input { set; get; }

        [BindProperty]
        public bool IsConfirmed { set; get; }

        public DeleteModel(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }
        public IActionResult OnGet() => NotFound("Not Found");

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return NotFound("Delete Error.");
            }

            var rr = await roleManager.FindByIdAsync(Input.Id);
            if (rr == null)
            {
                return NotFound("Role Not Found.");
            }

            ModelState.Clear();
            if (IsConfirmed)
            {
                // thực hiện xóa
                await roleManager.DeleteAsync(rr);
                StatusMessage = "Deleted role: " + rr.Name;
                return RedirectToPage("./Index");
            }
            else
            {
                Input.Name = rr.Name;
                IsConfirmed = true;
            }

            return Page();
        }
    }
}
