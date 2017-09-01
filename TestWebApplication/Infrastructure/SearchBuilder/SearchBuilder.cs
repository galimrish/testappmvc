using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web;
using TestWebApplication.Domain.Entities;
using TestWebApplication.WebUI.Models;

namespace TestWebApplication.WebUI.Infrastructure.SearchBuilder
{
    public class SearchBuilder
    {
        private ISearchParams _searchParams;
        public SearchBuilder() : this(new SearchParams()) { }
        public SearchBuilder(ISearchParams sp)
        {
            _searchParams = sp;
        }
        public SearchBuilder SetParams(string searchQuery = null, decimal? minPrice = null, decimal? maxPrice = null, IList<BrandModel> brands = null,
            decimal? minDisplaySize = null, decimal? maxDisplaySize = null, bool? lte = null, bool? dualSim = null)
        {
            _searchParams.SearchQuery = searchQuery ?? _searchParams.SearchQuery;
            _searchParams.MinPrice = minPrice ?? _searchParams.MinPrice;
            _searchParams.MaxPrice = maxPrice ?? _searchParams.MaxPrice;
            _searchParams.Brands = brands ?? _searchParams.Brands;
            _searchParams.MinDisplaySize = minDisplaySize ?? _searchParams.MinDisplaySize;
            _searchParams.MaxDisplaySize = maxDisplaySize ?? _searchParams.MaxDisplaySize;
            _searchParams.LTE = lte ?? _searchParams.LTE;
            _searchParams.DualSIM = dualSim ?? _searchParams.DualSIM;

            return this;
        }
        public SearchBuilder SetSearchQuery(string searchQuery)
        {
            _searchParams.SearchQuery = searchQuery;
            return this;
        }
        public SearchBuilder SetMinPrice(decimal minPrice)
        {
            _searchParams.MinPrice = minPrice;
            return this;
        }
        public SearchBuilder SetMaxPrice(decimal maxPrice)
        {
            _searchParams.MaxPrice = maxPrice;
            return this;
        }
        public SearchBuilder SetMinDisplaySize(decimal minDisplaySize)
        {
            _searchParams.MinDisplaySize = minDisplaySize;
            return this;
        }
        public SearchBuilder SetMaxDisplaySize(decimal maxDisplaySize)
        {
            _searchParams.MaxDisplaySize = maxDisplaySize;
            return this;
        }
        public SearchBuilder SetBrands(IList<BrandModel> brands)
        {
            _searchParams.Brands = brands;
            return this;
        }

        public Expression<Func<Product, bool>> Build()
        {
            var predicate = PredicateBuilder.New<Product>(true);
            if (!string.IsNullOrEmpty(_searchParams.SearchQuery))
            {
                var reg = new Regex("\\b" + _searchParams.SearchQuery + "\\b");
                predicate = predicate.And(p => reg.IsMatch(p.ProductName.ToLower())
                    || _searchParams.SearchQuery == p.ProductName.ToLower());
            }
            if (_searchParams.Brands != null && _searchParams.Brands.Count > 0)
            {
                List<string> brands = new List<string>();
                brands.AddRange(_searchParams.Brands.Where(b => b.IsSelected).Select(b => b.Name));
                if (brands.Count > 0)
                    predicate = predicate.And(p => brands.Contains(p.BrandName));
            }
            if (_searchParams.DualSIM)
                predicate = predicate.And(p => !string.IsNullOrEmpty(p.PhoneAttribute.DualSIM)
                    && p.PhoneAttribute.DualSIM.Contains("Есть"));
            if (_searchParams.LTE)
                predicate = predicate.And(p => !string.IsNullOrEmpty(p.PhoneAttribute.LTE)
                    && p.PhoneAttribute.LTE.Contains("Есть"));
            if (_searchParams.MaxPrice > 0)
                predicate = predicate.And(p => p.Price < _searchParams.MaxPrice);
            if (_searchParams.MinPrice > 0)
                predicate = predicate.And(p => p.Price > _searchParams.MinPrice);
            if (_searchParams.MaxDisplaySize > 0)
                predicate = predicate.And(p => SharedLogic.DisplaySizeFromStr(
                    p.PhoneAttribute.DisplaySize) < _searchParams.MaxDisplaySize);
            if (_searchParams.MinDisplaySize > 0)
                predicate = predicate.And(p => SharedLogic.DisplaySizeFromStr(
                    p.PhoneAttribute.DisplaySize) > _searchParams.MinDisplaySize);  
            return predicate;
        }
    }
}