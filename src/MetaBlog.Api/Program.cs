
using DotNetEnv;
using MetaBlog.Application;
using MetaBlog.Extensions.DependencyInjection;
using MetaBlog.Infrastructure.Data;
using Serilog;
using System.Threading.Tasks;

namespace MetaBlog.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
            Env.Load($"../../.env.{envName.ToLower()}");

            builder.Configuration.AddEnvironmentVariables();
            // Add services to the container.

            builder.Services.AddPresentation(builder.Configuration)
                            .AddApplication()
                           .AddInfrastructure(builder.Configuration);

            builder.Host.UseSerilog((context, loggerConfig) => {
                loggerConfig.ReadFrom.Configuration(context.Configuration);
                var sourceToken = builder.Configuration["BETTERSTACK_SOURCE_TOKEN"];
                var ingestHost = builder.Configuration["BETTERSTACK_INGEST_URL"];
                loggerConfig.WriteTo.BetterStack(sourceToken: sourceToken, betterStackEndpoint: ingestHost);

            });
            var app = builder.Build();

        
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwaggerUI(options => {
                    options.SwaggerEndpoint("/openapi/v1.json", "MetaBlog API V1"); 
                    options.EnableDeepLinking();
                    options.DisplayRequestDuration();
                    options.EnableFilter();
                    });
               await app.Init();
            }
            else
            {
                app.UseHsts();
            }
     
            app.UseCoreMiddlewares(builder.Configuration);
            app.MapControllers();
            app.UseAntiforgery();
            app.MapStaticAssets();

            app.Run();
        }
    }
}
