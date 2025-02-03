using System.Text.Json;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Events.Products;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Products.Events;

public sealed class ProductPriceChangedEventHandler : IEventHandler<ProductPriceChangedEvent>
{
    private readonly ILogger<ProductPriceChangedEventHandler> _logger;

    public ProductPriceChangedEventHandler(ILogger<ProductPriceChangedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(ProductPriceChangedEvent @event)
    {
        _logger.LogInformation("{EventName}: Product price changed ->  {EventData}", nameof(ProductPriceChangedEvent), JsonSerializer.Serialize(@event));
        return Task.CompletedTask;
    }
}