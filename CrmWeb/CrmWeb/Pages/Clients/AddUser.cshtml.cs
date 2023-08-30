using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CrmWeb.Models;
using CrmWeb.Data;
using System.Data.SqlClient;
using java.awt;

namespace CrmWeb.Pages.Clients
{
    public class AddUserModel : PageModel
    {
        [BindProperty]
        public string? Username { get; set; }
        [BindProperty]
        public string? Password { get; set; }
        [BindProperty]
        public string? Name { get; set; }
        [BindProperty]
        public string? Email { get; set; }
        [BindProperty]
        public string? Address { get; set; }
        [BindProperty]
        public string? City { get; set; }
        [BindProperty]
        public string? PLZ { get; set; }
        [BindProperty]
        public string? Phone { get; set; }

        public void OnPost()
        {
            String connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\User\\Documents\\DB.mdf;Integrated Security=True;Connect Timeout=30";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "INSERT INTO Users (UserName, Password, Name, Email, Address, City, PLZ, Phone) VALUES (@Username, @Password, @Name, @Email, @Address, @City, @PLZ, @Phone);";


                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@UserName", Username);
                    command.Parameters.AddWithValue("@Password", Password);
                    command.Parameters.AddWithValue("@Name", Name);
                    command.Parameters.AddWithValue("@Email", Email);
                    command.Parameters.AddWithValue("@Address", Address);
                    command.Parameters.AddWithValue("@City", City);
                    command.Parameters.AddWithValue("@PLZ", PLZ);
                    command.Parameters.AddWithValue("@Phone", Phone);

                    command.ExecuteReader();
                }
            }
        }


        public void OnGet()
        {
        }

    }
}
