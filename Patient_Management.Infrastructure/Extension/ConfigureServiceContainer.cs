using Patient_Management.Core.Cache;
using Patient_Management.Core.Contract;
using Patient_Management.Core.Contract.Repository;
using Patient_Management.Core.Implementation;
using Patient_Management.Core.Repository;
using Patient_Management.Domain.Entities;
using Patient_Management.Domain.Settings;
using Patient_Management.Infrastructure.Configs;
using Patient_Management.Persistence;
using AspNetCoreRateLimit;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Polly;
using Polly.Extensions.Http;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Patient_Management.Infrastructure.Extension
{
    public static class ConfigureServiceContainer
    {
        public static void AddDatabaseContext(this IServiceCollection serviceCollection,
             IConfiguration configuration, IConfigurationRoot configRoot)
        {
            serviceCollection.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite(configuration
                    .GetConnectionString("DBConnectionString") ?? configRoot["ConnectionStrings:DBConnectionString"]
                , b =>
                {
                    b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                });
            });
        }

        //public static void AddDatabaseContext(this IServiceCollection serviceCollection, IConfiguration configuration)
        //{
        //    var databaseOptions = new DatabaseOptions();
        //    var databaseSection = configuration.GetSection("DatabaseOptions");
        //    databaseSection.Bind(databaseOptions);

        //    serviceCollection.AddDbContext<ApplicationDbContext>(options =>
        //    {
        //        options.UseSqlite(databaseOptions.ConnectionString, b =>
        //        {
        //            b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
        //        });
        //    });
        //}



        public static void AddTransientServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IUserService, UserService>();
            serviceCollection.AddTransient<IAppSessionService, AppSessionService>();
            serviceCollection.AddTransient<IAppointmentRepository, AppointmentRepository>();
            serviceCollection.AddTransient<IPatientRecordRepository, PatientRecordRepository>();
            serviceCollection.AddTransient<IPatientRepository, PatientRepository>();

        }

        public static void AddScopedServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
            serviceCollection.AddScoped<IUserService, UserService>();
            serviceCollection.AddScoped<IAppointmentService, AppointmentService>();
            serviceCollection.AddScoped<IPatientRecordService, PatientRecordService>();
            serviceCollection.AddScoped<IPatientService, PatientService>();
        }

        public static void AddRepositoryServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        }

        public static void AddJwtIdentityService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<User, IdentityRole>(opt =>
            {
                opt.Lockout.AllowedForNewUsers = true;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5000);
                opt.Lockout.MaxFailedAccessAttempts = 5;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = configuration["JWTSettings:Issuer"],
                        ValidAudience = configuration["JWTSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]))
                    };
                    o.Events = new JwtBearerEvents()
                    {
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            var code = HttpStatusCode.Unauthorized;
                            context.Response.StatusCode = (int)code;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject(new Domain.Common.Response<string>("You are not Authorized", (int)code));
                            return context.Response.WriteAsync(result);
                        },
                        OnForbidden = context =>
                        {
                            var code = HttpStatusCode.Forbidden;
                            context.Response.StatusCode = (int)code;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject(new Domain.Common.Response<string>("You are not authorized to access this resource", (int)code));
                            return context.Response.WriteAsync(result);
                        },
                    };
                });
        }

        public static void AddCustomOptions(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddOptions<JWTSettings>().BindConfiguration("JWTSettings");
            serviceCollection.AddOptions<AdminOptions>().BindConfiguration("AdminOptions");
            serviceCollection.AddOptions<ApiResourceUrls>().BindConfiguration("ApiResourceUrls");
        }

        public static void AddCustomAutoMapper(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddAutoMapper(typeof(MappingProfileConfiguration));
        }


        public static void AddSwaggerOpenAPI(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSwaggerGen(setupAction =>
            {

                setupAction.SwaggerDoc(
                    "OpenAPISpecification",
                    new OpenApiInfo()
                    {
                        Title = "Patient_Management WebAPI",
                        Version = "1",
                        Description = "API Details for Patient_Management System",
                        Contact = new OpenApiContact()
                        {
                            Email = "yome@thePatient_Managementsystem.com",
                            Name = "The Patient_Management system",
                            Url = new Uri(" https://thePatient_Managementsystem.com")
                        },
                        License = new OpenApiLicense()
                        {
                            Name = "UNLICENSED"
                        }
                    });

                setupAction.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = $"Input your Bearer token in this format - Bearer token to access this API",
                });
                setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                        }, new List<string>()
                    },
                });
            });
        }

        public static void AddCustomControllers(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddEndpointsApiExplorer();

            serviceCollection.AddControllersWithViews()
                .AddNewtonsoftJson(ops =>
                {
                    ops.SerializerSettings.NullValueHandling = NullValueHandling.Include;
                    ops.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
                });
            serviceCollection.AddRazorPages();

            serviceCollection.Configure<ApiBehaviorOptions>(apiBehaviorOptions =>
                apiBehaviorOptions.InvalidModelStateResponseFactory = actionContext =>
                {
                    var logger = actionContext.HttpContext.RequestServices.GetRequiredService<ILogger<BadRequestObjectResult>>();
                    IEnumerable<string> errorList = actionContext.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage);
                    logger.LogError("Bad Request");
                    logger.LogError(string.Join(",", errorList));
                    return new BadRequestObjectResult(new Domain.Common.Response<IEnumerable<string>>("Patient_Management Validation Error", 400, errorList));
                });
        }

        
        public static IServiceCollection AddInMemoryCache(this IServiceCollection services)
        {
            services.AddMemoryCache();

            if (services.Contains(ServiceDescriptor.Transient<ICacheService, InMemoryCacheService>()))
            {
                return services;
            }

            services.AddTransient<ICacheService, InMemoryCacheService>();
            return services;
        }

        public static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration config,
            Action<RedisCacheOptions> setupAction = null)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (services.Contains(ServiceDescriptor.Transient<ICacheService, DistributedCacheService>()))
            {
                return services;
            }

            var redisOptions = new RedisCacheOptions();
            var redisSection = config.GetSection("Redis");

            redisSection.Bind(redisOptions);
            services.Configure<RedisCacheOptions>(redisSection);
            setupAction?.Invoke(redisOptions);

            services.AddStackExchangeRedisCache(options =>
            {
                options.InstanceName = config[redisOptions.Prefix];
                options.ConfigurationOptions = GetRedisConfigurationOptions(redisOptions);
            });

            services.AddTransient<ICacheService, DistributedCacheService>();

            return services;
        }

        private static ConfigurationOptions GetRedisConfigurationOptions(RedisCacheOptions redisOptions)
        {
            var configurationOptions = new ConfigurationOptions
            {
                ConnectTimeout = redisOptions.ConnectTimeout,
                SyncTimeout = redisOptions.SyncTimeout,
                ConnectRetry = redisOptions.ConnectRetry,
                AbortOnConnectFail = redisOptions.AbortOnConnectFail,
                ReconnectRetryPolicy = new ExponentialRetry(redisOptions.DeltaBackoffMiliseconds),
                KeepAlive = 5,
                Ssl = redisOptions.Ssl
            };

            if (!string.IsNullOrWhiteSpace(redisOptions.Password))
            {
                configurationOptions.Password = redisOptions.Password;
            }

            var endpoints = redisOptions.Url.Split(',');
            foreach (var endpoint in endpoints)
            {
                configurationOptions.EndPoints.Add(endpoint);
            }

            return configurationOptions;
        }

        
    }
}
