using CrmWeb.Pages.Clients;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using CrmWeb.Data;

namespace CrmWeb.api
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EditBillDetailsController : Controller
    {
        DbAddress Db = new DbAddress();
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public bool EditSavedBillDetails([FromBody] EditBillDetail details)
        {
            var partnerId = Request.Cookies["PartnerId"];

            using (SqlConnection connection = new SqlConnection(Db.DB()))
            {
                connection.Open();
                string saveDetail = "INSERT INTO OrderDetails (OrderId, Product, ExtraItems, UnitPrice, Date, PartnerId) VALUES (@OrderId, @Product, @ExtraItems, @UnitPrice, @Date, @partnerId);";
                using (SqlCommand command = new SqlCommand(saveDetail, connection))
                {
                    command.Parameters.AddWithValue("@partnerId", partnerId);
                    command.Parameters.AddWithValue("@OrderId", details.Id);
                    command.Parameters.AddWithValue("@Product", details.Product);
                    command.Parameters.AddWithValue("@ExtraItems", details.ExtraItems);
                    command.Parameters.AddWithValue("@UnitPrice", details.Price);
                    command.Parameters.AddWithValue("@Date", DateTime.Now);

                    command.ExecuteNonQuery();
                }
            }
            return true;
        }
    }
}
