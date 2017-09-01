using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestWebApplication.Domain.Abstract;
using TestWebApplication.Domain.Entities;
using TestWebApplication.WebUI.Models;

namespace TestWebApplication.WebUI.Infrastructure
{
    public class ShoppingCartBinder : IModelBinder
    {
        private const string CartSessionKey = "CartId";
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            
            string cartId = string.Empty;
            ShoppingCartViewModel model = (ShoppingCartViewModel)bindingContext.Model
                ?? new ShoppingCartViewModel();

            if(controllerContext.HttpContext.Session[CartSessionKey] == null)
            {
                if(!string.IsNullOrWhiteSpace(controllerContext.HttpContext.User.Identity.Name))
                {
                    controllerContext.HttpContext.Session[CartSessionKey] =
                        controllerContext.HttpContext.User.Identity.Name;
                }
                else
                {
                    Guid tempCartId = Guid.NewGuid();
                    controllerContext.HttpContext.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            cartId = controllerContext.HttpContext.Session[CartSessionKey].ToString();
            model.CartId = cartId;
            //if (repository.Carts != null && repository.Carts.Count() > 0)
            //{
            //    List<Cart> cartItems = repository.Carts.Where(c => c.CartId == cartId).ToList();
            //    decimal total = cartItems.Sum(c => c.Count != null ? c.Count * c.Product.Price : 0);
            //    int count = cartItems.Sum(c => c.Count != null ? c.Count : 0);

            //    model.CartItems = cartItems;
            //    model.CartTotal = total;
            //    model.Count = count;
            //}
            return model;
        }
    }
}