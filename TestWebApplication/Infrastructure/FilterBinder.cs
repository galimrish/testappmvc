using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestWebApplication.WebUI.Models;

namespace TestWebApplication.WebUI.Infrastructure
{
    public class FilterBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            FilterModel model = (FilterModel)bindingContext.Model
                ?? new FilterModel();
            var request = controllerContext.HttpContext.Request.QueryString;
            if(request.Count < 2)
                return model;
            decimal decNum;
            foreach (string parameter in request.AllKeys)
            {
                switch (parameter)
                {
                    case "DualSIM": if (true)
                        {
                            model.DualSIM = request["DualSIM"].ToLower().Contains("true"); break;
                        }

                    case "LTE": if (true)
                        {
                            model.DualSIM = request["LTE"].ToLower().Contains("true"); break;
                        }

                    case "MinPrice": if (true)
                        {
                            decNum = 0;
                            decimal.TryParse(request["MinPrice"], out decNum);
                            model.MinPrice = decNum;
                            break;
                        }

                    case "MaxPrice": if (true)
                        {
                            decNum = 0;
                            decimal.TryParse(request["MaxPrice"], out decNum);
                            model.MaxPrice = decNum;
                            break;
                        }

                    case "MinDisplaySize": if (true)
                        {
                            decNum = 0;
                            decimal.TryParse(request["MinDisplaySize"], out decNum);
                            model.MinDisplaySize = decNum;
                            break;
                        }

                    case "MaxDisplaySize": if (true)
                        {
                            decNum = 0;
                            decimal.TryParse(request["MaxDisplaySize"], out decNum);
                            model.MaxDisplaySize = decNum;
                            break;
                        }
                }
            }
            model.Brands = new List<BrandModel>();
            int i = 0;
            while (i < request.Count)
            {
                if (request.AllKeys.Contains(string.Format("Brands[{0}].IsSelected", i))
                    && request.AllKeys.Contains(string.Format("Brands[{0}].Name", i))
                    && request.AllKeys.Contains(string.Format("Brands[{0}].Code", i)))
                {
                    model.Brands.Add(new BrandModel()
                    {
                        Name = request[string.Format("Brands[{0}].Name", i)],
                        Code = request[string.Format("Brands[{0}].Code", i)],
                        IsSelected = bool.Parse(request[string.Format("Brands[{0}].IsSelected", i)])
                    });
                }
                i++;
            }
            return model;
        }
    }
}