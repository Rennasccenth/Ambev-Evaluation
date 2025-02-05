using Ambev.DeveloperEvaluation.Domain.Events.Abstractions;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Ambev.DeveloperEvaluation.ORM.Interceptors;

internal sealed class DomainEventInterceptor : SaveChangesInterceptor
{
    private readonly IDomainEventDispatcher _domainEventDispatcher;

    public DomainEventInterceptor(IDomainEventDispatcher domainEventDispatcher)
    {
        _domainEventDispatcher = domainEventDispatcher;
    }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        if (eventData.Context is null) return result;

        var entityEntries = eventData.Context.ChangeTracker
            .Entries<IEventableEntity>()
            .Where(x => x.Entity.DomainEvents.Any())
            .ToList();

        foreach (var eventableEntity in entityEntries)
        {
            _domainEventDispatcher.DispatchAndClearEventsAsync(eventableEntity.Entity).RunSynchronously();
        }
        return result;
    }

    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        if (eventData.Context is null) return result;

        var entityEntries = eventData.Context.ChangeTracker
            .Entries<IEventableEntity>()
            .Where(x => x.Entity.DomainEvents.Any())
            .ToList();

        List<Task> dispatchingTasks = [];
        dispatchingTasks.AddRange(entityEntries.Select(eventable => _domainEventDispatcher
            .DispatchAndClearEventsAsync(eventable.Entity)));

        await Task.WhenAll(dispatchingTasks);
        return result;
    }
}