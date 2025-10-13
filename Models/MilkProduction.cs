using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cow_Farm.Models
{
    public enum MilkingTime { Morning, Evening }
    public class MilkProduction
    {
        public int Id { get; set; }

        [Required]
        public int CowId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ReocordDate { get; set; }

        [Required]
        public MilkingTime MilkingTime { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be a positive number")]
        public decimal QuantityLiters { get; set; }

        [ForeignKey("CowId")]
        public virtual Cow? Cow { get; set; }
    }
}
