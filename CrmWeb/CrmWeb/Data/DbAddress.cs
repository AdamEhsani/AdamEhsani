namespace CrmWeb.Data
{
    public class DbAddress
    {
        public string DB()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            IConfiguration configuration = builder.Build();
            string CONNECTIONSTRING = configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
         //   string CONNECTIONSTRING = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\User\\Documents\\DB.mdf;Integrated Security=True;Connect Timeout=30";

            return CONNECTIONSTRING;
        }
    }
}