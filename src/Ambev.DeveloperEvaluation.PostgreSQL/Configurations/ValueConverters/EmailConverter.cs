using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Ambev.DeveloperEvaluation.ORM.Configurations.ValueConverters;

internal sealed class EmailConverter : ValueConverter<Email, string>
{
    public EmailConverter() 
        : base(email => email,
            value => value) { }
}