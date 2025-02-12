﻿using Ambev.DeveloperEvaluation.Application;
using Ambev.DeveloperEvaluation.Domain;
using Ambev.DeveloperEvaluation.MongoDB;
using Ambev.DeveloperEvaluation.PostgreSQL;
using Ambev.DeveloperEvaluation.Redis;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.DeveloperEvaluation.IoC;

public static class DependencyInjectionResolver
{
    /// <summary>
    /// Register dependencies from all related modules.
    /// </summary>
    public static void RegisterDependenciesServices(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .InstallRedisInfrastructure()
            .InstallMongoDbInfrastructure()
            .InstallPostgreSqlInfrastructure()
            .InstallApplicationLayer()
            .InstallDomainServices();
    }
}