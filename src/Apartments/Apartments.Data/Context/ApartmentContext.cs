using Apartments.Data.DataModels;
using Apartments.Data.ModelConfig;
using Bogus;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Data.Context
{
    public class ApartmentContext : DbContext
    {
        public ApartmentContext(DbContextOptions<ApartmentContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Address> Adresses { get; set; }

        public DbSet<Apartment> Apartments { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<BusyDate> BusyDates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AddressConfig());
            modelBuilder.ApplyConfiguration(new ApartmentConfig());
            modelBuilder.ApplyConfiguration(new CommentConfig());
            modelBuilder.ApplyConfiguration(new CountryConfig());
            modelBuilder.ApplyConfiguration(new OrderConfig());
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new DatesConfig());

            modelBuilder.Entity<Country>().HasData(GetCountries());
        }

        private List<Country> GetCountries()
        {
            List<Country> countries = new List<Country>();

            Faker<Country> fakeCountries = new Faker<Country>()
                .RuleFor(_ => _.Name, y => y.Address.Country());

            countries = fakeCountries.Generate(100);

            return countries;
        }
    }
}