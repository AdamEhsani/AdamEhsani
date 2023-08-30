using com.sun.tools.@internal.ws.wsdl.document.jaxws;
using CrmWeb.Data;
using CrmWeb.Pages.Clients;
using javax.xml.soap;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace CrmWeb.api
{
    [ApiController]
    [Route("api/[controller]/[action]")]

    public class EditOrderController : Controller
    {
        DbAddress Db = new DbAddress();

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public bool EditSavedOrder([FromBody] EditOrder order)
        {
            var partnerId = Request.Cookies["PartnerId"];
            int customerId = 0;
            using (SqlConnection connection = new SqlConnection(Db.DB()))
            {
                connection.Open();

                string selectSql = "SELECT * FROM Customer WHERE PartnerId = @partnerId AND Phone = @Phone;";

                using (SqlCommand selectCommand = new SqlCommand(selectSql, connection))
                {
                    selectCommand.Parameters.AddWithValue("@Phone", order.Phone);
                    selectCommand.Parameters.AddWithValue("@partnerId", partnerId);
                    SqlDataReader reader = selectCommand.ExecuteReader();

                    if (reader.Read())
                    {
                        customerId = reader.GetInt32(0);
                        reader.Close();
                    }

                    String sql = "UPDATE Orders " +
                             "SET CustomerName = @Name, CustomerAddress = @Address, OrderDate = @Date, TotalPrice = @Price, CustomerId = @CustomerId " +
                             "WHERE Id = @Id AND PartnerId = @PartnerId";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Id", order.Id);
                        command.Parameters.AddWithValue("@partnerId", partnerId);
                        command.Parameters.AddWithValue("@Name", order.Name);
                        command.Parameters.AddWithValue("@Address", order.Address);
                        command.Parameters.AddWithValue("@Date", DateTime.Now);
                        command.Parameters.AddWithValue("@Price", order.TotalPrice);
                        command.Parameters.AddWithValue("@CustomerId", customerId);

                        command.ExecuteNonQuery();
                    }
                }
                return true;
            }
        }
    }
}
