using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace DemoIdentty.Areas.Admin.Pages.Role
{
    public class AddModel : PageModel
    {
        readonly RoleManager<IdentityRole> roleManager;

        [TempData]
        public string? StatusMessage { set; get; }


        public class InputModel
        {
            [Display(Name = "Role Id")]
            public string? Id { set; get; }

            [Required(ErrorMessage = "Please input the Role name")]
            [Display(Name = "Role Name")]
            [StringLength(100, ErrorMessage ="{0} from {2} to{1} characters", MinimumLength = 3)]
            public string? Name { set; get; }
        }

        [BindProperty]
        public InputModel Input { set; get; }
        
        [BindProperty]
        public bool IsUpdate { set; get; }

        // Disable default page route access
        public IActionResult OnGet() => NotFound("Not Found");
        public IActionResult OnPost() => NotFound("Not Found");

        public AddModel(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        public IActionResult OnPostStartNewRole()
        {
            StatusMessage = "Input Role Information to create new Role";
            IsUpdate = false;
            ModelState.Clear(); // xóa model cũ
            return Page();
        }

        public async Task<IActionResult> OnPostStartUpdate()
        {
            StatusMessage = null;
            IsUpdate = true;
            if (Input.Id == null)
            {
                StatusMessage = "Error: Role invalid.";
                return Page();
            }

            var rr = await roleManager.FindByIdAsync(Input.Id);
            if (rr != null) 
            {
                Input.Name = rr.Name;
                ModelState.Clear(); // xóa model cũ
            }
            else
            {
                StatusMessage = $"Error: Role {Input.Id} not found.";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAddOrUpdate()
        {
            if (!ModelState.IsValid)
            {
                StatusMessage = null;
                return Page();
            }

            if (IsUpdate)
            {
                // cập nhật
                if (Input.Id == null)
                {
                    StatusMessage = "Error: Role invalid.";
                    return Page();
                }

                var rr = await roleManager.FindByIdAsync(Input.Id);
                if (rr != null)
                {
                    rr.Name = Input.Name;

                    var result = await roleManager.UpdateAsync(rr);
                    if (result.Succeeded)
                    {
                        StatusMessage = $"Update Role {Input.Id} Successfully";
                    }
                    else
                    {
                        StatusMessage = "Error: ";
                        foreach (var item in result.Errors)
                        {
                            StatusMessage += item.Description;
                        }
                    }
                }
                else
                {
                    StatusMessage = $"Error: Role {Input.Id} not found.";
                }
            }
            else
            {
                var rr = new IdentityRole(Input.Name);
                var result = await roleManager.CreateAsync(rr);
                if (result.Succeeded)
                {
                    StatusMessage = $"Create new role: {Input.Name} successfully";
                    return RedirectToPage("./Index");
                }
                else // báo lỗi
                {
                    StatusMessage = "Error: ";
                    foreach (var item in result.Errors)
                    {
                        StatusMessage += item.Description;
                    }
                }
            }
            return Page();
        }
    }
}
