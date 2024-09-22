
using Common.Services;
using Skud.Application;
using Skud.Application.Interfaces;
using Skud.Application.Services;
using Skud.Infrastructure;
using System.Reflection;

namespace Skud.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.RegisterAppServices("Skud.Application");
        builder.Services.AddScoped<ICurrentUser, CurrentUser>();
        builder.Services
            .AddCommonServices(Assembly.GetExecutingAssembly())
            .AddApplicationServices(builder.Configuration)
            .AddInfrastructureServices(builder.Configuration);

        var app = builder.Build();
        app.CommonConfigure();
        app.Run();
    }
}
