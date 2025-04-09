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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=DESKTOP-S1LVDJS\\SQLEXPRESS;Database=ComputerTrackerDB;Trusted_Connection=True;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Login)
            .IsUnique();

        modelBuilder.Entity<Computer>()
            .HasIndex(c => c.IPAddress)
            .IsUnique();

        modelBuilder.Entity<UsageSession>()
            .HasOne(us => us.Employee)
            .WithMany(e => e.UsageSessions)
            .HasForeignKey(us => us.EmployeeID);

        modelBuilder.Entity<UsageSession>()
            .HasOne(us => us.Computer)
            .WithMany(c => c.UsageSessions)
            .HasForeignKey(us => us.ComputerID);

        modelBuilder.Entity<SoftwareUsage>()
            .HasOne(su => su.Session)
            .WithMany(us => us.SoftwareUsages)
            .HasForeignKey(su => su.SessionID);

        modelBuilder.Entity<SoftwareUsage>()
            .HasOne(su => su.Software)
            .WithMany(s => s.SoftwareUsages)
            .HasForeignKey(su => su.SoftwareID);

        modelBuilder.Entity<UsageSession>()
        .ToTable(tb => tb.HasTrigger("trg_CalcSessionDuration"));
    }
}
