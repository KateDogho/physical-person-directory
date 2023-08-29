using Microsoft.EntityFrameworkCore;
using PhysicalPersonDirectory.Domain.CityManagement;
using PhysicalPersonDirectory.Domain.PhoneNumberManagement;
using PhysicalPersonDirectory.Domain.PhysicalPersonManagement;

namespace PhysicalPersonDirectory.Infrastructure;

public class PhysicalPersonDbContext : DbContext
{
    public PhysicalPersonDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PhysicalPerson>()
            .ToTable("PhysicalPersons");
        modelBuilder.Entity<PhysicalPerson>()
            .HasOne(entity => entity.City)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<PhysicalPerson>()
            .HasMany(entity => entity.PhoneNumbers)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<City>().ToTable("Cities")
            .HasData(new City
            {
                Id = 1,
                Name = "Tbilisi"
            }, new City
            {
                Id = 2,
                Name = "Kutaisi"
            }, new City
            {
                Id = 3,
                Name = "Batumi"
            }, new City
            {
                Id = 4,
                Name = "Other"
            });

        modelBuilder.Entity<PhoneNumber>().ToTable("PhoneNumbers");

        modelBuilder.Entity<RelatedPhysicalPerson>()
            .ToTable("RelatedPhysicalPersons")
            .HasKey(rrp => new { SourcePersonId = rrp.TargetPersonId, rrp.RelatedPersonId });

        modelBuilder.Entity<RelatedPhysicalPerson>()
            .HasOne(rpp => rpp.RelatedPerson)
            .WithMany()
            .HasForeignKey(rpp => rpp.RelatedPersonId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<RelatedPhysicalPerson>()
            .HasOne(rpp => rpp.TargetPerson)
            .WithMany()
            .HasForeignKey(rpp => rpp.TargetPersonId)
            .OnDelete(DeleteBehavior.NoAction);

        base.OnModelCreating(modelBuilder);
    }
}