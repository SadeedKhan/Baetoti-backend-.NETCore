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
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRolePrivilegeRepository, RolePrivilegeRepository>();
            services.AddScoped<IEmployeeRoleRepository, EmployeeRoleRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
            services.AddScoped<ITagsRepository, TagsRepository>();
            services.AddScoped<IUnitRepository, UnitRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IDesignationRepository, DesignationRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IItemTagRepository, ItemTagRepository>();
            services.AddScoped<IOTPRepository, OTPRepository>();
            services.AddScoped<IProviderRepository, ProviderRepository>();
            services.AddScoped<IDriverRepository, DriverRepository>();
            services.AddScoped<IItemReviewRepository, ItemReviewRepository>();
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<ISubMenuRepository, SubMenuRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            services.AddScoped<IDriverOrderRepository, DriverOrderRepository>();
            services.AddScoped<IProviderOrderRepository, ProviderOrderRepository>();

            #endregion

        }

    }
}
