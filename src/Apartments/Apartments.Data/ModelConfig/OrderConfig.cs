﻿using Apartments.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Data.ModelConfig
{
    /// <summary>
    /// Configuration for table Orders
    /// </summary>
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Id).ValueGeneratedOnAdd();

            builder.HasOne(_ => _.Apartment).WithMany(_ => _.Orders)
                .HasForeignKey(_ => _.ApartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(_ => _.Customer).WithMany(_ => _.Orders)
                .HasForeignKey(_ => _.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(_ => _.Dates).IsRequired();
        }
    }
}
