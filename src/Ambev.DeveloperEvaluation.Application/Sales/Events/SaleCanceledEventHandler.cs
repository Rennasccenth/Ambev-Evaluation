using System.Text.Json;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Sales.Events;
using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.Events;

public sealed class SaleCanceledEventHandler : IEventHandler<SaleCanceledEvent>
{
    private readonly ILogger<SaleCanceledEventHandler> _logger;

    public SaleCanceledEventHandler(ILogger<SaleCanceledEventHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(SaleCanceledEvent @event)
    {
        _logger.LogInformation("{EventName} detected with data: {EventData}", nameof(SaleCanceledEvent), JsonSerializer.Serialize(@event));
        return Task.CompletedTask;
    }
}