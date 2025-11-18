using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBproject
{
    class Program
    {
        static void Main(string[] args)
        {
            int totalRowsAffected = 0;
            string answer = "y";

            while (answer.ToLower() == "y")
            {
                Console.Write("Enter product name: ");
                string product_name = Console.ReadLine();

                Console.Write("Enter product quantity: ");
                int product_count = int.Parse(Console.ReadLine());

                totalRowsAffected += Shop.InsertUser(product_name, product_count);

                Console.Write("Continue entering another record? (y/n): ");
                answer = Console.ReadLine();
            }

            Console.WriteLine($"Total {totalRowsAffected} Rows affected.");

            List<Product> allProducts = Shop.GetAllProducts();

            foreach (var p in allProducts)
            {
                Console.WriteLine($"Product: {p.ProductName}, Count: {p.ProductCount}");
            }
        }

       
    }
}
