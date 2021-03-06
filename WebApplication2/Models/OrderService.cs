﻿using System;
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
        public int InsertOrder(Models.Order order)
        {
            string sql = @"INSERT INTO Sales.Orders(
	                        CustomerID,EmployeeID,orderdate,requireddate,shippeddate,shipperid,freight,
	                        shipname,shipaddress,shipcity,shipregion,shippostalcode,shipcountry
                        )VALUES(
							@custid,@empid,@orderdate,@requireddate,@shippeddate,@shipperid,@freight,
							@shipname,@shipaddress,@shipcity,@shipregion,@shippostalcode,@shipcountry
						)
						SELECT SCOPE_IDENTITY()";
            int orderId;
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@custid", order.CustId));
                cmd.Parameters.Add(new SqlParameter("@empid", order.EmpId));
                cmd.Parameters.Add(new SqlParameter("@orderdate", order.OrderDate));
                cmd.Parameters.Add(new SqlParameter("@requireddate", order.RequireDdate));
                cmd.Parameters.Add(new SqlParameter("@shippeddate", order.ShippedDate));
                cmd.Parameters.Add(new SqlParameter("@shipperid", order.ShipperId));
                cmd.Parameters.Add(new SqlParameter("@freight", order.Freight));
                cmd.Parameters.Add(new SqlParameter("@shipname", order.ShipName));
                cmd.Parameters.Add(new SqlParameter("@shipaddress", order.ShipAddress));
                cmd.Parameters.Add(new SqlParameter("@shipcity", order.ShipCity));
                cmd.Parameters.Add(new SqlParameter("@shipregion", order.ShipRegion));
                cmd.Parameters.Add(new SqlParameter("@shippostalcode", order.ShipPostalCode));
                cmd.Parameters.Add(new SqlParameter("@shipcountry", order.ShipCountry));

                orderId = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
            }
            return orderId;
        }
        /// <summary>
        /// 刪除訂單 By orderId
        /// </summary>
        public void DeleteOrderById(String orderId)
        {
            try
            {
                string sql = "DELETE FROM Sales.Orders Where orderid=@orderid";
                using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add(new SqlParameter("@orderid", orderId));
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 修改訂單
        /// </summary>
        public void UpdateOrder(Models.Order order)
        {
            try
            {
                string sql = @"UPDATE Sales.Orders SET 
	                        CustomerID=@custid,EmployeeID=@empid,orderdate=@orderdate,requireddate=@requireddate,
                            shippeddate=@shippeddate,shipperid=@shipperid,freight=@freight,shipname=@shipname,
                            shipaddress=@shipaddress,shipcity=@shipcity,shipregion=@shipregion,
                            shippostalcode=@shippostalcode,shipcountry=@shipcountry                          
                            WHERE orderid=@orderid";
                using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add(new SqlParameter("@orderid", order.OrderId));
                    cmd.Parameters.Add(new SqlParameter("@custid", order.CustId));
                    cmd.Parameters.Add(new SqlParameter("@empid", order.EmpId));
                    cmd.Parameters.Add(new SqlParameter("@orderdate", order.OrderDate));
                    cmd.Parameters.Add(new SqlParameter("@requireddate", order.RequireDdate));
                    cmd.Parameters.Add(new SqlParameter("@shippeddate", order.ShippedDate));
                    cmd.Parameters.Add(new SqlParameter("@shipperid", order.ShipperId));
                    cmd.Parameters.Add(new SqlParameter("@freight", order.Freight));
                    cmd.Parameters.Add(new SqlParameter("@shipname", order.ShipName));
                    cmd.Parameters.Add(new SqlParameter("@shipaddress", order.ShipAddress));
                    cmd.Parameters.Add(new SqlParameter("@shipcity", order.ShipCity));
                    cmd.Parameters.Add(new SqlParameter("@shipregion", order.ShipRegion));
                    cmd.Parameters.Add(new SqlParameter("@shippostalcode", order.ShipPostalCode));
                    cmd.Parameters.Add(new SqlParameter("@shipcountry", order.ShipCountry));

                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 依照訂單ID取得訂單資料
        /// </summary>
        /// <param name="id">訂單ID</param>
        /// <returns></returns>
        public Models.Order GetOrderById(string orderId)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT 
	                O.OrderId,O.CustomerID,C.Companyname As CustName,
	                O.EmployeeID,E.lastname+ E.firstname As EmpName,
	                O.OrderDate,O.RequireDdate,O.ShippedDate,
	                O.ShipperID,S.companyname As ShipperName,O.Freight,
	                O.ShipName,O.ShipAddress,O.ShipCity,O.ShipRegion,O.ShipPostalCode,O.ShipCountry
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
                    FROM Sales.Orders As O 
	                    INNER JOIN Sales.Customers As C ON O.CustomerID=C.CustomerID
	                    INNER JOIN HR.Employees As E On O.EmployeeID=E.EmployeeID
	                    INNER JOIN Sales.Shippers As S ON O.ShipperID=S.ShipperID
					WHERE (C.Companyname Like @CustName Or @CustName='') And 
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
                    Freight = (decimal)row["Freight"],
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
    }
}