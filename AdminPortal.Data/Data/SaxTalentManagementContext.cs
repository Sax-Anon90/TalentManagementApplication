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
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserRole> UserRoles { get; set; } = null!;

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

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Rolename)
                    .HasMaxLength(250)
                    .HasColumnName("ROLENAME");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Email)
                    .HasMaxLength(250)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.EmployeeNo)
                    .HasMaxLength(250)
                    .HasColumnName("EMPLOYEE_NO");

                entity.Property(e => e.FirstName).HasMaxLength(250);

                entity.Property(e => e.Lastname)
                    .HasMaxLength(250)
                    .HasColumnName("LASTNAME");

                entity.Property(e => e.Password).HasColumnName("PASSWORD");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("USER_ROLES");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Roleid).HasColumnName("ROLEID");

                entity.Property(e => e.Userid).HasColumnName("USERID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.Roleid)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ROLES_ID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_USER_ID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
