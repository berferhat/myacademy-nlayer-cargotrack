using CargoTrack.Entity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CargoTrack.DataAccess.Context
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cargo>()
                .HasOne(c => c.Sender)
                .WithMany(m => m.SentCargos)
                .HasForeignKey(c => c.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cargo>()
                .HasOne(c => c.Receiver)
                .WithMany(m => m.ReceivedCargos)
                .HasForeignKey(c => c.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cargo>()
                .HasOne(c => c.OriginBranch)
                .WithMany(m => m.OriginCargos)
                .HasForeignKey(c => c.OriginBranchId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cargo>()
                .HasOne(c => c.DestinationBranch)
                .WithMany(m => m.DestinationCargos)
                .HasForeignKey(c => c.DestinationBranchId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cargo>()
                .HasIndex(c => c.TrackCode)
                .IsUnique();

            // Cargo -> Movements (1-N)
            modelBuilder.Entity<CargoMovement>()
                .HasOne(m => m.Cargo)
                .WithMany(c => c.Movements)
                .HasForeignKey(m => m.CargoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CargoMovement>()
                .HasOne(m => m.TransferCenter)
                .WithMany()
                .HasForeignKey(m => m.TransferCenterId)
                .OnDelete(DeleteBehavior.Restrict);

            // Cargo -> Delivery (1-1)
            modelBuilder.Entity<Delivery>()
                .HasOne(d => d.Cargo)
                .WithOne(c => c.Delivery)
                .HasForeignKey<Delivery>(d => d.CargoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Cargo -> DeliveryExceptions (1-N)
            modelBuilder.Entity<DeliveryException>()
                .HasOne(e => e.Cargo)
                .WithMany(c => c.DeliveryExceptions)
                .HasForeignKey(e => e.CargoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Employee -> Branch
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Branch)
                .WithMany(b => b.Employees)
                .HasForeignKey(e => e.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            // Employee -> AppUser (opsiyonel)
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.AppUser)
                .WithMany()
                .HasForeignKey(e => e.AppUserId)
                .OnDelete(DeleteBehavior.Restrict);

            


            base.OnModelCreating(modelBuilder);
        }

        public DbSet<About> Abouts { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Cargo> Cargos { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<CargoMovement> CargoMovements { get; set; }
        public DbSet<TransferCenter> TransferCenters { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<DeliveryException> DeliveryExceptions { get; set; }
        public DbSet<CargoPrice> CargoPrices { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
    }
}
