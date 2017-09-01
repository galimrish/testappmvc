using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace TestWebApplication.WebUI.Infrastructure.Mappers
{
    public interface ICustomMapper
    {
        TDestination Map<TDestination>(object source);
        TDestination Map<TSource, TDestination>(TSource source);
        object Map(object source, Type sourceType, Type destinationType);

    }
}
