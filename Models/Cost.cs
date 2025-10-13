using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cow_Farm.Models
{
    public enum CostCategory { Feed, VeterinaryBills, Labor, Utilities }

    public class Cost
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime RecordDate { get; set; }

        [Required]
        public CostCategory CostCategory { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be a positive number")]
        public decimal Amount { get; set; }

        public string? Description { get; set; }
    }
}