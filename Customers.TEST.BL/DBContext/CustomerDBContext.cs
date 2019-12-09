using Customers.Model;
using Microsoft.EntityFrameworkCore;

namespace Customers.DbContexts
{
    public class CustomerDBContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        public CustomerDBContext()
        { }

        public CustomerDBContext(DbContextOptions<CustomerDBContext> options)
         : base(options)
        {
        }
    }
}
