using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Ambev.DeveloperEvaluation.ORM.Configurations.ValueConverters;

internal sealed class PhoneConverter : ValueConverter<Phone, string>
{
    public PhoneConverter() 
        : base(phone => phone,
            value => value) { }
}