using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using CrmWeb.Data;

namespace CrmWeb.Pages.Clients
{
    public class SettingModel : PageModel
    {
        DbAddress Db = new DbAddress();

        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string Phone { get; set; }

        [BindProperty]
        public string Address { get; set; }
        [BindProperty]
        public string City { get; set; }
        [BindProperty]
        public string PLZ { get; set; }

        [BindProperty]
        public string UstIdNr { get; set; }

        [BindProperty]
        public string DeliveryPrice { get; set; }

        [BindProperty]
        public string UserName { get; set; }

        [BindProperty]
        public string LastPassword { get; set; }      
        
        [BindProperty]
        public string NewPassword { get; set; }
        public string PartnerId { get; set; }

        public String errorMessage = string.Empty;
        public String successMessage = string.Empty;

        public void OnGet()
        {
            LoadSetting();
        }

        public void OnPost()
        {
            SaveSetting();
        }

        private void LoadSetting()
        {
            var partnerId = Request.Cookies["PartnerId"];

            using (SqlConnection connection = new SqlConnection(Db.DB()))
            {
                connection.Open();
                String sqlSetting = "SELECT * FROM Setting WHERE PartnerId = @partnerId";

                using (SqlCommand Command = new SqlCommand(sqlSetting, connection))
                {
                    Command.Parameters.AddWithValue("@partnerId", partnerId);

                    using (SqlDataReader reader = Command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Name = GetStringFromReader(reader, 1);
                            Address = GetStringFromReader(reader, 2);
                            City = GetStringFromReader(reader, 3);
                            PLZ = GetStringFromReader(reader, 4);
                            Phone = GetStringFromReader(reader, 5);
                            UstIdNr = GetStringFromReader(reader, 6);
                            DeliveryPrice = GetStringFromReader(reader, 7);
                        }
                    }
                }
            }
        }

        private string GetStringFromReader(SqlDataReader reader, int columnIndex)
        {
            if (!reader.IsDBNull(columnIndex) && !string.IsNullOrEmpty(reader.GetString(columnIndex)))
            {
                return reader.GetString(columnIndex);
            }
            return null;
        }

        private void SaveSetting()
        {
            var partnerId = Request.Cookies["PartnerId"];
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Address) || string.IsNullOrEmpty(Phone))
            {
                errorMessage = "All the fields are required";
                return;
            }
            using (SqlConnection connection = new SqlConnection(Db.DB()))
            {
                connection.Open();

                string sqlSetting = "UPDATE Setting " +
                             "SET Name = @Name, Address = @Address, City = @City, PLZ = @PLZ, Phone = @Phone, UstIdNr = @UstIdNr, DeliveryPrice = @DeliveryPrice " +
                             $"WHERE PartnerId = @partnerId";

                using (SqlCommand command = new SqlCommand(sqlSetting, connection))
                {
                    command.Parameters.AddWithValue("@partnerId", partnerId);

                    command.Parameters.AddWithValue("@Name", (object)Name ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Address", (object)Address ?? DBNull.Value);
                    command.Parameters.AddWithValue("@City", (object)City ?? DBNull.Value);
                    command.Parameters.AddWithValue("@PLZ", (object)PLZ ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Phone", (object)Phone ?? DBNull.Value);
                    command.Parameters.AddWithValue("@UstIdNr", (object)UstIdNr ?? DBNull.Value);
                    command.Parameters.AddWithValue("@DeliveryPrice", (object)DeliveryPrice ?? DBNull.Value);

                    command.ExecuteNonQuery();
                }

                String sqlUsers = "UPDATE Users " +
                            "SET Name = @Name, Address = @Address, City = @City, PLZ = @PLZ, Phone = @Phone " +
                            $"WHERE Id = @partnerId";

                using (SqlCommand command = new SqlCommand(sqlUsers, connection))
                {
                    command.Parameters.AddWithValue("@partnerId", partnerId);

                    command.Parameters.AddWithValue("@Address", (object)Address ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Name", (object)Name ?? DBNull.Value);
                    command.Parameters.AddWithValue("@City", (object)City ?? DBNull.Value);
                    command.Parameters.AddWithValue("@PLZ", (object)PLZ ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Phone", (object)Phone ?? DBNull.Value);

                    command.ExecuteNonQuery();
                }
            }
            successMessage = "Saved changed ";
        }
    }
}
