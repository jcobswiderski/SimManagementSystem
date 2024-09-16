using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SimManagementSystem.DataAccessLayer;

public partial class SimManagementSystemContext : DbContext
{
    public SimManagementSystemContext()
    {
    }

    public SimManagementSystemContext(DbContextOptions<SimManagementSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Device> Devices { get; set; }

    public virtual DbSet<Inspection> Inspections { get; set; }

    public virtual DbSet<InspectionType> InspectionTypes { get; set; }

    public virtual DbSet<Maintenance> Maintenances { get; set; }

    public virtual DbSet<MaintenanceType> MaintenanceTypes { get; set; }

    public virtual DbSet<Malfunction> Malfunctions { get; set; }

    public virtual DbSet<PredefinedSession> PredefinedSessions { get; set; }

    public virtual DbSet<RecoveryAction> RecoveryActions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SessionCategory> SessionCategories { get; set; }

    public virtual DbSet<SimulatorSession> SimulatorSessions { get; set; }

    public virtual DbSet<SimulatorState> SimulatorStates { get; set; }

    public virtual DbSet<Statistic> Statistics { get; set; }

    public virtual DbSet<TestQtg> TestQtgs { get; set; }

    public virtual DbSet<TestResult> TestResults { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SimManagementSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Polish_CI_AS");

        modelBuilder.Entity<Device>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Device_pk");

            entity.ToTable("Device");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.Tag).HasMaxLength(50);

            entity.HasMany(d => d.Malfunctions).WithMany(p => p.Devices)
                .UsingEntity<Dictionary<string, object>>(
                    "MalfunctionDevice",
                    r => r.HasOne<Malfunction>().WithMany()
                        .HasForeignKey("MalfunctionId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("MalfunctionDevice_Malfunction"),
                    l => l.HasOne<Device>().WithMany()
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("MalfunctionDevice_Device"),
                    j =>
                    {
                        j.HasKey("DeviceId", "MalfunctionId").HasName("MalfunctionDevice_pk");
                        j.ToTable("MalfunctionDevice");
                        j.IndexerProperty<int>("DeviceId").HasColumnName("Device_ID");
                        j.IndexerProperty<int>("MalfunctionId").HasColumnName("Malfunction_ID");
                    });
        });

        modelBuilder.Entity<Inspection>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Inspection_pk");

