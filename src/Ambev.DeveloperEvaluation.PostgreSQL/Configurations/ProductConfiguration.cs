using Ambev.DeveloperEvaluation.Domain.Aggregates.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.PostgreSQL.Configurations;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .HasColumnType(PostgreSqlConstants.Types.Guid)
            .HasDefaultValueSql(PostgreSqlConstants.DefaultValues.Guid);

        builder.HasIndex(e => new { e.Title, e.Description }).IsUnique();
        builder.Property(e => e.Title).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Price).HasPrecision(18, 2);
        builder.Property(e => e.Description).IsRequired().HasMaxLength(500);
        builder.Property(e => e.Category).IsRequired().HasMaxLength(80);
        builder.Property(e => e.Image).IsRequired().HasMaxLength(100);
        builder.ComplexProperty(e => e.Rating, propertyBuilder =>
        {
            propertyBuilder.Property(rating => rating.Rate)
                .IsRequired()
                .HasPrecision(18, 2);
            propertyBuilder.Property(rating => rating.Count)
                .IsRequired()
                .HasDefaultValue(0);
        });
    }
}