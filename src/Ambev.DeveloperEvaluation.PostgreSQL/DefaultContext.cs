using System.Reflection;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Products;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Users;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.PostgreSQL.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.PostgreSQL;

public class DefaultContext : DbContext
{
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }

    public DefaultContext(DbContextOptions<DefaultContext> options, IDomainEventDispatcher domainEventDispatcher)
        : base(options)
    {
        _domainEventDispatcher = domainEventDispatcher;
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new DomainEventInterceptor(_domainEventDispatcher));
        base.OnConfiguring(optionsBuilder);
    }
}