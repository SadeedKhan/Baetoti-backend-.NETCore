using Baetoti.Core.Interface.Base;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Core.Interface.Services;
using Baetoti.Infrastructure.Data.Context;
using Baetoti.Infrastructure.Data.Repositories;
using Baetoti.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections.Generic;

namespace Baetoti.IoC
{
    public class DependencyContainer
    {
        public static void RegisterBaetotiApiServices(IServiceCollection services)
        {

            #region Singleton
            services.AddSingleton<IEncryptionService, EncryptionService>();
            services.TryAdd(ServiceDescriptor.Singleton(typeof(IAppLogger<>), typeof(AppLoggerService<>)));
            #endregion

            #region Scoped
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IArgon2Service, HashingService>();
            #endregion

        }

        public static void RegisterBaetotiApiRepositories(IServiceCollection services)
        {
            ServiceProvider provider = services.BuildServiceProvider();
            var loggingDbContext = provider.GetRequiredService<LoggingDbContext>();

            #region Singleton
            services.AddSingleton<IExceptionRepository>(x => new ExceptionRepository(loggingDbContext));
            #endregion

            #region Scoped
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            #endregion

        }

    }
}
