using System.Reflection;
using AutoMapper;
using Core.User;
using Repository.User;

namespace Gateway.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserModel, UserEntity>().IgnoreAllNonExisting();
            CreateMap<UserEntity, UserModel>().IgnoreAllNonExisting();
        }
    }

    public static class AutoMapperExtensions 
    {
        public static IMappingExpression<TSource, TDestination> IgnoreAllNonExisting<TSource, TDestination> (this IMappingExpression<TSource, TDestination> expression)
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            var sourceType = typeof(TSource);
            var destinationProperties = typeof(TDestination).GetProperties(flags);

            foreach (var property in destinationProperties)
            {
                if (sourceType.GetProperty(property.Name, flags) == null)
                {
                    expression.ForMember(property.Name, opt => opt.Ignore());
                }
            }
            return expression;
        }
    }
}