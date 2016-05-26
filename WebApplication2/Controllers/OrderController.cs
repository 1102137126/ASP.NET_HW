using KendoGridBinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

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
                int a = orderService.InsertOrder(order);
                orderDetailsService.InsertOrderDetail(order.OrderDetails, a);
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
            order.OrderDetails = this.orderDetailsService.GetOrderByOrderId(id);
            ViewBag.EmpCodeData = this.codeService.GetEmp();
            ViewBag.ShipCodeData = this.codeService.GetShipper();
            ViewBag.CustCodeData = this.codeService.GetCustomer();
            ViewBag.OrderDate = string.Format("{0:yyyy-MM-dd}", order.OrderDate);
            ViewBag.RequireDdate = string.Format("{0:yyyy-MM-dd}", order.RequireDdate);
            ViewBag.ShippedDate = string.Format("{0:yyyy-MM-dd}", order.ShippedDate);
            ViewBag.ProductCodeData = this.codeService.GetProduct();
            ViewBag.OrderData = order;
            return View(order);
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
            orderDetailsService.UpdateOrderDeail(order.OrderDetails, order.OrderId);
            ViewBag.EmpCodeData = this.codeService.GetEmp();
            ViewBag.ShipCodeData = this.codeService.GetShipper();
            return View("Index");
        }
        /// <summary>
        /// 依訂單ID跟產品ID刪除訂單明細
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult DeleteOrderDetail(string orderId, string productId)
        {
            try
            {
                orderDetailsService.DeleteOrderDetailByProductId(orderId, productId);
                return this.Json(true);
            }
            catch (Exception)
            {
                return this.Json(false);
            }
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
                orderDetailsService.DeleteOrderDetail(orderId);
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

        public JsonResult SearchResultSection(KendoGridRequest request, Models.OrderSearchArg arg)
        {
            //string a = Split(id, ",");
            //var result = this.orderService.GetOrderByCondtioin(keywd); ;
            return null;
            //return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult code(KendoGridRequest request, string keywd)
        {
            var result = this.codeService.GetProduct();
            switch (keywd)
            {
                case "#Product":
                    result = this.codeService.GetProduct();
                    break;
                case "#ShipperId":
                    result = this.codeService.GetShipper();
                    break;
                case "#EmpId":
                    result = this.codeService.GetEmp();
                    break;
                case "#CustName":
                    result = this.codeService.GetCustomer();
                    break;
                default:
                    result = this.codeService.GetCustomer();
                    break;
            }
            //return Json(new KendoGrid<SelectListItem>(request, result));
            return Json(result, JsonRequestBehavior.AllowGet);
        }

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