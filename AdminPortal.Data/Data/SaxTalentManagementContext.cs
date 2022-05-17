using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AdminPortal.Data.Data
{
    public partial class SaxTalentManagementContext : DbContext
    {
        public SaxTalentManagementContext()
        {
        }

        public SaxTalentManagementContext(DbContextOptions<SaxTalentManagementContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ApplicationLog> ApplicationLogs { get; set; } = null!;
        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<CourseCategory> CourseCategories { get; set; } = null!;
        public virtual DbSet<CourseEnrollment> CourseEnrollments { get; set; } = null!;
        public virtual DbSet<CourseFileAttachment> CourseFileAttachments { get; set; } = null!;
        public virtual DbSet<Department> Departments { get; set; } = null!;
        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<EmployeeFileAttachment> EmployeeFileAttachments { get; set; } = null!;
        public virtual DbSet<Gender> Genders { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=(LocalDB)\\MSSQLLocalDB;Database=SaxTalentManagement;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationLog>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Course");

                entity.Property(e => e.CourseName).HasMaxLength(250);

                entity.HasOne(d => d.CourseCategory)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.CourseCategoryId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_CourseCategoryId");
            });

            modelBuilder.Entity<CourseCategory>(entity =>
            {
                entity.ToTable("CourseCategory");

                entity.Property(e => e.CategoryName).HasMaxLength(250);
            });

            modelBuilder.Entity<CourseEnrollment>(entity =>
            {
                entity.Property(e => e.Status).HasMaxLength(250);

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.CourseEnrollments)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_CourseEnrollmentId");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.CourseEnrollments)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_EmployeeEnrollmentId");
            });

            modelBuilder.Entity<CourseFileAttachment>(entity =>
            {
                entity.ToTable("CourseFileAttachment");

                entity.Property(e => e.FileName).HasMaxLength(250);

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.CourseFileAttachments)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_CourseId");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("Department");

                entity.Property(e => e.Department1)
                    .HasMaxLength(250)
                    .HasColumnName("Department");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.Department).HasMaxLength(250);

                entity.Property(e => e.EmployeeNo).HasMaxLength(250);

                entity.Property(e => e.FirstName).HasMaxLength(250);

                entity.Property(e => e.Gender).HasMaxLength(250);

                entity.Property(e => e.LastName).HasMaxLength(250);

                entity.Property(e => e.PositionTitle).HasMaxLength(250);
            });

            modelBuilder.Entity<EmployeeFileAttachment>(entity =>
            {
                entity.ToTable("EmployeeFileAttachment");

                entity.Property(e => e.FileName).HasMaxLength(250);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.EmployeeFileAttachments)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_EmployeeFileAttachmentId");
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.ToTable("Gender");

                entity.Property(e => e.Gender1)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Gender");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
