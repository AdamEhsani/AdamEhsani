namespace CrmWeb.Data
{
    public class DbAddress
    {
        public string DB()
        {
            const String CONNECTIONSTRING = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\User\\Documents\\DB.mdf;Integrated Security=True;Connect Timeout=30";
            return CONNECTIONSTRING;
        }
    }
}
