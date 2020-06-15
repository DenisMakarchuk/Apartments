using Apartments.Data.DataModels;
using Apartments.Data.ModelConfig;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new AddressConfig());
            modelBuilder.ApplyConfiguration(new ApartmentConfig());
            modelBuilder.ApplyConfiguration(new CommentConfig());
            modelBuilder.ApplyConfiguration(new CountryConfig());
            modelBuilder.ApplyConfiguration(new OrderConfig());
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new DatesConfig());
            modelBuilder.ApplyConfiguration(new RefreshTokenConfig());

            modelBuilder.Entity<Country>().HasData(GetCountries());
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Order ||
                    entry.Entity is BusyDate ||
                    entry.Entity is Country ||
                    entry.Entity is User ||
                    entry.Entity is RefreshToken)
                {
                    continue;
                };

                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues["IsDeleted"] = false;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.CurrentValues["IsDeleted"] = true;
                        break;
                }
            }
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