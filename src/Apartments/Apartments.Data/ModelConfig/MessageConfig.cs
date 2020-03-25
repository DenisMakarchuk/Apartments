using Apartments.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Data.ModelConfig
{
    /// <summary>
    /// Configuration for table Messages
    /// </summary>
    public class MessageConfig : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("Messages");

            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Id).ValueGeneratedOnAdd();

            builder.HasOne(_ => _.Destination).WithMany(_ => _.ReceivedMessages)
                .HasForeignKey(_ => _.DestinationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(_ => _.Author).WithMany(_ => _.SentMessages)
                .HasForeignKey(_ => _.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(_ => _.Title).HasMaxLength(50);
            builder.Property(_ => _.Text).IsRequired().HasMaxLength(500);
        }
    }
}
