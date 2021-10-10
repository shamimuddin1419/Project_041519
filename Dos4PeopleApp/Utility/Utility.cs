using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Dos4PeopleApp.Utility
{
    public static class Utility
    {
        public static string connectionString;
        public static SqlConnection GetConnection()
        {           
            var conn = new SqlConnection(connectionString);
            return conn;
        }
    }
}
