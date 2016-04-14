using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication2.Controllers
{
    public class OrderController : Controller
    {
        Models.CodeService codeService = new Models.CodeService();
        Models.OrderService orderService = new Models.OrderService();
        Models.OrderDetailsService orderDetailsService = new Models.OrderDetailsService();
        /// <summary>
        /// 訂單管理首頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.EmpCodeData = this.codeService.GetEmp(-1);
            ViewBag.ShipCodeData = this.codeService.GetShipper(-1);
            return View();
        }
        /// <summary>
        /// 取得訂單查詢結果
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        [HttpPost()]
        public ActionResult Index(Models.OrderSearchArg arg)
        {
            ViewBag.EmpCodeData = this.codeService.GetEmp(-1);
            ViewBag.ShipCodeData = this.codeService.GetShipper(-1);
            ViewBag.SearchResult = orderService.GetOrderByCondtioin(arg);
            return View("Index");
        }
        /// <summary>
        /// 新增訂單的畫面
        /// </summary>
        /// <returns></returns>
        public ActionResult InsertOrder()
        {
            ViewBag.EmpCodeData = this.codeService.GetEmp(-1);
            ViewBag.ShipCodeData = this.codeService.GetShipper(-1);
            ViewBag.CustCodeData = this.codeService.GetCustomer(-1);
            ViewBag.ProductCodeData = this.codeService.GetProduct();
            return View(new Models.Order());
        }
        /// <summary>
        /// 新增訂單存檔的Action
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost()]
        public ActionResult InsertOrder(Models.Order order)
        {
            if (ModelState.IsValid)
            {
                int a = orderService.InsertOrder(order);
                return RedirectToAction("Index/" + a);
            }
            return View(order);
            //return View();
        }
        /*public ActionResult InsertOrder(Models.Order order)
        {
            ViewBag.EmpCodeData = this.codeService.GetEmp();
            ViewBag.ShipCodeData = this.codeService.GetShipper();
            orderService.InsertOrder(order);
            return View("Index");
        }*/
        /// <summary>
        /// 依訂單ID取得訂單資料的畫面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet()]
        public ActionResult Update(string id)
        {
            Models.Order order = this.orderService.GetOrderById(id);
            Models.OrderDetails orderDetails = this.orderDetailsService.GetOrderByorderId(id);
            ViewBag.EmpCodeData = this.codeService.GetEmp(Convert.ToInt32(order.EmpId));
            ViewBag.ShipCodeData = this.codeService.GetShipper(Convert.ToInt32(order.ShipperId));
            ViewBag.CustCodeData = this.codeService.GetCustomer(Convert.ToInt32(order.CustId));
            ViewBag.OrderDate = string.Format("{0:yyyy-MM-dd}", order.OrderDate);
            ViewBag.RequireDdate = string.Format("{0:yyyy-MM-dd}", order.RequireDdate);
            ViewBag.ShippedDate = string.Format("{0:yyyy-MM-dd}", order.ShippedDate);
            ViewBag.OrderData = order;
            ViewBag.OrderDetailsData = orderDetails;
            return View(new Models.Order());
        }
        /// <summary>
        /// 修改訂單存檔的Action
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost()]
        public ActionResult Update(Models.Order order)
        {
            orderService.UpdateOrder(order);
            ViewBag.EmpCodeData = this.codeService.GetEmp(-1);
            ViewBag.ShipCodeData = this.codeService.GetShipper(-1);
            return View("Index");
            //ViewBag.EmpCodeData = this.codeService.GetEmp(Convert.ToInt32(order.EmpId));
            //ViewBag.ShipCodeData = this.codeService.GetShipper(Convert.ToInt32(order.ShipperId));
            //ViewBag.CustCodeData = this.codeService.GetCustomer(Convert.ToInt32(order.CustId));
            //ViewBag.OrderDate = string.Format("{0:yyyy-MM-dd}", order.OrderDate);
            //ViewBag.RequireDdate = string.Format("{0:yyyy-MM-dd}", order.RequireDdate);
            //ViewBag.ShippedDate = string.Format("{0:yyyy-MM-dd}", order.ShippedDate);
            //ViewBag.OrderData = order;
            //orderService.UpdateOrder(order);
            //return View(order);
        }
        /// <summary>
        /// 依訂單ID刪除訂單
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult DeleteOrder(string orderId)
        {
            try
            {
                orderService.DeleteOrderById(orderId);
                return this.Json(true);
            }
            catch (Exception)
            {

                return this.Json(false);
            }
        }
        /// <summary>
        /// 取得系統時間
        /// </summary>
        /// <returns></returns>
        /*public ActionResult GetSysDate()
        {
            return PartialView("_SysDatePartial");
        }*/

        
        [HttpGet()]
        public JsonResult TestJson()
        {
            var result = new Models.Order() { CustId = "1102137103", CustName = "阿涵" };
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Index2(string id)
        {
            return View();
        }
    }
}