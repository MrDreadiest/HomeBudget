using HomeBudget.Api.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HomeBudget.Api.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        //Tablica reprezentująca usera powstaje sama poprzez IdentityDbContext
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionCategory> TransactionCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // 1:1 User -> Address
            builder.Entity<User>()
                .HasOne(u => u.Address)
                .WithOne(a => a.User)
                .HasForeignKey<Address>(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // 1:N Budget -> TransactionCategory
            builder.Entity<TransactionCategory>()
                .HasOne(ec => ec.Budget)
                .WithMany(b => b.TransactionCategories)
                .HasForeignKey(ec => ec.BudgetId)
                .OnDelete(DeleteBehavior.Cascade);

            // 1:N Budget -> Transaction
            builder.Entity<Transaction>()
                .HasOne(e => e.Budget)
                .WithMany(b => b.Transactions)
                .HasForeignKey(e => e.BudgetId)
                .OnDelete(DeleteBehavior.Cascade);

            // N:1 Transaction -> TransactionCategory
            builder.Entity<Transaction>()
                .HasOne(e => e.TransactionCategory)
                .WithMany(ec => ec.Transactions)
                .HasForeignKey(e => e.TransactionCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Transaction>()
                .Property(e => e.TotalAmount)
                .HasColumnType("decimal(18,2)");

            // N:M User -> Budget
            builder.Entity<Budget>()
                .HasMany(b => b.Users)
                .WithMany(u => u.Budgets);

        }
    }
}