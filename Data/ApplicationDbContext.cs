using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ContactManager.Models;
using Microsoft.AspNetCore.Identity;

namespace ContactManager.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Contact entity
            modelBuilder.Entity<Contact>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Phone).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Address).HasMaxLength(500);
                entity.Property(e => e.ProfilePhotoPath).HasMaxLength(255);
                entity.Property(e => e.DateCreated).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.LastModified).HasDefaultValueSql("CURRENT_TIMESTAMP");
                
                // Configure relationship with User
                entity.HasOne(c => c.User)
                      .WithMany()
                      .HasForeignKey(c => c.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Create index for better performance
                entity.HasIndex(c => c.UserId);
                entity.HasIndex(c => new { c.UserId, c.IsFavorite });
                entity.HasIndex(c => new { c.UserId, c.Name });
            });
        }
    }
}
