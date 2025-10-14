using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SkillBridge.DataAccess.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblCompany> TblCompanies { get; set; }

    public virtual DbSet<TblIndividualPf> TblIndividualPfs { get; set; }

    public virtual DbSet<TblJobPost> TblJobPosts { get; set; }

    public virtual DbSet<TblMentor> TblMentors { get; set; }

    public virtual DbSet<TblProject> TblProjects { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=SkillBridgeDB;User Id=sa;Password=sasa@123;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblCompany>(entity =>
        {
            entity.HasKey(e => e.CompanyId).HasName("PK__Tbl_Comp__2D971CAC5DBF79BA");

            entity.ToTable("Tbl_Company");

            entity.Property(e => e.Address)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.BusWebUrl)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Bus_web_url");
            entity.Property(e => e.ComImg)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CompanyName)
                .HasMaxLength(75)
                .IsUnicode(false);
            entity.Property(e => e.DeleteFlag).HasDefaultValue(false);
        });

        modelBuilder.Entity<TblIndividualPf>(entity =>
        {
            entity.HasKey(e => e.IndividualId).HasName("PK__Tbl_Indi__2DA106D60C4D0062");

            entity.ToTable("Tbl_Individual_Pf");

            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.DeleteFlag).HasDefaultValue(false);
            entity.Property(e => e.Exp)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.IndividualImg)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.IndividualName)
                .HasMaxLength(75)
                .IsUnicode(false);
            entity.Property(e => e.InterestArea)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.IsStudent)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.MentorToken).HasColumnName("Mentor_Token");
            entity.Property(e => e.ProField)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("Pro_Field");
            entity.Property(e => e.ProjToken).HasColumnName("Proj_Token");
            entity.Property(e => e.RoleCode)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblJobPost>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PK__Tbl_JobP__AA126018305003EA");

            entity.ToTable("Tbl_JobPost");

            entity.Property(e => e.PostId)
                .HasMaxLength(70)
                .IsUnicode(false);
            entity.Property(e => e.DeleteFlag).HasDefaultValue(false);
            entity.Property(e => e.IsComplete).HasDefaultValue(false);
            entity.Property(e => e.JpImg)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PostDes)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblMentor>(entity =>
        {
            entity.HasKey(e => e.MentorId).HasName("PK__Tbl_Ment__053B7E9890FD05BF");

            entity.ToTable("Tbl_Mentor");

            entity.Property(e => e.DeleteFlag).HasDefaultValue(false);
            entity.Property(e => e.MentorImg)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.MentorName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.RoleCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SpecialField)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Special_Field");
        });

        modelBuilder.Entity<TblProject>(entity =>
        {
            entity.HasKey(e => e.ProjectId).HasName("PK__Tbl_Proj__761ABEF0C7A28E61");

            entity.ToTable("Tbl_Project");

            entity.Property(e => e.AvaPos).HasColumnName("Ava_Pos");
            entity.Property(e => e.DeleteFlag).HasDefaultValue(false);
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.IsComplete).HasDefaultValue(false);
            entity.Property(e => e.ProjImg)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ProjectDes)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ProjectName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ProjectSub)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ProjectTitle)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Tbl_User__1788CC4CE66AD9AD");

            entity.ToTable("Tbl_User");

            entity.Property(e => e.DeleteFlag).HasDefaultValue(false);
            entity.Property(e => e.Email)
                .HasMaxLength(75)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(75)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNo)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.RoleCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(75)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
