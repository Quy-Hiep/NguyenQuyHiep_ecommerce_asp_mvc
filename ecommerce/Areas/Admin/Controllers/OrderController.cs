using ecommerce.Context;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ecommerce.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        ecomerce_asp_mvcEntities objecomerce_Asp_MvcEntities = new ecomerce_asp_mvcEntities();

        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {
            var strSession = Session["isAdmin"];
            if (strSession == null)
            {
                return RedirectToAction("Login", "home");

            }
            var lstOrder = new List<Order>();
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
                //lấy danh sách đơn hàng theo từ khóa tìm kiếm
                lstOrder = objecomerce_Asp_MvcEntities.Orders.Where(n => n.Status != 0).ToList();
            }
            else
            {
                //lấy all đơn hàng trong bảng Order
                lstOrder = objecomerce_Asp_MvcEntities.Orders.Where(n => n.Status != 0).ToList();
            }
            ViewBag.CurrentFilter = SearchString;
            //số lượng item cua 1 trang
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            //sắp xếp theo id sản phẩm,sp mới đưa lên đầu
            lstOrder = lstOrder.OrderByDescending(n => n.Id).ToList();
            return View(lstOrder.ToPagedList(pageNumber, pageSize));
        }
        //Sửa đơn hàng
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var objOrder = objecomerce_Asp_MvcEntities.Orders.Where(n => n.Id == Id).FirstOrDefault();
            return View(objOrder);
        }
        [HttpPost]
        public ActionResult Edit(int Id, Order objOrder)
        {
            objOrder.CreatedOnUtc = DateTime.Now;
            objecomerce_Asp_MvcEntities.Entry(objOrder).State = EntityState.Modified;
            objecomerce_Asp_MvcEntities.SaveChanges();
            return RedirectToAction("Index");
        }

        //Chi tiết đơn hàng
        public ActionResult Details(int Id)
        {
            var lstOrder = new List<OrderDetail>();
            lstOrder = objecomerce_Asp_MvcEntities.OrderDetails.Where(n => n.OrderId == Id).ToList();
            productInfo(lstOrder);
            return View(lstOrder);
        }
        
        //Xóa đơn hàng vĩnh viễn
        public ActionResult Delete(int Id)
        {
            var objOrder = objecomerce_Asp_MvcEntities.Orders.Where(n => n.Id == Id).FirstOrDefault();
            return View(objOrder);
        }
        [HttpPost]
        public ActionResult Delete(Order objOrd)
        {
            var objOrder = objecomerce_Asp_MvcEntities.Orders.Where(n => n.Id == objOrd.Id).FirstOrDefault();
            objecomerce_Asp_MvcEntities.Orders.Remove(objOrder);
            objecomerce_Asp_MvcEntities.SaveChanges();
            return RedirectToAction("Index");
        }
        //Thùng Rác
        public ActionResult Trash()
        {

            var lstOrder = objecomerce_Asp_MvcEntities.Orders.Where(n => n.Status == 0).ToList();
            return View(lstOrder);
        }
        //Đưa vào thùng rác
        public ActionResult DelTrash(int id)
        {
            Order order = objecomerce_Asp_MvcEntities.Orders.Find(id);
            order.Status = 0;
            objecomerce_Asp_MvcEntities.Entry(order).State = EntityState.Modified;
            objecomerce_Asp_MvcEntities.SaveChanges();
            return RedirectToAction("Index");
        }
        //Khôi phục (xóa khỏi thùng rác)
        public ActionResult ReTrash(int id)
        {
            Order order = objecomerce_Asp_MvcEntities.Orders.Find(id);
            order.Status = 1;
            objecomerce_Asp_MvcEntities.Entry(order).State = EntityState.Modified;
            objecomerce_Asp_MvcEntities.SaveChanges();
            return RedirectToAction("Trash", "Order");
        }
        void SetAlert(string message, string type)
        {

            if (type == "success")
            {
                TempData["AlertMessage"] = "THÀNH CÔNG! " + message;
                TempData["AlertType"] = "alert-success";
            }
            else if (type == "error")
            {
                TempData["AlertMessage"] = "THẤT BẠI!" + message;
                TempData["AlertType"] = "alert-danger";
            }
        }
        void userInfo(List<Order> lstOrder)
        {
            List<object> userArr = new List<object>();
            foreach (var item in lstOrder)
            {
                var objUser = objecomerce_Asp_MvcEntities.Users.FirstOrDefault(n => n.Id == item.UserId);
                item.UserInfo = objUser;
                userArr.Add(item.UserInfo);
            }
            ViewBag.userInfo = userArr;
        }
        void productInfo(List<OrderDetail> lstOrder)
        {
            List<object> productArr = new List<object>();
            foreach (var item in lstOrder)
            {
                var objProduct = objecomerce_Asp_MvcEntities.Products.FirstOrDefault(n => n.Id == item.ProductId);
                item.ProductInfo = objProduct;
                productArr.Add(item.ProductInfo);
            }
            ViewBag.productInfo = productArr;
        }
    }
}