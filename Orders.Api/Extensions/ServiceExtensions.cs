using AspNetCoreRateLimit;
using Orders.Contracts;
using Orders.LoggerService;
using Orders.Service.Contracts;
using Orders.Service;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orders.Repository;
using DotNetEnv.Configuration;
using DotNetEnv;


namespace Orders.Api.Extensions
{
    public static class ServiceExtensions
    {
        // CORS cross-origin resource sharing

        // AllowAnyOrigin -> WithOrigins("example.com")
        // AllowAnyMethod -> WithMethods("POST", "GET")
        // AllowAnyHeader -> WithHeaders("accept", "content-type")
        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithExposedHeaders("X-Pagination"));
            });

        public static void ConfigureIISIntegration(this IServiceCollection services) =>
            services.Configure<IISOptions>(options =>
            {
            });

        public static void ConfigureLoggerService(this IServiceCollection services) =>
            services.AddSingleton<ILoggerManager, LoggerManager>();

        public static void ConfigureRepositoryManager(this IServiceCollection services) =>
            services.AddScoped<IRepositoryManager, RepositoryManager>();

        // public static void ConfigureServiceManager(this IServiceCollection services) =>
        //     services.AddScoped<IServiceManager, ServiceManager>();


        public static void ConfigureEnvironmentVariables(this IServiceCollection services) =>
            new ConfigurationBuilder()
                .AddDotNetEnv(".env", LoadOptions.TraversePath()) // Simply add the DotNetEnv configuration source!
                .Build();
            
        public static void ConfigureMySqlContext(this IServiceCollection services, IConfiguration configuration) {
            var connectionString = configuration.GetConnectionString("mysqlConnection")
                ?? throw new InvalidOperationException("please Set your mysqlConnection string");

            var serverVersion = ServerVersion.AutoDetect(connectionString);
            services.AddDbContext<RepositoryContext>(opts => opts.UseMySql(connectionString, serverVersion, b => b.MigrationsAssembly("Orders.Api")));

        }
        public static IMvcBuilder AddCustomCSVFormatter(this IMvcBuilder builder) => builder.AddMvcOptions(config => config.OutputFormatters.Add(new
            CsvOutputFormatter()));

        public static void ConfigureVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(opt =>
            {
                opt.ReportApiVersions = true;
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ApiVersionReader = new HeaderApiVersionReader("api-version");
                //opt.Conventions.Controller<SampleController>()
                //    .HasApiVersion(new ApiVersion(1, 0));
                //opt.Conventions.Controller<SampleV2Controller>()
                //    .HasDeprecatedApiVersion(new ApiVersion(2, 0));
            }
                );
        }

        public static void ConfigureResponseCaching(this IServiceCollection services) => services.AddResponseCaching();

        public static void ConfigureHttpCacheHeaders(this IServiceCollection services) =>
            services.AddHttpCacheHeaders((expirationOpt) =>
            {
                expirationOpt.MaxAge = 65;
                expirationOpt.CacheLocation = CacheLocation.Private;
            },
            (validationOpt) =>
            {
                validationOpt.MustRevalidate = true;
            });

        public static void ConfigureRateLimitingOptions(this IServiceCollection services)
        {
            var rateLimitRules = new List<RateLimitRule>
            {
                new RateLimitRule
                {
                    Endpoint = "*",
                    Limit = 100,
                    Period = "1m"
                }
            };

            services.Configure<IpRateLimitOptions>(opt => { opt.GeneralRules = rateLimitRules; });
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        }



        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Orders API",
                    Version = "v1",

                    Description = "Orders API for handling making of Orders",
                    
                });

                s.SwaggerDoc("v2", new OpenApiInfo { Title = "Orders API", Version = "v2" });

                var xmlFile = $"{typeof(Presentation.AssemblyReference).Assembly.GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                s.IncludeXmlComments(xmlPath);

                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Place to add JWT with Bearer",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                s.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Name = "Bearer",
                        },
                        new List<string>()
                    }
            });
            }
            );
        }
    }

}
