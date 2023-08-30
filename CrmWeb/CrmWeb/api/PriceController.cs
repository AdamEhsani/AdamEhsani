using CrmWeb.Data;
using CrmWeb.Pages.Clients;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace CrmWeb.api
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PriceController : Controller
    {
        DbAddress Db = new DbAddress();
        private const string _sizeSmal = "Small";
        private const string _sizeMedium = "Medium";
        private const string _sizeLarge = "Large";
        private const string _sizeFamily = "Family";
        private const string _sizeParty = "Party";
        private const string _sizeNormal = "Normal";


        public object GetExtraItems([FromBody] SelectedSizeProduct productSize)
        {
            var partnerId = Request.Cookies["PartnerId"];
            List<AddExtraItems> ItemsList = new List<AddExtraItems>();

            var result = new List<string>();
            using (SqlConnection connection = new SqlConnection(Db.DB()))
            {
                connection.Open();
                string sql = "SELECT * FROM Items WHERE PartnerId = @PartnerId;";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@PartnerId", partnerId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        string size = productSize.Size;

                        while (reader.Read())
                        {
                            AddExtraItems ExtraItem = new AddExtraItems();
                            if (size == _sizeNormal)
                            {
                                ExtraItem.Name = reader.GetString(1);
                                ExtraItem.Price = reader.GetString(2);
                                ItemsList.Add(ExtraItem);
                            }
                            else if (size == _sizeSmal)
                            {
                                ExtraItem.Name = reader.GetString(1);
                                ExtraItem.Price = reader.GetString(2);
                                ItemsList.Add(ExtraItem);
                            }
                            else if (size == _sizeMedium)
                            {
                                ExtraItem.Name = reader.GetString(1);
                                ExtraItem.Price = reader.GetString(3);
                                ItemsList.Add(ExtraItem);
                            }
                            else if (size == _sizeLarge)
                            {
                                ExtraItem.Name = reader.GetString(1);
                                ExtraItem.Price = reader.GetString(4);
                                ItemsList.Add(ExtraItem);
                            }
                            else if (size == _sizeFamily)
                            {
                                ExtraItem.Name = reader.GetString(1);
                                ExtraItem.Price = reader.GetString(5);
                                ItemsList.Add(ExtraItem);
                            }
                            else if (size == _sizeParty)
                            {
                                ExtraItem.Name = reader.GetString(1);
                                ExtraItem.Price = reader.GetString(6);
                                ItemsList.Add(ExtraItem);
                            }
                        }

                    }
                }
            }
            return ItemsList;
        }



        [HttpPost]
        public string GetProductPrices([FromBody] SelectedSizeProduct productSize)
        {
            var partnerId = Request.Cookies["PartnerId"];
            using (SqlConnection connection = new SqlConnection(Db.DB()))
            {
                connection.Open();
                string sql = "SELECT * FROM Product WHERE Id = @productId AND PartnerId = @PartnerId;";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@productId", productSize.Id);
                    command.Parameters.AddWithValue("@PartnerId", partnerId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string size = productSize.Size;


                            switch (size)
                            {
                                case _sizeNormal:
                                case _sizeSmal:
                                    return reader.GetString(2);

                                case _sizeMedium:
                                   return reader.GetString(3);
                                  
                                case _sizeLarge:
                                    return reader.GetString(4);

                                case _sizeFamily:
                                    return reader.GetString(5);

                                case _sizeParty:
                                   return reader.GetString(6);
                              
                            }
                        }
                    }
                }
            }

            return "";
        }
    }
}