﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PhysicalPersonDirectory.Infrastructure;

#nullable disable

namespace PhysicalPersonDirectory.Infrastructure.Migrations
{
    [DbContext(typeof(PhysicalPersonDbContext))]
    partial class PhysicalPersonDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PhysicalPersonDirectory.Domain.CityManagement.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Cities", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Tbilisi"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Kutaisi"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Batumi"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Other"
                        });
                });

            modelBuilder.Entity("PhysicalPersonDirectory.Domain.PhoneNumberManagement.PhoneNumber", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("PhysicalPersonId")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PhysicalPersonId");

                    b.ToTable("PhoneNumbers", (string)null);
                });

            modelBuilder.Entity("PhysicalPersonDirectory.Domain.PhysicalPersonManagement.PhysicalPerson", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CityId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("IdentificationNumber")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("PhysicalPersons", (string)null);
                });

            modelBuilder.Entity("PhysicalPersonDirectory.Domain.PhysicalPersonManagement.RelatedPhysicalPerson", b =>
                {
                    b.Property<int>("TargetPersonId")
                        .HasColumnType("int");

                    b.Property<int>("RelatedPersonId")
                        .HasColumnType("int");

                    b.Property<int>("RelationType")
                        .HasColumnType("int");

                    b.HasKey("TargetPersonId", "RelatedPersonId");

                    b.HasIndex("RelatedPersonId");

                    b.ToTable("RelatedPhysicalPersons", (string)null);
                });

            modelBuilder.Entity("PhysicalPersonDirectory.Domain.PhoneNumberManagement.PhoneNumber", b =>
                {
                    b.HasOne("PhysicalPersonDirectory.Domain.PhysicalPersonManagement.PhysicalPerson", null)
                        .WithMany("PhoneNumbers")
                        .HasForeignKey("PhysicalPersonId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PhysicalPersonDirectory.Domain.PhysicalPersonManagement.PhysicalPerson", b =>
                {
                    b.HasOne("PhysicalPersonDirectory.Domain.CityManagement.City", "City")
                        .WithMany()
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("City");
                });

            modelBuilder.Entity("PhysicalPersonDirectory.Domain.PhysicalPersonManagement.RelatedPhysicalPerson", b =>
                {
                    b.HasOne("PhysicalPersonDirectory.Domain.PhysicalPersonManagement.PhysicalPerson", "RelatedPerson")
                        .WithMany()
                        .HasForeignKey("RelatedPersonId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("PhysicalPersonDirectory.Domain.PhysicalPersonManagement.PhysicalPerson", "TargetPerson")
                        .WithMany()
                        .HasForeignKey("TargetPersonId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("RelatedPerson");

                    b.Navigation("TargetPerson");
                });

            modelBuilder.Entity("PhysicalPersonDirectory.Domain.PhysicalPersonManagement.PhysicalPerson", b =>
                {
                    b.Navigation("PhoneNumbers");
                });
#pragma warning restore 612, 618
        }
    }
}
