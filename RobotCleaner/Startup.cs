using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RobotCleaner.Classes;
using RobotCleaner.Interfaces;
using Serilog;
using Serilog.Events;
using ILogger = Serilog.ILogger;

namespace RobotCleaner
{
    internal static class Startup
    {
        public static IServiceProvider CreateServiceProvider()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            services.AddLogging((loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog();
            }));

            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.Register<ILogger>((_) => new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.Console(LogEventLevel.Information)
                    .WriteTo.File("logs.log")
                    .CreateLogger())
                .SingleInstance();

            IContainer container = builder.Build();
            return container.Resolve<IServiceProvider>();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddLogging();
            services.AddScoped<IRobot, Robot>();
            services.AddScoped<ICompass, Compass>();
            services.AddScoped<IBattery, Battery>();
            services.AddScoped<INavigation, Navigation>();
        }
    }
}