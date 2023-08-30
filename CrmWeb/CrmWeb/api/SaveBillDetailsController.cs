using com.sun.tools.@internal.ws.wsdl.document.jaxws;
using CrmWeb.Data;
using CrmWeb.Pages.Clients;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace CrmWeb.api
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SaveBillDetailsController : Controller
    {
        DbAddress Db = new DbAddress();
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public bool SaveDetails([FromBody] BillDetails details)
        {
            var partnerId = Request.Cookies["PartnerId"];
            using (SqlConnection connection = new SqlConnection(Db.DB()))
            {
                connection.Open();

                string saveDetail = "INSERT INTO OrderDetails (OrderId, Product, UnitPrice, ExtraItems, Date, PartnerId) VALUES (@OrderId, @Product, @UnitPrice, @ExtraItems, @Date, @partnerId);";
                using (SqlCommand command = new SqlCommand(saveDetail, connection))
                {
                    command.Parameters.AddWithValue("@partnerId", partnerId);
                    command.Parameters.AddWithValue("@OrderId", details.OrderId);
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