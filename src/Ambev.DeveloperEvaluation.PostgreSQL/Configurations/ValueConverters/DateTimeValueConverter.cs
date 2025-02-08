using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Ambev.DeveloperEvaluation.PostgreSQL.Configurations.ValueConverters;

public sealed class DateTimeValueConverter : ValueConverter<DateTime, DateTime>
{
    public DateTimeValueConverter()
        : base(dateTime => dateTime.ToUniversalTime(),
            storedDateTime => DateTime.SpecifyKind(storedDateTime, DateTimeKind.Utc)) { }
}