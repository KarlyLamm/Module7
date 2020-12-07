using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// for file access and manipulation
using System.IO;
// for database access
using System.Data.SqlClient;



namespace DatabaseAndFileExample
{
    class DatabaseandFileExample
    {
        static void Main(string[] args)
        {
            //Database objects
            SqlConnection conn =
                   new SqlConnection(Properties.Settings.Default.connectionString);
            SqlDataReader reader;

            string custId;
            StringBuilder query = new StringBuilder();

            //File Objects
            const string FILE_PATH = @"c:/users/karly/orders/customer_orders.txt";
            FileStream file;
            StreamWriter custStream;

            try
            {
                // Get value from user
                Console.WriteLine("Please enter the customer's id");
                custId = Console.ReadLine();

                //opens connection
                conn.Open();

                // Get my query
                query.Append("Select OrderId, OrderDate, ShippedDate, ");
                query.Append("Freight, ShipCountry From Orders O Join Customers C ON");
                query.Append("O.CustomerId = C.CustomerId ");
                // Only get data about what user requested in custId
                query.Append("Where C.CustomerId = '");
                query.Append(custId);
                query.Append("'");

                //Use a class to pass query into database and return result
                reader = new SqlCommand(query.ToString(), conn).ExecuteReader();

                if (reader.HasRows)
                {
                    //Allows you to write to the file
                    using (file = new FileStream(FILE_PATH, FileMode.Create))
                    using (custStream = new StreamWriter(file)) 

                        while (reader.Read())
                        {
                            Console.WriteLine(reader.GetInt32(0) + " ");
                            Console.WriteLine(reader.GetDateTime(1) + " ");
                            Console.WriteLine(reader.GetDateTime(2) + " ");
                            Console.WriteLine(reader.GetDecimal(3) + " ");
                            Console.WriteLine(reader.GetString(4) + " ");

                            custStream.WriteLine(reader.GetInt32(0) + " ");
                            custStream.WriteLine(reader.GetDateTime(1) + " ");
                            custStream.WriteLine(reader.GetDateTime(2) + " ");
                            custStream.WriteLine(reader.GetDecimal(3) + " ");
                            custStream.WriteLine(reader.GetString(4) + " ");

                        }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
                Console.Read();
            }
        }
    }
}
