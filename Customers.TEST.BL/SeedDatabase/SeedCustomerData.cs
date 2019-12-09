using Customers.DbContexts;
using Customers.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;


namespace Customers.SeedDatabase
{
    public class SeedCustomerData
    {

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var dbContext = new CustomerDBContext(
            serviceProvider.GetRequiredService<DbContextOptions<CustomerDBContext>>()))
            {


                if (!dbContext.Customers.Any())
                    dbContext.Customers.AddRange(new[]
                    {
                    new Customer(){
                        FirstName = "Atira", 
                        LastName = "Mukadam",
                        DateOfBirth = DateTime.Now.AddYears(-19)
                    },
                    new Customer(){
                        FirstName = "Azhar", 
                        LastName = "Wangde", 
                        DateOfBirth = DateTime.Now.AddYears(-17)
                    },
                    new Customer(){
                        FirstName = "Kumar",
                        LastName = "Sangakkar", 
                        DateOfBirth = DateTime.Now.AddYears(-33)
                    },
                     new Customer(){
                        FirstName = "Arvinda", 
                        LastName = "DeSilva",
                        DateOfBirth = DateTime.Now.AddYears(-41)
                    },
                      new Customer(){
                        FirstName = "Shane",
                        LastName = "Watson",
                        DateOfBirth = DateTime.Now.AddYears(-63)
                    }
                });

                dbContext.SaveChanges();
            }
        }
    }
}
