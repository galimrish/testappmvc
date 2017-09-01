using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestWebApplication.WebUI.Infrastructure;
using TestWebApplication.WebUI.Models;

namespace TestWebApplication.WebUI.Controllers
{
    public class HomeController : CustomController
    {
        public ActionResult Index()
        {
            List<Category> categoryList = new List<Category>() 
            {
                new Category("Мобильные телефоны", "MobilePhones"),
                new Category("Чехлы для мобильных телефонов", "MobilePhoneCases")
            };
            return View(categoryList);
            //return RedirectToAction("Catalog", "Product");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}