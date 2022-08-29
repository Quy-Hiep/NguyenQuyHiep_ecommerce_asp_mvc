using ecommerce.Context;
using ecommerce.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ecommerce.Controllers
{
    public class HomeController : Controller
    {
        ecomerce_asp_mvcEntities objecomerce_Asp_MvcEntities = new ecomerce_asp_mvcEntities();
        public ActionResult Index()
        {
            HomeModel objHomeModel = new HomeModel();
            objHomeModel.ListCategory = objecomerce_Asp_MvcEntities.Categories.ToList();
            objHomeModel.ListProduct = objecomerce_Asp_MvcEntities.Products.ToList();
            return View(objHomeModel);
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User _user)
        {
            if (ModelState.IsValid)
            {
                var check = objecomerce_Asp_MvcEntities.Users.FirstOrDefault(s => s.Email == _user.Email);
                if (check == null)
                {
                    _user.Password = GetMD5(_user.Password);
                    objecomerce_Asp_MvcEntities.Configuration.ValidateOnSaveEnabled = false;
                    objecomerce_Asp_MvcEntities.Users.Add(_user);
                    objecomerce_Asp_MvcEntities.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Email already exists";
                    return View();
                }


            }
            return View();


        }

        //create a string MD5
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

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            if (ModelState.IsValid)
            {


                var f_password = GetMD5(password);
                var data = objecomerce_Asp_MvcEntities.Users.Where(s => s.Email.Equals(email) && s.Password.Equals(f_password)).ToList();
                if (data.Count() > 0)
                {
                    //add session
                    Session["FullName"] = data.FirstOrDefault().FirstName + " " + data.FirstOrDefault().LastName;
                    Session["Email"] = data.FirstOrDefault().Email;
                    Session["idUser"] = data.FirstOrDefault().Id;
                    Session["idAdmin"] = data.FirstOrDefault().IsAdmin;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Login failed";
                    return RedirectToAction("Login");
                }
            }
            return View();
        }


        //Logout
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Login");
        }

        public ActionResult Search(string currentFilter, string SearchString, int? page)
        {
            var lstProduct = new List<Product>();
            var ListCategory = objecomerce_Asp_MvcEntities.Categories.ToList();
            var ListBrand = objecomerce_Asp_MvcEntities.Brands.ToList();
            var count = 0;//dùng để đếm tổng số sản phẩm
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
                //lấy danh sách sản phẩm theo từ khóa tìm kiếm
                lstProduct = objecomerce_Asp_MvcEntities.Products.Where(n => /*n.Deleted == false &&*/ n.Name.Contains(SearchString)).ToList();
            }
            else
            {
                //lấy all sản phẩm trong bảng product
                lstProduct = objecomerce_Asp_MvcEntities.Products/*.Where(n => n.Deleted == false)*/.ToList();
            }
            //vòng lặp đếm tổng số sản phẩm kiếm được
            foreach (var item in lstProduct)
            {
                count += 1;
            }
            ViewBag.Count = count;
            ViewBag.CurrentFilter = SearchString;
            ViewBag.Category = ListCategory;
            ViewBag.Brand = ListBrand;
            //số lượng item cua 1 trang
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            //sắp xếp theo id sản phẩm,sp mới đưa lên đầu
            lstProduct = lstProduct.OrderByDescending(n => n.Id).ToList();
            return View(lstProduct.ToPagedList(pageNumber, pageSize));
        }






        public ActionResult About()
        {
            ViewBag.Message = "Về chúng tôi.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Cộng tác với chúng tôi.";

            return View();
        }
    }
}