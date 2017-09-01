using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestWebApplication.WebUI.Controllers;
using TestWebApplication.Domain.Abstract;
using Moq;
using TestWebApplication.WebUI.Infrastructure.Mappers;
using TestWebApplication.Domain.Entities;
using TestWebApplication.WebUI.Models;
using System.Collections.Generic;
using AutoMapper;
using System.Linq;


namespace TestWebApplication.WebUI.Tests.Controllers
{
    [TestClass]
    public class ShoppingCartControllerTest
    {
        [TestMethod]
        public void MappingIsValid()
        {
            ICustomMapper mapper = new CustomMapper();
            ShoppingCartViewModel cart = new ShoppingCartViewModel()
            {
                CartId = "8",
                CartTotal = 800,
                Count = 48,
                OrderInfoDto = new List<OrderInfoDto>
                {
                    new OrderInfoDto()
                    {
                        OrderDetailId = 123,
                        OrderId = 214,
                        PricePerItem = 234,
                        Quantity = 10
                    }
                },
                UserInfoDto = new UserInfoDto() 
                {
                    Address = "NewAddress",
                    Name = "Ivan",
                    Email = "qwdqewf@daef.ew",
                    Phone = "1234342",
                }
            };
            Order order = new Order()
            {
                UserInfo = mapper.Map<UserInfo>(cart.UserInfoDto),
                OrderDetails = mapper.Map<List<OrderDetail>>(cart.OrderInfoDto)
                
            };
            
            Assert.AreEqual(order.UserInfo.Address, cart.UserInfoDto.Address);
            Assert.AreNotEqual(order.OrderDetails.First(), cart.OrderInfoDto.First());
            Assert.AreEqual(order.OrderDetails.First().PricePerItem, cart.OrderInfoDto.First().PricePerItem);

            order.UserInfo.Address = "Dom";
            cart.UserInfoDto = mapper.Map<UserInfoDto>(order.UserInfo);
            order.OrderDetails.Add(new OrderDetail()
            {
                OrderDetailId = 5,
                OrderId = 2,
                PricePerItem = 4,
                Quantity = 2
            });
            cart.OrderInfoDto = mapper.Map<List<OrderInfoDto>>(order.OrderDetails);

            Assert.AreEqual(order.UserInfo.Address, cart.UserInfoDto.Address);
            Assert.AreEqual(order.OrderDetails.ElementAt(1).OrderDetailId,
                order.OrderDetails.ElementAt(1).OrderDetailId);

        }

        //[TestMethod]
        //public void MapperConfCheck()
        //{
        //    Mapper.Initialize(cfg =>
        //        {
        //            cfg.CreateMap<Order, ShoppingCartViewModel>();
        //            //cfg.CreateMap<ShoppingCartViewModel, Order>();
        //            //cfg.CreateMap<OrderDetail, OrderInfoDto>();
        //            //cfg.CreateMap<OrderInfoDto, OrderDetail>();
        //            cfg.CreateMap<UserInfo, UserInfoDto>();
        //            //cfg.CreateMap<OrderDetail, OrderInfoDto>()
        //            //.ForMember(dest => dest.ThmbImg, opt => opt.Ignore())
        //            //.ForMember(dest => dest.ProductName, opt => opt.Ignore());
        //            //cfg.CreateMap<OrderInfoDto, OrderDetail>()
        //            //    .ForMember(dest => dest.Order, opt => opt.Ignore())
        //            //    .ForMember(dest => dest.Product, opt => opt.Ignore());
        //        });
        //}
    }
}
