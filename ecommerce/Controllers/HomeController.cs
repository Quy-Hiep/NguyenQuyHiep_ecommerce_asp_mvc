using ecommerce.Context;
using ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ecommerce.Controllers
{
    public class HomeController : Controller
    {
        ecomerce_asp_mvcEntities objecomerce_Asp_MvcEntities2 = new ecomerce_asp_mvcEntities();
        public ActionResult Index()
        {
            HomeModel objHomeModel = new HomeModel();
            objHomeModel.ListCategory = objecomerce_Asp_MvcEntities2.Categories.ToList();
            objHomeModel.ListProduct = objecomerce_Asp_MvcEntities2.Products.ToList();
            return View(objHomeModel);
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