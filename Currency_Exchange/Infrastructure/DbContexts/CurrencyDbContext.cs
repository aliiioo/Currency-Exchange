using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbContexts
{
    public class CurrencyDbContext:IdentityDbContext<ApplicationUser>
    {
        public CurrencyDbContext(DbContextOptions<CurrencyDbContext> contextOptions):base(contextOptions)
        {
                
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Currency?> Currencies { get; set; }
        public DbSet<TwoFactorAuthentication> TwoFactorAuthentications { get; set; }
        public DbSet<SystemLog> SystemLogs { get; set; }
        public DbSet<ExchangeRate> ExchangeRates { get; set; }
        public DbSet<CurrencyExchangeFees> CurrencyExchangeFees{ get; set; }
        public DbSet<CurrencyTransformFees> CurrencyTransformFees{ get; set; }
        public DbSet<OthersAccount>  OthersAccounts{ get; set; }
        public DbSet<DeletedAccount>  DeletedAccounts{ get; set; }




        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region Seedata

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "1",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = "2",
                    Name = "Customer",
                    NormalizedName = "CUSTOMER"
                }
            );

            // Create a hasher to hash the password before seeding the user to the database
            var hasher = new PasswordHasher<ApplicationUser>();

            // Seed Users
            builder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = "1",
                    UserName = "admin@example.com",
                    NormalizedUserName = "ADMIN@EXAMPLE.COM",
                    Email = "admin@example.com",
                    NormalizedEmail = "ADMIN@EXAMPLE.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "Admin@123"),
                    SecurityStamp = string.Empty,
                    FullName = "Admin User"
                }
            );

            // Seed UserRoles
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "1",
                    UserId = "1"
                }
            );

            #endregion
            //


            builder.Entity<Account>().HasQueryFilter(e => e.IsDeleted==false&&e.IsActive);

            builder.Entity<Transaction>()
                .HasOne(t => t.FromAccount)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.FromAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Transaction>()
                .HasOne(t => t.ToAccount)
                .WithMany()
                .HasForeignKey(t => t.ToAccountId)
                .OnDelete(DeleteBehavior.Restrict);

           
        }
    }
}
