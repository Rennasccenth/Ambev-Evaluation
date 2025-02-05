using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Ambev.DeveloperEvaluation.ORM.Configurations.ValueConverters;

public sealed class DateTimeValueConverter : ValueConverter<DateTime, DateTime>
{
    public DateTimeValueConverter()
        : base(dateTime => dateTime.ToUniversalTime(),
            storedDateTime => DateTime.SpecifyKind(storedDateTime, DateTimeKind.Utc)) { }
}