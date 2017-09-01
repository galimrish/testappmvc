using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestWebApplication.Domain.Abstract;
using TestWebApplication.Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;

namespace TestWebApplication.WebUI.Models
{
    public partial class ShoppingCart
    {
        IProductRepository repository;
        private const string CartSessionKey = "CartId";
        public string ShoppingCartId { get; private set; }

        public ShoppingCart(HttpContextBase context, IProductRepository pr)
        {
            repository = pr;
            ShoppingCartId = GetCartId(context);
        }

        private string GetCartId(HttpContextBase context)
        {
            try
            {
                if (context.Session[CartSessionKey] == null)
                {
                    string userId = context.User.Identity.GetUserId();
                    if (!string.IsNullOrWhiteSpace(userId))
                    {
                        ApplicationUser user = context.GetOwinContext()
                            .GetUserManager<ApplicationUserManager>().FindById(userId);
                        if (repository.UserInfo.FirstOrDefault(u => u.Email == user.Email) == null)
                            repository.SaveUserInfo(new UserInfo()
                            {
                                Address = user.Address,
                                Email = user.Email,
                                Name = user.Name,
                                Phone = user.PhoneNumber,
                                Username = user.UserName
                            });
                        context.Session[CartSessionKey] = user.UserName;
                    }
                    else
                    {
                        Guid tempCartId = Guid.NewGuid();
                        context.Session[CartSessionKey] = tempCartId;
                    }
                }
                return context.Session[CartSessionKey].ToString();
            }
            catch
            {
                return null;
            }
        }

        public void ClearSession(HttpContextBase context)
        {
            context.Session[CartSessionKey] = null;
        }

        public List<Cart> CartItems
        {
            get
            {
                return repository.Carts.Where(
                    cart => cart.CartId == ShoppingCartId).ToList();
            }
        }

        public int CartItemsCount
        {
            get
            {
                return CartItems.Count;
            }
        }

        public decimal TotalPrice
        {
            get
            {
                return CartItems.Sum(c => c.Count != null ?
                    c.Count * c.Product.Price : 0);
            }
        }

        public int OrdersCount
        {
            get
            {
                return repository.Orders.Count();
            }
        }

        public UserInfo UserInfo
        {
            get
            {
                return repository.UserInfo.FirstOrDefault(ui => ui.Username == ShoppingCartId);
            }
        }

        public async Task<UserInfo> FindByEmailAsync(string email)
        {
            UserInfo user = await repository.FindUserByEmailAsync(email);
            return user;
        }

        //public void AddToCart(Product product)
        //{
        //    repository.AddProductToCart(ShoppingCartId, product);
        //}

        //public int RemoveFromCart(int id)
        //{
        //    return repository.RemoveFromCart(ShoppingCartId, id);
        //}

        //public void EmptyCart()
        //{
        //    repository.RemoveCart();
        //}

        ////public List<Cart> GetCartItems()
        ////{
        ////    return repository.Carts.Where(
        ////        cart => cart.CartId == ShoppingCartId).ToList();
        ////}

        //public bool SetProductCount(Product product, int count)
        //{
        //    return repository.SetProductCount(ShoppingCartId, product, count);
        //}

        ////public decimal GetTotal()
        ////{
        ////    // Multiply album price by count of that album to get 
        ////    // the current price for each of those albums in the cart
        ////    // sum all album price totals to get the cart total
        ////    //decimal? total = (from cartItems in repository.Carts
        ////    //                  where cartItems.CartId == ShoppingCartId
        ////    //                  select (int?)cartItems.Count *
        ////    //                  cartItems.Product.Price).Sum();            
        ////    List<Cart> carts = new List<Cart>(repository.Carts.Where(c => c.CartId == ShoppingCartId && c.Count != null));
        ////    decimal total = carts.Sum(c => c.Count * c.Product.Price);
        ////    return total;
        ////}

        //public int CreateOrder(Order order)
        //{
        //    return repository.CreateOrder(order, ShoppingCartId);
        //}

        ////public int GetOrdersCount()
        ////{
        ////    return repository.Orders.Count();
        ////}

        //public void MigrateCart(string userName)
        //{
        //    repository.MigrateCart(ShoppingCartId, userName);
        //}
    }
}