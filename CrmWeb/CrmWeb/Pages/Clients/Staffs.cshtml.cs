using CrmWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace CrmWeb.Pages.Clients
{
    public class StaffsModel : PageModel
    {
        DbAddress Db = new DbAddress();
        public List<NewStaffModel> Staffs { get; set; }

        public void OnGet()
        {
            Staffs = new List<NewStaffModel>();
            var partnerId = Request.Cookies["PartnerId"];

            using (SqlConnection connection = new SqlConnection(Db.DB()))
            {
                connection.Open();
                String sql = "SELECT * FROM Staff WHERE PartnerId = @partnerId";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@partnerId", partnerId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            NewStaffModel staff = new NewStaffModel();
                            staff.Id = reader.GetInt32(0);
                            staff.Name = GetStringFromReader(reader, 1);
                            staff.Department = GetStringFromReader(reader, 2);
                            staff.Phone = GetStringFromReader(reader ,3);

                            Staffs.Add(staff);
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
    }

}
