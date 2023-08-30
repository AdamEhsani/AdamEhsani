using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using CrmWeb.Data;

namespace CrmWeb.api
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserLocationController : ControllerBase
    {
        DbAddress Db = new DbAddress();
        public void GetUserLocationAsync()
        {
            string address = string.Empty;

            using (SqlConnection connection = new SqlConnection(Db.DB()))
            {
                connection.Open();
                String sql = "SELECT * FROM Setting";

                using (SqlCommand Command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            address = $"{Reader.GetString(3)} + {Reader.GetString(4)}";
                        }
                    }
                }
            }

            var client = new RestClient("https://nominatim.openstreetmap.org");
            var request = new RestRequest($"/search?q={address}&format=json&countrycodes=de-DE", Method.Get);
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                NominatimService _nominatimService = new NominatimService();
                var jsonArray = JArray.Parse(response.Content);

                _nominatimService.latitude = jsonArray[0]["lat"];
                _nominatimService.longitude = jsonArray[0]["lon"];
            }
        }
    }
}
