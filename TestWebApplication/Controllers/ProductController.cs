using LinqKit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestWebApplication.Domain.Abstract;
using TestWebApplication.Domain.Entities;
using TestWebApplication.WebUI.Infrastructure;
using TestWebApplication.WebUI.Infrastructure.Attributes;
using TestWebApplication.WebUI.Infrastructure.SearchBuilder;
using TestWebApplication.WebUI.Models;

namespace TestWebApplication.WebUI.Controllers
{
    public class ProductController : CustomController
    {
        // GET: Product
        private IProductRepository repository;
        public int pageSize = 10;

        public ProductController(IProductRepository pr)
        {
            repository = pr;
            //repository.Products.Select(p => p.ProductId);
            //repository.PhoneAttributes.Select(p => p.ProductId);
            //repository.Carts.Select(c => c.CartId);
        }

        public ActionResult Catalog(string category, int page = 1)
        {
            if (!ValidCategory(category))
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new PagingInfo() {CurrentCategory=category, CurrentPage=page, TotalItems=-1 });
        }

        [InternalAction]
        public ActionResult List(string category, int page = 1, string queryString = null)
        {
            try
            {
                FilterModel filter = new FilterModel();
                filter.Brands = new List<BrandModel>();
                SetFilterFromQueryString(ref filter, queryString);
                SearchBuilder sb = new SearchBuilder(filter);
                var predicate = sb.Build();
                ProductListViewModel model = new ProductListViewModel();
                model.ProductsInCart = new List<int>();
                ShoppingCart cart = new ShoppingCart(this.HttpContext, repository);
                if (cart.CartItems != null)
                {
                    foreach (Cart item in cart.CartItems)
                    {
                        model.ProductsInCart.Add(item.ProductId);
                    }
                }
                List<Product> query = new List<Product>(repository.Products);
                if (!string.IsNullOrEmpty(category))
                {
                    filter.Category = category;
                    query = query.Where(p => p.ProductCategory == filter.Category).ToList();
                    if (query == null)
                    {
                        return PartialView(new ProductListViewModel()
                        {
                            PagingInfo = new PagingInfo()
                            {
                                CurrentCategory = category,
                                CurrentPage = page,
                                ItemsPerPage = pageSize,
                                TotalItems = 0
                            },
                            Products = null,
                            ProductsInCart = model.ProductsInCart,
                            QueryString = queryString
                        });
                    }
                }
                List<Product> sortedProducts = new List<Product>(query
                    .Where(predicate.Compile()));                
                model.PagingInfo = new PagingInfo()
                {
                    CurrentCategory = category,
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = sortedProducts.Count
                };
                if ((sortedProducts.Count) > 0 && (page > model.PagingInfo.TotalPages))
                    throw new HttpException(404, "Page Not Found");
                model.QueryString = queryString;
                switch (filter.SortBy)
                {
                    case SortOrderType.PriceLowToHigh : 
                        {
                            model.Products = sortedProducts.OrderBy(product => product.Price)
                                .Skip((page - 1) * pageSize).Take(pageSize);
                            break;
                        }
                    case SortOrderType.PriceHighToLow :
                        {
                            model.Products = sortedProducts.OrderByDescending(product => product.Price)
                                .Skip((page - 1) * pageSize).Take(pageSize);
                            break;
                        }
                    default :
                        {
                            model.Products = sortedProducts.OrderBy(product => product.ProductId)
                                .Skip((page - 1) * pageSize).Take(pageSize);
                            break;
                        }
                }
                return PartialView(model);
            }
            catch(Exception ex)
            {
                HttpException httpEx = ex as HttpException;
                if (httpEx != null)
                {
                    throw new HttpException(httpEx.GetHttpCode(), "partial", httpEx.InnerException);
                }
                throw new Exception("partial", ex.InnerException);
            }
        }

