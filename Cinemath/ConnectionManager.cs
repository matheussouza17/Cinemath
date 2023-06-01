using Microsoft.Data.SqlClient;

namespace Cinemath
{
    public class ConnectionManager
    {
        private static string ConnStr = @"Persist Security Info=False;User ID=developer;Initial Catalog=db-mathcine;Data Source=db-mathcine.database.windows.net";

        public static SqlConnection GetConnection()
        {
            var cn = new SqlConnection(ConnStr);
            return cn;
        }

    }
}

