using HealthCare.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthCare.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Patient> Patients { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Medication> Medications { get; set; }
    public DbSet<Provider> Providers { get; set; }
    public DbSet<Record> Records { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Patient)
            .WithMany()  
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Provider)
            .WithMany()
            .HasForeignKey(a => a.ProviderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AppointmentMedication>()  
            .HasKey(am => new { am.AppointmentId, am.MedicationId });

        modelBuilder.Entity<AppointmentMedication>()
            .HasOne(am => am.Appointment)
            .WithMany(a => a.Medications)
            .HasForeignKey(am => am.AppointmentId);

        modelBuilder.Entity<AppointmentMedication>()
            .HasOne(am => am.Medication)
            .WithMany()
            .HasForeignKey(am => am.MedicationId);
        
        modelBuilder.Entity<Record>()
            .HasOne(r => r.Patient)
            .WithMany()
            .HasForeignKey(r => r.PatientId)
            .HasPrincipalKey(p => p.Id);
    }
}