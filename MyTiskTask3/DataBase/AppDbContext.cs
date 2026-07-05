using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.IO;
using MyTiskTask3.Model;

namespace MyTiskTask3.DataBase
{
    internal class AppDbContext : DbContext
    {

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserModel> UsersModels { get; set; }
        public virtual DbSet<UserTask> UserTasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app.db");
                optionsBuilder.UseSqlite($"Data Source={dbPath}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Связь 1:1 User → UserModel
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserModel)
                .WithOne(um => um.User)
                .HasForeignKey<UserModel>(um => um.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь 1:М User → UserTask (напрямую!)
            modelBuilder.Entity<User>()
                .HasMany(u => u.UserTasks)
                .WithOne(ut => ut.User)
                .HasForeignKey(ut => ut.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
         
    
    }
}