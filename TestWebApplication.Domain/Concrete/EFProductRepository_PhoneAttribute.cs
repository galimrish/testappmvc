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
        public IEnumerable<PhoneAttribute> PhoneAttributes
        {
            get { return context.PhoneAttribute; }
        }
        public void SavePhoneAttribute(PhoneAttribute phoneAttribute)
        {
            if (phoneAttribute.ProductId == 0)
                context.PhoneAttribute.Add(phoneAttribute);
            else
            {
                PhoneAttribute dbEntry = context.PhoneAttribute.Find(phoneAttribute.ProductId);
                if (dbEntry != null)
                {
                    dbEntry.BatteryCapacity = phoneAttribute.BatteryCapacity;
                    dbEntry.CameraPixels = phoneAttribute.CameraPixels;
                    dbEntry.CPUfreq = phoneAttribute.CPUfreq;
                    dbEntry.DisplayResolution = phoneAttribute.DisplayResolution;
                    dbEntry.DisplaySize = phoneAttribute.DisplaySize;
                    dbEntry.DualSIM = phoneAttribute.DualSIM;
                    dbEntry.FrontalCamera = phoneAttribute.FrontalCamera;
                    dbEntry.GPS = phoneAttribute.GPS;
                    dbEntry.InternalMemory = phoneAttribute.InternalMemory;
                    dbEntry.LTE = phoneAttribute.LTE;
                    dbEntry.Model = phoneAttribute.Model;
                    dbEntry.Platform = phoneAttribute.Platform;
                    dbEntry.RAM = phoneAttribute.RAM;
                    dbEntry.SIMtype = phoneAttribute.SIMtype;
                    dbEntry.Size = phoneAttribute.Size;
                }
            }
            context.SaveChanges();
        }
    }
}