        [ChildActionOnly]
        public ActionResult FilterPanel(string category)
        {
            try
            {
                FilterModel filter = new FilterModel();
                List<Product> products = new List<Product>(repository.Products);
                List<Brand> brands = new List<Brand>(repository.Brands);
                if (!string.IsNullOrEmpty(category))
                {
                    filter.Category = category;
                    products = products.Where(p => p.ProductCategory == filter.Category).ToList();
                    brands = brands.Where(b => b.Category == filter.Category).ToList();
                }
                filter.Brands = new List<BrandModel>();
                string filterView = string.Empty;
                foreach (Brand item in brands)
                {
                    filter.Brands.Add(new BrandModel()
                    {
                        Name = item.Name,
                        Code = item.Code,
                        IsSelected = false
                    });
                }
                switch (category)
                {
                    case "MobilePhones":
                        {
                            List<PhoneAttribute> phoneAttributes = new List<PhoneAttribute>(
                                repository.PhoneAttributes);
                            decimal maxPrice = 0;
                            decimal maxDisplaySize = 0;
                            foreach (var phone in products)
                            {
                                maxPrice = maxPrice < phone.Price ? phone.Price : maxPrice;
                                decimal displaySize = SharedLogic.DisplaySizeFromStr(
                                    phoneAttributes.First(p => p.ProductId == phone.ProductId).DisplaySize);
                                maxDisplaySize = maxDisplaySize < displaySize ? displaySize : maxDisplaySize;
                            }
                            filter.MinPrice = 0;
                            filter.MaxPrice = maxPrice;
                            filter.MinDisplaySize = 0;
                            filter.MaxDisplaySize = maxDisplaySize;
                            filter.Brands = filter.Brands.OrderBy(x => x.Name).ToList();
                            //filterView = "MobilePhoneFilter";
                            break;
                        }

                    case "MobilePhoneCases":
                    case null:
                        {
                            decimal maxPrice = 0;
                            foreach (var item in products)
                            {
                                maxPrice = maxPrice < item.Price ? item.Price : maxPrice;
                            }
                            filter.MinPrice = 0;
                            filter.MaxPrice = maxPrice;
                            filter.Brands = filter.Brands.OrderBy(x => x.Name).ToList();
                            //filterView = "All";
                            break;
                        }

                    default: throw new HttpException(404, "Page Not Found");
                }
                SetFilterFromQueryString(ref filter, HttpContext.Request.QueryString.ToString());
                return PartialView("FilterPanel", filter);
            }
            catch (Exception ex)
            {
                HttpException httpEx = ex as HttpException;
                if (httpEx != null)
                {
                    throw new HttpException(httpEx.GetHttpCode(), "partial", httpEx.InnerException);
                }
                throw new Exception("partial", ex.InnerException);
            }
        }

