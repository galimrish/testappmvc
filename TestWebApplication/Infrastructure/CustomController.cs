using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestWebApplication.WebUI.Infrastructure
{
    public abstract class CustomController : Controller
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            MvcApplication.Logger.Error(filterContext.Exception);
            int errorCode = filterContext.Exception is HttpException ?
                ((HttpException)filterContext.Exception).GetHttpCode() : -1;
            string viewName = "~/Views/Error/IndexPartial.cshtml";
            string errorMessage = string.Empty;
            if (filterContext.IsChildAction ||
                filterContext.Exception.Message == "partial")
            {
                viewName = errorCode == 404 ?
                "~/Views/Error/NotFoundPartial.cshtml"
                : "~/Views/Error/IndexPartial.cshtml";
            }
            else
            {
                if (filterContext.Exception.Message.Contains("recipient verification failed"))
                    errorMessage = "Не удалось отправить сообщение на указанный вами адрес электронной почты";
                viewName = errorCode == 404 ?
                "~/Views/Error/NotFound.cshtml"
                : "~/Views/Error/Index.cshtml";
            }
            filterContext.Result = new ViewResult
            {
                ViewName = viewName,
                ViewData = new ViewDataDictionary(errorMessage)
            };
        }
    }
}