using Azure;
using HomeBudget.Api.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection.Emit;

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
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // 1:1 User -> Address
            builder.Entity<User>()
                .HasOne(u => u.Address)
                .WithOne(a => a.User)
                .HasForeignKey<Address>(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // 1:N Budget -> ExpenseCategory
            builder.Entity<ExpenseCategory>()
                .HasOne(ec => ec.Budget)
                .WithMany(b => b.ExpenseCategories)
                .HasForeignKey(ec => ec.BudgetId)
                .OnDelete(DeleteBehavior.Cascade);

            // 1:N Budget -> Expense
            builder.Entity<Expense>()
                .HasOne(e => e.Budget)
                .WithMany(b => b.Expenses)
                .HasForeignKey(e => e.BudgetId)
                .OnDelete(DeleteBehavior.Cascade);

            // N:1 Expense -> ExpenseCategory
            builder.Entity<Expense>()
                .HasOne(e => e.ExpenseCategory)
                .WithMany(ec => ec.Expenses)
                .HasForeignKey(e => e.ExpenseCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Expense>()
                .Property(e => e.TotalAmount)
                .HasColumnType("decimal(18,2)");

            // N:M User -> Budget
            builder.Entity<Budget>()
                .HasMany(b => b.Users)
                .WithMany(u => u.Budgets);

        }
    }
}