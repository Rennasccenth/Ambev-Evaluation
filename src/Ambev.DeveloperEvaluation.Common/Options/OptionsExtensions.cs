using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Ambev.DeveloperEvaluation.Common.Options;

public static class OptionExtensions
{
    /// <summary>
    /// Register a <see cref="IOptions{Type}"/> 
    /// </summary>
    /// <param name="services">Updating <see cref="IServiceCollection"/></param>
    /// <param name="sectionName">Section that contains the configuring option.</param>
    /// <param name="validateOnStart">Optional parameter that informs if the option should be validated before retrieval.</param>
    /// <typeparam name="T">Type to be mapped out from configuration.</typeparam>
    /// <returns>Updated <see cref="IServiceCollection"/></returns>
    public static IServiceCollection RegisterOption<T>(
        this IServiceCollection services,
        string sectionName,
        bool validateOnStart = true) where T : class
    {
        OptionsBuilder<T> optionsBuilder = services.AddOptions<T>()
            .BindConfiguration(sectionName);

        if (validateOnStart)
        {
            optionsBuilder.ValidateDataAnnotations()
                .ValidateOnStart();
        }
            
        return services;
    }

    public static IServiceCollection RegisterOption<T>(this IServiceCollection services, bool validateOnStart = true) where T : class 
        => services.RegisterOption<T>(sectionName: typeof(T).Name, validateOnStart);
    
    public static IServiceCollection RegisterOption<T>(
        this IServiceCollection services,
        T instance,
        ServiceLifetime serviceLifetime = ServiceLifetime.Singleton ) where T : class
    {
        ArgumentNullException.ThrowIfNull(instance);

        switch (serviceLifetime)
        {
            case ServiceLifetime.Singleton:
                services.TryAddSingleton<IOptions<T>>(new OptionsWrapper<T>(instance));
                break;
            case ServiceLifetime.Scoped:
                services.TryAddScoped<IOptions<T>>(_ => new OptionsWrapper<T>(instance));
                break;
            case ServiceLifetime.Transient:
                services.TryAddTransient<IOptions<T>>(_ => new OptionsWrapper<T>(instance));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(serviceLifetime), serviceLifetime, null);
        }

        return services;
    }
}
