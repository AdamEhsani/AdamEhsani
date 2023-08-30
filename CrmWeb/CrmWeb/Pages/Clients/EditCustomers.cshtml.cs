using CrmWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace CrmWeb.Pages.Clients
{
    public class EditCustomerModel : PageModel
    {
        DbAddress Db = new DbAddress();
        public String errorMessage = string.Empty;
        public String successMessage = string.Empty;

        [BindProperty(SupportsGet = true)]
        public int CustomerId { get; set; }

        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string Phone { get; set; }

        [BindProperty]
        public string Address { get; set; }

        [BindProperty]
        public string Plz { get; set; }

        public void OnGet()
        {
            String id = Request.Query["Id"];
            try
            {

                using (SqlConnection connection = new SqlConnection(Db.DB()))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Customer WHERE id=@Id";

                    using (SqlCommand Command = new SqlCommand(sql, connection))
                    {
                        Command.Parameters.AddWithValue("@Id", id);
                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            if (Reader.Read())
                            {
                                CustomerId = Reader.GetInt32(0);
                                Name = Reader.GetString(1);
                                Phone = Reader.GetString(2);
                                Address = Reader.GetString(3);
                                Plz = Reader.GetString(4).Trim();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        public IActionResult OnPostAsync()
        {
            if (string.IsNullOrEmpty(Name)
                && string.IsNullOrEmpty(Phone))
            {
                errorMessage = "All fields are required";
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Update user in database

            using (SqlConnection connection = new SqlConnection(Db.DB()))
            {
                connection.Open();

                String sql = "UPDATE Customer " +
                             "SET Name = @CustomerInputName, Address = @CustomerInputAddress, Phone = @CustomerInputPhone, PLZ = @CustomerInputPlz " +
                             "WHERE id = @Id";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", CustomerId);
                    command.Parameters.AddWithValue("@CustomerInputName", Name.Trim());
                    command.Parameters.AddWithValue("@CustomerInputPhone", Phone.Trim());
                    command.Parameters.AddWithValue("@CustomerInputAddress", Address.Trim());
                    command.Parameters.AddWithValue("@CustomerInputPlz", Plz.Trim());

                    command.ExecuteNonQuery();
                }
            }

            // Redirect to confirmation page
            return RedirectToPage("/Clients/Customers");
        }
    }
}
