using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cow_Farm.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class VaccineType
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(100)]
        [Display(Name = "Veccine Manufacturer Company")]
        public string? VaccineManufacturer { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be a positive number")]
        public decimal Price { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }
    }
}