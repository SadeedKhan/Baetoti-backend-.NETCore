using AutoMapper;
using Baetoti.Core.Entites;
using Baetoti.Shared.Request.Auth;
using Baetoti.Shared.Request.Category;
using Baetoti.Shared.Request.SubCategory;
using Baetoti.Shared.Response.Category;
using Baetoti.Shared.Response.SubCategory;
using System;
using System.Linq;
using System.Reflection;

namespace Baetoti.API.Common.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());

            CreateMap<AuthRequest, User>();
            //CreateMap<RegisterUserRequest, User>();
            //CreateMap<UpdateUserRequest, User>();
            CreateMap<CategoryRequest, Category>();
            CreateMap<Category, CategoryResponse>();
            CreateMap<SubCategoryRequest, SubCategory>();
            CreateMap<SubCategory, SubCategoryResponse>();
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
                .ToList();
            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var methodInfo = type.GetMethod("Mapping")
                    ?? type.GetInterface("IMapFrom`1").GetMethod("Mapping");
                methodInfo?.Invoke(instance, new object[] { this });
            }
        }

    }
}
