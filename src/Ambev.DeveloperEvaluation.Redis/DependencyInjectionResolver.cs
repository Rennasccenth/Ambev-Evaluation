using Ambev.DeveloperEvaluation.Common.Options;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Sales.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Ambev.DeveloperEvaluation.Redis;

public static class DependencyInjectionResolver
{
    public static IServiceCollection InstallRedisInfrastructure(this IServiceCollection serviceCollection)
    {
        serviceCollection.RegisterOption<RedisSettings>(RedisSettings.SectionName);
        serviceCollection.AddTransient<ISalesCounter, RedisSalesCounter>();

        serviceCollection.AddSingleton<IConnectionMultiplexer>(provider =>
        {
            RedisSettings redisSettings = provider.GetRequiredService<IOptions<RedisSettings>>().Value;
            var configurationOptions = new ConfigurationOptions
            {
                EndPoints = { redisSettings.ConnectionString },
                AbortOnConnectFail = false,
                ReconnectRetryPolicy = new ExponentialRetry((int)redisSettings.RetryDelayInMilliseconds),
                ConnectTimeout = (int)redisSettings.CommandTimeoutInSeconds, 
                SyncTimeout = (int)redisSettings.SyncTimeout
            };
    
            return ConnectionMultiplexer.Connect(configurationOptions);
        });
        
        serviceCollection.AddScoped<IDatabase>(serviceProvider =>
        {
            IConnectionMultiplexer multiplexer = serviceProvider.GetRequiredService<IConnectionMultiplexer>();
            return multiplexer.GetDatabase();
        });

        return serviceCollection;
    }
}