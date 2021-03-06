﻿using Apartments.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Data.ModelConfig
{
    /// <summary>
    /// Configuration for table Users
    /// </summary>
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(_ => _.Id);

            builder.Property(_ => _.NickName).IsRequired();

            builder.HasMany(_ => _.Apartments).WithOne(_ => _.Owner)
                .HasForeignKey(_ => _.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(_ => _.Orders).WithOne(_ => _.Customer)
                .HasForeignKey(_ => _.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(_ => _.Comments).WithOne(_ => _.Author)
                .HasForeignKey(_ => _.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(_ => _.RefreshTokens).WithOne(_ => _.User)
                .HasForeignKey(_ => _.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}