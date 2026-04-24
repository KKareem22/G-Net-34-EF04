using G_Net_34_EF04.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Net_34_EF04
{
    internal class BankDbContext :DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=BankDB;Trusted_Connection=true;TrustServerCertificate=true");

        }
        public DbSet<Branch>Branches { get; set; }
        public DbSet<Account>Accounts { get; set; }
        public DbSet<Manager>Managers { get; set; }
        public DbSet<Transaction>Transactions { get; set; }
        public DbSet<Customer>Customers { get; set; }
        public DbSet<CustomerAccount>CustomerAccounts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Owned
            modelBuilder.Entity<Branch>()
                .OwnsOne(b => b.Address, a =>
                {
                    a.Property(ad => ad.Street).HasMaxLength(50);
                    a.Property(ad=>ad.City).HasMaxLength(50);
                    a.Property(ad => ad.Country).HasMaxLength(50);
                });
            modelBuilder.Entity<Customer>()
                .OwnsOne(c => c.Address, a =>
                {
                    a.Property(ad => ad.Street).HasMaxLength(50);
                    a.Property(ad => ad.City).HasMaxLength(50);
                    a.Property(ad => ad.Country).HasMaxLength(50);
                });
            #endregion

            //Branch
            modelBuilder.Entity<Branch>(entity =>
            {
                entity.HasKey(e => e.Code);
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
                entity.HasOne(e=>e.Manager)
                .WithOne(m=>m.Branch)
                .HasForeignKey<Branch>(b=>b.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);
            }
            );
            //Manager
            modelBuilder.Entity<Manager>(entity =>
            {
                entity.Property(e => e.FullName).HasMaxLength(50);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
                entity.Property(e=>e.Email).HasMaxLength(50);
            }
            );
            //Customer
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.FullName).HasMaxLength(50);
                entity.Property(e => e.NationalId).HasMaxLength(20);
                entity.HasIndex(e => e.NationalId).IsUnique();
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
                entity.Property(e => e.Email).HasMaxLength(50);
            }
            );
            //Account
            modelBuilder.Entity<Account>(entity =>
            {
                
                entity.Property(a => a.CurrentBalance).HasPrecision(18, 2);

                entity.HasOne(a=>a.Branch)
                .WithMany(b=>b.Accounts)
                .HasForeignKey(a=>a.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(a=>a.Transactions)
                .WithOne(t=>t.Account)
                .HasForeignKey(t=>t.AccountNumber)
                .OnDelete(DeleteBehavior.Restrict);
            });
            //Transaction
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(t => t.TransactionNumber);
                entity.Property(t => t.Amount).HasPrecision(18, 2);
                entity.Property(t=>t.Note).HasMaxLength(200);

            });
            //CustomerAccount
            modelBuilder.Entity<CustomerAccount>(entity =>
            {
                entity.HasKey(ca => new { ca.CustomerId, ca.AccountId });
                entity.HasOne(ca => ca.Customer)
                .WithMany(c => c.CustomerAccounts)
                .HasForeignKey(ca => ca.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ca=>ca.Account)
                .WithMany(a=>a.Owners)
                .HasForeignKey(ca=>ca.AccountId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            //Seed Data
            modelBuilder.Entity<Manager>().HasData(
                new Manager() {Id=1 ,FullName="Kareem Mahdy",PhoneNumber="01276983179",Email="Kareem755@gmail.com",HireDate=new DateTime(2022,5,1)},
                new Manager() {Id=2 ,FullName="Mazen Ahmed",PhoneNumber="015762383004",Email="mazenahmed15@gmail.com",HireDate=new DateTime(2024,2,1)}
                
                );
            modelBuilder.Entity<Branch>().HasData(
                new
                {
                    Code = "BR001",
                    Name = "Main Branch",
                    PhoneNumber = "0123456789",
                    ManagerId = 1
                },
                new
                {
                    Code = "BR002",
                    Name = "City Branch",
                    PhoneNumber = "9876543210",
                    ManagerId = 2
                }
            );

            
            modelBuilder.Entity<Branch>().OwnsOne(b => b.Address).HasData(
                new
                {
                    BranchCode = "BR001", 
                    Street = "Road 9, Maadi",
                    City = "Cairo",
                    Country = "Egypt"
                },
                new
                {
                    BranchCode = "BR002",
                    Street = "Corniche Road",
                    City = "Alexandria",
                    Country = "Egypt"
                }
            );


        }

    }
}
