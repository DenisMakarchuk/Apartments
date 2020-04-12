using Apartments.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Data.ModelConfig
{
    /// <summary>
    /// Configuration for table BusyDates
    /// </summary>
    public class DatesConfig : IEntityTypeConfiguration<BusyDate>
    {
        public void Configure(EntityTypeBuilder<BusyDate> builder)
        {
            builder.ToTable("BusyDates");

            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Id).ValueGeneratedOnAdd();

            builder.HasOne(_ => _.Order).WithMany(_ => _.Dates)
                .HasForeignKey(_ => _.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(_ => _.Apartment).WithMany(_ => _.Dates)
                .HasForeignKey(_ => _.ApartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(_ => _.Date).IsRequired();
        }
    }
}
