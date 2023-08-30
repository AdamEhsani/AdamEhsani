using CrmWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace CrmWeb.Pages.Clients
{
    public class EditExtraItemsModel : PageModel
    {
        DbAddress Db = new DbAddress();

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public string Item { get; set; }

        [BindProperty]
        public string PriceS { get; set; }

        [BindProperty]
        public string PriceM { get; set; }
        [BindProperty]
        public string PriceL { get; set; }
        [BindProperty]
        public string PriceXL { get; set; }
        [BindProperty]
        public string PriceXXL { get; set; }

        public void OnGet()
        {
            var partnerId = Request.Cookies["PartnerId"];

            using (SqlConnection connection = new SqlConnection(Db.DB()))
            {
                connection.Open();

                String sql = "SELECT * FROM Items WHERE PartnerId = @partnerId AND Id = @Id;";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);
                    command.Parameters.AddWithValue("@partnerId", partnerId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Item = reader.GetString(1);
                            PriceS = reader.GetString(2);
                            PriceM = reader.GetString(3);
                            PriceL = reader.GetString(4);
                            PriceXL = reader.GetString(5);
                            PriceXXL = reader.GetString(6);
                        }
                    }
                }
            }
        }

        public IActionResult OnPostAsync()
        {
            using (SqlConnection connection = new SqlConnection(Db.DB()))
            {
                connection.Open();

                String sql = "UPDATE Items SET Name = @Item, PriceS = @PriceS, PriceM = @PriceM, PriceL = @PriceL, PriceXL = @PriceXL, PriceXXL = @PriceXXL WHERE Id = @Id;";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);
                    command.Parameters.AddWithValue("@Item", Item);
                    command.Parameters.AddWithValue("@PriceS", PriceS);
                    command.Parameters.AddWithValue("@PriceM", PriceM);
                    command.Parameters.AddWithValue("@PriceL", PriceL);
                    command.Parameters.AddWithValue("@PriceXL", PriceXL);
                    command.Parameters.AddWithValue("@PriceXXL", PriceXXL);

                    command.ExecuteNonQuery();
                }
            }
            return RedirectToPage("/Clients/ExtraItems");
        }
    }
}
