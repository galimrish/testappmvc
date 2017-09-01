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
        public IEnumerable<Brand> Brands
        {
            get { return context.Brand; }
        }
        public void SaveBrand(Brand brandCode)
        {
            Brand dbEntry = context.Brand.Find(brandCode.Name);
            if (dbEntry == null)
                context.Brand.Add(brandCode);
            else
                dbEntry.Code = brandCode.Code;
            context.SaveChanges();
        }

        public string GetBrandCode(string brandName)
        {
            return context.Brand.First(b => b.Name == brandName).Code;
        }
    }
}
