using CrmWeb.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace CrmWeb.Pages.Clients
{
    public class ExtraItemsModel : PageModel
    {
        DbAddress Db = new DbAddress();

        public List<NewExtraItemModel> ExtraItems { get; set; }

        public void OnGet()
        {
            ExtraItems = new List<NewExtraItemModel>();

            var partnerId = Request.Cookies["PartnerId"];

            using (SqlConnection connection = new SqlConnection(Db.DB()))
            {
                connection.Open();
                String sql = "SELECT * FROM Items WHERE PartnerId = @partnerId";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@partnerId", partnerId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            NewExtraItemModel ExtraItem = new NewExtraItemModel();
                            ExtraItem.Id = reader.GetInt32(0);
                            ExtraItem.Item = reader.GetString(1);
                            ExtraItem.PriceS = reader.GetString(2);
                            ExtraItem.PriceM = reader.GetString(3);
                            ExtraItem.PriceL = reader.GetString(4);
                            ExtraItem.PriceXL = reader.GetString(5);
                            ExtraItem.PriceXXL = reader.GetString(6);

                            ExtraItems.Add(ExtraItem);
                        }
                    }
                }
            }
        }
    }
}
