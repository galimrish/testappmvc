using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace TestWebApplication.WebUI.Infrastructure.Mappers
{
    public class CustomMapper : ICustomMapper
    {
        MapperConfiguration _config;
        public CustomMapper()
        {
            var profileType = typeof(Profile);
            // Get an instance of each Profile in the executing assembly.
            var profiles = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => profileType.IsAssignableFrom(t)
                    && t.GetConstructor(Type.EmptyTypes) != null)
                .Select(Activator.CreateInstance)
                .Cast<Profile>();
            _config = new MapperConfiguration(c =>
            {
                foreach (var profile in profiles)
                {
                    c.AddProfile(profile);
                }
            });
            //_config = new MapperConfiguration(cfg => cfg.AddProfile<CustomProfile>());
            
        }

        public object Map(object source, Type sourceType, Type destinationType)
        {
            return _config.CreateMapper().Map(source, sourceType, destinationType);
        }

        public TDestination Map<TDestination>(object source)
        {
            return _config.CreateMapper().Map<TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return _config.CreateMapper().Map<TSource, TDestination>(source);
        }
    }
}