using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM.Configurations.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .HasColumnType(PostgreSqlConstants.Types.Guid)
            .HasDefaultValueSql(PostgreSqlConstants.DefaultValues.Guid);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasConversion<EmailConverter>()
            .HasMaxLength(100);
        builder.Property(u => u.Firstname).IsRequired().HasMaxLength(50);
        builder.Property(u => u.Lastname).IsRequired().HasMaxLength(50);
        builder.Property(u => u.Username).IsRequired().HasMaxLength(50);
        builder.Property(u => u.Password)
            .IsRequired()
            .HasConversion<PasswordConverter>()
            .HasMaxLength(100);
        builder.ComplexProperty(u => u.Address, propertyBuilder =>
        {
            propertyBuilder.Property(addr => addr.Street).IsRequired().HasMaxLength(50);
            propertyBuilder.Property(addr => addr.City).IsRequired().HasMaxLength(50);
            propertyBuilder.Property(addr => addr.Number).IsRequired();
            propertyBuilder.Property(addr => addr.ZipCode).IsRequired().HasMaxLength(50);
            propertyBuilder.Property(addr => addr.Latitude).IsRequired().HasMaxLength(15);
            propertyBuilder.Property(addr => addr.Longitude).IsRequired().HasMaxLength(15);
        });
        builder.Property(u => u.Phone)
            .IsRequired()
            .HasConversion<PhoneConverter>()
            .HasMaxLength(20);

        builder.Property(u => u.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(u => u.Role)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(u => u.CreatedAt)
            .IsRequired()
            .HasColumnType(PostgreSqlConstants.Types.DateTime);

        builder.Property(u => u.CreatedAt)
            .HasColumnType(PostgreSqlConstants.Types.DateTime);
    }
}
