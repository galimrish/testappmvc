using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using TestWebApplication.WebUI.Models;

namespace TestWebApplication.WebUI.Infrastructure.AjaxHelpers
{
    public static class Paging
    {
        public static MvcHtmlString PageLinks(this AjaxHelper ajax, PagingInfo pagingInfo, AjaxOptions ajaxOptions,
            Func<int, string> pageUrl)
        {
            if (pagingInfo.TotalPages < 2)
                return null;
            StringBuilder result = new StringBuilder();
            for (int i = 1; i <= pagingInfo.TotalPages; i++)
            {
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("data-ajax", "true");
                tag.MergeAttribute("data-ajax-method", ajaxOptions.HttpMethod);
                tag.MergeAttribute("data-ajax-mode", ajaxOptions.InsertionMode.ToString().ToLower());
                tag.MergeAttribute("data-ajax-update", '#' + ajaxOptions.UpdateTargetId);
                tag.MergeAttribute("href", pageUrl(i));
                tag.MergeAttribute("data-ajax-begin", ajaxOptions.OnBegin);
                tag.MergeAttribute("data-ajax-failure", ajaxOptions.OnFailure);
                tag.InnerHtml = i.ToString();
                if (i == pagingInfo.CurrentPage)
                {
                    tag.AddCssClass("selected");
                    tag.AddCssClass("btn-primary");
                }
                tag.AddCssClass("btn btn-default");
                result.Append(tag.ToString());
            }
            return MvcHtmlString.Create(result.ToString());
        }
    }
}