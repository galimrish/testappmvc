using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWebApplication.Domain.Entities;

namespace TestWebApplication.Domain.Abstract
{
    public interface IProductRepository
    {
        IEnumerable<Product> Products { get; }
        IEnumerable<PhoneAttribute> PhoneAttributes { get; }
        IEnumerable<PhoneCasesAttribute> PhoneCasesAttributes { get; }
        IEnumerable<ImageLink> ImageLinks { get; }
        IEnumerable<Brand> Brands { get; }
        IEnumerable<Cart> Carts { get; }
        IEnumerable<Order> Orders { get; }
        IEnumerable<OrderDetail> OrderDetails { get; }
        IEnumerable<UserInfo> UserInfo { get; }
        void SaveProduct(Product product);
        Product DeleteProduct(int productId);
        void SavePhoneAttribute(PhoneAttribute phoneAtribute);
        void SavePhoneCasesAttribute(PhoneCasesAttribute phoneCasesAttribute);
        void SaveImageLink(ImageLink imageLink);
        void SaveBrand(Brand brand);
        string GetBrandCode(string brandCode);
        void AddProductToCart(string cartId, Product product);
        bool SetProductCount(string cartId, Product product, int count);
        int RemoveFromCart(int recordId);
        void RemoveFromCart(IEnumerable<int> itemsToRemove);
        void RemoveCart(string cartId = null);
        //int SaveOrder(Order order);
        int SaveOrder(Order order, string cartId = null);
        void DeleteOrder(int orderId);
        void SaveOrderDetail(OrderDetail orderDetail);
        void DeleteOrderDetail(int orderDetailId);
        Task<UserInfo> FindUserByEmailAsync(string email);
        string SaveUserInfo(UserInfo userInfo);
        void DeleteUserInfo(string userName);
        void MigrateCart(string cartId, string userName);
    }
}
