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
    public class CategoryController : Controller
    {
        // GET: Admin/Category
        ecomerce_asp_mvcEntities objecomerce_Asp_MvcEntities = new ecomerce_asp_mvcEntities();
        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {
            //var strSession = Session["isAdmin"];
            //if (strSession == null)
            //{
            //    return RedirectToAction("LoginAdmin", "LoginAdmin");

            //}
            var lstCategory = new List<Category>();
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
                lstCategory = objecomerce_Asp_MvcEntities.Categories.Where(n => n.Deleted != true && n.Name.Contains(SearchString)).ToList();
            }
            else
            {
                //lấy all sản phẩm trong bảng product
                lstCategory = objecomerce_Asp_MvcEntities.Categories.Where(n => n.Deleted != true).ToList();
            }
            ViewBag.CurrentFilter = SearchString;
            //số lượng item cua 1 trang
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            //sắp xếp theo id sản phẩm,sp mới đưa lên đầu
            lstCategory = lstCategory.OrderByDescending(n => n.Id).ToList();
            return View(lstCategory.ToPagedList(pageNumber, pageSize));
        }
        //Thêm danh mục
        [HttpGet]
        public ActionResult Create()
        {

            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(Category objCategory)
        {

            try
            {
                if (objCategory.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(objCategory.ImageUpload.FileName);
                    string extension = Path.GetExtension(objCategory.ImageUpload.FileName);
                    fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                    objCategory.Avartar = fileName;
                    objCategory.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/content/images/items/"), fileName));
                }
                objecomerce_Asp_MvcEntities.Categories.Add(objCategory);
                objecomerce_Asp_MvcEntities.SaveChanges();
                SetAlert("Thêm Thương hiệu thành công!", "success");
                return RedirectToAction("Index");
            }
            catch
            {

                return View(objCategory);
            }

        }


        //Sửa danh mục
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var opjCategory = objecomerce_Asp_MvcEntities.Categories.Where(n => n.Id == Id).FirstOrDefault();
            return View(opjCategory);
        }
        [HttpPost]
        public ActionResult Edit(int Id, Category objCategory)
        {
            try
            {
                if (objCategory.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(objCategory.ImageUpload.FileName);
                    string extension = Path.GetExtension(objCategory.ImageUpload.FileName);
                    fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                    objCategory.Avartar = fileName;
                    objCategory.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/content/images/items/"), fileName));
                }
                objecomerce_Asp_MvcEntities.Entry(objCategory).State = EntityState.Modified;
                objecomerce_Asp_MvcEntities.SaveChanges();
                SetAlert("Sửa Danh mục thành công!", "success");
                return RedirectToAction("Index");
            }
            catch
            {

                return View(objCategory);
            }

        }

        //Chi tiết danh mục
        public ActionResult Details(int Id)
        {
            var opjCategory = objecomerce_Asp_MvcEntities.Categories.Where(n => n.Id == Id).FirstOrDefault();
            return View(opjCategory);
        }

        //Xóa danh mục vĩnh viễn
        public ActionResult Delete(int Id)
        {
            var opjCategory = objecomerce_Asp_MvcEntities.Categories.Where(n => n.Id == Id).FirstOrDefault();
            return View(opjCategory);
        }
        [HttpPost]
        public ActionResult Delete(Category objCate)
        {
            var opjCategory = objecomerce_Asp_MvcEntities.Categories.Where(n => n.Id == objCate.Id).FirstOrDefault();
            objecomerce_Asp_MvcEntities.Categories.Remove(opjCategory);
            objecomerce_Asp_MvcEntities.SaveChanges();
            return RedirectToAction("Index");
        }
        //Thùng Rác
        public ActionResult Trash()
        {

            var lstCategory = objecomerce_Asp_MvcEntities.Categories.Where(n => n.Deleted == true).ToList();
            return View(lstCategory);
        }
        //Đưa vào thùng rác
        public ActionResult DeleteTrash(int id)
        {
            Category category = objecomerce_Asp_MvcEntities.Categories.Find(id);
            category.Deleted = true;
            category.UpdatedOnUtc = DateTime.Now;
            objecomerce_Asp_MvcEntities.Entry(category).State = EntityState.Modified;
            objecomerce_Asp_MvcEntities.SaveChanges();
            return RedirectToAction("Index");
        }
        //Khôi phục(xóa khỏi thùng rác)
        public ActionResult ReTrash(int id)
        {
            Category category = objecomerce_Asp_MvcEntities.Categories.Find(id);
            category.Deleted = false;
            category.UpdatedOnUtc = DateTime.Now;
            objecomerce_Asp_MvcEntities.Entry(category).State = EntityState.Modified;
            objecomerce_Asp_MvcEntities.SaveChanges();
            return RedirectToAction("Trash", "Category");
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