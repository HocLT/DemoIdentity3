using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DemoIdentty.Data
{
    public class AppUser: IdentityUser
    {
        [PersonalData]
        [StringLength(100)]
        public string? Name { get; set; }
        [PersonalData]
        public DateTime? Birthday { get; set; }
    }
}
