using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using TestWebApplication.Domain.Entities;
using TestWebApplication.WebUI.Models;

namespace TestWebApplication.WebUI.Infrastructure.Mappers
{
    public class CustomProfile : Profile
    {
        public CustomProfile()
        {
            CreateMap<UserInfo, UserInfoDto>().ReverseMap();
            CreateMap<OrderDetail, OrderInfoDto>().ReverseMap();
        }
    }
}