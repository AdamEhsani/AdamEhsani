using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using CrmWeb.Data;

namespace CrmWeb.Pages.Clients
{
    public class NewProductModel : PageModel
    {
        DbAddress Db = new DbAddress();
        public ProductInfo Products = new ProductInfo();
        public List<Categories> categories = new List<Categories>();
        public String errorMessage = string.Empty;
        public String successMessage = string.Empty;

        public void OnGet()
        {
            GetCategory();
        }

        private void GetCategory()
        {
            var partnerId = Request.Cookies["PartnerId"];

            using (SqlConnection connection = new SqlConnection(Db.DB()))
            {
                connection.Open();
                String sql = "SELECT * FROM Categories WHERE PartnerId = @partnerId";

                using (SqlCommand Command = new SqlCommand(sql, connection))
                {
                    Command.Parameters.AddWithValue("@partnerId", partnerId);
                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            Categories category = new Categories();
                            category.Id = Reader.GetInt32(0);
                            category.Category = Reader.GetString(1);
                            category.Type = Reader.GetString(2);

                            categories.Add(category);
                        }
                    }
                }
            }
        }

        public void OnPost()
        {
            Products.PartnerId = Request.Cookies["PartnerId"];

            Products.Name = Request.Form["Name"];
            if (string.IsNullOrWhiteSpace(Products.Name))
            {
                errorMessage = "All the fields are required";
                return;
            }

            if (!string.IsNullOrWhiteSpace(Request.Form["PriceS"]))
            {
                Products.PriceS = Request.Form["PriceS"];
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["PriceM"]))
            {
                Products.PriceM = Request.Form["PriceM"];
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["PriceL"]))
            {
                Products.PriceL = Request.Form["PriceL"];
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["PriceXL"]))
            {
                Products.PriceXL = Request.Form["PriceXL"];
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["PriceXXL"]))
            {
                Products.PriceXXL = Request.Form["PriceXXL"];
            }

            Products.Category = Request.Form["Category"];

            try
            {
                using (SqlConnection connection = new SqlConnection(Db.DB()))
                {
                    connection.Open();

                    String sql = "INSERT INTO Product" +
                        "(Name, PriceS, PriceM, PriceL, PriceXL, PriceXXL, ProductsTypeId, PartnerId) VALUES" +
                        "(@Name, @PriceS, @PriceM, @PriceL, @PriceXL, @PriceXXL, @ProductsTypeId, @PartnerId);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Name", Products.Name);
                        command.Parameters.AddWithValue("@PriceS", Products.PriceS);
                        command.Parameters.AddWithValue("@PriceM", Products.PriceM);
                        command.Parameters.AddWithValue("@PriceL", Products.PriceL);
                        command.Parameters.AddWithValue("@PriceXL", Products.PriceXL);
                        command.Parameters.AddWithValue("@PriceXXL", Products.PriceXXL);
                        command.Parameters.AddWithValue("@ProductsTypeId", Products.Category);
                        command.Parameters.AddWithValue("@partnerId", Products.PartnerId);

                        command.ExecuteReader();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Products.Name = string.Empty;
            Products.PriceS = string.Empty;
            Products.PriceL = string.Empty;
            Products.PriceM = string.Empty;
            Products.PriceXL = string.Empty;
            Products.PriceXXL = string.Empty;
            Products.Category = string.Empty;
            successMessage = "New Product added Correctly";
            GetCategory();
        }
    }

    public class ProductInfo
    {
        public string PartnerId { get; set; }
        public int Id { get; set; }
        public string? Name { get; set; }
        public string PriceS { get; set; } = "0";
        public string PriceM { get; set; } = "0";
        public string PriceL { get; set; } = "0";
        public string PriceXL { get; set; } = "0";
        public string PriceXXL { get; set; } = "0";
        public string? Category { get; set; }
    }
}