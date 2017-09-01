using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWebApplication.Domain.Entities
{
    public partial class Order
    {
        public int OrderId { get; set; }
        public string Username { get; set; }
        public decimal Total { get; set; }
        public System.DateTime OrderDate { get; set; }
        public virtual UserInfo UserInfo { get; set; }
        public virtual  ICollection<OrderDetail> OrderDetails { get; set; }
    }

    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductImage { get; set; }
        public string ProductName { get; set; }
        public decimal PricePerItem { get; set; }
        public int Quantity { get; set; }
        public virtual Product Product { get; set; }
        public virtual Order Order { get; set; }
    }

    public class UserInfo
    {
        [Key]
        public string Username { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
