using ecommerce.Context;
using ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ecommerce.Controllers
{
    public class CategoryController : Controller
    {
        ecomerce_asp_mvcEntities objecomerce_Asp_MvcEntities = new ecomerce_asp_mvcEntities();
        // GET: Category
        public ActionResult Index()
        {
            var ListCategory = objecomerce_Asp_MvcEntities.Categories.ToList();
            return View(ListCategory);
        }

        public ActionResult ProductCategory(int Id)
        {
            CategoryModel objCategoryModel = new CategoryModel();
            objCategoryModel.ListProduct = objecomerce_Asp_MvcEntities.Products.Where(n => n.CategoryId == Id).ToList();
            objCategoryModel.ListCategory = objecomerce_Asp_MvcEntities.Categories.ToList();
            return View(objCategoryModel);
        }
    }
}