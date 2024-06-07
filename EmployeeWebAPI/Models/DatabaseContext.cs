using Intuit.Ipp.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeeWebAPI.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public  DbSet<EmployeeClass> Employee { get; set; }
        public  DbSet<UserClass> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserClass>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("User");
                entity.Property(e => e.UserId).HasColumnName("UserId");
                entity.Property(e => e.UserName).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.Email).HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.Password).HasMaxLength(50).IsUnicode(false);
                
            });

            modelBuilder.Entity<EmployeeClass>(entity =>
            {
                entity.ToTable("Emp");
                //entity.Property(e => e.EmployeeId).HasColumnName("EmployeeId");
                entity.Property(e => e.EmployeeName).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.BirthDate).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.Gender).HasMaxLength(20).IsUnicode(false);
                entity.Property(e => e.Email).HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.Password).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.Image).HasMaxLength(100).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        private void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {

        }
    }
}