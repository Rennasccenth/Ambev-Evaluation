using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Ambev.DeveloperEvaluation.Common.Monitoring;

public static class OpenTelemetryExtension
{
    public static IHostApplicationBuilder ConfigureOpenTelemetry(this IHostApplicationBuilder hostApplicationBuilder)
    {
        hostApplicationBuilder.Logging.AddOpenTelemetry(openTelemetryLoggerOptions =>
        {
            openTelemetryLoggerOptions.IncludeFormattedMessage = true;
            openTelemetryLoggerOptions.IncludeScopes = true;
        });

        hostApplicationBuilder.Services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(hostApplicationBuilder.Environment.ApplicationName))
            .WithMetrics(meterProviderBuilder =>
            {
                meterProviderBuilder
                    .AddRuntimeInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation();
            })
            .WithTracing(tracerProviderBuilder =>
            {
                if (hostApplicationBuilder.Environment.IsDevelopment())
                {
                    tracerProviderBuilder.SetSampler<AlwaysOnSampler>();
                }

                tracerProviderBuilder
                    .AddGrpcClientInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation();
            })
            .WithLogging()
            .UseOtlpExporter();

        return hostApplicationBuilder;
    }
}