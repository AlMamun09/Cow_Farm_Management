using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Cow_Farm.Models
{
    public class ApplicationUser: IdentityUser
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;
    }
}
