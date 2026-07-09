using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MyTiskTask4.Model;

namespace MyTiskTask4.DataBase
{
    public class AppDbContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserModel> UsersModels { get; set; }
        public virtual DbSet<UserTask> UserTasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // ✅ Используем путь из DatabasePath
                var dbPath = DatabasePath.GetDatabasePath();
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

            // Связь 1:М User → UserTask
            modelBuilder.Entity<User>()
                .HasMany(u => u.UserTasks)
                .WithOne(ut => ut.User)
                .HasForeignKey(ut => ut.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }

        public void EnsureDatabaseCreated()
        {
            Database.EnsureCreated();
        }

        public static string GetDatabasePathStatic()
        {
            return DatabasePath.GetDatabasePath();
        }
    }
}