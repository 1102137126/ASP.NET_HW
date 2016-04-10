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
        /// <summary>
        /// 訂單管理首頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.EmpCodeData = this.codeService.GetEmp();
            ViewBag.ShipCodeData = this.codeService.GetShipper();
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
            ViewBag.EmpCodeData = this.codeService.GetEmp();
            ViewBag.ShipCodeData = this.codeService.GetShipper();
            Models.OrderService orderService = new Models.OrderService();
            ViewBag.SearchResult = orderService.GetOrderByCondtioin(arg);
            return View("Index");
        }
        /// <summary>
        /// 新增訂單的畫面
        /// </summary>
        /// <returns></returns>
        public ActionResult InsertOrder()
        {
            ViewBag.EmpCodeData = this.codeService.GetEmp();
            ViewBag.ShipCodeData = this.codeService.GetShipper();
            ViewBag.CustCodeData = this.codeService.GetCustomer();
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
                return RedirectToAction("Index");
            }
            return View(order);
            //return View();
        }
        /*public ActionResult InsertOrder(Models.Order order)
        {
            ViewBag.EmpCodeData = this.codeService.GetEmp();
            ViewBag.ShipCodeData = this.codeService.GetShipper();
            Models.OrderService orderService = new Models.OrderService();
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
            ViewBag.OrderData = this.orderService.GetOrderById(id);
            return View();
        }
        /// <summary>
        /// 修改訂單存檔的Action
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost()]
        public ActionResult Update(Models.Order order)
        {
            Models.OrderService orderService = new Models.OrderService();
            orderService.UpdateOrder(order);
            return View("Index");
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
                Models.OrderService orderService = new Models.OrderService();
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