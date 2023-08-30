using CrmWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace CrmWeb.Pages.Clients
{
    public class EditCategoryModel : PageModel
    {
        DbAddress Db = new DbAddress();

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public string Category { get; set; }

        [BindProperty]
        public string Type { get; set; }

        public void OnGet()
        {

            using (SqlConnection connection = new SqlConnection(Db.DB()))
            {
                connection.Open();

                String sql = "SELECT * FROM Categories WHERE PartnerId = @PartnerId AND Id = @Id;";
                var partnerId = Request.Cookies["PartnerId"];

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);
                    command.Parameters.AddWithValue("@PartnerId", partnerId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Category = reader["Category"].ToString().Trim();
                            Type = reader["Type"].ToString().Trim();
                        }
                    }
                }
            }
        }

        public IActionResult OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            using (SqlConnection connection = new SqlConnection(Db.DB()))
            {
                connection.Open();

                String sql = "UPDATE Categories SET Category = @Category, Type = @Type WHERE PartnerId = PartnerId AND Id = @Id;";
                var partnerId = Request.Cookies["PartnerId"];

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);
                    command.Parameters.AddWithValue("@PartnerId", partnerId);
                    command.Parameters.AddWithValue("@Category", Category);
                    command.Parameters.AddWithValue("@Type", Type);

                    command.ExecuteNonQuery();
                }
            }
            return RedirectToPage("/Clients/ProductCategories");
        }
    }
}
