using CrmWeb.Data;
using CrmWeb.Pages.Clients;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace CrmWeb.api
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ChangePass : Controller
    {
        DbAddress Db = new DbAddress();
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public bool SaveChanges([FromBody] ChangePassInfo info)
        {
            var partnerId = Request.Cookies["PartnerId"];
            using (SqlConnection connection = new SqlConnection(Db.DB()))
            {
                connection.Open();

                string sqlUsers = "SELECT * FROM Users WHERE Id = @partnerId";
                using (SqlCommand command = new SqlCommand(sqlUsers, connection))
                {
                    command.Parameters.AddWithValue("@partnerId", partnerId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read() && info.UserName == reader.GetString(1) && info.LastPass == reader.GetString(2))
                        {
                            UpdateDB(partnerId,info.NewPass);
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
        }

        private void UpdateDB(string partner, string pass)
        {
            using (SqlConnection connection = new SqlConnection(Db.DB()))
            {
                connection.Open();
                string sqlPass = "UPDATE Users SET Password = @Password WHERE Id = @partnerId";
                using (SqlCommand updateCommand = new SqlCommand(sqlPass, connection))
                {
                    updateCommand.Parameters.AddWithValue("@partnerId", partner);
                    updateCommand.Parameters.AddWithValue("@Password", pass);
                    updateCommand.ExecuteNonQuery();
                }
            }
        }

        public class ChangePassInfo
        {
            public string UserName { get; set; }
            public string LastPass { get; set; }
            public string NewPass { get; set; }
        }
    }
}
