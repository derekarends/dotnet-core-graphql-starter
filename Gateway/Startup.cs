using System;
using System.Text;
using AspNetCoreRateLimit;
using AutoMapper;
using Core.Settings;
using Gateway.Deps;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Prometheus;

namespace Gateway
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly ILoggerFactory _loggerFactory;
        
        public Startup(IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var builder = new ConfigurationBuilder();

            if (env.IsProduction())
                builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            else
                builder.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false);

            builder.AddEnvironmentVariables();
            _configuration = builder.Build();
            _loggerFactory = loggerFactory;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var logger = _loggerFactory.CreateLogger("baseLogger");
            
            services.AddOptions();
            services.AddAutoMapper();

            // Redis Cache
//            services.AddDistributedRedisCache(options =>
//            {
//                options.Configuration = "127.0.0.1";
//                options.InstanceName = "DocflowInstance";
//            });

            // Rate Limiting
            services.Configure<IpRateLimitOptions>(_configuration.GetSection("IpRateLimiting"));
            services.Configure<IpRateLimitPolicies>(_configuration.GetSection("IpRateLimitPolicies"));
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
//            services.AddSingleton<IIpPolicyStore, DistributedCacheIpPolicyStore>();
//            services.AddSingleton<IRateLimitCounterStore,DistributedCacheRateLimitCounterStore>();
            
            // Settings
            services.AddSingleton(o => new SettingsModel
            {
                Environment = _configuration["Environment"],
                Host = _configuration["Host"],
                ConnectionStrings = _configuration["ConnectionStrings:SampleApp"],
                Database = _configuration["Databases:SampleApp"],
                JwtKey = _configuration["Jwt:Key"],
                JwtIssuer = _configuration["Jwt:Issuer"],
                JwtExpiresInMinutes = int.Parse(_configuration["Jwt:ExpiresInMinutes"])
            });
            
            // JWT 
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ClockSkew = TimeSpan.Zero,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = _configuration["Jwt:Issuer"],
                        ValidAudience = _configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]))
                    };
                });
            

           // GraphQL
            services.AddGraphQL();
            
            // Interface Mappings
            services.AddTokenDeps(logger);
            services.AddUserDeps(logger);
            services.AddEmailDeps(logger);
            
            // CORS
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .Build());
            });
            
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddMvc();
            services.AddSession();
            services.AddApiVersioning();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            
            app.UseIpRateLimiting();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseMetricServer();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseSession();
            app.UseMvc();
        }
    }
}