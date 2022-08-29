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
    public class BrandController : Controller
    {
        // GET: Admin/Brand
        ecomerce_asp_mvcEntities objecomerce_Asp_MvcEntities = new ecomerce_asp_mvcEntities();
        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {
            var strSession = Session["isAdmin"];
            if (strSession == null)
            {
                return RedirectToAction("Login", "Home");

            }
            var lstBrand = new List<Brand>();
            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }
            if (!string.IsNullOrEmpty(SearchString))
            {
                //lấy danh sách danh mục theo từ khóa tìm kiếm
                lstBrand = objecomerce_Asp_MvcEntities.Brands.Where(n => n.Deleted != true && n.Name.Contains(SearchString)).ToList();
            }
            else
            {
                //lấy all sản phẩm trong bảng product
                lstBrand = objecomerce_Asp_MvcEntities.Brands.Where(n => n.Deleted != true).ToList();
            }
            ViewBag.CurrentFilter = SearchString;
            //số lượng item cua 1 trang
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            //sắp xếp theo id sản phẩm,sp mới đưa lên đầu
            lstBrand = lstBrand.OrderByDescending(n => n.Id).ToList();
            return View(lstBrand.ToPagedList(pageNumber, pageSize));
        }
        //Thêm thương hiệu
        [HttpGet]
        public ActionResult Create()
        {

            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(Brand objBrand)
        {

            try
            {
                if (objBrand.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(objBrand.ImageUpload.FileName);
                    string extension = Path.GetExtension(objBrand.ImageUpload.FileName);
                    fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                    objBrand.Avatar = fileName;
                    objBrand.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/content/images/items/"), fileName));
                }
                objecomerce_Asp_MvcEntities.Brands.Add(objBrand);
                objecomerce_Asp_MvcEntities.SaveChanges();
                SetAlert("Thêm Thương hiệu thành công!", "success");
                return RedirectToAction("Index");
            }
            catch
            {

                return View(objBrand);
            }

        }


        //Sửa thương hiệu
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var objBrand = objecomerce_Asp_MvcEntities.Brands.Where(n => n.Id == Id).FirstOrDefault();
            return View(objBrand);
        }
        [HttpPost]
        public ActionResult Edit(int Id, Brand objbrand)
        {
            try
            {
                if (objbrand.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(objbrand.ImageUpload.FileName);
                    string extension = Path.GetExtension(objbrand.ImageUpload.FileName);
                    fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                    objbrand.Avatar = fileName;
                    objbrand.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/content/images/items/"), fileName));
                }
                objecomerce_Asp_MvcEntities.Entry(objbrand).State = EntityState.Modified;
                objecomerce_Asp_MvcEntities.SaveChanges();
                SetAlert("Sửa Thương hiệu thành công!", "success");
                return RedirectToAction("Index");
            }
            catch
            {

                return View(objbrand);
            }

        }

        //Chi tiết danh mục
        public ActionResult Details(int Id)
        {
            var objBrand = objecomerce_Asp_MvcEntities.Brands.Where(n => n.Id == Id).FirstOrDefault();
            return View(objBrand);
        }

        //Xóa vĩnh viễn thương hiệu
        public ActionResult Delete(int Id)
        {
            var objBrand = objecomerce_Asp_MvcEntities.Brands.Where(n => n.Id == Id).FirstOrDefault();
            return View(objBrand);
        }
        [HttpPost]
        public ActionResult Delete(Brand objCate)
        {
            var objBrand = objecomerce_Asp_MvcEntities.Brands.Where(n => n.Id == objCate.Id).FirstOrDefault();
            objecomerce_Asp_MvcEntities.Brands.Remove(objBrand);
            objecomerce_Asp_MvcEntities.SaveChanges();
            return RedirectToAction("Index");
        }
        //Thùng Rác
        public ActionResult Trash()
        {

            var lstbrand = objecomerce_Asp_MvcEntities.Brands.Where(n => n.Deleted == true).ToList();
            return View(lstbrand);
        }
        //Đưa vào thùng rác
        public ActionResult DeleteTrash(int id)
        {
            Brand brand = objecomerce_Asp_MvcEntities.Brands.Find(id);
            brand.Deleted = true;
            brand.UpdatedOnUtc = DateTime.Now;
            objecomerce_Asp_MvcEntities.Entry(brand).State = EntityState.Modified;
            objecomerce_Asp_MvcEntities.SaveChanges();
            return RedirectToAction("Index");
        }
        //Khôi phục(xóa khỏi thùng rác)
        public ActionResult ReTrash(int id)
        {
            Brand brand = objecomerce_Asp_MvcEntities.Brands.Find(id);
            brand.Deleted = false;
            brand.UpdatedOnUtc = DateTime.Now;
            objecomerce_Asp_MvcEntities.Entry(brand).State = EntityState.Modified;
            objecomerce_Asp_MvcEntities.SaveChanges();
            return RedirectToAction("Trash", "brand");
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