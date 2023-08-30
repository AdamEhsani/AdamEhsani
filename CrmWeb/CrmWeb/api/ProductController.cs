using CrmWeb.Data;
using CrmWeb.Pages.Clients;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace CrmWeb.api
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductController : Controller
    {
        DbAddress Db = new DbAddress();
        public int selectedId { get; set; }

        private int GetCategory(int id)
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
                            return Int32.Parse(reader.GetString(2));
                        }
                    }
                }
            }
            return 1;
        }

        [HttpPost]
        public object GetProductSizes([FromBody] SelectedProduct productId)
        {
            selectedId = productId.ProductId;
            string[] productSizes = new string[5];

            using (SqlConnection connection = new SqlConnection(Db.DB()))
            {
                connection.Open();
                string sql = "SELECT * FROM Product WHERE Id = @productId;";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@productId", selectedId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int category = GetCategory(reader.GetInt32(8));
                            switch (category)
                            {
                                case 1:
                                    productSizes[0] = "Small";
                                    break;
                                case 2:
                                    productSizes[0] = "Small";
                                    productSizes[1] = "Medium";
                                    break;
                                case 3:
                                    productSizes[0] = "Small";
                                    productSizes[1] = "Medium";
                                    productSizes[2] = "Large";
                                    break;
                                case 4:
                                    productSizes[0] = "Small";
                                    productSizes[1] = "Medium";
                                    productSizes[2] = "Large";
                                    productSizes[3] = "Family";
                                    break;
                                case 5:
                                    productSizes[0] = "Small";
                                    productSizes[1] = "Medium";
                                    productSizes[2] = "Large";
                                    productSizes[3] = "Family";
                                    productSizes[4] = "Party";
                                    break;
                                default:
                                    productSizes[0] = "Normal";
                                    break;
                            }

                        }
                    }
                }
            }

            return Json(productSizes);
        }
    }
}
