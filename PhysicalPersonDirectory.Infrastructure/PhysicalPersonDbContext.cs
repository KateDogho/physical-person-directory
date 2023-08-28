using Microsoft.EntityFrameworkCore;
using PhysicalPersonDirectory.Domain;

namespace PhysicalPersonDirectory.Infrastructure;

public class PhysicalPersonDbContext : DbContext
{
    public PhysicalPersonDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PhysicalPerson>()
            .ToTable("PhysicalPersons")
            .HasMany(pp => pp.RelatedPhysicalPersons)
            .WithOne(rp => rp.TargetPerson)
            .HasForeignKey(rp => rp.TargetPersonId)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<PhysicalPerson>()
            .HasOne(entity => entity.City)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<PhysicalPerson>()
            .HasMany(entity => entity.PhoneNumbers)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<City>().ToTable("Cities")
            .HasData(new()
            {
                Id = 1,
                Name = "Tbilisi"
            }, new()
            {
                Id = 2,
                Name = "Kutaisi"
            }, new()
            {
                Id = 3,
                Name = "Batumi"
            }, new()
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

        base.OnModelCreating(modelBuilder);
    }
}