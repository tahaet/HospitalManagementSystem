using Hospital.Models;
using Hospital.Utility;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hospital.DataAccess.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Floor> Floors { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<DoctorDetails> DoctorDetails { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Vaccine> Vaccines { get; set; }
        public DbSet<ConsultCategory> ConsultCategories { get; set; }
        public DbSet<Treatment> Treatments { get; set; }
        public DbSet<TestCategory> TestCategories { get; set; }
        public DbSet<TestResult> TestResults { get; set; }
        public DbSet<Consultation> Consultations { get; set; }
        public DbSet<VaccineAppointment> VaccineAppointments { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<DoctorDetails>().Property(x => x.Gender).HasDefaultValue(SD.Gender.Male);
            modelBuilder.Entity<DoctorDetails>().Property(x => x.Duty).HasDefaultValue(SD.DayTime.Morning);
            modelBuilder.Entity<DoctorDetails>().Property(x => x.Days).HasDefaultValue(SD.Days.Monday);
            modelBuilder.Entity<PatientDetails>().Property(x => x.Gender).HasDefaultValue(SD.Gender.Male);
            modelBuilder.Entity<PatientDetails>().Property(x => x.StatusPatient).HasDefaultValue(SD.StatusPatient.Cured);
            modelBuilder.Entity<PatientDetails>().Property(x => x.PatientType).HasDefaultValue(SD.PatientType.InPatient);
            modelBuilder.Entity<Medicine>().Property(x => x.Type).HasDefaultValue(SD.MedicineType.Tablet);
            modelBuilder.Entity<Vaccine>().Property(x => x.Type).HasDefaultValue(SD.VaccineType.Subcutaneous);
            modelBuilder.Entity<TestResult>().Property(x => x.PaymentStatus).HasDefaultValue(SD.PaymentStatus.UnPaid);
            modelBuilder.Entity<Consultation>().Property(x => x.PaymentStatus).HasDefaultValue(SD.PaymentStatus.UnPaid);
            modelBuilder.Entity<Consultation>().Property(x => x.Approved).HasDefaultValue(false);
            modelBuilder.Entity<VaccineAppointment>().Property(x => x.Taken).HasDefaultValue(true);
            modelBuilder.Entity<VaccineAppointment>().Property(x => x.DayTime).HasDefaultValue(SD.DayTime.Morning);
            modelBuilder.Entity<Prescription>().Property(x => x.DayTime).HasDefaultValue(SD.DayTime.Morning);
            modelBuilder.Entity<Expense>().Property(x => x.PaymentStatus).HasDefaultValue(SD.PaymentStatus.UnPaid);
        }
        
    }
}