using Castle.Core.Configuration;
using Microsoft.EntityFrameworkCore;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class DatabaseFixture : IDisposable
    {
        public ApiDBContext Context { get; private set; }
        public DatabaseFixture()
        {
            // Set up the test database connection and initialize the context
            var options = new DbContextOptionsBuilder<ApiDBContext>()
                
                .UseSqlServer("Data Source = ATARA; Initial Catalog = ApiDB_test; Integrated Security = True; Trust Server Certificate=True")
                .Options;
            Context = new ApiDBContext(options);
            Context.Database.EnsureCreated();
        }

  

        public void Dispose()
        {
            // Clean up the test database after all tests are completed
            Context.Database.EnsureDeleted();
            Context.Dispose();
        }
    }
}
