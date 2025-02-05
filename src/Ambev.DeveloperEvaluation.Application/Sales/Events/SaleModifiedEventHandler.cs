using System.Text.Json;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Sales.Events;
using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.Events;

public sealed class SaleModifiedEventHandler : IEventHandler<SaleModifiedEvent>
{
    private readonly ILogger<SaleModifiedEventHandler> _logger;

    public SaleModifiedEventHandler(ILogger<SaleModifiedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(SaleModifiedEvent @event)
    {
        _logger.LogInformation("{EventName} detected with data: {EventData}", nameof(SaleModifiedEvent), JsonSerializer.Serialize(@event));
        return Task.CompletedTask;
    }
}