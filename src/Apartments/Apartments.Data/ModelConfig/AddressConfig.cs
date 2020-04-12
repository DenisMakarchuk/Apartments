using Apartments.Data.DataModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Data.ModelConfig
{
    /// <summary>
    /// Configuration for table Adresses
    /// </summary>
    public class AddressConfig : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Adresses");

            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Id).ValueGeneratedOnAdd();

            builder.HasOne(_ => _.Country).WithMany(_ => _.Addresses)
                .HasForeignKey(_ => _.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(_ => _.Apartment).WithOne(_ => _.Address)
                .HasForeignKey<Apartment>(_ => _.AddressId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(_ => _.City).IsRequired().HasMaxLength(50);
            builder.Property(_ => _.Street).IsRequired().HasMaxLength(50);
        }
    }
}
