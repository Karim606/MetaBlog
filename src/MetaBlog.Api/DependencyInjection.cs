using MetaBlog.Api.Common;
using MetaBlog.Api.Common.RouteConstraints;
using MetaBlog.Api.Infrastructure;
using MetaBlog.Api.OpenApi.Transformers;
using MetaBlog.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using Serilog;
using System.Reflection.Metadata;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
namespace MetaBlog.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
        {
            // Add services to the container.
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer()
                .AddRouteContraints()
                .AddCustomProblemDetails()
                .AddCustomApiVersioning()
                .AddApiDocumentation()
                .AddExceptionHandling()
                .AddControllersWithJsonConfiguration()
                .AddIdentityInfrastructure()
                .AddAppRateLimiting()
                .AddOutputCaching();
              

            return services;
        }

        public static IServiceCollection AddRouteContraints(this IServiceCollection services) {
            services.Configure<RouteOptions>(options =>
            {
                options.ConstraintMap.Add("LikeTargetType", typeof(LikeTargetTypeConstraint));
            });
            return services;
        }

        public static IServiceCollection AddOutputCaching(this IServiceCollection services)
        {
            services.AddOutputCache(options =>
            {
                options.SizeLimit = 1024 * 1024 * 100; // 100 MB
                options.AddBasePolicy(builder =>
                {
                    builder.Expire(TimeSpan.FromSeconds(60))
                           .SetVaryByHeader("Accept-Encoding")
                           .SetVaryByQuery("page", "pageSize");
                });
            });
            return services;
        }
        public static IServiceCollection AddAppRateLimiting(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {

                options.AddSlidingWindowLimiter("slidingwindow", limiterOptions =>
                {
                    limiterOptions.AutoReplenishment = true;
                    limiterOptions.PermitLimit = 100;
                    limiterOptions.QueueLimit = 10;
                    limiterOptions.Window = TimeSpan.FromMinutes(1);
                    limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    limiterOptions.SegmentsPerWindow = 6;
                });

                options.RejectionStatusCode = 429;
                options.OnRejected = async (context, token) =>
                {
                    context.HttpContext.Response.Headers.RetryAfter = "60";
                    await Task.CompletedTask;
                };
            });
            return services;
        }

        public static IServiceCollection AddCustomProblemDetails(this IServiceCollection services)
        {

            services.AddProblemDetails(options => options.CustomizeProblemDetails = (context) =>
                {
                    context.ProblemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;
                    context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";

                });
            return services;
        }


        public static IServiceCollection AddCustomApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            }).AddMvc();
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            return services;


        }
        public static IServiceCollection AddApiDocumentation(this IServiceCollection services)
        {
            string[] versions = ["v1"];
            foreach (var version in versions)
            {
                services.AddOpenApi(options =>
                {
                    options.AddDocumentTransformer<VersionInfoTransformer>();
                    options.AddDocumentTransformer<BearerSchemeSecurityTransformer>();
                    options.AddOperationTransformer<BearerSchemeSecurityTransformer>();
                    //options.AddSchemaTransformer<EnumSchemaTransformer>();

                });
            }
            return services;
        }

        public static IServiceCollection AddExceptionHandling(this IServiceCollection services)
        {
            services.AddExceptionHandler<GlobalExceptionHandler>();
            return services;
        }

        public static IServiceCollection AddControllersWithJsonConfiguration(this IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                });
             services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            return services;
        }

        public static IServiceCollection AddIdentityInfrastructure(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<ICurrentRequestContext, CurrentRequestContext>();
            return services;
        }

        public static IApplicationBuilder UseCoreMiddlewares(this IApplicationBuilder app,IConfiguration configuration)
        {
            app.UseExceptionHandler();

            app.UseStatusCodePages();

            app.UseHttpsRedirection();

            app.UseSerilogRequestLogging();

            app.UseRateLimiter();

            app.UseAuthentication();

            app.UseAuthorization();
            
            app.UseStaticFiles();

            app.UseOutputCache();

            return app;
        }
    }
}
