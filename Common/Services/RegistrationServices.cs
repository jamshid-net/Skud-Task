using Autofac;
using Autofac.Extensions.DependencyInjection;
using Common.Configure;
using Common.Enums.EnumUniqueValueSettings;
using Common.Helpers;
using Common.JwtAuth;
using Common.ResponseResult;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using Serilog;
using Serilog.Events;
using System.Reflection;

namespace Common.Services;

public static class RegistrationServices
{
    /// <summary>
    /// Register services with AutoFac
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="applicationName"></param>
    /// <returns></returns>
    public static WebApplicationBuilder RegisterAppServices(this WebApplicationBuilder builder, string applicationName)
    {

        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterAssemblyTypes(Assembly.Load(applicationName))
                                                                          .Where(t => t.Name.EndsWith("Service"))
                                                                          .AsImplementedInterfaces());
        return builder;
    }

    public static IServiceCollection AddCommonServices(this IServiceCollection services, Assembly assembly)
    {

        var executingAppName = assembly.GetName().Name;
        EnumValidator.ValidateEnumValues(assembly);
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        //var logPath = environment == Environments.Development ? "logs" : $@"C:\inetpub\logs\eRecruitingLogs\{executingAppName}";
        var logPath = "logs";
        //add log
        Log.Logger = new LoggerConfiguration()
                           .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                           .Enrich.FromLogContext()
                           //.WriteTo.File(@$"{logPath}\log.log", LogEventLevel.Warning, rollingInterval: RollingInterval.Day)
                           .WriteTo.Console(LogEventLevel.Warning)
                           .CreateLogger();
        Log.Fatal("App start!");

        services.Configure<ForwardedHeadersOptions>(cfg =>
        {
            cfg.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        });

        //add cors
        services.AddCors(options =>
        {
            options.AddPolicy(
               name: "AllowOrigin",
               builder =>
               {
                   builder
                  // .AllowAnyOrigin()
                  .WithOrigins("http://localhost:4217",
                               "http://localhost:4216",
                               "http://localhost:4218",
                               "http://localhost:4200",
                               "http://localhost:5200",
                               "http://localhost:6200",
                               "http://localhost:5000",
                               "http://localhost:5001",
                               "http://localhost:5002",
                               "http://localhost:5003",
                               "http://localhost:5004",
                               "http://localhost:5005",
                               "http://localhost:82",
                               "http://localhost:55463",
                               "http://0.0.0.0:5000",
                               "http://localhost:55463")
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
               });
        });

        //add swagger
        services.AddSwaggerDocument(
            document =>
            {
                document.AddSecurity("Bearer Token", Enumerable.Empty<string>(),
                    new OpenApiSecurityScheme
                    {
                        Type = OpenApiSecuritySchemeType.ApiKey,
                        Name = "Authorization",
                        In = OpenApiSecurityApiKeyLocation.Header,
                        Scheme = "Bearer",
                        Description = "Type into the textbox: Bearer {your JWT token}.",

                    });

                document.OperationProcessors.Add(
                    new AspNetCoreOperationSecurityScopeProcessor("Bearer Token"));
                document.UseControllerSummaryAsTagDescription = true;



            });

        // Adding Authentication  
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        // Adding Jwt Bearer  
        .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = AuthOptions.Issuer,
                    ValidateAudience = true,
                    ValidAudience = AuthOptions.Audience,
                    ValidateLifetime = true,
                    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,

                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Append("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };

            });

        services.AddControllers()
                .AddNewtonsoftJson(x =>
                {
                    x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    #region DateConverters
                    x.SerializerSettings.Converters.Add(new CustomDateTimeConverter());
                    x.SerializerSettings.Converters.Add(new CustomDateTimeNullableConverter());
                    x.SerializerSettings.Converters.Add(new CustomDateOnlyConverter());
                    x.SerializerSettings.Converters.Add(new CustomDateOnlyNullableConverter());
                    #endregion
                });
        services.AddHttpContextAccessor();

        return services;
    }

    public static WebApplication CommonConfigure(this WebApplication app)
    {
        //if (app.Environment.IsDevelopment())
        {
            app.UseOpenApi();
            app.UseSwaggerUi();
        }

        app.CustomExceptionMiddleware();
        app.UseForwardedHeaders();
        app.UseCors("AllowOrigin");

        app.UseRouting();

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        if (app.Services.GetService<IHttpContextAccessor>() != null)
            HttpContextHelper.Accessor = app.Services.GetRequiredService<IHttpContextAccessor>();
        return app;
    }

}

