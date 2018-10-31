using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Gateway
{
    public class Program
    {
        private static string CurrentDirectory => Directory.GetCurrentDirectory();
        
        private static IConfiguration Configuration 
        {
            get
            {
                var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                var builder = new ConfigurationBuilder();

                if (string.Equals(env, "production", StringComparison.OrdinalIgnoreCase))
                    builder.AddJsonFile("appsettings.json", false, true);
                else
                    builder.AddJsonFile($"appsettings.{env}.json", false);

                builder.AddEnvironmentVariables();
                return builder.Build();
            }
        }
        
        
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                BuildWebHost(args).Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseContentRoot(CurrentDirectory)
                .UseWebRoot(Path.Combine(CurrentDirectory, "public"))
                .UseStartup<Startup>()
                .UseSerilog()
                .Build();
        }
}