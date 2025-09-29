
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

            // Add services to the container.

            builder.Services.AddPresentation(builder.Configuration)
                            .AddApplication()
                           .AddInfrastructure(builder.Configuration);

            builder.Host.UseSerilog((context, Loggerconfig) => Loggerconfig.ReadFrom.Configuration(context.Configuration));
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
