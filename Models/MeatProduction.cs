using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cow_Farm.Models
{
    public class MeatProduction
    {
        public int Id { get; set; }

        [Required]
        public int CowId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ProductionDate { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Weight must be a positive number")]
        public decimal WeightInKg { get; set; }


        [ForeignKey("CowId")]
        public virtual Cow? Cow { get; set; }
    }
}
