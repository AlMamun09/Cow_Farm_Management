using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Cow_Farm.Models
{
    public enum Gender { Male, Female }
    public enum CowStatus { Milking, Dry, Calf, Bull, Sold }
    public class Cow
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tag Number")]
        public string TagNumber { get; set; } = string.Empty;

        [Required]
        public string? Name { get; set; }

        public string Breed { get; set; } = string.Empty;

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        public CowStatus Status { get; set; }

        [Display(Name = "Mother (Dam)")]
        public int? DamId { get; set; }

        [Display(Name = "Father (Sire)")]
        public int? SireId { get; set; }

        [ForeignKey("DamId")]
        public virtual Cow? Dam { get; set; }

        [ForeignKey("SireId")]
        public virtual Cow? Sire { get; set; }

        // Navigation properties for related records
        public virtual ICollection<MilkProduction> MilkProductions { get; set; } = new List<MilkProduction>();
        public virtual ICollection<HealthRecord> HealthRecords { get; set; } = new List<HealthRecord>();
        public virtual ICollection<Vaccination> Vaccinations { get; set; } = new List<Vaccination>();

    }
}
