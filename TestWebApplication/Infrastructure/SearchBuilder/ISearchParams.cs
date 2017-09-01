using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWebApplication.WebUI.Models;


namespace TestWebApplication.WebUI.Infrastructure.SearchBuilder
{
    public interface ISearchParams
    {
        string SearchQuery { get; set; }
        decimal MinPrice { get; set; }
        decimal MaxPrice { get; set; }
        IList<BrandModel> Brands{ get; set; }

        bool DualSIM { get; set; }
        decimal MinDisplaySize { get; set; }
        decimal MaxDisplaySize { get; set; }
        bool LTE { get; set; }
    }
}
