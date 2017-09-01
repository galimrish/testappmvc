using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestWebApplication.WebUI;
using TestWebApplication.WebUI.Controllers;
using TestWebApplication.WebUI.Infrastructure;
using System.Text.RegularExpressions;

namespace TestWebApplication.WebUI.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void IsDecimalCorrect()
        {
            bool exp1 = SharedLogic.IsDecimal("20.2");
            bool exp2 = SharedLogic.IsDecimal("20,2");
            bool exp3 = SharedLogic.IsDecimal("20,2.2");
            bool exp4 = SharedLogic.IsDecimal("20,2s");
            bool exp5 = SharedLogic.IsDecimal(".20");
            bool exp6 = SharedLogic.IsDecimal("20");

            Assert.AreEqual(true, exp1);
            Assert.AreEqual(true, exp2);
            Assert.AreNotEqual(true, exp3);
            Assert.AreNotEqual(true, exp4);
            Assert.AreEqual(false, exp5);
            Assert.AreEqual(true, exp6);
        }

        //[TestMethod]
        //public void RegExTest()
        //{
        //    string exp1 = "samsung galaxy j1 j120f (2016)";
        //    string exp2 = "samsung galaxy";
        //    string exp3 = "samsung";

        //    Assert.AreEqual(1, regTester(exp1));
        //    Assert.AreEqual(2, regTester(exp2));
        //    Assert.AreEqual(2, regTester(exp3));

        //}

        //private int regTester(string exp)
        //{
        //    List<string> list = new List<string>()
        //    {
        //        "samsung galaxy j1 j120f (2016)",
        //        "samsung",
        //        "samsung galaxy j1 j120f"
        //    };
        //    Regex reg = new Regex("\\b" + exp + "\\b");
        //    var query = list.Where(x => reg.IsMatch(x));
        //    return query.Count();
        //}
    }
}
