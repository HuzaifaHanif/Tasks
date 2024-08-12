using Microsoft.EntityFrameworkCore;
using Task8.Models;
using Task8.Models.Employees;
using Task8.Models.Kafka;
using Task8.Models.RabbitMq;
using Tasks.Models.AzureBusService;


namespace Task8.Data;

public partial class SoftechWorldWideContext : DbContext
{
    public SoftechWorldWideContext()
    {
    }

    public SoftechWorldWideContext(DbContextOptions<SoftechWorldWideContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Employee> Employees { get; set; }
    public DbSet<Kafka> Kafkas { get; set; } // New table
    public DbSet<RabbitMq> RabbitMQs { get; set; } // New table

    public DbSet<AzureBus> AzureBuses { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__C52E0BA8A3DB27C0");
            entity.Property(e => e.Id).HasColumnName("employee_id");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("first_name");
            entity.Property(e => e.HireDate)
                .HasColumnType("date")
                .HasColumnName("hire_date"); 
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("last_name");
            entity.Property(e => e.Salary)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("salary");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
