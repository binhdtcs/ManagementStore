using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Management_Store
{
     class DBConnection
    {
        public string MyConnection()
        {
            string con = @"Data Source=LAPTOP-87899HGJ;Initial Catalog=Management_Store;Integrated Security=True";
            return con;
        }
    }
}
