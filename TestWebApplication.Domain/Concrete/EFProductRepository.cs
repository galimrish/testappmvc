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
        EFDbContext context = new EFDbContext();
        public IEnumerable<Product> Products
        {
            get { return context.Product; }
        }
        public IEnumerable<PhoneCasesAttribute> PhoneCasesAttributes
        {
            get { return context.PhoneCasesAttribute; }
        }
        public IEnumerable<ImageLink> ImageLinks
        {
            get { return context.ImageLink; }
        }
        public void SaveProduct(Product product)
        {
            if (product.ProductId == 0)
                context.Product.Add(product);
            else
            {
                Product dbEntry = context.Product.Find(product.ProductId);
                if (dbEntry != null)
                {
                    dbEntry.ProductName = product.ProductName;
                    dbEntry.ProductCategory = product.ProductCategory;
                    dbEntry.BrandName = product.BrandName;
                    dbEntry.Price = product.Price;
                }
            }
            context.SaveChanges();
        }

        public Product DeleteProduct(int productId)
        {
            Product product = context.Product.Find(productId);
            if (product != null)
            {
                var imageLinks = context.ImageLink.Where(x => x.ProductId == productId);
                if (imageLinks != null)
                    context.ImageLink.RemoveRange(imageLinks);
                PhoneAttribute phoneAttribute = context.PhoneAttribute.Find(productId);
                if (phoneAttribute != null)
                    context.PhoneAttribute.Remove(phoneAttribute);
                PhoneCasesAttribute phoneCasesAttribute = context.PhoneCasesAttribute.Find(productId);
                if (phoneCasesAttribute != null)
                    context.PhoneCasesAttribute.Remove(phoneCasesAttribute);
                context.Product.Remove(product);
                context.SaveChanges();
            }
            return product;
        }

        public void SavePhoneCasesAttribute(PhoneCasesAttribute pca)
        {
            throw new NotImplementedException();
        }

        public void SaveImageLink(ImageLink imageLink)
        {
            if (imageLink.ImageLinkId == 0)
                context.ImageLink.Add(imageLink);
            else
            {
                ImageLink dbEntry = context.ImageLink.Find(imageLink.ImageLinkId);
                if (dbEntry != null)
                {
                    dbEntry = imageLink;
                }
            }
            context.SaveChanges();
        }
    }
}
