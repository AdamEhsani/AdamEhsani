using CrmWeb.Data;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace CrmWeb.api
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class GetCategory : Controller
    {
        DbAddress Db = new DbAddress();
        public class CategoryInfo
        {
            public string CategoryId { get; set; }
        }

        public string Category([FromBody] CategoryInfo category)
        {
            using (SqlConnection connection = new SqlConnection(Db.DB()))
            {
                connection.Open();

                String sql = "SELECT * FROM Categories WHERE PartnerId = @PartnerId AND Id = @Id;";
                var partnerId = Request.Cookies["PartnerId"];

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", category.CategoryId);
                    command.Parameters.AddWithValue("@PartnerId", partnerId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader.GetString(2);
                        }
                    }
                }
            }
            return "";
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
