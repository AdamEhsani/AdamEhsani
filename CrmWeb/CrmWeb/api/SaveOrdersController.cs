using CrmWeb.Data;
using CrmWeb.Pages.Clients;
using Microsoft.AspNetCore.Mvc;
using sun.reflect.generics.tree;
using System.Data.SqlClient;

namespace CrmWeb.api
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SaveOrdersController : Controller
    {
        DbAddress Db = new DbAddress();
        int customerId;
        string customerName = string.Empty;
        string customerPhone = string.Empty;
        string customerAddress = string.Empty;
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string SaveOrder([FromBody] Orders order)
        {
            var partnerId = Request.Cookies["PartnerId"];
            using (SqlConnection connection = new SqlConnection(Db.DB()))
            {
                connection.Open();
                string selectSql = "SELECT * FROM Customer WHERE PartnerId = @partnerId AND Phone = @Phone;";

                using (SqlCommand selectCommand = new SqlCommand(selectSql, connection))
                {
                    selectCommand.Parameters.AddWithValue("@Phone", order.Phone);
                    selectCommand.Parameters.AddWithValue("@partnerId", partnerId);
                    SqlDataReader reader = selectCommand.ExecuteReader();

                    if (reader.Read())
                    {
                        customerId = reader.GetInt32(0);
                        customerName = reader.GetString(1).Trim();
                        customerPhone = reader.GetString(2).Trim();
                        customerAddress = reader.GetString(3).Trim();

                        reader.Close();
                    }
                    else
                    {
                        reader.Close();

                        string saveNewCustomer = "INSERT INTO Customer (Name, Address, Phone, PLZ, PartnerId) VALUES (@Name, @Address, @Phone, @PLZ, @partnerId);";

                        using (SqlCommand insertCommand = new SqlCommand(saveNewCustomer, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@partnerId", partnerId);
                            insertCommand.Parameters.AddWithValue("@Name", order.Name.Trim());
                            insertCommand.Parameters.AddWithValue("@Address", order.Address.Trim());
                            insertCommand.Parameters.AddWithValue("@Phone", order.Phone);
                            insertCommand.Parameters.AddWithValue("@PLZ", string.Empty);

                            insertCommand.ExecuteNonQuery();
                        }

                        reader.Close();
                        string selectNewCustomer = "SELECT * FROM Customer WHERE PartnerId = @partnerId AND Phone = @Phone;";
                        using (SqlCommand newCustomer = new SqlCommand(selectNewCustomer, connection))
                        {
                            newCustomer.Parameters.AddWithValue("@Phone", order.Phone);
                            newCustomer.Parameters.AddWithValue("@partnerId", partnerId);
                            SqlDataReader Reader = newCustomer.ExecuteReader();

                            if (Reader.Read())
                            {
                                customerId = Reader.GetInt32(0);
                                Reader.Close();
                            }
                        }

                        customerName = order.Name.Trim();
                        customerPhone = order.Phone.Trim();
                        customerAddress = order.Address.Trim();
                    }
                }

                string saveOrder = "INSERT INTO Orders (CustomerName, CustomerAddress, OrderDate, TotalPrice, PartnerId, CustomerId) VALUES (@Name, @Address, @Date, @Price, @partnerId, @CustomerId);";
                using (SqlCommand command = new SqlCommand(saveOrder, connection))
                {
                    command.Parameters.AddWithValue("@partnerId", partnerId);
                    command.Parameters.AddWithValue("@Name", customerName.Trim());
                    command.Parameters.AddWithValue("@Address", customerAddress.Trim());
                    command.Parameters.AddWithValue("@Date", DateTime.Now);
                    command.Parameters.AddWithValue("@Price", order.TotalPrice);
                    command.Parameters.AddWithValue("@CustomerId", customerId);

                    command.ExecuteNonQuery();
                }
            }
            return SetLastOrderID();
        }

        private string SetLastOrderID()
        {
            var partnerId = Request.Cookies["PartnerId"];
            using (SqlConnection connection = new SqlConnection(Db.DB()))
            {
                connection.Open();
                string lastOrderId = "SELECT MAX(Id) FROM Orders WHERE PartnerId = @partnerId;";
                using (SqlCommand newCustomer = new SqlCommand(lastOrderId, connection))
                {
                    newCustomer.Parameters.AddWithValue("@partnerId", partnerId);
                    SqlDataReader Reader = newCustomer.ExecuteReader();

                    if (Reader.Read())
                    {
                        return $"{Reader.GetInt32(0)}";
                    }
                }
            }
            return "";
        }
    }
}
