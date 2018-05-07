using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.NET
{
    class Program
    {
        static void Main(string[] args) 
        {
            string str = @"data source=(local);database=BaseCardDB;user id=sa;password=123456;";
            using (SqlConnection connection = new SqlConnection(str))
            {
                string sql = "select * from product";

                SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(reader["ProductName"].ToString().Trim());
                    }
                }
            }
            Console.WriteLine("ok");
            Console.ReadLine();
        }
    }
    
}
