using Ambev.DeveloperEvaluation.Common.HealthChecks;
using Ambev.DeveloperEvaluation.Common.Logging;
using Ambev.DeveloperEvaluation.IoC;

namespace Ambev.DeveloperEvaluation.WebApi;

public sealed class Program
{
    public static void Main(string[] args)
    {
        try
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            builder.AddDefaultLogging();

            builder.Services
                .InstallApiDependencies(builder.Configuration)
                .RegisterDependenciesServices();

            WebApplication app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseExceptionHandler();
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseBasicHealthChecks();

            app.MapControllers();

            app.Run();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}
