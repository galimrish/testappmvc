using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWebApplication.Domain.Abstract;
using TestWebApplication.Domain.Entities;
using System.Data.Entity; 

namespace TestWebApplication.Domain.Concrete
{
    public partial class EFProductRepository : IProductRepository
    {
        public IEnumerable<UserInfo> UserInfo
        {
            get { return context.UserInfo; }
        }

        public string SaveUserInfo(UserInfo userInfo)
        {
            if (string.IsNullOrWhiteSpace(userInfo.Username))
                userInfo.Username = userInfo.Email;
            UserInfo dbEntry = context.UserInfo.Find(userInfo.Username);
            if (dbEntry == null)
                context.UserInfo.Add(userInfo);
            else
            {
                dbEntry.Address = userInfo.Address;
                dbEntry.Email = userInfo.Email;
                dbEntry.Name = userInfo.Name;
                dbEntry.Phone = userInfo.Phone;
                dbEntry.Username = userInfo.Username;
            }
            context.SaveChanges();
            return userInfo.Username;
        }

        public void DeleteUserInfo(string userName)
        {
            var dbEntry = context.UserInfo.Find(userName);
            if (dbEntry != null)
            {
                var orders = context.Order.Where(o => o.UserInfo.Username == userName);
                if (orders != null)
                    context.Order.RemoveRange(orders);
                context.UserInfo.Remove(dbEntry);
                context.SaveChanges();
            }
        }

        public async Task<UserInfo> FindUserByEmailAsync(string email)
        {
            if (context.UserInfo != null)
            {
                var dbEntry = await context.UserInfo.Where(u => u.Email == email).ToListAsync();
                if (dbEntry != null && dbEntry.Count > 0)
                    return dbEntry.First();
            }
            return null;
        }
    }
}
