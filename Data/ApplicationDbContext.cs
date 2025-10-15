using Cow_Farm.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cow_Farm.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        // DbSet for each of your models
        public DbSet<Cow> Cows { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }
        public DbSet<MilkProduction> MilkProductions { get; set; }
        public DbSet<MeatProduction> MeatProductions { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Cost> Costs { get; set; }
        public DbSet<Vaccination> Vaccinations { get; set; }
        public DbSet<VaccineType> VaccineTypes { get; set; }
    }
}