using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestWebApplication.WebUI.Models;

namespace TestWebApplication.WebUI.Infrastructure.SearchBuilder
{
    public class SearchParams: ISearchParams
    {
        public string SearchQuery { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public IList<BrandModel> Brands { get; set; }
        public SortOrderType SortBy { get; set; }

        public bool DualSIM { get; set; }
        public decimal MinDisplaySize { get; set; }
        public decimal MaxDisplaySize { get; set; }
        public bool LTE { get; set; }

        public SearchParams()
        {
            SortBy = SortOrderType.Relevance;
            DualSIM = LTE = false;
        }
    }
}