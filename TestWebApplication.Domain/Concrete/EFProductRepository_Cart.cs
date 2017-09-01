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
        public IEnumerable<Entities.Cart> Carts
        {
            get { return context.Cart; }
        }

        public void AddProductToCart(string cartId, Product product)
        {
            Cart dbEntry = context.Cart.SingleOrDefault(c => c.CartId == cartId
                && c.ProductId == product.ProductId);
            if (dbEntry == null)
            {
                dbEntry = new Cart
                {
                    CartId = cartId,
                    Count = 1,
                    DateCreated = DateTime.Now,
                    ProductId = product.ProductId
                };
                context.Cart.Add(dbEntry);
            }
            else
            {
                dbEntry.Count++;
            }
            context.SaveChanges();
        }

        public bool SetProductCount(string cartId, Product product, int count)
        {
            Cart dbEntry = context.Cart.SingleOrDefault(c => c.CartId == cartId
                && c.ProductId == product.ProductId);
            if (dbEntry == null)
                return false;
            dbEntry.Count = count;
            context.SaveChanges();
            return true;
        }

        //public int LabelForRemove(int recordId)
        //{
        //    Cart dbEntry = context.Cart.Single(c => c.RecordId == recordId);
        //    if (dbEntry == null)
        //        return -1;
        //    dbEntry.LabeledForRemove = true;
        //    context.SaveChanges();
        //    string cartId = dbEntry.CartId;
        //    var remainingItems = context.Cart.Where(c => c.CartId == cartId);
        //    if (remainingItems == null)

        //}

        public int RemoveFromCart(int recordId)
        {
            Cart dbEntry = context.Cart.Single(c => c.RecordId == recordId);            
            int itemCount = -1;

            if (dbEntry != null)
            {
                string cartId = dbEntry.CartId;
                context.Cart.Remove(dbEntry);
                context.SaveChanges();
                var cartItems = context.Cart.Where(c => c.CartId == cartId);
                if (cartItems != null)
                    itemCount = cartItems.Count();
            }
            return itemCount;
        }
        
        public void RemoveFromCart(IEnumerable<int> itemsToRemove)
        {
            var dbEntry = context.Cart.Where(c => itemsToRemove.Contains(c.RecordId));
            if (dbEntry != null)
            {
                context.Cart.RemoveRange(dbEntry);
                context.SaveChanges();
            }
        }

        public void RemoveCart(string cartId = null)
        {
            var dbEntry = string.IsNullOrEmpty(cartId) ? context.Cart
                : context.Cart.Where(c => c.CartId == cartId);
            if (dbEntry != null)
            {
                context.Cart.RemoveRange(dbEntry);
                context.SaveChanges();
            }
        }

        public void MigrateCart(string cartId, string userName)
        {
            var dbEntry = context.Cart.Where(
                cart => cart.CartId == cartId);

            foreach (var cartItem in dbEntry)
            {
                cartItem.CartId = userName;
            }
            context.SaveChanges();
        }
    }
}
