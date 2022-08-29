using ecommerce.Context;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ecommerce.Common;

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
        public ActionResult Index(string CurrentFilter, string SearchString, int? page)
        {
            var strSession = Session["isAdmin"];
            if (strSession == null)
            {
                return RedirectToAction("Login", "Home");

            }
            var lstProduct = new List<Product>();
            if (SearchString !=null)
            {
                page = 1;
            }
            else
            {
                SearchString = CurrentFilter;
            }
            if (!string.IsNullOrEmpty(SearchString))
            {
                //lay danh sach san pham theo tu khoa tim kiem
                lstProduct = objecomerce_Asp_MvcEntities.Products.Where(n=>n.Name.Contains(SearchString)).ToList();

            }
            else
            {
                //lấy tất cả sản phẩm(không bị xóa)
                lstProduct = objecomerce_Asp_MvcEntities.Products.Where(n => n.Deleted == false).ToList();
            }
            ViewBag.CurrentFilter = SearchString;
            int pageSize = 7;
            int pageNumber = (page ?? 1);
            //sap xep theo id san pham. san pham moi dua len dau
            lstProduct = lstProduct.OrderByDescending(n => n.Id).ToList();
            return View(lstProduct.ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Create()
        {
            this.LoadData();
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(Product objProduct)
        {
            this.LoadData();
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
                SetAlert("Thêm sản phẩm thành công!", "success");
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(objProduct);
            }
        }
        public ActionResult Details(int Id)
        {
            var objProduct = objecomerce_Asp_MvcEntities.Products.Where(n => n.Id == Id).FirstOrDefault();
            objProduct.CategoryName = objecomerce_Asp_MvcEntities.Categories.Where(n => n.Id == objProduct.CategoryId).FirstOrDefault().Name;
            objProduct.BrandName = objecomerce_Asp_MvcEntities.Brands.Where(n => n.Id == objProduct.BrandId).FirstOrDefault().Name;
            return View(objProduct);
        }
        //xóa vĩnh viễn
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
            SetAlert("Xóa sản phẩm thành công", "success");
            return RedirectToAction("Index");
        }
        //Thùng rác
        public ActionResult Trash()
        {
            var lstProduct = new List<Product>();
            lstProduct = objecomerce_Asp_MvcEntities.Products.Where(n => n.Deleted == true).ToList();
            return View(lstProduct);
        }
        //Đưa sản phẩm vào thùng rác
        public ActionResult DeleteTrash(int id)
        {
            Product product = objecomerce_Asp_MvcEntities.Products.Find(id);
            product.Deleted = true;
            objecomerce_Asp_MvcEntities.Entry(product).State = EntityState.Modified;
            objecomerce_Asp_MvcEntities.SaveChanges();
            return RedirectToAction("Index");
        }
        //khôi phục sản phẩm từ thùng rác
        public ActionResult ReTrash(int Id)
        {
            Product product = objecomerce_Asp_MvcEntities.Products.Find(Id);
            product.Deleted = false;
            objecomerce_Asp_MvcEntities.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            this.LoadData();
            var objProduct = objecomerce_Asp_MvcEntities.Products.Where(n => n.Id == Id).FirstOrDefault();
            return View(objProduct);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(Product objProduct)
        {
            this.LoadData();
            try
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
                SetAlert("Sửa sản phẩm thành công!", "success");
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(objProduct);
            }
            
        }
        void LoadData()
        {
            Common objCommon = new Common();
            var lstCat = objecomerce_Asp_MvcEntities.Categories.ToList();
            ListtoDataTableConverter converter = new ListtoDataTableConverter();
            DataTable dtCategory = converter.ToDataTable(lstCat);
            ViewBag.ListCategory = objCommon.ToSelectList(dtCategory, "Id", "Name");

            var lstBrand = objecomerce_Asp_MvcEntities.Brands.ToList();
            DataTable dtBrand = converter.ToDataTable(lstBrand);
            ViewBag.ListBrand = objCommon.ToSelectList(dtBrand, "Id", "Name");

            //Loại sản phẩm
            List<ProductType> lstProductType = new List<ProductType>();
            ProductType objProductType = new ProductType();
            objProductType.Id = 01;
            objProductType.Name = "Giảm giá sốc";
            lstProductType.Add(objProductType);

            objProductType = new ProductType();
            objProductType.Id = 02;
            objProductType.Name = "Đề xuất";
            lstProductType.Add(objProductType);

            DataTable dtProductType = converter.ToDataTable(lstProductType);
            ViewBag.ProductType = objCommon.ToSelectList(dtProductType, "Id", "Name");
        }
        void SetAlert(string message, string type)
        {

            if (type == "success")
            {
                TempData["AlertMessage"] = "Thành công!   " + message;
                TempData["AlertType"] = "alert-success";
            }
            else if (type == "error")
            {
                TempData["AlertMessage"] = "Thất bại!   " + message;
                TempData["AlertType"] = "alert-danger";
            }
        }
    }
}