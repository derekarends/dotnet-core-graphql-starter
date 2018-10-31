using AutoMapper;
using Core.Email;
using Core.Settings;
using Core.Token;
using Core.User;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Prometheus;
using Repository.Core;
using Repository.User;
using Service.User;

namespace Gateway.Deps
{
    public static class UserDeps
    {
        public static void AddUserDeps(this IServiceCollection services, ILogger logger)
        {
            var sp = services.BuildServiceProvider();

            var context = new MongoContext(sp.GetService<SettingsModel>());
            IUserRepository ur = new UserRepository(context, sp.GetService<IMapper>());
//            ur = new Repository.User.UserCache(ur, sp.GetService<IDistributedCache>());
            ur = new Repository.User.UserMetrics(ur,
                Metrics.CreateCounter("UserRepoCounter", "Number of requests received."), 
                Metrics.CreateSummary("UserRepoSummary", "Total duration of repo requests in milliseconds.", new SummaryConfiguration())
            );
            services.AddSingleton(ur);
            
            IUserService us = new UserService(ur, sp.GetService<IEmailService>(), sp.GetService<ITokenService>());
            us = new Service.User.UserMetrics(us, 
                Metrics.CreateCounter("UserServiceCounter", "Number of requests received."), 
                Metrics.CreateSummary("UserServiceSummary", "Total duration of requests in milliseconds.", new SummaryConfiguration())
            );
            services.AddSingleton(us);
        }
    }
}