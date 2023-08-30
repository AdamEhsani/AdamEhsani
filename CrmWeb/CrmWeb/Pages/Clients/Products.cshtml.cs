using CrmWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace CrmWeb.Pages.Clients
{
    public class ProductsModel : PageModel
    {
        [BindProperty]
        public string Category { get; set; }

        DbAddress Db = new DbAddress();
        public List<ProductInfo> Products = new List<ProductInfo>();
        public List<Categories> Categories = new List<Categories>();
        public void OnGet()
        {
            SetProducts();
        }
        public void SetProducts()
        {
            var partnerId = Request.Cookies["PartnerId"];

            using (SqlConnection connection = new SqlConnection(Db.DB()))
            {
                connection.Open();
                String sql = "SELECT * FROM Product WHERE PartnerId = @partnerId";

                using (SqlCommand Command = new SqlCommand(sql, connection))
                {
                    Command.Parameters.AddWithValue("@partnerId", partnerId);
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
                SetCategory();
            }
        }
        public void SetCategory()
        {
            var partnerId = Request.Cookies["PartnerId"];

            using (SqlConnection connection = new SqlConnection(Db.DB()))
            {
                connection.Open();
                String sqlCategory = "SELECT * FROM Categories WHERE PartnerId = @partnerId";

                using (SqlCommand Command = new SqlCommand(sqlCategory, connection))
                {
                    Command.Parameters.AddWithValue("@partnerId", partnerId);
                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            Categories Category = new Categories();
                            Category.Id = Reader.GetInt32(0);
                            Category.Category = Reader.GetString(1);

                            Categories.Add(Category);

                        }
                    }
                }
            }
        }

        public void OnPost()
        {
            var partnerId = Request.Cookies["PartnerId"];
            if (!string.IsNullOrEmpty(Category))
            {
                using (SqlConnection connection = new SqlConnection(Db.DB()))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Product WHERE PartnerId = @partnerId";

                    using (SqlCommand Command = new SqlCommand(sql, connection))
                    {
                        Command.Parameters.AddWithValue("@partnerId", partnerId);
                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                if (GetCategory(Reader.GetInt32(8)) == Category)
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
                }
                SetCategory();
            }
            else
            {
                SetProducts();
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
    }
}