        [InternalAction]
        public JsonResult GetProductsNames(string title)
        {
            try
            {
                var keys = repository.Products
                    .Where(p => p.ProductName.ToLower().Contains(title.ToLower()))
                    .Select(p => p.ProductName ).Take(5);
                return Json(keys, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        public ActionResult ItemDetail(int itemId = -1)
        {
            try
            {
                if (itemId == -1)
                    throw new HttpException(404, "Page Not Found");
                Product item = repository.Products.FirstOrDefault(p => p.ProductId == itemId);
                if (item == null)
                    throw new HttpException(404, "Page Not Found");
                ProductDetailViewModel model = new ProductDetailViewModel();
                model.Product = item;
                model.Category = SharedLogic.TranslateCategory(item.ProductCategory);
                model.ImgUrls = new List<string>();
                model.ThmbUrls = new List<string>();
                foreach (ImageLink imageLink in item.ImageLinks)
                {
                    string link = imageLink.Link;
                    model.ThmbUrls.Add(link);
                    link = link.Remove(link.IndexOf("thumbs/"), 7);
                    link = link.Remove(link.IndexOf("140x140/"), 8);
                    model.ImgUrls.Add(link);
                }
                model.IsInCart = false;
                ShoppingCart cart = new ShoppingCart(this.HttpContext, repository);
                if (cart.CartItems != null)
                {
                    model.IsInCart = cart.CartItems.Any(c => c.ProductId == itemId);
                }
                return View(model);
            }
            catch (Exception ex)
            {
                //ViewBag.errorMessage = ex.Message;
                //return View("Error");
                throw ex;
            }
        }

        private void SetFilterFromQueryString(ref FilterModel filter, string queryString)
        {
            QueryStringCollection request = new QueryStringCollection();

            if (Request.QueryString.Count > 0)
            {
                request.GetDataFromString(SharedLogic.DecodeUrl(Request.QueryString.ToString()));
            }

            if (!string.IsNullOrEmpty(queryString))
            {
                request.GetDataFromString(SharedLogic.DecodeUrl(queryString));
            }

            if (request.Count == 0)
                return;

            IEnumerable<Brand> dbBrands = repository.Brands;
            foreach (string param in request.Keys)
            {
                try
                {
                    switch (param)
                    {
                        case "q":
                            {
                                filter.SearchQuery = string.IsNullOrWhiteSpace(request[param]) ? string.Empty
                                    : request[param].ToLower();
                                break;
                            }

                        case "DualSIM":
                            {

                                filter.DualSIM = request[param] == "true";
                                break;
                            }

                        case "LTE":
                            {
                                filter.LTE = request[param] == "true";
                                break;
                            }

                        case "MinPrice":
                            {
                                filter.MinPrice = request.ExtractDec(param);
                                break;
                            }

                        case "MaxPrice":
                            {
                                decimal decNum = request.ExtractDec(param);
                                if (decNum > 0)
                                    filter.MaxPrice = decNum;
                                break;
                            }

                        case "MinDisplaySize":
                            {
                                filter.MinDisplaySize = request.ExtractDec(param);
                                break;
                            }

                        case "MaxDisplaySize":
                            {
                                decimal decNum = request.ExtractDec(param);
                                if (decNum > 0)
                                    filter.MaxDisplaySize = decNum;
                                break;
                            }

                        case "sortby":
                            {
                                filter.SortBy = request[param] == "asc" ? SortOrderType.PriceLowToHigh :
                                    request[param] == "desc" ? SortOrderType.PriceHighToLow :
                                    SortOrderType.Relevance;
                                break;
                            }

                        default:
                            {
                                if (param.Contains("br_"))
                                {
                                    string brandCode = param.Substring(3);
                                    var query = dbBrands.First(b => b.Code == brandCode);
                                    string brandName = query == null ? string.Empty : query.Name;
                                    BrandModel brand = filter.Brands.Where(b => b.Code == brandCode).FirstOrDefault();
                                    if (brand != null)
                                        brand.IsSelected = request[param] == "true";
                                    else
                                        filter.Brands.Add(new BrandModel()
                                        {
                                            Name = brandName,
                                            Code = brandCode,
                                            IsSelected = request[param] == "true"
                                        });
                                }
                                break;
                            }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private bool ValidCategory(string category)
        {
            switch (category)
            {
                case "MobilePhones": return true;
                case "MobilePhoneCases": return true;
                case null:
                    {
                        if (HttpContext.Request.QueryString == null || HttpContext.Request.QueryString.Count == 0)
                            return false;
                        return true;
                    }
                default: return false;            
            }
        }

        
        //private string GetFilterValue(string filterKey)
        //{
        //    string result = string.Empty;
        //    int firstIndex = filterKey.IndexOf('=') + 1;
        //    if (firstIndex < 0)
        //        return result;
        //    int i = firstIndex;
        //    while (i < filterKey.Length - 1)
        //    {
        //        result += filterKey[i];
        //        i++;
        //    }
        //    return result;
        //}

        //private string GetFilterKey(string filterKey)
        //{
        //    string result = string.Empty;
        //    int lastIndex = filterKey.IndexOf('=');
        //    if (lastIndex < 0)
        //        return result;
        //    int i = 0;
        //    while (i < lastIndex - 1)
        //    {
        //        result += filterKey[i];
        //        i++;
        //    }
        //    return result;
        //}

        //public ActionResult FillBrandsCodes()
        //{
        //    List<Product> products = new List<Product>(repository.Products);
        //    List<Brand> brands = new List<Brand>(repository.Brands);
        //    int number = 1;
        //    foreach (Product p in products)
        //    {
        //        if(!brands.Any(b => b.Name == p.BrandName))
        //        {
        //            repository.SaveBrand(new Brand()
        //            {
        //                Name = p.BrandName,
        //                Code = number.ToString("D4"),
        //                Category = p.ProductCategory
        //            });
        //            number++;
        //        }
        //    }
        //    return RedirectToAction("Index", "Home");
        //}

    //    public ActionResult DeleteExcessProducts()
    //    {
    //        List<Product> products = new List<Product>(repository.Products);
    //        List<ImageLink> imgs = new List<ImageLink>(repository.ImageLinks);
    //        for (int i = 0; i < products.Count; i++)
    //        {
    //            if (products.Any(p => p.ProductId == i) && !imgs.Any(l => l.ProductId == i))
    //            {
    //                repository.DeleteProduct(i);
    //            }
    //        }
    //        return RedirectToAction("Index", "Home");
    //    }
    }
}