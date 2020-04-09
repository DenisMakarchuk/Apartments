using Apartments.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Data.ModelConfig
{
    /// <summary>
    /// Configuration for table Apartments
    /// </summary>
    public class ApartmentConfig : IEntityTypeConfiguration<Apartment>
    {
        public void Configure(EntityTypeBuilder<Apartment> builder)
        {
            builder.ToTable("Apartments");

            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Id).ValueGeneratedOnAdd();

            builder.HasOne(_ => _.Address).WithOne(_ => _.Apartment)
                .HasForeignKey<Address>(_ => _.ApartmentId)
                .OnDelete(DeleteBehavior.Cascade).IsRequired();

            builder.HasOne(_ => _.Owner).WithMany(_ => _.Apartments)
                .HasForeignKey(_ => _.OwnerId)
                .OnDelete(DeleteBehavior.Restrict).IsRequired();

            builder.HasMany(_ => _.Orders).WithOne(_ => _.Apartment)
                .HasForeignKey(_ => _.ApartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(_ => _.Comments).WithOne(_ => _.Apartment)
                .HasForeignKey(_ => _.ApartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(_ => _.Dates).WithOne(_ => _.Apartment)
                .HasForeignKey(_ => _.ApartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(_ => _.Price).IsRequired();

            builder.Property(_ => _.Title).IsRequired().HasMaxLength(120);
            builder.Property(_ => _.Text).IsRequired().HasMaxLength(500);
        }
    }
}
