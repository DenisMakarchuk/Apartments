using Apartments.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Data.ModelConfig
{
    /// <summary>
    /// Configuration for table Comments
    /// </summary>
    public class CommentConfig : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comments");

            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Id).ValueGeneratedOnAdd();

            builder.HasOne(_ => _.Apartment).WithMany(_ => _.Comments)
                .HasForeignKey(_ => _.ApartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(_ => _.Author).WithMany(_ => _.Comments)
                .HasForeignKey(_ => _.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(_ => _.Title).IsRequired().HasMaxLength(25);
            builder.Property(_ => _.Text).IsRequired().HasMaxLength(255);

            builder.Property<bool>("IsDeleted");
            builder.HasQueryFilter(post => EF.Property<bool>(post, "IsDeleted") == false);
        }
    }
}