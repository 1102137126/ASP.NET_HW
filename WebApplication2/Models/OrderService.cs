using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace WebApplication2.Models
{
    public class OrderService
    {
        /// <summary>
        /// 取得DB連線字串
        /// </summary>
        /// <returns></returns>
        private string GetDBConnectionString()
        {
            return
                System.Configuration.ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString.ToString();
        }
        /// <summary>
        /// 新增訂單
        /// </summary>
        public void InsertOrder(Models.Order order)
        {
        }
        /// <summary>
        /// 刪除訂單 By Id
        /// </summary>
        public void DeleteOrderById(String id)
        {
        }
        /// <summary>
        /// 修改訂單
        /// </summary>
        public void UpdateOrder()
        {
        }
        /// <summary>
        /// 取得訂單
        /// </summary>
        /// <param name="id">訂單ID</param>
        /// <returns></returns>
        public Models.Order GetOrderById(string orderId)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT 
	                        O.OrderID, O.CustomerID, C.CompanyName, 
	                        O.EmployeeID, E.LastName + E.FirstName AS EmpName, 
	                        O.OrderDate, O.RequiredDate, O.ShippedDate, 
	                        O.ShipperID, S.CompanyName, O.Freight, 
	                        O.ShipName, O.ShipAddress, O.ShipCity, O.ShipRegion, O.ShipPostalCode, O.ShipCountry
                        FROM Sales.Orders AS O 
	                        INNER JOIN Sales.Customers AS C ON O.CustomerID = C.CustomerID
	                        INNER JOIN HR.Employees AS E ON O.EmployeeID = E.EmployeeID
	                        INNER JOIN Sales.Shippers AS S ON O.ShipperID = S.ShipperID
                        WHERE O.OrderID = @OrderId";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@OrderId", orderId));

                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapOrderDataToList(dt).FirstOrDefault();
        }
        /// <summary>
        /// 取得訂單
        /// </summary>
        /// <returns></returns>
        public List<Models.Order> GetOrders()
        {
            return new List<Order>();
        }

        private List<Models.Order> MapOrderDataToList(DataTable orderData)
        {
            List<Models.Order> result = new List<Order>();


            foreach (DataRow row in orderData.Rows)
            {
                result.Add(new Order()
                {
                    CustId = row["CustomerID"].ToString(),
                    CustName = row["CustName"].ToString(),
                    EmpId = (int)row["EmployeeID"],
                    EmpName = row["EmpName"].ToString(),
                    Freight = (decimal)row["Freight"],//(decimal)row["Freight"],
                    OrderDate = row["OrderDate"] == DBNull.Value ? (DateTime?)null : (DateTime)row["OrderDate"],
                    OrderId = (int)row["OrderId"],
                    RequireDdate = row["RequireDdate"] == DBNull.Value ? (DateTime?)null : (DateTime)row["RequireDdate"],
                    ShipAddress = row["ShipAddress"].ToString(),
                    ShipCity = row["ShipCity"].ToString(),
                    ShipCountry = row["ShipCountry"].ToString(),
                    ShipName = row["ShipName"].ToString(),
                    ShippedDate = row["ShippedDate"] == DBNull.Value ? (DateTime?)null : (DateTime)row["ShippedDate"],
                    ShipperId = (int)row["ShipperID"],
                    ShipperName = row["ShipperName"].ToString(),
                    ShipPostalCode = row["ShipPostalCode"].ToString(),
                    ShipRegion = row["ShipRegion"].ToString()
                });
            }
            return result;
        }
        /// <summary>
		/// 依照條件取得訂單資料
		/// </summary>
		/// <returns></returns>
		public List<Models.Order> GetOrderByCondtioin(Models.OrderSearchArg arg)
        {

            DataTable dt = new DataTable();
            string sql = @"SELECT 
					O.OrderId,O.CustomerID,C.Companyname As CustName,
	                O.EmployeeID,E.lastname+ E.firstname As EmpName,
	                O.OrderDate,O.RequireDdate,O.ShippedDate,
	                O.ShipperID,S.companyname As ShipperName,O.Freight,
	                O.ShipName,O.ShipAddress,O.ShipCity,O.ShipRegion,O.ShipPostalCode,O.ShipCountry
                    From Sales.Orders As O 
	                INNER JOIN Sales.Customers As C ON O.CustomerID=C.CustomerID
	                INNER JOIN HR.Employees As E On O.EmployeeID=E.EmployeeID
	                inner JOIN Sales.Shippers As S ON O.ShipperID=S.ShipperID
					Where (C.Companyname Like @CustName Or @CustName='') And 
						  (O.OrderDate=@OrderDate Or @OrderDate='')  And 
						  (O.EmployeeID=@EmpId Or @EmpId='') And 
						  (O.OrderId=@OrderId Or @OrderId='') And 
						  (O.ShipperID=@ShipperId Or @ShipperId='') And 
						  (O.ShippedDate=@ShippedDate Or @ShippedDate='') And 
						  (O.RequireDdate=@RequireDdate Or @RequireDdate='')";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@CustName", arg.CustName == null ? string.Empty : arg.CustName));
                cmd.Parameters.Add(new SqlParameter("@OrderDate", arg.OrderDate == null ? string.Empty : arg.OrderDate));
                cmd.Parameters.Add(new SqlParameter("@EmpId", arg.EmpId == null ? string.Empty : arg.EmpId));
                cmd.Parameters.Add(new SqlParameter("@OrderId", arg.OrderId == null ? string.Empty : arg.OrderId));
                cmd.Parameters.Add(new SqlParameter("@ShipperId", arg.ShipperId == null ? string.Empty : arg.ShipperId));
                cmd.Parameters.Add(new SqlParameter("@ShippedDate", arg.ShippedDate == null ? string.Empty : arg.ShippedDate));
                cmd.Parameters.Add(new SqlParameter("@RequireDdate", arg.RequireDdate == null ? string.Empty : arg.RequireDdate));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }


            return this.MapOrderDataToList(dt);
        }
    }
}