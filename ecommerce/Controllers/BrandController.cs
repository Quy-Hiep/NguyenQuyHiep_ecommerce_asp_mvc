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
    public class BrandController : Controller
    {
        ecomerce_asp_mvcEntities objecomerce_Asp_MvcEntities = new ecomerce_asp_mvcEntities();
        // GET: Brand
        public ActionResult Index()
        {
            var ListBrand = objecomerce_Asp_MvcEntities.Brands.ToList();
            return View(ListBrand);
        }

        public ActionResult ProductBrand(int Id, int? page)
        {
            var ListProduct = objecomerce_Asp_MvcEntities.Products.Where(n => n.BrandId == Id).ToList();
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
    }
}