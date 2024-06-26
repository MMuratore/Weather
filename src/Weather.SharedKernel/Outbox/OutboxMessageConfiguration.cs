﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Weather.SharedKernel.Outbox;

internal sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable(nameof(OutboxMessage));
        builder.HasKey(t => t.Id);

        builder.Property(x => x.Type).HasMaxLength(1000);
        builder.Property(x => x.Content).HasMaxLength(1000);

        builder.HasIndex(x => x.CompleteTime);
        builder.HasIndex(x => x.CreationTime).IsDescending();
    }
}