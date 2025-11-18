using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DBproject
{
    class Shop
    {
        public static int InsertUser(string product_name, int product_count)
        {
            string connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=Shop;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";

            string query = "INSERT INTO Products (product_name, product_count) VALUES (@product_name, @product_count)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@product_name", product_name);
                    command.Parameters.AddWithValue("@product_count", product_count);

                    connection.Open();
                    return command.ExecuteNonQuery();
                }

            }

        }
        public static List<Product> GetAllProducts()
        {
            string connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=Shop;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";
            string query = "SELECT product_name, product_count FROM Products";

            List<Product> products = new List<Product>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Product p = new Product
                        {
                            ProductName = reader.GetString(0),
                            ProductCount = reader.GetInt32(1)
                        };
                        products.Add(p);
                    }
                }
            }
            return products;


        }

    }
}
