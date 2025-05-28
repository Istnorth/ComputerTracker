using ComputerTracker.Data.DbModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;

internal class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Computer> Computers { get; set; }
    public DbSet<UsageSession> UsageSessions { get; set; }
    public DbSet<Software> Softwares { get; set; }
    public DbSet<SoftwareUsage> SoftwareUsages { get; set; }
    public DbSet<ComputerSystemData> ComputerSystemDatas { get; set; }
    public DbSet<ComputerGpu> Gpus { get; set; }
    public DbSet<Keyboard> Keyboards { get; set; }
    public DbSet<Mouse> Mice { get; set; }
    public DbSet<Printer> Printers { get; set; }
    public DbSet<Scanner> Scanners { get; set; }
    public DbSet<ComputerTracker.Data.DbModel.Monitor> Monitors { get; set; }
    public DbSet<KeyLogEntry> KeyLogEntries { get; set; }
    public DbSet<AppUsageEntry> AppUsageEntries { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=DESKTOP-S1LVDJS\\SQLEXPRESS;Database=CTDB;Trusted_Connection=True;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ── User ─────────────────────────────────────────────────────────────────────
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Login)
            .IsUnique();

        // ── Department / Employee ────────────────────────────────────────────────────
        modelBuilder.Entity<Employee>()
            .HasOne(e => e.Department)
            .WithMany(d => d.Employees)
            .HasForeignKey(e => e.DepartmentID)
            .OnDelete(DeleteBehavior.Restrict);

        // ── Computer ─────────────────────────────────────────────────────────────────
        modelBuilder.Entity<Computer>()
            .Property(c => c.Host)
            .HasColumnType("varchar(100)")
            .IsRequired();
        modelBuilder.Entity<Computer>()
            .Property(c => c.Port)
            .IsRequired();
        modelBuilder.Entity<Computer>()
            .HasIndex(c => c.IPAddress)
            .IsUnique();
        modelBuilder.Entity<Computer>()
            .HasIndex(c => new { c.Host, c.Port })
            .IsUnique();

        // ── UsageSession ─────────────────────────────────────────────────────────────
        modelBuilder.Entity<UsageSession>()
            .HasOne(us => us.Employee)
            .WithMany(e => e.UsageSessions)
            .HasForeignKey(us => us.EmployeeID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UsageSession>()
            .HasOne(us => us.Computer)
            .WithMany(c => c.UsageSessions)
            .HasForeignKey(us => us.ComputerID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UsageSession>()
            .ToTable(tb => tb.HasTrigger("trg_CalcSessionDuration"));

        // ── SoftwareUsage ────────────────────────────────────────────────────────────
        modelBuilder.Entity<SoftwareUsage>()
            .HasOne(su => su.Session)
            .WithMany(us => us.SoftwareUsages)
            .HasForeignKey(su => su.SessionID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SoftwareUsage>()
            .HasOne(su => su.Software)
            .WithMany(s => s.SoftwareUsages)
            .HasForeignKey(su => su.SoftwareID)
            .OnDelete(DeleteBehavior.Cascade);

        // ── ComputerSystemData ───────────────────────────────────────────────────────
        modelBuilder.Entity<ComputerSystemData>(b =>
        {
            b.HasKey(sd => sd.SystemDataID);

            b.Property(sd => sd.Timestamp)
             .HasColumnType("datetime2")
             .IsRequired();

            // OS
            b.Property(sd => sd.OSVersion)
             .HasColumnType("varchar(150)")
             .IsRequired(false);
            b.Property(sd => sd.OSCaption)
             .HasColumnType("varchar(200)")
             .IsRequired(false);
            b.Property(sd => sd.OSManufacturer)
             .HasColumnType("varchar(100)")
             .IsRequired(false);
            b.Property(sd => sd.WindowsDirectory)
             .HasColumnType("varchar(200)")
             .IsRequired(false);

            // CPU
            b.Property(sd => sd.CPUName)
             .HasColumnType("varchar(200)")
             .IsRequired(false);
            b.Property(sd => sd.CpuCores);
            b.Property(sd => sd.CpuThreads);
            b.Property(sd => sd.CpuClockMHz);

            // связь с Computer
            b.HasOne(sd => sd.Computer)
             .WithMany(c => c.SystemData)
             .HasForeignKey(sd => sd.ComputerID)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── ComputerGpu ──────────────────────────────────────────────────────────────
        modelBuilder.Entity<ComputerGpu>()
            .HasKey(g => g.ComputerGpuID);
        modelBuilder.Entity<ComputerGpu>()
            .Property(g => g.Name).HasColumnType("varchar(200)").IsRequired();
        modelBuilder.Entity<ComputerGpu>()
            .HasOne(g => g.Computer)
            .WithMany(c => c.Gpus)
            .HasForeignKey(g => g.ComputerId)
            .OnDelete(DeleteBehavior.Cascade);

        // ── Keyboard ─────────────────────────────────────────────────────────────────
        modelBuilder.Entity<Keyboard>()
            .HasKey(k => k.KeyboardID);
        modelBuilder.Entity<Keyboard>()
            .Property(k => k.Name).HasColumnType("varchar(200)").IsRequired();
        modelBuilder.Entity<Keyboard>()
            .HasOne(k => k.Computer)
            .WithMany(c => c.Keyboards)
            .HasForeignKey(k => k.ComputerId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Keyboard>()
            .Property(d => d.Description)
            .IsRequired(false);

        // ── Mouse ────────────────────────────────────────────────────────────────────
        modelBuilder.Entity<Mouse>()
            .HasKey(m => m.MouseID);
        modelBuilder.Entity<Mouse>()
            .Property(m => m.Name).HasColumnType("varchar(200)").IsRequired();
        modelBuilder.Entity<Mouse>()
            .HasOne(m => m.Computer)
            .WithMany(c => c.Mice)
            .HasForeignKey(m => m.ComputerId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Mouse>()
            .Property(d => d.Description)
            .IsRequired(false);

        // ── Printer ──────────────────────────────────────────────────────────────────
        modelBuilder.Entity<Printer>()
            .HasKey(p => p.PrinterID);
        modelBuilder.Entity<Printer>()
            .Property(p => p.Name).HasColumnType("varchar(200)").IsRequired();
        modelBuilder.Entity<Printer>()
            .HasOne(p => p.Computer)
            .WithMany(c => c.Printers)
            .HasForeignKey(p => p.ComputerId)
            .OnDelete(DeleteBehavior.Cascade);

        // ── Scanner ──────────────────────────────────────────────────────────────────
        modelBuilder.Entity<Scanner>()
            .HasKey(s => s.ScannerID);
        modelBuilder.Entity<Scanner>()
            .Property(s => s.Name).HasColumnType("varchar(200)").IsRequired();
        modelBuilder.Entity<Scanner>()
            .HasOne(s => s.Computer)
            .WithMany(c => c.Scanners)
            .HasForeignKey(s => s.ComputerId)
            .OnDelete(DeleteBehavior.Cascade);

        // ── Monitor ──────────────────────────────────────────────────────────────────
        modelBuilder.Entity<ComputerTracker.Data.DbModel.Monitor>()
            .HasKey(m => m.MonitorID);
        modelBuilder.Entity<ComputerTracker.Data.DbModel.Monitor>()
            .Property(m => m.Name).HasColumnType("varchar(200)").IsRequired();
        modelBuilder.Entity<ComputerTracker.Data.DbModel.Monitor>()
            .HasOne(m => m.Computer)
            .WithMany(c => c.Monitors)
            .HasForeignKey(m => m.ComputerId)
            .OnDelete(DeleteBehavior.Cascade);

        // ── KeyLogEntry ──────────────────────────────────────────────────────────────
        modelBuilder.Entity<KeyLogEntry>()
            .HasKey(k => k.KeyLogEntryID);
        modelBuilder.Entity<KeyLogEntry>()
            .Property(k => k.Key).HasColumnType("varchar(100)").IsRequired();
        modelBuilder.Entity<KeyLogEntry>()
            .HasOne(k => k.Computer)
            .WithMany(c => c.KeyLogEntries)
            .HasForeignKey(k => k.ComputerId)
            .OnDelete(DeleteBehavior.Cascade);

        // ── AppUsageEntry ────────────────────────────────────────────────────────────
        modelBuilder.Entity<AppUsageEntry>()
            .HasKey(a => a.AppUsageEntryID);
        modelBuilder.Entity<AppUsageEntry>()
            .Property(a => a.WindowTitle).HasColumnType("varchar(200)").IsRequired();
        modelBuilder.Entity<AppUsageEntry>()
            .HasOne(a => a.Computer)
            .WithMany(c => c.AppUsageEntries)
            .HasForeignKey(a => a.ComputerId)
            .OnDelete(DeleteBehavior.Cascade);
    }

}
