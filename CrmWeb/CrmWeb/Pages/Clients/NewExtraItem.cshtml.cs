using CrmWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace CrmWeb.Pages.Clients
{
    public class NewExtraItemModel : PageModel
    {
        DbAddress Db = new DbAddress();

        public string PartnerId { get; set; }

        [BindProperty]
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

        public String successMessage = string.Empty;

        public void OnPost()
        {
            var partnerId = Request.Cookies["PartnerId"];

            using (SqlConnection connection = new SqlConnection(Db.DB()))
            {
                connection.Open();

                String sql = "INSERT INTO Items" +
                    "(Name, PriceS, PriceM, PriceL, PriceXL, PriceXXL, PartnerId) VALUES" +
                    "(@Item, @PriceS, @PriceM, @PriceL, @PriceXL, @PriceXXL, @partnerId);";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@partnerId", partnerId);

                    command.Parameters.AddWithValue("@Item", Item);
                    command.Parameters.AddWithValue("@PriceS", PriceS);
                    command.Parameters.AddWithValue("@PriceM", PriceM);
                    command.Parameters.AddWithValue("@PriceL", PriceL);
                    command.Parameters.AddWithValue("@PriceXL", PriceXL);
                    command.Parameters.AddWithValue("@PriceXXL", PriceXXL);

                    command.ExecuteNonQuery();
                }
            }
            successMessage = "New Extra item added Correctly";
            Item = String.Empty;
            PriceS = String.Empty;
            PriceM = String.Empty;
            PriceL = String.Empty;
            PriceXL = String.Empty;
            PriceXXL = String.Empty;
        }
    }
}
