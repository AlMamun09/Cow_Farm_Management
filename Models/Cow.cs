using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cow_Farm.Models
{
    public enum Gender { Male, Female }
    public enum Status { Healthy, Sick, Sold, Deceased, Pragnent }
    public class Cow
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Breed { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        public Gender Gender { get; set; }

        public Status Status { get; set; }

        public decimal Weight { get; set; }

    }
}
