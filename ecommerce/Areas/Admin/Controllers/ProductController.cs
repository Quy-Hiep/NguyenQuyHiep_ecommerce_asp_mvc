using ecommerce.Context;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ecommerce.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        ecomerce_asp_mvcEntities objecomerce_Asp_MvcEntities = new ecomerce_asp_mvcEntities();
        // GET: Admin/Product
        //public ActionResult Index(string SearchString)
        //{
        //    //var lstProduct = objecomerce_Asp_MvcEntities.Products.ToList();
        //    var lstProduct = objecomerce_Asp_MvcEntities.Products.Where(n=>n.Name.Contains(SearchString)).ToList();
        //    return View(lstProduct);
        //}
        public ActionResult Index(string curentFilter, string SearchString, int? page)
        {
            var lstProduct = new List<Product>();
            if (SearchString !=null)
            {
                page = 1;
            }
            else
            {
                SearchString = curentFilter;
            }
            if (!string.IsNullOrEmpty(SearchString))
            {
                //lay danh sach san pham theo tu khoa tim kiem
                lstProduct = objecomerce_Asp_MvcEntities.Products.Where(n=>n.Name.Contains(SearchString)).ToList();

            }
            else
            {
                //lay all san pham
                lstProduct = objecomerce_Asp_MvcEntities.Products.ToList();
            }
            ViewBag.CurrenFiler = SearchString;
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            //sap xep theo id san pham. san pham moi dua len dau
            lstProduct = lstProduct.OrderByDescending(n => n.Id).ToList();
            return View(lstProduct.ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Create()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult Create(Product objProduct)
        {
            try
            {
                if (objProduct.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(objProduct.ImageUpload.FileName);
                    string extension = Path.GetExtension(objProduct.ImageUpload.FileName);
                    fileName = fileName +"_" +long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                    objProduct.Avartar = fileName;
                    objProduct.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/content/images/items/"), fileName));
                }
                objecomerce_Asp_MvcEntities.Products.Add(objProduct);
                objecomerce_Asp_MvcEntities.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
            

        }
        public ActionResult Details(int Id)
        {
            var objProduct = objecomerce_Asp_MvcEntities.Products.Where(n => n.Id == Id).FirstOrDefault();
            return View(objProduct);
        }
        [HttpGet]
        public ActionResult Delete(int Id)
        {
            var objProduct = objecomerce_Asp_MvcEntities.Products.Where(n => n.Id == Id).FirstOrDefault();
            return View(objProduct);
        }
        [HttpPost]
        public ActionResult Delete(Product objPro)
        {
            var objProduct = objecomerce_Asp_MvcEntities.Products.Where(n => n.Id == objPro.Id).FirstOrDefault();
            objecomerce_Asp_MvcEntities.Products.Remove(objProduct);
            objecomerce_Asp_MvcEntities.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var objProduct = objecomerce_Asp_MvcEntities.Products.Where(n => n.Id == Id).FirstOrDefault();
            return View(objProduct);
        }
        [HttpPost]
        public ActionResult Edit(Product objProduct)
        {
            if (objProduct.ImageUpload != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(objProduct.ImageUpload.FileName);
                string extension = Path.GetExtension(objProduct.ImageUpload.FileName);
                fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                objProduct.Avartar = fileName;
                objProduct.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/content/images/items/"), fileName));
            }
            objecomerce_Asp_MvcEntities.Entry(objProduct).State = EntityState.Modified;
            objecomerce_Asp_MvcEntities.SaveChanges();
            return RedirectToAction("Index");
        }
   
        
    }
}