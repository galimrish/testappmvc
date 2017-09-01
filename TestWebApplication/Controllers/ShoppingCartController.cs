using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TestWebApplication.Domain.Abstract;
using TestWebApplication.Domain.Entities;
using TestWebApplication.WebUI.Infrastructure;
using TestWebApplication.WebUI.Infrastructure.Attributes;
using TestWebApplication.WebUI.Infrastructure.Mappers;
using TestWebApplication.WebUI.Models;

namespace TestWebApplication.WebUI.Controllers
{
    public class ShoppingCartController : CustomController
    {
        private IProductRepository repository;
        private ICustomMapper CustomMapper;
        private const string appName = "TestWebApplication";

        public ShoppingCartController(IProductRepository pr, ICustomMapper m)
        {
            repository = pr;
            CustomMapper = m;
        }

        // GET: ShoppingCart
        public ActionResult Order()
        {
            try
            {
                ShoppingCart shoppingCart = new ShoppingCart(this.HttpContext, repository);
                return View(new ShoppingCartViewModel()
                {
                    CartItems = shoppingCart.CartItems,
                    CartTotal = shoppingCart.TotalPrice,
                    UserInfoDto = CustomMapper.Map<UserInfoDto>(shoppingCart.UserInfo)
                        ??  new UserInfoDto()
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult AddToCart(int productId)
        {
            try
            {
                Product addedItem = repository.Products.Single(p => p.ProductId == productId);
                ShoppingCart shoppingCart = new ShoppingCart(this.HttpContext, repository);
                repository.AddProductToCart(shoppingCart.ShoppingCartId, addedItem);

                int itemsCount = shoppingCart.CartItemsCount;

                return Json(new { itemsCount }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult SetProductAmount(int productId, int count)
        {
            bool result;
            try
            {
                Product product = repository.Products.Single(p => p.ProductId == productId);
                ShoppingCart shoppingCart = new ShoppingCart(this.HttpContext, repository);
                result = repository.SetProductCount(shoppingCart.ShoppingCartId, product, count);
            }
            catch
            {
                 result = false;
            }
            return Json(new { result });
        }

        [HttpPost]
        public JsonResult LabelItemForRemove(int itemId)
        {
            try
            {
                List<int> itemsToRemove;
                if (HttpContext.Cache["ItemsToRemove"] == null)
                {
                    itemsToRemove = new List<int> { itemId };
                    HttpContext.Cache.Add("ItemsToRemove", itemsToRemove, null,
                        System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0,40,0),
                        System.Web.Caching.CacheItemPriority.Normal, null);
                }
                else
                {
                    itemsToRemove = HttpContext.Cache.Get("ItemsToRemove") as List<int>;
                    if (!itemsToRemove.Contains(itemId))
                    {
                        itemsToRemove.Add(itemId);
                        HttpContext.Cache.Insert("ItemsToRemove", itemsToRemove);
                    }
                }
                ShoppingCart shoppingCart = new ShoppingCart(this.HttpContext, repository);
                var itemsRemain = shoppingCart.CartItems.Where(c => !itemsToRemove.Contains(c.RecordId));
                int cartItemsCount = itemsRemain == null ? 0 : itemsRemain.Count();
                return Json(new { cartItemsCount });
            }
            catch
            {
                return null;
            }
        }
        
        [HttpPost]
        public JsonResult ClearCart()
        {
            bool succeeded;
            try
            {
                ShoppingCart shoppingCart = new ShoppingCart(this.HttpContext, repository);
                List<int> itemsToRemove = shoppingCart.CartItems.Select(c => c.RecordId).ToList();
                if (HttpContext.Cache["ItemsToRemove"] == null)
                {
                    HttpContext.Cache.Add("ItemsToRemove", itemsToRemove, null,
                        System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, 40, 0),
                        System.Web.Caching.CacheItemPriority.Normal, null);
                }
                else
                {
                    HttpContext.Cache.Insert("ItemsToRemove", itemsToRemove);
                }
                succeeded = true;
            }
            catch
            {
                succeeded = false;
            }
            return Json(new { succeeded });
        }

        [HttpPost]
        public JsonResult RestoreCart()
        {
            bool succeeded;
            try
            {
                if (HttpContext.Cache["ItemsToRemove"] != null)
                {
                    HttpContext.Cache.Remove("ItemsToRemove");
                }
                succeeded = true;
            }
            catch
            {
                succeeded = false;
            }
            return Json(new { succeeded });
        }

        [HttpPost]
        public JsonResult RemoveFromCart(int itemId)
        {
            try
            {
                if (HttpContext.Cache["ItemsToRemove"] != null)
                {
                    List<int> itemsToRemove = HttpContext.Cache.Get("ItemsToRemove") as List<int>;
                    itemsToRemove.RemoveAll(i => i == itemId);
                    HttpContext.Cache.Insert("ItemsToRemove", itemsToRemove);
                }
                int itemsRemain = repository.RemoveFromCart(itemId);
                return Json(new { itemsRemain });
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult RestoreItem(int itemId)
        {
            try
            {
                ShoppingCart shoppingCart = new ShoppingCart(this.HttpContext, repository);
                int itemsRemain = shoppingCart.CartItemsCount;
                if (HttpContext.Cache["ItemsToRemove"] != null)
                {
                    List<int> itemsToRemove = HttpContext.Cache.Get("ItemsToRemove") as List<int>;
                    itemsToRemove.RemoveAll(i => i == itemId);
                    HttpContext.Cache.Insert("ItemsToRemove", itemsToRemove);
                    itemsRemain -= itemsToRemove.Count;
                }
                return Json(new { itemsRemain });
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public async Task<ActionResult> SubmitOrder(UserInfoDto userInfo)
        {
            try
            {
                ShoppingCart shoppingCart = new ShoppingCart(this.HttpContext, repository);
                if (HttpContext.Cache["ItemsToRemove"] != null)
                {
                    List<int> itemsToRemove = HttpContext.Cache.Get("ItemsToRemove") as List<int>;
                    if(itemsToRemove != null)
                        repository.RemoveFromCart(itemsToRemove);
                }
                if (!ModelState.IsValid || shoppingCart.CartItems.Count < 1)
                    return View("Order", new ShoppingCartViewModel()
                    {
                        CartItems = shoppingCart.CartItems,
                        CartTotal = shoppingCart.TotalPrice,
                        UserInfoDto = userInfo
                    });
                List<OrderDetail> orderDetails = new List<OrderDetail>();
                StringBuilder infoItems = new StringBuilder();
                decimal total = 0;
                foreach (Cart item in shoppingCart.CartItems)
                {
                    orderDetails.Add(new OrderDetail()
                    {
                        PricePerItem = item.Product.Price,
                        ProductId = item.ProductId,
                        ProductImage = item.Product.ImageLinks != null ?
                            item.Product.ImageLinks.First().Link : string.Empty,
                        ProductName = item.Product.ProductName,
                        Quantity = item.Count
                    });
                    decimal subtotal = item.Product.Price * item.Count;
                    total += subtotal;
                    infoItems.AppendFormat("{0} x {1}   {2:c}{3}",
                        item.Count, item.Product.ProductName, subtotal, Environment.NewLine);
                }
                string userName = repository.SaveUserInfo(CustomMapper.Map<UserInfo>(userInfo));
                Order order = new Order()
                {
                    //OrderId = shoppingCart.OrdersCount + 1,
                    Username = userName,
                    OrderDetails = orderDetails,
                    Total = total
                };
                int orderN = repository.SaveOrder(order, shoppingCart.ShoppingCartId);
                if (orderN < 0)
                    throw new Exception("Database error. Faled to create order");
                await SendOrderInfoEmail(infoItems.ToString(), orderN, total, userInfo.Email);
                return View("OrderAccepted", orderN);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [InternalAction]
        public ActionResult CartSummary()
        {
            ShoppingCart shoppingCart = new ShoppingCart(this.HttpContext, repository);
            return PartialView("CartSummary", shoppingCart.CartItemsCount);
        }

        [InternalAction]
        public ActionResult FastOrder(ProductDetailViewModel pdvmodel)
        {
            ShoppingCartViewModel scvmodel = new ShoppingCartViewModel
            {
                OrderInfoDto = new List<OrderInfoDto>
                {
                    new OrderInfoDto()
                    {
                        ProductId = pdvmodel.Product.ProductId,
                        ProductName = pdvmodel.Product.ProductName,
                        PricePerItem = pdvmodel.Product.Price,
                        ProductImage = pdvmodel.ThmbUrls != null ?
                            pdvmodel.ThmbUrls.First() : string.Empty,
                        Quantity = 1
                    }
                }
            };
            return PartialView("ModalFastOrder", scvmodel);
        }

        [HttpPost]
        public async Task<ActionResult> FastOrder(ShoppingCartViewModel model)
        {
            try
            {
                decimal total = model.OrderInfoDto.First().PricePerItem * model.OrderInfoDto.First().Quantity;
                string orderInfo = string.Format("{0} x {1}   {2:c}{3}", model.OrderInfoDto.First().Quantity,
                    model.OrderInfoDto.First().ProductName, total, Environment.NewLine);
                string userName = repository.SaveUserInfo(CustomMapper.Map<UserInfo>(model.UserInfoDto));
                Order order = new Order()
                {
                    Username = userName,
                    OrderDetails = CustomMapper.Map<List<OrderDetail>>(model.OrderInfoDto),
                    Total = total
                };
                int orderN = repository.SaveOrder(order);
                if (orderN < 0)
                    throw new Exception("Database error. Faled to create order");
                await SendOrderInfoEmail(orderInfo, orderN, total, model.UserInfoDto.Email);
                return View("OrderAccepted", orderN);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult ClearSession(string returnUrl)
        {
            ShoppingCart cart = new ShoppingCart(this.HttpContext, repository);
            cart.ClearSession(this.HttpContext);
            if (returnUrl == null)
                return RedirectToAction("Index", "Home");
            return RedirectToAction(returnUrl);
        }
        
        public async Task<JsonResult> EmaiAvailableAsync(string email)
        {
            string message = string.Empty;
            if (email.Length > 50)
            {
                message = "Слишком длинный email";
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            ShoppingCart cart = new ShoppingCart(this.HttpContext, repository);
            var userInfo = await cart.FindByEmailAsync(email);
            if (userInfo == null)
                return Json(true, JsonRequestBehavior.AllowGet);
            message = string.Format("Пользователь с адресом \"{0}\" уже зарегестрирован", email);
            return Json(message == null, JsonRequestBehavior.AllowGet);
        }

        private async Task SendOrderInfoEmail(string orderItems, int orderNumber, decimal total, string destination)
        {
            EmailService emailService = new EmailService(false);
            StringBuilder info = new StringBuilder()
                .AppendFormat("Заказ №{0}{1}", orderNumber, Environment.NewLine)
                .AppendFormat("Вы выполнили заказ на сайте {0}.{1}", appName, Environment.NewLine)
                .AppendLine("---")
                .AppendLine("Товары:")
                .Append(orderItems)
                .AppendLine()
                .AppendFormat("Итого: {0:c}{1}", total, Environment.NewLine)
                .AppendLine("---")
                .AppendLine()
                .AppendLine("Внимание это сайт разработан в демонстрационных целях, " +
                "и не занимается какой либо коммерческой деятельностью!");
            IdentityMessage emailMessage = new IdentityMessage()
            {
                Body = info.ToString(),
                Destination = destination,
                Subject = "Оформление заказа"
            };
            await emailService.SendAsync(emailMessage); 
        }
    }
}