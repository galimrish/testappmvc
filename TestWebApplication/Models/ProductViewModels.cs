using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using TestWebApplication.Domain.Entities;
using TestWebApplication.WebUI.Infrastructure.SearchBuilder;

namespace TestWebApplication.WebUI.Models
{
    public class ProductListViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string QueryString { get; set; }
        public List<int> ProductsInCart { get; set; }
    }

    public class ProductDetailViewModel
    {
        public Product Product { get; set; }
        public List<string> ImgUrls { get; set; }
        public List<string> ThmbUrls { get; set; }
        public string Category { get; set; }
        public bool IsInCart { get; set; }
    }

    public class PagingInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public string CurrentCategory { get; set; }
        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage); }
        }
    }

    public class FilterModel : ISearchParams
    {
        private string _category;        
        public string Category
        {
            get { return _category; }
            set
            {
                if (value == null)
                    return;
                switch (value)
                {
                    case "MobilePhones": _category = "Мобильные телефоны"; break;
                    case "MobilePhoneCases": _category = "Чехлы для мобильных телефонов"; break;
                    default: throw new HttpException(404, "Unknown category");
                }
                
            }
        }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public IList<BrandModel> Brands { get; set; }

        [DisplayName("Две SIM-карты")]
        public bool DualSIM { get; set; }
        public decimal MinDisplaySize { get; set; }
        public decimal MaxDisplaySize { get; set; }

        [DisplayName("4G")]
        public bool LTE { get; set; }

        public string SearchQuery { get; set; }

        public SortOrderType SortBy { get; set; }
    }

    public class BrandModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsSelected { get; set; }
    }

    public class Category
    {
        public string Title { get; set; }
        public string Value { get; set; }
        public Category(string title, string value)
        {
            Title = title;
            Value = value;
        }
    }

    public enum SortOrderType
    {
        Relevance = 0,
        PriceLowToHigh = 1,
        PriceHighToLow = 2
    }

}