using Apartments.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Data.ModelConfig
{
    /// <summary>
    /// Configuration for table Countries
    /// </summary>
    public class CountryConfig : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.ToTable("Countries");

            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Id).ValueGeneratedOnAdd();

            builder.Property(_ => _.Name).IsRequired().HasMaxLength(50);

            builder.HasMany(_ => _.Addresses).WithOne(_ => _.Country)
                .HasForeignKey(_ => _.CountryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
