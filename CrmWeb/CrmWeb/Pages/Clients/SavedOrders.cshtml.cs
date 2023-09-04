using CrmWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace CrmWeb.Pages.Clients
{
    public class SavedOrdersModel : PageModel
    {
        public List<Orders> Orders = new List<Orders>();
        public List<NewStaffModel> Drivers = new List<NewStaffModel>();
        DbAddress Db = new DbAddress();

        [BindProperty]
        public DateTime filterDate { get; set; }

        [BindProperty]
        public string filterDriver { get; set; }


        public void OnPost()
        {
            DateTime orderDate = DateTime.Now;
            string dateformated = filterDate.ToString("d");
            if (!string.IsNullOrEmpty(filterDriver) && dateformated != "01.01.0001")
            {
                var partnerId = Request.Cookies["PartnerId"];
                using (SqlConnection connection = new SqlConnection(Db.DB()))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Orders WHERE PartnerId = @partnerId";

                    using (SqlCommand Command = new SqlCommand(sql, connection))
                    {
                        Command.Parameters.AddWithValue("@partnerId", partnerId);

                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                orderDate = Reader.GetDateTime(3);
                                string orderDriver = GetStringFromReader(Reader, 4);
                                if (filterDriver == orderDriver && orderDate.ToString("d") == dateformated)
                                {
                                    Orders order = new Orders();
                                    order.Id = Reader.GetInt32(0);
                                    order.Name = Reader.GetString(1);
                                    order.Address = Reader.GetString(2);
                                    order.OrderDate = orderDate;
                                    order.Driver = orderDriver;
                                    order.TotalPrice = Reader.GetString(5);

                                    Orders.Add(order);
                                }
                            }
                        }
                    }
                }
                SetDrivers();
                filterDate = orderDate;
            }
            else if (dateformated == "01.01.0001" && !string.IsNullOrEmpty(filterDriver))
            {
                var partnerId = Request.Cookies["PartnerId"];
                using (SqlConnection connection = new SqlConnection(Db.DB()))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Orders WHERE PartnerId = @partnerId";

                    using (SqlCommand Command = new SqlCommand(sql, connection))
                    {
                        Command.Parameters.AddWithValue("@partnerId", partnerId);

                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                string orderDriver = GetStringFromReader(Reader, 4);
                                if (filterDriver == orderDriver)
                                {
                                    Orders order = new Orders();
                                    order.Id = Reader.GetInt32(0);
                                    order.Name = Reader.GetString(1);
                                    order.Address = Reader.GetString(2);
                                    order.OrderDate = Reader.GetDateTime(3);
                                    order.Driver = orderDriver;
                                    order.TotalPrice = Reader.GetString(5);

                                    Orders.Add(order);
                                }
                            }
                        }
                    }
                }
                SetDrivers();
            }
            else if (dateformated != "01.01.0001" && string.IsNullOrEmpty(filterDriver))
            {
                var partnerId = Request.Cookies["PartnerId"];
                using (SqlConnection connection = new SqlConnection(Db.DB()))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Orders WHERE PartnerId = @partnerId";

                    using (SqlCommand Command = new SqlCommand(sql, connection))
                    {
                        Command.Parameters.AddWithValue("@partnerId", partnerId);

                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                orderDate = Reader.GetDateTime(3);
                                if (orderDate.ToString("d") == dateformated)
                                {
                                    Orders order = new Orders();
                                    order.Id = Reader.GetInt32(0);
                                    order.Name = Reader.GetString(1);
                                    order.Address = Reader.GetString(2);
                                    order.OrderDate = orderDate;
                                    order.Driver = GetStringFromReader(Reader, 4);
                                    order.TotalPrice = Reader.GetString(5);

                                    Orders.Add(order);
                                }
                            }
                        }
                    }
                }
                SetDrivers();
                filterDate = orderDate;
            }
            else
            {
                OnGet();
            }
        }

        public void OnGet()
        {
            filterDate = DateTime.Today;
            var partnerId = Request.Cookies["PartnerId"];
            using (SqlConnection connection = new SqlConnection(Db.DB()))
            {
                connection.Open();
                String sql = "SELECT * FROM Orders WHERE PartnerId = @partnerId";

                using (SqlCommand Command = new SqlCommand(sql, connection))
                {
                    Command.Parameters.AddWithValue("@partnerId", partnerId);

                    using (SqlDataReader reader = Command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader.GetDateTime(3).ToString("d") == filterDate.ToString("d"))
                            {
                                Orders order = new Orders
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    Address = reader.GetString(2),
                                    OrderDate = DateTime.Parse(reader.GetDateTime(3).ToString("d")),
                                    Driver = GetStringFromReader(reader, 4),
                                    TotalPrice = reader.GetString(5)
                                };
                                Orders.Add(order);
                            }
                        }
                    }
                }
            }
            SetDrivers();
        }

        private void SetDrivers()
        {
            var partnerId = Request.Cookies["PartnerId"];
            using (SqlConnection connection = new SqlConnection(Db.DB()))
            {
                connection.Open();
                String query = "SELECT * FROM Staff WHERE PartnerId = @partnerId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@partnerId", partnerId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            NewStaffModel driver = new NewStaffModel();
                            driver.Id = reader.GetInt32(0);
                            driver.Name = reader.GetString(1);
                            Drivers.Add(driver);
                        }
                    }
                }
            }
        }

        private string GetStringFromReader(SqlDataReader reader, int columnIndex)
        {
            if (!reader.IsDBNull(columnIndex) && !string.IsNullOrEmpty(reader.GetString(columnIndex)))
            {
                return reader.GetString(columnIndex);
            }
            return null;
        }
    }
}
