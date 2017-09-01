using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWebApplication.Domain.Abstract;
using TestWebApplication.Domain.Entities;

namespace TestWebApplication.Domain.Concrete
{
    public partial class EFProductRepository : IProductRepository
    {
        public IEnumerable<Order> Orders
        {
            get { return context.Order; }
        }

        public IEnumerable<OrderDetail> OrderDetails
        {
            get { return context.OrderDetail; }
        }

        public int SaveOrder(Order order, string cartId = null)
        {
            Order dbEntry = context.Order.Find(order.OrderId);
            if (dbEntry == null)
            {
                //decimal orderTotal = 0;
                //orderTotal = order.OrderDetails.Sum(o => o.PricePerItem * o.Quantity);
                //order.Total = orderTotal;
                order.OrderDate = DateTime.Now;
                context.Order.Add(order);
                context.SaveChanges();
                if (!string.IsNullOrWhiteSpace(cartId))
                    RemoveCart(cartId);
                return order.OrderId;
            }
            return -1;
        }

        //public int SaveOrder(Order order)
        //{
        //    OrderDetail orderDetail = order.OrderDetails.SingleOrDefault();
        //    if (orderDetail == null)
        //        return -1;
        //    context.OrderDetail.Add(orderDetail);
        //    if(context.UserInfo.Find(order.UserInfo.Username) == null)
        //        context.UserInfo.Add(order.UserInfo);
        //    order.Total = orderDetail.Quantity * orderDetail.PricePerItem;
        //    order.OrderDate = DateTime.Now;
        //    context.Order.Add(order);
        //    context.SaveChanges();
        //    return order.OrderId;
        //}

        public void SaveOrderDetail(OrderDetail orderDetail)
        {
            OrderDetail dbEntry = context.OrderDetail.Find(orderDetail.OrderId);
            if (dbEntry == null)
                context.OrderDetail.Add(orderDetail);
            else
            {
                dbEntry.OrderId = orderDetail.OrderId;
                dbEntry.PricePerItem = orderDetail.PricePerItem;
                dbEntry.ProductId = orderDetail.ProductId;
                dbEntry.ProductImage = orderDetail.ProductImage;
                dbEntry.ProductName = orderDetail.ProductName;
                dbEntry.Quantity = orderDetail.Quantity;
            }
            context.SaveChanges();
        }

        public void DeleteOrderDetail(int orderDetailId)
        {
            var dbEntry = context.OrderDetail.Find(orderDetailId);
            if(dbEntry != null)
            {
                context.OrderDetail.Remove(dbEntry);
                context.SaveChanges();
            }
        }

        public void DeleteOrder(int orderId)
        {
            var dbEntry = context.Order.Find(orderId);
            if (dbEntry != null)
            {
                foreach (var orderDetail in dbEntry.OrderDetails)
                {
                    DeleteOrderDetail(orderDetail.OrderDetailId);
                }
                DeleteUserInfo(dbEntry.UserInfo.Username);
                context.Order.Remove(dbEntry);
                context.SaveChanges();
            }
        }
    }
}
