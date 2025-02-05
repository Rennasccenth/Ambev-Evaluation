using System.Text.Json;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Sales.Events;
using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.Events;

public sealed class SaleTerminatedEventHandler : IEventHandler<SaleTerminatedEvent>
{
    private readonly ILogger<SaleTerminatedEventHandler> _logger;

    public SaleTerminatedEventHandler(ILogger<SaleTerminatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(SaleTerminatedEvent @event)
    {
        _logger.LogInformation("{EventName} detected with data: {EventData}", nameof(SaleTerminatedEvent), JsonSerializer.Serialize(@event));
        return Task.CompletedTask;
    }
}