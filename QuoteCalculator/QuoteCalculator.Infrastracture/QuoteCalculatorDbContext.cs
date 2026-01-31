using Microsoft.EntityFrameworkCore;
using QuoteCalculator.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace QuoteCalculator.Infrastracture
{
    public class QuoteCalculatorDbContext : DbContext
    {
        public QuoteCalculatorDbContext() { }

        public QuoteCalculatorDbContext(DbContextOptions<QuoteCalculatorDbContext> options) : base(options) { }

        public DbSet<Applicant> Applicants { get; set; }

        public DbSet<LoanApplication> LoanApplications { get; set; }

        public DbSet<ProductType> ProductTypes { get; set; }

        public DbSet<Blacklist> Blacklists { get; set; }

        public DbSet<GlobalConfiguration> GlobalConfigurations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Applicant>(entity =>
            {
                entity.HasKey(e => e.ApplicantId);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(100);
                entity.Property(e => e.FirstName).IsRequired();
                entity.Property(e => e.LastName).IsRequired();
                entity.Property(e => e.DateOfBirth).IsRequired();
                entity.Property(e => e.MobileNumber).IsRequired();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.RowVersion)
                      .HasColumnName("xmin")
                      .HasColumnType("xid")
                      .IsConcurrencyToken()
                      .ValueGeneratedOnAddOrUpdate();
            });

            modelBuilder.Entity<LoanApplication>(entity =>
            {
                entity.HasKey(e => e.LoanApplicationId);
                entity.Property(e => e.LoanApplicationPublicId).IsRequired().HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.ApplicantId).IsRequired();
                entity.Property(e => e.Amount).IsRequired().HasColumnType("decimal(19,4)");
                entity.Property(e => e.Term).IsRequired();
                entity.Property(e => e.ProductType);
                entity.Property(e => e.Status).IsRequired();
                entity.Property(e => e.EstablishmentFee).HasColumnType("decimal(19,4)");
                entity.Property(e => e.InterestFee).HasColumnType("decimal(19,4)");
                entity.Property(e => e.WeeklyRepayment).HasColumnType("decimal(19,4)");
                entity.Property(e => e.MonthlyRepayment).HasColumnType("decimal(19,4)");
                entity.Property(e => e.TotalRepayment).HasColumnType("decimal(19,4)");
                entity.Property(e => e.CreatedAt).IsRequired()
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
                entity.Property(e => e.UpdatedAt);
                entity.Property(e => e.ReferenceNumber);
                entity.Property(e => e.Remarks);
                entity.Property(e => e.RowVersion)
                      .HasColumnName("xmin")
                      .HasColumnType("xid")
                      .IsConcurrencyToken()
                      .ValueGeneratedOnAddOrUpdate();
            });

            modelBuilder.Entity<ProductType>(entity =>
            {
                entity.HasKey(e => e.ProductTypeId);
                entity.Property(e => e.ProductName).IsRequired();
                entity.Property(e => e.MinimumTerm).IsRequired();
                entity.Property(e => e.MaximumTerm).IsRequired();
                entity.Property(e => e.MinimumAmount).IsRequired().HasColumnType("decimal(19,4)");
                entity.Property(e => e.MaximumAmount).IsRequired().HasColumnType("decimal(19,4)");
                entity.Property(e => e.InterestType).HasDefaultValue(0);
                entity.Property(e => e.InterestFreeMonths).IsRequired();
                entity.Property(e => e.InterestRate).IsRequired().HasColumnType("decimal(5,4)");

                entity.HasData(
                    new ProductType
                    {
                        ProductTypeId = 1,
                        ProductName = "Product A",
                        MinimumTerm = 0,
                        MaximumTerm = 24,
                        MinimumAmount = 500m,
                        MaximumAmount = 5000m,
                        InterestType = 1,
                        InterestFreeMonths = 0,
                        InterestRate = 0
                    },
                    new ProductType
                    {
                        ProductTypeId = 2,
                        ProductName = "Product B",
                        MinimumTerm = 6,
                        MaximumTerm = 24,
                        MinimumAmount = 2000m,
                        MaximumAmount = 25000m,
                        InterestType = 2,
                        InterestFreeMonths = 2,
                        InterestRate = 9.8m
                    },
                    new ProductType
                    {
                        ProductTypeId = 3,
                        ProductName = "Product C",
                        MinimumTerm = 3,
                        MaximumTerm = 60,
                        MinimumAmount = 1000m,
                        MaximumAmount = 50000m,
                        InterestType = 0,
                        InterestFreeMonths = 0,
                        InterestRate = 7.2m
                    }
                );
            });

            modelBuilder.Entity<Blacklist>(entity =>
            {
                entity.HasKey(e => e.BlacklistId);
                entity.Property(e => e.BlacklistType).IsRequired();
                entity.Property(e => e.Value).IsRequired();

                entity.HasData(
                    new Blacklist
                    {
                        BlacklistId = 1,
                        BlacklistType = 1,
                        Value = "0422111333"
                    },
                    new Blacklist
                    {
                        BlacklistId = 2,
                        BlacklistType = 2,
                        Value = "flower.com.au"
                    }
                );
            });

            modelBuilder.Entity<GlobalConfiguration>(entity =>
            {
                entity.HasKey(e => e.GlobalConfigurationId);
                entity.Property(e => e.Key).IsRequired();
                entity.Property(e => e.Value).IsRequired();

                entity.HasData(
                    new GlobalConfiguration
                    {
                        GlobalConfigurationId = 1,
                        Key = "EstablishmentFee",
                        Value = "300"
                    }
                );
            });
        }
    }
}

