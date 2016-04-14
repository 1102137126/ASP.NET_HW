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
    public class OrderDetailsService
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
        /*public List<Models.Order> InsertOrderDetail(Models.OrderDetails orderDetail)
        {
            string sql = @"INSERT INTO Sales.Orders(
	                        CustomerID,EmployeeID,orderdate,requireddate,shippeddate,shipperid,freight,
	                        shipname,shipaddress,shipcity,shipregion,shippostalcode,shipcountry
                        )VALUES(
							@custid,@empid,@orderdate,@requireddate,@shippeddate,@shipperid,@freight,
							@shipname,@shipaddress,@shipcity,@shipregion,@shippostalcode,@shipcountry
						)
						SELECT SCOPE_IDENTITY()
						";
            int orderId;
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@custid", order.CustId));

                orderId = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
            }
            return null;
        }*/
        public Models.OrderDetails GetOrderByorderId(string orderId)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT OrderID, ProductID, UnitPrice, Qty, Discount
                        FROM Sales.OrderDetails
                        WHERE OrderID=@OrderId";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@OrderId", orderId));

                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapOrderDetailsDataToList(dt).FirstOrDefault();
        }
        private List<Models.OrderDetails> MapOrderDetailsDataToList(DataTable orderData)
        {
            List<Models.OrderDetails> result = new List<OrderDetails>();

            foreach (DataRow row in orderData.Rows)
            {
                result.Add(new OrderDetails()
                {
                    OrderId = (int)row["OrderID"],
                    ProductId = (int)row["ProductID"],
                    UnitPrice = (int)row["UnitPrice"],
                    Qty = (decimal)row["Qty"],
                    Discount = (int)row["Discount"]
                });
            }
            return result;
        }
    }
}