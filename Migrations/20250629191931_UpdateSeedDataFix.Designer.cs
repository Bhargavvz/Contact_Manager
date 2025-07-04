﻿// <auto-generated />
using System;
using ContactManager.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ContactManager.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250629191931_UpdateSeedDataFix")]
    partial class UpdateSeedDataFix
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.6");

            modelBuilder.Entity("ContactManager.Models.Contact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Contacts");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Address = "123 Main St, New York, NY 10001",
                            DateCreated = new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "john.doe@example.com",
                            Name = "John Doe",
                            Phone = "+1-555-0123"
                        },
                        new
                        {
                            Id = 2,
                            Address = "456 Oak Ave, Los Angeles, CA 90210",
                            DateCreated = new DateTime(2025, 1, 2, 11, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "jane.smith@example.com",
                            Name = "Jane Smith",
                            Phone = "+1-555-0456"
                        },
                        new
                        {
                            Id = 3,
                            Address = "789 Pine St, Chicago, IL 60601",
                            DateCreated = new DateTime(2025, 1, 3, 12, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "bob.johnson@example.com",
                            Name = "Bob Johnson",
                            Phone = "+1-555-0789"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
