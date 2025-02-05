using System.Text.Json;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Sales.Events;
using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.Events;

public sealed class SaleCreatedEventHandler : IEventHandler<SaleCreatedEvent>
{
    private readonly ILogger<SaleCreatedEventHandler> _logger;

    public SaleCreatedEventHandler(ILogger<SaleCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(SaleCreatedEvent @event)
    {
        _logger.LogInformation("{EventName} detected with data: {EventData}", nameof(SaleCreatedEvent), JsonSerializer.Serialize(@event));
        return Task.CompletedTask;
    }
}