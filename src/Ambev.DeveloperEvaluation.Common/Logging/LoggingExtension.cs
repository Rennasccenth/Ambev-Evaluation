using Serilog;
using Serilog.Exceptions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Common.Logging;

/// <summary>
/// Add default Logging configuration to project.
/// </summary>
public static class LoggingExtension
{
    public static IHostApplicationBuilder ConfigureSerilog(this IHostApplicationBuilder hostApplicationBuilder)
    {
        hostApplicationBuilder.Logging.ClearProviders();

        hostApplicationBuilder.Services.AddSerilog(loggerConfiguration =>
            {
                loggerConfiguration
                    .Enrich.FromLogContext()
                    .Enrich.WithExceptionDetails()
                    .Enrich.WithCorrelationId()
                    .WriteTo.Async(writer => writer.Console());
            },
            preserveStaticLogger: true,
            writeToProviders: true);

        return hostApplicationBuilder;
    }
}
