using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ado.Models
{
    public class Connection
    {
        public SqlConnection connection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["banSanPham"].ConnectionString;
           return new SqlConnection(connectionString);
        }
    }
}