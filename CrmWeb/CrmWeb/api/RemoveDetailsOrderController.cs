using CrmWeb.Data;
using CrmWeb.Pages.Clients;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace CrmWeb.api
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RemoveDetailsOrderController : Controller
    {
        DbAddress Db = new DbAddress();
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public bool RemoveDetails([FromBody] OrdersID order)
        {
            if (Request.Cookies.TryGetValue("PartnerId", out var partnerId))
            {
                using (SqlConnection connection = new SqlConnection(Db.DB()))
                {
                    connection.Open();

                    string sql = "DELETE FROM OrderDetails WHERE PartnerId = @PartnerId AND OrderId = @OrderId";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@PartnerId", partnerId);
                        command.Parameters.AddWithValue("@OrderId", order.Id);

                        command.ExecuteNonQuery();
                    }
                }
                return true;
            }
            return false;
        }
    }
}
