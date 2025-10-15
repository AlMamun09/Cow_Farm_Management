using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cow_Farm.Models
{
    public enum  HealthEventType { Treatment, GeneralCheckUp }
    public class HealthRecord
    {
        public int Id { get; set; }

        [Required]
        public int CowId { get; set; }

        [Required]
        public HealthEventType EventType { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime RecordDate { get; set; }

        [Required]
        public string? Description { get; set; }

        public string? Veterinarian { get; set; }

        [ForeignKey("CowId")]
        public virtual Cow? Cow { get; set; }

    }
}
