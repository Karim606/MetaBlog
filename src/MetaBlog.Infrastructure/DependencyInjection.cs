using MetaBlog.Application.Common.Interfaces;
using MetaBlog.Application.Features.Comments;
using MetaBlog.Application.Features.Favorites;
using MetaBlog.Application.Features.Follow;
using MetaBlog.Application.Features.Likes;
using MetaBlog.Application.Features.Posts;
using MetaBlog.Domain.RepositoriesInterfaces;
using MetaBlog.Infrastructure.Common.Interfaces;
using MetaBlog.Infrastructure.Data;
using MetaBlog.Infrastructure.Identity;
using MetaBlog.Infrastructure.QueryServices.CommentQueryService;
using MetaBlog.Infrastructure.QueryServices.FavoriteQueryService;
using MetaBlog.Infrastructure.QueryServices.FollowQueryService;
using MetaBlog.Infrastructure.QueryServices.LikesQueryServer;
using MetaBlog.Infrastructure.QueryServices.PostQueryService;
using MetaBlog.Infrastructure.Repositories;
using MetaBlog.Infrastructure.Services;
using MetaBlog.Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
namespace MetaBlog.Extensions.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection Services, IConfiguration Configuration)
        {
            

            Services.AddDbContext(Configuration)
                   .AddJwtService(Configuration)
                   .AddConfigurations(Configuration)
                   .AddServices()
                   .AddRepositories()
                   .AddQueryServices()
                   .AddHybridCache();
            Services.AddScoped<IIdentityService, IdentityService>();

            return Services;
        }
        private static IServiceCollection AddDbContext(this IServiceCollection Services, IConfiguration Configuration)
        {
            var ConnectionString = Configuration.GetConnectionString("Default");
            Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(ConnectionString));
            Services.AddIdentity<IdentityAppUser, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 10;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.SignIn.RequireConfirmedAccount = false;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            Services.AddScoped<DbIntialiser>();

            return Services;
        }

        public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailSettings>(
            configuration.GetSection("EmailSettings"));

            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));




            return services;
        }

        private static IServiceCollection AddHybridCache(this IServiceCollection Services)
        {
            Services.AddHybridCache(options => options.DefaultEntryOptions = new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(10),
                LocalCacheExpiration = TimeSpan.FromMinutes(30)
            });
            return Services;
        }

        private static IServiceCollection AddJwtService(this IServiceCollection Services, IConfiguration Configuration)
        {
            #region JwtSettings
            Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["JwtSettings:Issuer"],
                    ValidAudience = Configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration["JwtSettings:SecretKey"])),
                    RoleClaimType = ClaimTypes.Role
                };
            });
            #endregion
            Services.AddScoped<IJwtService, JwtService>();
            return Services;
        }

        private static IServiceCollection AddServices(this IServiceCollection Services)
        {

            Services.AddScoped<IEmailService, SmtpEmailService>();
            return Services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection Services)
        {
            Services.AddScoped<IPostRepository, PostRepository>();
            Services.AddScoped<ICommentRepository, CommentRepository>();
            Services.AddScoped<ILikeRepository, LikeRepository>(); 
            Services.AddScoped<IFavoriteRepository,FavoriteRepository>();
            Services.AddScoped<IDomainUserRepository, DomainUserRepository>();
            Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            Services.AddScoped<IFollowRepository, FollowRepository>();
            return Services;
        }
        public static IServiceCollection AddQueryServices(this IServiceCollection Services)
        {
            Services.AddScoped<IPostQueryService, PostQueryService>();
            Services.AddScoped<ICommentQueryService, CommentQueryService>();
            Services.AddScoped<IFavoriteQueryService, FavoriteQueryService>();
            Services.AddScoped<IFollowQueryService, FollowQueryService>();
            Services.AddScoped<ILikeQueryService, LikesQueryService>();
            return Services;
        }
    }
}