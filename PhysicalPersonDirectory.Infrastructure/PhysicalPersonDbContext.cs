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
        modelBuilder.Entity<PhysicalPerson>().ToTable("PhysicalPersons");

        modelBuilder.Entity<City>().ToTable("Cities");
        
        modelBuilder.Entity<PhoneNumber>().ToTable("PhoneNumbers");
        
        modelBuilder.Entity<RelatedPhysicalPerson>()
            .ToTable("RelatedPhysicalPerson")
            .HasKey(rrp=>new { SourcePersonId = rrp.TargetPersonId, rrp.RelatedPersonId});
        
        modelBuilder.Entity<RelatedPhysicalPerson>()
            .HasOne(rpp => rpp.TargetPerson)
            .WithMany(pp => pp.RelatedPhysicalPersons)
            .HasForeignKey(rpp => rpp.TargetPersonId)
            .OnDelete(DeleteBehavior.ClientCascade);

        modelBuilder.Entity<RelatedPhysicalPerson>()
            .HasOne(rpp => rpp.TargetPerson)
            .WithMany()
            .HasForeignKey(rpp => rpp.TargetPersonId)
            .OnDelete(DeleteBehavior.ClientCascade);
        
        base.OnModelCreating(modelBuilder);
    }
}