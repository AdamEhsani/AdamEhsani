using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CrmWeb.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string UserName { get; set; }
        [BindProperty]
        public string Password { get; set; }

        private const String CONNECTIONSTRING = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\User\\Documents\\DB.mdf;Integrated Security=True;Connect Timeout=30";
        private string _userName = "";
        private string _password = "";

        public void OnGet()
        {

        }

        public void OnPost()
        {
           
        }
    }
}