using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TestWebApplication.Domain.Entities;
using System.Web.Mvc.Html;

namespace TestWebApplication.WebUI.Infrastructure.HtmlHelpers
{
    public static class ModelHelper
    {
        public static MvcHtmlString HiddenForCart(this HtmlHelper html, IEnumerable<Cart> cartItems)
        {
            StringBuilder result = new StringBuilder();
            string nameTemplate = "CartItems[{0}].{1}";
            for (int i = 0; i < cartItems.Count(); i++)
            {
                result.Append(html.Hidden(string.Format(nameTemplate, i, "CartId"),
                    cartItems.ElementAt(i).CartId));
                result.Append(html.Hidden(string.Format(nameTemplate, i, "Count"),
                    cartItems.ElementAt(i).Count));
                result.Append(html.Hidden(string.Format(nameTemplate, i, "DateCreated"),
                    cartItems.ElementAt(i).DateCreated));
                result.Append(html.Hidden(string.Format(nameTemplate, i, "ProductId"),
                    cartItems.ElementAt(i).ProductId));
                result.Append(html.Hidden(string.Format(nameTemplate, i, "RecordId"),
                    cartItems.ElementAt(i).RecordId));
                result.Append(html.Hidden(string.Format(nameTemplate, i, "Product.ProductId"),
                    cartItems.ElementAt(i).Product.ProductId));
                result.Append(html.Hidden(string.Format(nameTemplate, i, "Product.ImageLinks"),
                    cartItems.ElementAt(i).Product.ImageLinks));
                result.Append(html.Hidden(string.Format(nameTemplate, i, "Product.Price"),
                    cartItems.ElementAt(i).Product.Price));
                result.Append(html.Hidden(string.Format(nameTemplate, i, "Product.ProductName"),
                    cartItems.ElementAt(i).Product.ProductName));
                result.Append(html.Hidden(string.Format(nameTemplate, i, "Product.Brand"),
                    cartItems.ElementAt(i).Product.Brand));
                result.Append(html.Hidden(string.Format(nameTemplate, i, "Product.BrandName"),
                    cartItems.ElementAt(i).Product.BrandName));
                result.Append(html.Hidden(string.Format(nameTemplate, i, "Product.ImageLinks"),
                    cartItems.ElementAt(i).Product.ImageLinks));
                result.Append(html.Hidden(string.Format(nameTemplate, i, "Product.PhoneAttribute"),
                    cartItems.ElementAt(i).Product.PhoneAttribute));
                result.Append(html.Hidden(string.Format(nameTemplate, i, "Product.PhoneCasesAttribute"),
                    cartItems.ElementAt(i).Product.PhoneCasesAttribute));
                result.Append(html.Hidden(string.Format(nameTemplate, i, "Product.ProductCategory"),
                    cartItems.ElementAt(i).Product.ProductCategory));
            }
            return MvcHtmlString.Create(result.ToString());
        }
    }
}