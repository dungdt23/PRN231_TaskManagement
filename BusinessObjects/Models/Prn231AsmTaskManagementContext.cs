using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BusinessObjects.Models;

public partial class Prn231AsmTaskManagementContext : DbContext
{
    public Prn231AsmTaskManagementContext()
    {
    }

    public Prn231AsmTaskManagementContext(DbContextOptions<Prn231AsmTaskManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<TaskAssign> TaskAssigns { get; set; }

    public virtual DbSet<TaskComment> TaskComments { get; set; }

    public virtual DbSet<TaskLog> TaskLogs { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<TeamLeader> TeamLeaders { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<UserTeam> UserTeams { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=DungDT; database = PRN231_ASM_TaskManagement;uid=sa;pwd=123; Trusted_Connection=SSPI;Encrypt=false;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");

            entity.Property(e => e.Categoryname).HasMaxLength(50);
            entity.Property(e => e.Des).HasMaxLength(50);
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("Member");

            entity.Property(e => e.Address).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Firstname).HasMaxLength(50);
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.Lastname).HasMaxLength(50);
            entity.Property(e => e.Middlename).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.ToTable("Project");

            entity.Property(e => e.Des).HasMaxLength(50);
            entity.Property(e => e.Projectname).HasMaxLength(50);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.Des).HasMaxLength(50);
            entity.Property(e => e.Rolename).HasMaxLength(50);
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.ToTable("Task");

            entity.Property(e => e.Des).HasMaxLength(50);
            entity.Property(e => e.Taskname).HasMaxLength(50);

            entity.HasOne(d => d.Category).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Task_Category");

            entity.HasOne(d => d.Team).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Task_Team");
        });

        modelBuilder.Entity<TaskAssign>(entity =>
        {
            entity.ToTable("TaskAssign");

            entity.Property(e => e.AssignDate).HasColumnType("datetime");
            entity.Property(e => e.Des).HasMaxLength(50);
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("datetime");

            entity.HasOne(d => d.Task).WithMany(p => p.TaskAssigns)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskAssign_Task");

            entity.HasOne(d => d.User).WithMany(p => p.TaskAssigns)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskAssign_Member");
        });

        modelBuilder.Entity<TaskComment>(entity =>
        {
            entity.HasKey(e => e.CommentId);

            entity.ToTable("TaskComment");

            entity.Property(e => e.CommentContent).HasMaxLength(250);
            entity.Property(e => e.Date).HasColumnType("datetime");

            entity.HasOne(d => d.Reply).WithMany(p => p.TaskCommentReplies)
                .HasForeignKey(d => d.ReplyId)
                .HasConstraintName("FK_TaskComment_Member1");

            entity.HasOne(d => d.Task).WithMany(p => p.TaskComments)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskComment_Task");

            entity.HasOne(d => d.User).WithMany(p => p.TaskCommentUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskComment_Member");
        });

        modelBuilder.Entity<TaskLog>(entity =>
        {
            entity.ToTable("TaskLog");

            entity.Property(e => e.Des).HasMaxLength(50);

            entity.HasOne(d => d.TaskAssign).WithMany(p => p.TaskLogs)
                .HasForeignKey(d => d.TaskAssignId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskLog_TaskAssign");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.ToTable("Team");

            entity.Property(e => e.Des).HasMaxLength(50);
            entity.Property(e => e.Teamname).HasMaxLength(50);

            entity.HasOne(d => d.Project).WithMany(p => p.Teams)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Team_Project");
        });

        modelBuilder.Entity<TeamLeader>(entity =>
        {
            entity.ToTable("TeamLeader");

            entity.HasOne(d => d.Team).WithMany(p => p.TeamLeaders)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TeamLeader_Team");

            entity.HasOne(d => d.User).WithMany(p => p.TeamLeaders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TeamLeader_Member");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.ToTable("UserRole");

            entity.Property(e => e.Des).HasMaxLength(50);

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRole_Role");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRole_Member");
        });

        modelBuilder.Entity<UserTeam>(entity =>
        {
            entity.ToTable("UserTeam");

            entity.Property(e => e.Des).HasMaxLength(50);
            entity.Property(e => e.Role).HasMaxLength(20);

            entity.HasOne(d => d.Team).WithMany(p => p.UserTeams)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserTeam_Team");

            entity.HasOne(d => d.User).WithMany(p => p.UserTeams)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserTeam_Member");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
