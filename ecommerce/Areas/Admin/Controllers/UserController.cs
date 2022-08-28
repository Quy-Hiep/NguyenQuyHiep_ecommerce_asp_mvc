using ecommerce.Context;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ecommerce.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        // GET: Admin/User
        ecomerce_asp_mvcEntities objecomerce_Asp_MvcEntities = new ecomerce_asp_mvcEntities();

        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {
            var strSession = Session["isAdmin"];
            if (strSession == null)
            {
                return RedirectToAction("Login", "Home");

            }
            var lstUser = new List<User>();
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
                //lấy danh sách user theo từ khóa tìm kiếm(dựa theo họ, tên, mail)
                lstUser = objecomerce_Asp_MvcEntities.Users.Where(n => n.FirstName.Contains(SearchString) && n.FirstName.Contains(SearchString) && n.Email.Contains(SearchString)).ToList();
            }
            else
            {
                //lấy all user
                lstUser = objecomerce_Asp_MvcEntities.Users.ToList();
            }
            ViewBag.CurrentFilter = SearchString;
            //số lượng item cua 1 trang
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            //sắp xếp theo id ,user mới đưa lên đầu
            lstUser = lstUser.OrderByDescending(n => n.Id).ToList();
            return View(lstUser.ToPagedList(pageNumber, pageSize));
        }
        //Thêm User
        [HttpGet]
        public ActionResult Create()
        {

            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(User objUser)
        {

            if (ModelState.IsValid)
            {
                var check = objecomerce_Asp_MvcEntities.Users.FirstOrDefault(s => s.Email == objUser.Email);
                if (check == null)
                {
                    objUser.Password = GetMD5(objUser.Password);
                    objecomerce_Asp_MvcEntities.Configuration.ValidateOnSaveEnabled = false;
                    objecomerce_Asp_MvcEntities.Users.Add(objUser);
                    objecomerce_Asp_MvcEntities.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Email đã tồn trại";
                    return View();
                }

            }
            return View(objUser);
        }
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;
            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }

        //Sửa tài khoản
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var objUser = objecomerce_Asp_MvcEntities.Users.Where(n => n.Id == Id).FirstOrDefault();
            return View(objUser);
        }
        [HttpPost]
        public ActionResult Edit(int Id, User objUser)
        {
            if (ModelState.IsValid)
            {
                objUser.Password = GetMD5(objUser.Password);
                objecomerce_Asp_MvcEntities.Configuration.ValidateOnSaveEnabled = false;
                objecomerce_Asp_MvcEntities.Entry(objUser).State = EntityState.Modified;
                objecomerce_Asp_MvcEntities.SaveChanges();
                return RedirectToAction("Index", "User");
            }
            return View("Index"); //objUser      
        }


        //Xóa User
        public ActionResult Delete(int Id)
        {
            var objUser = objecomerce_Asp_MvcEntities.Users.Where(n => n.Id == Id).FirstOrDefault();
            return View(objUser);
        }
        [HttpPost]
        public ActionResult Delete(User objUse)
        {
            var objUser = objecomerce_Asp_MvcEntities.Users.Where(n => n.Id == objUse.Id).FirstOrDefault();
            objecomerce_Asp_MvcEntities.Users.Remove(objUser);
            objecomerce_Asp_MvcEntities.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}