using CrmWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Text;

namespace CrmWeb.Pages.Clients
{
    public class EditStaffsModel : PageModel
    {
        DbAddress Db = new DbAddress();

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public string FirstName { get; set; }

        [BindProperty]
        public string Department { get; set; }    
        
        [BindProperty]
        public string? Phone { get; set; }

        public void OnGet()
        {
            var deCodeId = Convert.FromBase64String(Request.Query["id"].ToString());
            Id = int.Parse(Encoding.UTF8.GetString(deCodeId));

            using (SqlConnection connection = new SqlConnection(Db.DB()))
            {
                connection.Open();

                String sql = "SELECT * FROM Staff WHERE Id = @Id;";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            FirstName = reader["FirstName"].ToString().Trim();
                            Department = reader["Department"].ToString().Trim();
                            Phone = reader["Phone"].ToString().Trim();
                        }
                    }
                }
            }
        }

        public IActionResult OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Update user in database

            using (SqlConnection connection = new SqlConnection(Db.DB()))
            {
                connection.Open();

                String sql = "UPDATE Staff SET FirstName = @FirstName, Department = @Department, Phone = @Phone WHERE Id = @Id;";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);
                    command.Parameters.AddWithValue("@FirstName", FirstName);
                    command.Parameters.AddWithValue("@Department", Department);
                    command.Parameters.AddWithValue("@Phone", Phone);

                    command.ExecuteNonQuery();
                }
            }

            // Redirect to confirmation page
            return RedirectToPage("/Clients/Staffs");

        }
    }
}
