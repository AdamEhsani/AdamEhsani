using CrmWeb.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace CrmWeb.Pages.Clients
{
    public class NewCustomerModel : PageModel
    {
        DbAddress Db = new DbAddress();
        public CustomerInfo Customers = new CustomerInfo();
        public String errorMessage = string.Empty;
        public String successMessage = string.Empty;
        public void OnGet()
        {

        }

        public void OnPost()
        {
            Customers.Name = Request.Form["Name"];
            Customers.Address = Request.Form["Address"];
            Customers.Phone = Request.Form["Phone"];
            Customers.PLZ = Request.Form["PLZ"];
            var partnerId = Request.Cookies["PartnerId"];

            if (string.IsNullOrWhiteSpace(Customers.Name)
                || string.IsNullOrWhiteSpace(Customers.Phone))
            {
                errorMessage = "All the fields are required";
                return;
            }

            // save the new Customer into the datebase
            try
            {
                using (SqlConnection connection = new SqlConnection(Db.DB()))
                {
                    connection.Open();

                    String sql = "INSERT INTO Customer" +
                        "(Name, Address, Phone, PLZ, PartnerId) VALUES" +
                        "(@Name, @Address, @Phone, @PLZ, @partnerId);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", Customers.Id);
                        command.Parameters.AddWithValue("@Name", Customers.Name.Trim());
                        command.Parameters.AddWithValue("@Address", Customers.Address.Trim());
                        command.Parameters.AddWithValue("@Phone", Customers.Phone);
                        command.Parameters.AddWithValue("@PLZ", Customers.PLZ.Trim());
                        command.Parameters.AddWithValue("@partnerId", partnerId);

                        command.ExecuteReader();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Customers.Name = string.Empty;
            Customers.Address = string.Empty;
            Customers.Phone = string.Empty;
            Customers.PLZ = string.Empty;
            successMessage = "New Customer added Correctly";
        }
    }

    public class CustomerInfo
    {
        public string PartnerId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string PLZ { get; set; }
    }
}
