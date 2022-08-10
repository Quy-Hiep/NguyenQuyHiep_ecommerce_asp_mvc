using ecommerce.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ecommerce.Controllers
{
    public class ProductController : Controller
    {
        ecomerce_asp_mvcEntities objecomerce_Asp_MvcEntities2 = new ecomerce_asp_mvcEntities();
        public ActionResult Detail(int Id)
        {
            var objProduct = objecomerce_Asp_MvcEntities2.Products.Where(n => n.Id == Id).FirstOrDefault();
            return View(objProduct);
        }
    }
}