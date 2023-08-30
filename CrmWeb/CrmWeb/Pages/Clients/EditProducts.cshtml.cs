using com.sun.xml.@internal.bind.v2.model.core;
using CrmWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace CrmWeb.Pages.Clients
{
    public class EditProductsModel : PageModel
    {
        public ProductInfo Products = new ProductInfo();
        public List<Categories> categories = new List<Categories>();
        public String errorMessage = string.Empty;
        public String successMessage = string.Empty;
        DbAddress Db = new DbAddress();

        [BindProperty(SupportsGet = true)]
        public int ProductId { get; set; }

        [BindProperty]
        public string? ProduktInputName { get; set; }

        [BindProperty]
        public string ProduktInputPreisS { get; set; } = "0";

        [BindProperty]
        public string ProduktInputPreisM { get; set; } = "0";

        [BindProperty]
        public string ProduktInputPreisL { get; set; } = "0";

        [BindProperty]
        public string ProduktInputPreisXL { get; set; } = "0";

        [BindProperty]
        public string ProduktInputPreisXXL { get; set; } = "0";

        [BindProperty]
        public string? Categoryvalue { get; set; }

        [BindProperty]
        public string? CategoryType { get; set; }

        public void OnGet()
        {
            String id = Request.Query["Id"];
            var partnerId = Request.Cookies["PartnerId"];

            try
            {
                using (SqlConnection connection = new SqlConnection(Db.DB()))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Product WHERE PartnerId = @partnerId AND id=@Id";

                    using (SqlCommand Command = new SqlCommand(sql, connection))
                    {
                        Command.Parameters.AddWithValue("Id", id);
                        Command.Parameters.AddWithValue("partnerId", partnerId);
                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            if (Reader.Read())
                            {
                                ProductId = Reader.GetInt32(0);
                                ProduktInputName = Reader.GetString(1);
                                ProduktInputPreisS = Reader.GetString(2);
                                ProduktInputPreisM = Reader.GetString(3);
                                ProduktInputPreisL = Reader.GetString(4);
                                ProduktInputPreisXL = Reader.GetString(5);
                                ProduktInputPreisXXL = Reader.GetString(6);
                                Categoryvalue = GetCategory(Reader.GetInt32(8));
                            }
                        }
                    }
                }

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
                                category.PartnerId = $"{Reader.GetInt32(3)}";
                                categories.Add(category);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
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
                            CategoryType = reader.GetString(2);
                            return reader.GetString(1);
                        }
                    }
                }
            }
            return "";
        }

        public IActionResult OnPostAsync()
        {
            if (string.IsNullOrEmpty(ProduktInputName))
            {
                errorMessage = "All fields are required";
                RedirectToPage("/Clients/Products");
            }

            try
            {
                var partnerId = Request.Cookies["PartnerId"];
                using (SqlConnection connection = new SqlConnection(Db.DB()))
                {
                    connection.Open();

                    String sql = "UPDATE Product " +
                                 "SET Name = @ProduktInputName, PriceS = @ProduktInputPreisS, PriceM = @ProduktInputPreisM, PriceL = @ProduktInputPreisL, PriceXL = @ProduktInputPreisXL, PriceXXL = @ProduktInputPreisXXL, ProductsTypeId = @ProductsTypeId " +
                                 "WHERE id = @Id AND PartnerId = @partnerId";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@PartnerId", partnerId);

                        checkCategoryAndPrice(Categoryvalue);
                        command.Parameters.AddWithValue("@Id", ProductId);
                        command.Parameters.AddWithValue("@ProduktInputName", ProduktInputName);
                        command.Parameters.AddWithValue("@ProduktInputPreisS", ProduktInputPreisS);
                        command.Parameters.AddWithValue("@ProduktInputPreisM", ProduktInputPreisM);
                        command.Parameters.AddWithValue("@ProduktInputPreisL", ProduktInputPreisL);
                        command.Parameters.AddWithValue("@ProduktInputPreisXL", ProduktInputPreisXL);
                        command.Parameters.AddWithValue("@ProduktInputPreisXXL", ProduktInputPreisXXL);
                        command.Parameters.AddWithValue("@ProductsTypeId", Categoryvalue);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
           return RedirectToPage("/Clients/Products");
        }

        private void checkCategoryAndPrice(string category)
        {
            string _ = GetCategory(Int32.Parse(category));

            switch (CategoryType)
            {
                case "1":
                    ProduktInputPreisM = "0";
                    ProduktInputPreisL = "0";
                    ProduktInputPreisXL = "0";
                    ProduktInputPreisXXL = "0";
                    break;
                case "2":
                    ProduktInputPreisL = "0";
                    ProduktInputPreisXL = "0";
                    ProduktInputPreisXXL = "0";
                    break;
                case "3":
                    ProduktInputPreisXL = "0";
                    ProduktInputPreisXXL = "0";
                    break;
                case "4":
                    ProduktInputPreisXXL = "0";
                    break;
            }
        }
    }
}
