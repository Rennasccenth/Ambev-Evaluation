using System.Reflection;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Services;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Strategies;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Sales.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.DeveloperEvaluation.Domain;

public static class DependencyInjectionResolver
{
    public static IServiceCollection InstallDomainServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddValidatorsFromAssembly(
            assembly: Assembly.GetExecutingAssembly(),
            includeInternalTypes: true);

        serviceCollection.AddScoped<ISalesService, SalesService>();
        serviceCollection.AddScoped<ICartsService, CartsService>();
        serviceCollection.AddScoped<IProductPriceResolver, ProductPriceResolver>();
        serviceCollection.AddScoped<IDiscountStrategy, ItemQuantity>();

        return serviceCollection;
    }
}