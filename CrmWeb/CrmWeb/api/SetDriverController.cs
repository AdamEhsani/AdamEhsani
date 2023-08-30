using CrmWeb.Data;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace CrmWeb.api
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SetDriverController : Controller
    {
        DbAddress Db = new DbAddress();
        string SelectedDriver = string.Empty;
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public void AddDriver([FromBody] Driver driver)
        {
            using (SqlConnection connection = new SqlConnection(Db.DB()))
            {
                connection.Open();
                String sqlDriver = "SELECT * FROM Staff WHERE Id = @DriverId";

                using (SqlCommand commandDriver = new SqlCommand(sqlDriver, connection))
                {
                    commandDriver.Parameters.AddWithValue("@DriverId", driver.DriverId);

                    SqlDataReader reader = commandDriver.ExecuteReader();

                    if (reader.Read())
                    {
                        SelectedDriver = reader.GetString(1);
                        reader.Close();
                    }
                }

                String sqlOrder = "UPDATE Orders " +
                                 "SET Driver = @DriverName " +
                                 "WHERE id = @OrderId";

                using (SqlCommand command = new SqlCommand(sqlOrder, connection))
                {
                    command.Parameters.AddWithValue("@OrderId", driver.OrderId);
                    command.Parameters.AddWithValue("@DriverName", SelectedDriver);

                    command.ExecuteReader();
                }
            }
        }
    }

    public class Driver
    {
        public int OrderId { get; set; }
        public int DriverId { get; set; }
    }
}
