﻿using AutoMapper;
using Baetoti.Core.Entites;
using Baetoti.Shared.Request.Auth;
using Baetoti.Shared.Request.Category;
using Baetoti.Shared.Request.SubCategory;
using Baetoti.Shared.Request.TagRequest;
using Baetoti.Shared.Request.UnitRequest;
using Baetoti.Shared.Request.User;
using Baetoti.Shared.Request.UserRole;
using Baetoti.Shared.Response.Category;
using Baetoti.Shared.Response.Department;
using Baetoti.Shared.Response.Designation;
using Baetoti.Shared.Response.SubCategory;
using Baetoti.Shared.Response.TagResponse;
using Baetoti.Shared.Response.Unit;
using Baetoti.Shared.Response.User;
using Baetoti.Shared.Response.UserRole;
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
            CreateMap<TagRequest, Tags>();
            CreateMap<Tags, TagResponse>();
            CreateMap<UnitRequest, Unit>();
            CreateMap<Unit, UnitResponse>();
            CreateMap<UserRequest, User>();
            CreateMap<User, UserResponse>();
            CreateMap<AssignRoleRequest, UserRoles>();
            CreateMap<UserRoles, UserRoleResponse>();
            CreateMap<Department, DepartmentResponse>();
            CreateMap<Designation, DesignationResponse>();
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
