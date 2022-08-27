using ecommerce.Context;
using ecommerce.Models;
using PagedList;
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

        public ActionResult ProductCategory(int Id, int? page)
        {
            var ListProduct = objecomerce_Asp_MvcEntities.Products.Where(n => n.CategoryId == Id).ToList();
            var ListCategory = objecomerce_Asp_MvcEntities.Categories.ToList();
            var ListBrand = objecomerce_Asp_MvcEntities.Brands.ToList();
            var count = 0;//dùng để đếm tổng số sản phẩm
            foreach (var item in ListProduct)
            {
                count += 1;
            }
            ViewBag.Count = count;
            int pageSize = 5;
            int pageNumber = (page ?? 1);
           
            ViewBag.Category = ListCategory;
            ViewBag.Brand = ListBrand;

            //sắp xếp theo id sản phẩm,sp mới đưa lên đầu
            ListProduct = ListProduct.OrderByDescending(n => n.Id).ToList();
            return View(ListProduct.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult ProductCategor(int Id, int? page)
        {
            var listProduct = objecomerce_Asp_MvcEntities.Products.Where(n => n.CategoryId == Id).ToList();
            var lstBrand = objecomerce_Asp_MvcEntities.Brands.ToList();
            var count = 0;
            foreach (var item in listProduct)
            {
                count += 1;
            }
            ViewBag.Count = count;
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            List<object> brandArr = new List<object>();
            foreach (var item in lstBrand)
            {
                brandArr.Add(item);
            }
            ViewBag.Brand = brandArr;
            return View(listProduct.ToPagedList(pageNumber, pageSize));
        }
    }
}