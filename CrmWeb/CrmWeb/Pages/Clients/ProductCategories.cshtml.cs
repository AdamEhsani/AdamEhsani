using CrmWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Drawing;

namespace CrmWeb.Pages.Clients
{
    public class ProductCategoriesModel : PageModel
    {
        public String errorMessage = string.Empty;
        public List<Categories> Categories = new List<Categories>();
        DbAddress Db = new DbAddress();

        [BindProperty]
        public string Category { get; set; }

        [BindProperty]
        public string Type { get; set; }

        public void OnPost()
        {
            var partnerId = Request.Cookies["PartnerId"];

            if (!string.IsNullOrEmpty(Category) && !string.IsNullOrEmpty(Type))
            {
                using (SqlConnection connection = new SqlConnection(Db.DB()))
                {
                    connection.Open();

                    String sql = "INSERT INTO Categories" +
                        "(Category, PartnerId, Type) VALUES" +
                        "(@Category, @PartnerId, @Type);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@partnerId", partnerId);

                        command.Parameters.AddWithValue("@Type", Type);
                        command.Parameters.AddWithValue("@Category", Category);

                        command.ExecuteReader();
                    }
                }
            }
            else
            {
                errorMessage = "All fields are required";
            }
            OnGet();
            Category = string.Empty;
            Type = string.Empty;
        }

        public void OnGet()
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

                            Categories.Add(category);
                        }
                    }
                }
            }
        }
    }
}

