using DnuGame.Api.Modules.Rooms.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DnuGame.Api.Modules.Rooms.Configuration;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable("Rooms");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Slug)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(r => r.Color)
            .IsRequired()
            .HasMaxLength(7); // #RRGGBB

        builder.Property(r => r.Icon)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(r => r.UserLimit)
            .IsRequired();

        builder.Property(r => r.IsOpen)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(r => r.CreatedAt)
            .IsRequired();

        builder.Property(r => r.UpdatedAt)
            .IsRequired();

        // Índice único para Slug
        builder.HasIndex(r => r.Slug)
            .IsUnique()
            .HasDatabaseName("IX_Rooms_Slug");

        // Índice para IsOpen (para consultas de filtrado)
        builder.HasIndex(r => r.IsOpen)
            .HasDatabaseName("IX_Rooms_IsOpen");
    }
}
