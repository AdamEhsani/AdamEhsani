using CrmWeb.Data;
using CrmWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace CrmWeb.api
{
    [ApiController]
    [Route("api/[controller]/[action]")]

    public class NewProductController : Controller
    {
        DbAddress Db = new DbAddress();
        public IActionResult Index()
        {
            return View();
        }

        public bool CreateNewProduct([FromBody] Product product)
        {
            var partnerId = Request.Cookies["PartnerId"];
            try
            {

                using (SqlConnection connection = new SqlConnection(Db.DB()))
                {
                    connection.Open();

                    String sql = "INSERT INTO Product" +
                        "(Name, PriceS, PriceM, PriceL, PriceXL, PriceXXL, ProductsTypeId, PartnerId) VALUES" +
                        "(@Name, @PriceS, @PriceM, @PriceL, @PriceXL, @PriceXXL, @ProductsTypeId, @PartnerId);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Name", product.Name);
                        command.Parameters.AddWithValue("@PriceS", product.PriceS);
                        command.Parameters.AddWithValue("@PriceM", product.PriceM);
                        command.Parameters.AddWithValue("@PriceL", product.PriceL);
                        command.Parameters.AddWithValue("@PriceXL", product.PriceXL);
                        command.Parameters.AddWithValue("@PriceXXL", product.PriceXXL);
                        command.Parameters.AddWithValue("@ProductsTypeId", product.Category);
                        command.Parameters.AddWithValue("@partnerId", partnerId);

                        command.ExecuteReader();
                    }
                }
                return true;
            }catch (Exception ex)
            {
                return false;
            }
        }
    }
}
