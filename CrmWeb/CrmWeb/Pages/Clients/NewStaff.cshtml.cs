using CrmWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace CrmWeb.Pages.Clients
{
    public class NewStaffModel : PageModel
    {
        DbAddress Db = new DbAddress();

        public String successMessage = string.Empty;
        public string PartnerId { get; set; }

        [BindProperty]
        public int Id { get; set; }

        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string Department { get; set; }    
        
        [BindProperty]
        public string Phone { get; set; } = string.Empty;

        public void OnGet()
        {

        }

        public void OnPost()
        {
            // Insert user into database
            var partnerId = Request.Cookies["PartnerId"];
            using (SqlConnection connection = new SqlConnection(Db.DB()))
            {
                connection.Open();

                String sql = "INSERT INTO Staff" +
                    "(FirstName, Department, Phone, PartnerId) VALUES" +
                    "(@Name, @Department, @Phone, @PartnerId);";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@partnerId", partnerId);

                    command.Parameters.AddWithValue("@Name", Name);
                    command.Parameters.AddWithValue("@Department", Department);
                    command.Parameters.AddWithValue("@Phone", Phone);

                    command.ExecuteReader();
                }
            }
            successMessage = "New staff added Correctly";
            Name = String.Empty;
            Department = String.Empty;
            Phone = String.Empty;
        }
    }
}
