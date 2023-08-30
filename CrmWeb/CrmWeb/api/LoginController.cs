using CrmWeb.Data;
using CrmWeb.Pages.Clients;
using java.net;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace CrmWeb.api
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LoginController : Controller
    {
        LoginInfo _loginInfo = new LoginInfo();
        DbAddress Db = new DbAddress();
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public object GetCheckUserPassword([FromBody] LoginInfo login)
        {
            string[] UserPassword = new string[2];
            using (SqlConnection connection = new SqlConnection(Db.DB()))
            {
                connection.Open();
                String sql = "SELECT * FROM Users";

                using (SqlCommand Command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {
                        if (Reader.HasRows)
                        {
                            while (Reader.Read())
                            {
                                if (!Reader.IsDBNull(1))
                                {
                                    if (Reader.GetString(1) == login.UserName && Reader.GetString(2) == login.Password)
                                    {
                                        UserPassword[0] = Reader.GetString(1);
                                        UserPassword[1] = Reader.GetString(2);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                SetPartnerId(UserPassword[0], UserPassword[1]);
            }
            return UserPassword;
        }

        private void TestContext(string userName, string password)
        {

        }

        private void SetPartnerId(string userName, string password)
        {
            using (SqlConnection connection = new SqlConnection(Db.DB()))
            {
                connection.Open();
                String sql = "SELECT * FROM Users";

                using (SqlCommand Command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            if (Reader.GetString(1) == userName && Reader.GetString(2) == password)
                            {
                                _loginInfo.PartnerId = Reader.GetInt32(0);
                                HttpCookie cookie = new HttpCookie("PartnerId", Reader.GetInt32(0).ToString());
                                Response.Cookies.Append("PartnerId", Reader.GetInt32(0).ToString());
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
