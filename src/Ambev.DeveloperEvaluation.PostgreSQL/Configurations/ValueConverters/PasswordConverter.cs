using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Ambev.DeveloperEvaluation.PostgreSQL.Configurations.ValueConverters;

internal sealed class PasswordConverter : ValueConverter<Password, string>
{
    public PasswordConverter() 
        : base(password => password,
            value => value) { }
}