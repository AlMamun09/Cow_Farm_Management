using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cow_Farm.Models
{
    public class Vaccination
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Cow")]
        public int CowId { get; set; }
        public virtual Cow? Cow { get; set; }

        [Required]
        [Display(Name = "Vaccine Name")]
        public int VaccineTypeId { get; set; }
        public virtual VaccineType? VaccineType { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date Given")]
        public DateTime DateGiven { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Next Due Date")]
        public DateTime? NextDueDate { get; set; }
    }
}