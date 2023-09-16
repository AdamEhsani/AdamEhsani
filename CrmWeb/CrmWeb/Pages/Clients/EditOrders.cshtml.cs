using CrmWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace CrmWeb.Pages.Clients
{
    public class EditOrdersModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public List<SettingModel> Store = new List<SettingModel>();
        public List<ProductInfo> Products = new List<ProductInfo>();
        public List<ProductInfo> SelecdedProduct = new List<ProductInfo>();
        public List<BillDetails> Details = new List<BillDetails>();
        public List<NewExtraItemModel> ExtraItems = new List<NewExtraItemModel>();
        public List<Orders> Orders = new List<Orders>();

        DbAddress Db = new DbAddress();

        [BindProperty]
        public string Name { get; set; }
        [BindProperty]
        public string Phone { get; set; }
        [BindProperty]
        public string Address { get; set; }
        [BindProperty]
        public string PLZ { get; set; }
        [BindProperty]
        public string ProduktInputName { get; set; }     
        
        [BindProperty]
        public string SumePrice { get; set; }

        [BindProperty]
        public string StoreName { get; set; }

        [BindProperty]
        public string StoreAddress { get; set; }

        [BindProperty]
        public string StorePhone { get; set; }

        [BindProperty]
        public string StoreCity { get; set; }

        [BindProperty]
        public string StorePLZ { get; set; }

        [BindProperty]
        public string StoreDeliveryPrice { get; set; }

        [BindProperty]
        public string StoreStId { get; set; }



        public void OnPost(string selectedValue)
        {
            string PartnerId = Request.Cookies["PartnerId"];

            using (SqlConnection connection = new SqlConnection(Db.DB()))
            {
                connection.Open();
                String sql = "SELECT * FROM Customer WHERE PartnerId = @partnerId AND Phone = @selectedValue;";

                if (!string.IsNullOrEmpty(selectedValue))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@selectedValue", selectedValue);
                        command.Parameters.AddWithValue("@partnerId", PartnerId);

                        using (SqlDataReader Reader = command.ExecuteReader())
                        {
                            if (Reader.Read())
                            {
                                Name = Reader.GetString(1);
                                Phone = selectedValue;
                                Address = Reader.GetString(3).Trim();
                            }
                        }
                    }
                }
            }

            using (SqlConnection connection = new SqlConnection(Db.DB()))
            {
                connection.Open();
                String sql = "SELECT * FROM Items WHERE PartnerId = @partnerId;";

                using (SqlCommand Command = new SqlCommand(sql, connection))
                {
                    Command.Parameters.AddWithValue("@partnerId", PartnerId);

                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            NewExtraItemModel extraItem = new NewExtraItemModel();
                            extraItem.Id = Reader.GetInt32(0);
                            extraItem.Item = Reader.GetString(1);
                            extraItem.PriceS = Reader.GetString(2);
                            extraItem.PriceM = Reader.GetString(3);
                            extraItem.PriceL = Reader.GetString(4);
                            extraItem.PriceXL = Reader.GetString(5);
                            extraItem.PriceXXL = Reader.GetString(6);
                            ExtraItems.Add(extraItem);
                        }
                    }
                }
            }

            using (SqlConnection connection = new SqlConnection(Db.DB()))
            {
                connection.Open();
                String sql = "SELECT * FROM Product WHERE PartnerId = @partnerId;";

                using (SqlCommand Command = new SqlCommand(sql, connection))
                {
                    Command.Parameters.AddWithValue("@partnerId", PartnerId);

                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            ProductInfo product = new ProductInfo();
                            product.Id = Reader.GetInt32(0);
                            product.Name = Reader.GetString(1);
                            product.PriceS = Reader.GetString(2);
                            product.PriceM = Reader.GetString(3);
                            product.PriceL = Reader.GetString(4);
                            product.PriceXL = Reader.GetString(5);
                            product.PriceXXL = Reader.GetString(6);
                            product.Category = GetCategory(Reader.GetInt32(8));

                            Products.Add(product);
                        }
                    }
                }
            }
            GetCostumers();
            StoreInfo();
        }

        public void OnGet()
        {
            string PartnerId = Request.Cookies["PartnerId"];
            using (SqlConnection connection = new SqlConnection(Db.DB()))
            {
                connection.Open();
                String sql = "SELECT * FROM Items WHERE PartnerId = @partnerId;";

                using (SqlCommand Command = new SqlCommand(sql, connection))
                {
                    Command.Parameters.AddWithValue("@partnerId", PartnerId);

                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            NewExtraItemModel extraItem = new NewExtraItemModel();
                            extraItem.Id = Reader.GetInt32(0);
                            extraItem.Item = Reader.GetString(1);
                            extraItem.PriceS = Reader.GetString(2);
                            extraItem.PriceM = Reader.GetString(3);
                            extraItem.PriceL = Reader.GetString(4);
                            extraItem.PriceXL = Reader.GetString(5);
                            extraItem.PriceXXL = Reader.GetString(6);
                            ExtraItems.Add(extraItem);
                        }
                    }
                }
            }

            using (SqlConnection connection = new SqlConnection(Db.DB()))
            {
                connection.Open();
                String sql = "SELECT * FROM Product WHERE PartnerId = @partnerId;";

                using (SqlCommand Command = new SqlCommand(sql, connection))
                {
                    Command.Parameters.AddWithValue("@partnerId", PartnerId);

                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            ProductInfo product = new ProductInfo();
                            product.Id = Reader.GetInt32(0);
                            product.Name = Reader.GetString(1);
                            product.PriceS = Reader.GetString(2);
                            product.PriceM = Reader.GetString(3);
                            product.PriceL = Reader.GetString(4);
                            product.PriceXL = Reader.GetString(5);
                            product.PriceXXL = Reader.GetString(6);
                            product.Category = GetCategory(Reader.GetInt32(8));

                            Products.Add(product);

                        }
                    }
                }
            }
            GetCostumers();
            GetOrderDetails();
            StoreInfo();
        }

        private void GetOrderDetails()
        {
            using (SqlConnection connection = new SqlConnection(Db.DB()))
            {
                connection.Open();

                String sql = "SELECT * FROM OrderDetails WHERE PartnerId = @PartnerId AND OrderId = @Id;";
                var partnerId = Request.Cookies["PartnerId"];

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);
                    command.Parameters.AddWithValue("@PartnerId", partnerId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            BillDetails details = new BillDetails()
                            {
                                Product = reader.GetString(2),
                                ExtraItems = reader.GetString(3),
                                Price = reader.GetString(4),
                            };
                            Details.Add(details);
                        }
                    }
                }
            }
        }

        private string GetCategory(int id)
        {
            using (SqlConnection connection = new SqlConnection(Db.DB()))
            {
                connection.Open();

                String sql = "SELECT * FROM Categories WHERE PartnerId = @PartnerId AND Id = @Id;";
                var partnerId = Request.Cookies["PartnerId"];

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@PartnerId", partnerId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader.GetString(1);
                        }
                    }
                }
            }
            return "";
        }

        public void GetCostumers()
        {
            string PartnerId = Request.Cookies["PartnerId"];
            int customerId = 0;
            using (SqlConnection connection = new SqlConnection(Db.DB()))
            {
                connection.Open();
                String selectOrder = "SELECT * FROM Orders WHERE Id = @Id AND PartnerId = @partnerId;";

                using (SqlCommand CommandOrder = new SqlCommand(selectOrder, connection))
                {
                    CommandOrder.Parameters.AddWithValue("@partnerId", PartnerId);
                    CommandOrder.Parameters.AddWithValue("@Id", Id);

                    using (SqlDataReader reader = CommandOrder.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            customerId = reader.GetInt32(7);
                            Address = reader.GetString(2);
                            SumePrice = reader.GetString(5);
                        }
                        reader.Close();
                    }
                }

                String sql = "SELECT * FROM Customer WHERE Id = @Id AND PartnerId = @partnerId;";

                using (SqlCommand Command = new SqlCommand(sql, connection))
                {
                    Command.Parameters.AddWithValue("@partnerId", PartnerId);
                    Command.Parameters.AddWithValue("@Id", customerId);

                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            Name = Reader.GetString(1);
                            Phone = Reader.GetString(2);
                        }
                    }
                }
            }
        }
        public void StoreInfo()
        {
            string PartnerId = Request.Cookies["PartnerId"];

            using (SqlConnection connection = new SqlConnection(Db.DB()))
            {
                connection.Open();
                String sql = "SELECT * FROM Setting WHERE PartnerId = @partnerId;";

                using (SqlCommand Command = new SqlCommand(sql, connection))
                {
                    Command.Parameters.AddWithValue("@partnerId", PartnerId);

                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            StoreName = GetStringFromReader(Reader, 1);
                            StoreAddress = GetStringFromReader(Reader, 2);
                            StoreCity = GetStringFromReader(Reader, 3);
                            StorePLZ = GetStringFromReader(Reader, 4);
                            StorePhone = GetStringFromReader(Reader, 5);
                            StoreStId = GetStringFromReader(Reader, 6);
                            StoreDeliveryPrice = GetStringFromReader(Reader, 7);
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

