using Apartments.Data.DataModels;
using Apartments.Data.ModelConfig;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Data.Context
{
    public class ApartmentContext : DbContext
    {
        public ApartmentContext()
        {

        }

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

            foreach (Countries c in Enum.GetValues(typeof(Countries)))
            {
                string[] name = c.ToString().Split("_");
                countries.Add(new Country() 
                { 
                    Id = new Guid(), 
                    Name = string.Join(" ", name) 
                });
            }

            return countries;
        }
    }

    enum Countries
    {
        Afghanistan,
        Albania,
        Algeria,
        Andorra,
        Angola,
        Antigua_and_Barbuda,
        Argentina,
        Armenia,
        Australia,
        Austria,
        Azerbaijan,
        Bahamas,
        Bahrain,
        Bangladesh,
        Barbados,
        Belarus,
        Belgium,
        Belize,
        Benin,
        Bhutan,
        Bolivia,
        Bosnia_and_Herzegovina,
        Botswana,
        Brazil,
        Brunei,
        Bulgaria,
        Burkina_Faso,
        Burundi,
        Cabo_Verde,
        Cambodia,
        Cameroon,
        Canada,
        CAR,
        Chad,
        Chile,
        China,
        Colombia,
        Comoros,
        Congo, 
        Costa_Rica,
        Croatia,
        Cuba,
        Cyprus,
        Czechia,
        Denmark,
        Djibouti,
        Dominica,
        Dominican_Republic,
        Ecuador,
        Egypt,
        El_Salvador,
        Equatorial_Guinea,
        Eritrea,
        Estonia,
        Eswatini,
        Ethiopia,
        Fiji,
        Finland,
        France,
        Gabon,
        Gambia,
        Georgia,
        Germany,
        Ghana,
        Greece,
        Grenada,
        Guatemala,
        Guyana,
        Haiti,
        Honduras,
        Hungary,
        Iceland,
        India,
        Indonesia,
        Iran,
        Iraq,
        Ireland,
        Israel,
        Italy,
        Jamaica,
        Japan,
        Jordan,
        Kazakhstan,
        Kenya,
        Kiribati,
        Kosovo,
        Kuwait,
        Kyrgyzstan,
        Laos,
        Latvia,
        Lebanon,
        Lesotho,
        Liberia,
        Libya,
        Liechtenstein,
        Lithuania,
        Luxembourg,
        Madagascar,
        Malawi,
        Malaysia,
        Maldives,
        Mali,
        Malta,
        Marshall_Islands,
        Mauritania,
        Mauritius,
        Mexico,
        Micronesia,
        Moldova,
        Monaco,
        Mongolia,
        Montenegro,
        Morocco,
        Mozambique,
        Myanmar,
        Namibia,
        Nauru,
        Nepal,
        Netherlands,
        New_Zealand,
        Nicaragua,
        Niger,
        Nigeria,
        North_Macedonia,
        Norway,
        Oman,
        Pakistan,
        Palau,
        Palestine,
        Panama,
        Papua_New_Guinea,
        Paraguay,
        Philippines,
        Poland,
        Portugal,
        Qatar,
        Romania,
        Russia,
        Rwanda,
        Samoa,
        San_Marino,
        Saudi_Arabia,
        Senegal,
        Serbia,
        Seychelles,
        Sierra_Leone,
        Singapore,
        Slovakia,
        Slovenia,
        Solomon_Islands,
        Somalia,
        Korea,
        Spain,
        Sri_Lanka,
        Sudan,
        Suriname,
        Sweden,
        Switzerland,
        Syria,
        Taiwan,
        Tajikistan,
        Tanzania,
        Thailand,
        Togo,
        Tonga,
        Trinidad_and_Tobago,
        Tunisia,
        Turkey,
        Turkmenistan,
        Tuvalu,
        Uganda,
        Ukraine,
        Arabie,
        Anglia,
        USA,
        Uruguay,
        Uzbekistan,
        Vanuatu,
        Vatican,
        Venezuela,
        Vietnam,
        Yemen,
        Zambia,
        Zimbabwe
    }
}
