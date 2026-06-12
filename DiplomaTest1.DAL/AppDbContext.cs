using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DiplomaTest1.Core.Models;

namespace DiplomaTest1.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Vaccine> Vaccines { get; set; }
        public DbSet<Infection> Infections { get; set; }
        public DbSet<Record> Records { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    string connectionString = "Server = localhost; Port = 5432; User Id = postgres; Password = Trololo0+; Database = testDiplomBD";
        //    optionsBuilder.UseNpgsql(connectionString);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("testSchema");

            modelBuilder.Entity<User>().ToTable("Users");

            modelBuilder.Entity<Role>().ToTable("Roles");

            modelBuilder.Entity<Record>().ToTable("Records");

            modelBuilder.Entity<Vaccine>().ToTable("Vaccines");

            modelBuilder.Entity<Infection>().ToTable("Infections");
        }
    }
}
