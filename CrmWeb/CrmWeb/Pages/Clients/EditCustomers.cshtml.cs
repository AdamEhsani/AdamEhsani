using CrmWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Text;

namespace CrmWeb.Pages.Clients
{
    public class EditCustomerModel : PageModel
    {
        DbAddress Db = new DbAddress();
        public String errorMessage = string.Empty;
        public String successMessage = string.Empty;

        [BindProperty(SupportsGet = true)]
        public string CustomerId { get; set; }

        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string Phone { get; set; }

        [BindProperty]
        public string Address { get; set; }

        public void OnGet()
        {
            var deCodeId = Convert.FromBase64String(Request.Query["id"].ToString());
            CustomerId = Encoding.UTF8.GetString(deCodeId);
            var partnerId = Request.Cookies["PartnerId"];

            try
            {
                using (SqlConnection connection = new SqlConnection(Db.DB()))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Customer WHERE Id=@Id AND PartnerId = @partnerId";

                    using (SqlCommand Command = new SqlCommand(sql, connection))
                    {
                        Command.Parameters.AddWithValue("@Id", CustomerId);
                        Command.Parameters.AddWithValue("@partnerId", partnerId);

                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            if (Reader.Read())
                            {
                                Name = Reader.GetString(1);
                                Phone = Reader.GetString(2);
                                Address = Reader.GetString(3);
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

            using (SqlConnection connection = new SqlConnection(Db.DB()))
            {
                connection.Open();

                String sql = "UPDATE Customer " +
                             "SET Name = @CustomerInputName, Address = @CustomerInputAddress, Phone = @CustomerInputPhone " +
                             "WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", CustomerId);
                    command.Parameters.AddWithValue("@CustomerInputName", Name.Trim());
                    command.Parameters.AddWithValue("@CustomerInputPhone", Phone.Trim());
                    command.Parameters.AddWithValue("@CustomerInputAddress", Address.Trim());
                    command.ExecuteNonQuery();
                }
            }
            return RedirectToPage("/Clients/Customers");
        }
    }
}