            entity.ToTable("Inspection");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.InspectionTypeId).HasColumnName("Inspection_Type_ID");
            entity.Property(e => e.Notice).HasMaxLength(800);

            entity.HasOne(d => d.InspectionType).WithMany(p => p.Inspections)
                .HasForeignKey(d => d.InspectionTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Inspection_Inspection_Type");

            entity.HasOne(d => d.OperatorNavigation).WithMany(p => p.Inspections)
                .HasForeignKey(d => d.Operator)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Inspection_User");
        });

        modelBuilder.Entity<InspectionType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Inspection_Type_pk");

            entity.ToTable("Inspection_Type");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(80)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Maintenance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Maintenance_pk");

            entity.ToTable("Maintenance");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Date).HasColumnType("datetime");

            entity.HasOne(d => d.ExecutorNavigation).WithMany(p => p.Maintenances)
                .HasForeignKey(d => d.Executor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Maintenance_User");

            entity.HasOne(d => d.TypeNavigation).WithMany(p => p.Maintenances)
                .HasForeignKey(d => d.Type)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Maintenance_Maintenance_Type");
        });

        modelBuilder.Entity<MaintenanceType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Maintenance_Type_pk");

            entity.ToTable("Maintenance_Type");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.Tasks)
                .HasMaxLength(2500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Malfunction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Malfunction_pk");

            entity.ToTable("Malfunction");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DateBegin)
                .HasColumnType("datetime")
                .HasColumnName("Date_Begin");
            entity.Property(e => e.DateEnd)
                .HasColumnType("datetime")
                .HasColumnName("Date_End");
            entity.Property(e => e.Description).HasMaxLength(2500);
            entity.Property(e => e.Name).HasMaxLength(180);
            entity.Property(e => e.UserHandler).HasColumnName("User_Handler");
            entity.Property(e => e.UserReporter).HasColumnName("User_Reporter");

            entity.HasOne(d => d.UserHandlerNavigation).WithMany(p => p.MalfunctionUserHandlerNavigations)
                .HasForeignKey(d => d.UserHandler)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Malfunction_Handler");

            entity.HasOne(d => d.UserReporterNavigation).WithMany(p => p.MalfunctionUserReporterNavigations)
                .HasForeignKey(d => d.UserReporter)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Malfunction_Reporter");
        });

        modelBuilder.Entity<PredefinedSession>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Predefined_Session_pk");

            entity.ToTable("Predefined_Session");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Abbreviation).HasMaxLength(15);
            entity.Property(e => e.Description).HasMaxLength(600);
            entity.Property(e => e.Name).HasMaxLength(240);

            entity.HasOne(d => d.CategoryNavigation).WithMany(p => p.PredefinedSessions)
                .HasForeignKey(d => d.Category)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Predefined_Session_SessionCategory");
        });

        modelBuilder.Entity<RecoveryAction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("RecoveryAction_pk");

            entity.ToTable("RecoveryAction");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(2500);
            entity.Property(e => e.MalfunctionId).HasColumnName("Malfunction_ID");

            entity.HasOne(d => d.Malfunction).WithMany(p => p.RecoveryActions)
                .HasForeignKey(d => d.MalfunctionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RecoveryAction_Malfunction");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Role_pk");

            entity.ToTable("Role");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(80)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SessionCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Session_Category_pk");

            entity.ToTable("Session_Category");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<SimulatorSession>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Simulator_Session_pk");

            entity.ToTable("Simulator_Session");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BeginDate).HasColumnType("datetime");
            entity.Property(e => e.CopilotSeat).HasColumnName("Copilot_Seat");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.ObserverSeat).HasColumnName("Observer_Seat");
            entity.Property(e => e.PilotSeat).HasColumnName("Pilot_Seat");
            entity.Property(e => e.PredefinedSession).HasColumnName("Predefined_Session");
            entity.Property(e => e.SupervisorSeat).HasColumnName("Supervisor_Seat");

            entity.HasOne(d => d.CopilotSeatNavigation).WithMany(p => p.SimulatorSessionCopilotSeatNavigations)
                .HasForeignKey(d => d.CopilotSeat)
                .HasConstraintName("SimulatorSession_Pilot");

            entity.HasOne(d => d.ObserverSeatNavigation).WithMany(p => p.SimulatorSessionObserverSeatNavigations)
                .HasForeignKey(d => d.ObserverSeat)
                .HasConstraintName("SimulatorSession_Copilot");

            entity.HasOne(d => d.PilotSeatNavigation).WithMany(p => p.SimulatorSessionPilotSeatNavigations)
                .HasForeignKey(d => d.PilotSeat)
                .HasConstraintName("SimulatorSession_Supervisor");

            entity.HasOne(d => d.PredefinedSessionNavigation).WithMany(p => p.SimulatorSessions)
                .HasForeignKey(d => d.PredefinedSession)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Simulator_Session_Predefined_Session");

            entity.HasOne(d => d.SupervisorSeatNavigation).WithMany(p => p.SimulatorSessionSupervisorSeatNavigations)
                .HasForeignKey(d => d.SupervisorSeat)
                .HasConstraintName("SimulatorSession_Observer");
        });

        modelBuilder.Entity<SimulatorState>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Simulator_State_pk");

            entity.ToTable("Simulator_State");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MeterState).HasColumnName("Meter_State");
            entity.Property(e => e.StartupTime)
                .HasColumnType("datetime")
                .HasColumnName("Startup_Time");

            entity.HasOne(d => d.OperatorNavigation).WithMany(p => p.SimulatorStates)
                .HasForeignKey(d => d.Operator)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Simulator_State_User");
        });

        modelBuilder.Entity<Statistic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Statistics_pk");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DateBegin)
                .HasColumnType("datetime")
                .HasColumnName("Date_Begin");
            entity.Property(e => e.DateEnd)
                .HasColumnType("datetime")
                .HasColumnName("Date_End");
            entity.Property(e => e.EfficiencyFactor).HasColumnName("Efficiency_Factor");
            entity.Property(e => e.MaintenancesCount).HasColumnName("Maintenances_Count");
            entity.Property(e => e.MalfunctionsCount).HasColumnName("Malfunctions_Count");
            entity.Property(e => e.OperatingTime).HasColumnName("Operating_Time");
            entity.Property(e => e.SessionsTime).HasColumnName("Sessions_Time");
        });

        modelBuilder.Entity<TestQtg>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Test_QTG_pk");

            entity.ToTable("Test_QTG");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description)
                .HasMaxLength(2500)
                .IsUnicode(false);
            entity.Property(e => e.Stage)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(80)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TestResult>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Test_Result_pk");

            entity.ToTable("Test_Result");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Observation)
                .HasMaxLength(1200)
                .IsUnicode(false);

            entity.HasOne(d => d.ExcutorNavigation).WithMany(p => p.TestResults)
                .HasForeignKey(d => d.Excutor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Test_Result_User");

            entity.HasOne(d => d.TestNavigation).WithMany(p => p.TestResults)
                .HasForeignKey(d => d.Test)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Test_Result_Test_QTG");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("User_pk");

            entity.ToTable("User");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FirstName)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.Login).HasMaxLength(64);
            entity.Property(e => e.Password).HasMaxLength(512);
            entity.Property(e => e.RefreshToken).HasMaxLength(512);
            entity.Property(e => e.RefreshTokenExp).HasColumnType("datetime");
            entity.Property(e => e.Salt).HasMaxLength(512);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("UserRole_Role"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("UserRole_User"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId").HasName("UserRole_pk");
                        j.ToTable("UserRole");
                        j.IndexerProperty<int>("UserId").HasColumnName("User_ID");
                        j.IndexerProperty<int>("RoleId").HasColumnName("Role_ID");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
