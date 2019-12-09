namespace CustomerUnitTests
{
    using Customers.DbContexts;
    using Customers.DTO;
    using Customers.Model;
    using Customers.Repo;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    /// <summary>
    /// Defines the <see cref="CustomersUnitTest" />
    /// </summary>
    public class CustomersUnitTest
    {
        /// <summary>
        /// The AddCustomerAndGetCustomers_ReturnsThreeCustomers
        /// </summary>
        [Fact]
        public void AddCustomerAndGetCustomers_ReturnsThreeCustomers()
        {

            // Arrange

            var connectionStringBuilder =
                new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connection = new SqliteConnection(connectionStringBuilder.ToString());
            var options = new DbContextOptionsBuilder<CustomerDBContext>()
                .UseSqlite(connection)
                .Options;

            using (var context = new CustomerDBContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                context.Customers.AddRange(new[]
                {
                    new Customer(){
                        FirstName = "Salman",
                        LastName = "Rasheed",
                        DateOfBirth = DateTime.Now.AddYears(-15)
                    },
                    new Customer(){
                        FirstName = "Raheem",
                        LastName = "Khan",
                        DateOfBirth = DateTime.Now.AddYears(-21)
                    },
                    new Customer(){
                        FirstName = "Baba",
                        LastName = "Ganouj",
                        DateOfBirth = DateTime.Now.AddYears(-63)
                    }
                });


                context.SaveChanges();

                var customerRepo = new CustomerRepo(context);

                var customers = customerRepo.GetCustomers();
                // Assert
                Assert.Equal(3, (customers.Result).Count);
            }
        }

        /// <summary>
        /// The GetCustomerById_ReturnsCustomerByIdTwo
        /// </summary>
        [Fact]
        public void GetCustomerById_ReturnsCustomerByIdTwo()
        {

            // Arrange

            var connectionStringBuilder =
                new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connection = new SqliteConnection(connectionStringBuilder.ToString());
            var options = new DbContextOptionsBuilder<CustomerDBContext>()
                .UseSqlite(connection)
                .Options;

            using (var context = new CustomerDBContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                context.Customers.AddRange(new[]
                {
                    new Customer(){
                        FirstName = "Salman",
                        LastName = "Rasheed",
                        DateOfBirth = DateTime.Now.AddYears(-15)
                    },
                    new Customer(){
                        FirstName = "Raheem",
                        LastName = "Khan",
                        DateOfBirth = DateTime.Now.AddYears(-21)
                    },
                    new Customer(){
                        FirstName = "Baba",
                        LastName = "Ganouj",
                        DateOfBirth = DateTime.Now.AddYears(-63)
                    }
                });

                context.SaveChanges();

                var customerRepo = new CustomerRepo(context);

                // Act
                var customer = customerRepo.FindById(2);

                // Assert First Name
                Assert.Equal("Raheem", (customer.Result).FirstName);
                // Assert Last Name
                Assert.Equal("Khan", (customer.Result).LastName);
            }
        }

        /// <summary>
        /// The AddCustomer_ReturnsCustomerByFirstNameAndByLastName
        /// </summary>
        [Fact]
        public void AddCustomer_ReturnsCustomerByFirstNameAndByLastName()
        {
            var connectionStringBuilder =
               new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connection = new SqliteConnection(connectionStringBuilder.ToString());
            var options = new DbContextOptionsBuilder<CustomerDBContext>()
                .UseSqlite(connection)
                .Options;

            using (var context = new CustomerDBContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                context.Customers.AddRange(new[]
                {
                    new Customer(){
                        FirstName = "Salman",
                        LastName = "Rasheed",
                        DateOfBirth = DateTime.Now.AddYears(-15)
                    },
                    new Customer(){
                        FirstName = "Raheem",
                        LastName = "Khan",
                        DateOfBirth = DateTime.Now.AddYears(-21)
                    },
                    new Customer(){
                        FirstName = "Baba",
                        LastName = "Ganouj",
                        DateOfBirth = DateTime.Now.AddYears(-63)
                    }
                });


                context.SaveChanges();

                var customerRepo = new CustomerRepo(context);
                var dto = new CustomerDto()
                {
                    FirstName = "Mathew",
                    LastName = "Martin",
                    DateOfBirth = Convert.ToDateTime("1984/12/20")
                };


                var customer = customerRepo.AddCustomer(dto);

                // Assert
                Assert.Equal("Mathew", (customer.Result).FirstName);
                // Assert
                Assert.Equal("Martin", (customer.Result).LastName);
            }
        }

        /// <summary>
        /// The UpadeCustomer_UpdatesCustomerWithIdOne
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task UpadeCustomer_UpdateCustomerWithIdOne()
        {
            var connectionStringBuilder =
               new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connection = new SqliteConnection(connectionStringBuilder.ToString());
            var options = new DbContextOptionsBuilder<CustomerDBContext>()
                .UseSqlite(connection)
                .Options;

            using (var context = new CustomerDBContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                context.Customers.AddRange(new[]
                {
                    new Customer(){
                        FirstName = "Salman",
                        LastName = "Rasheed",
                        DateOfBirth = DateTime.Now.AddYears(-15)
                    },
                    new Customer(){
                        FirstName = "Raheem",
                        LastName = "Khan",
                        DateOfBirth = DateTime.Now.AddYears(-21)
                    },
                    new Customer(){
                        FirstName = "Baba",
                        LastName = "Ganouj",
                        DateOfBirth = DateTime.Now.AddYears(-63)
                    }
                });

                context.SaveChanges();

                var customerRepository = new CustomerRepo(context);
                var dto = new CustomerDto()
                {
                    FirstName = "Mathew",
                    LastName = "Martin",
                    DateOfBirth = Convert.ToDateTime("1984/12/20")
                };

                // Act
                await customerRepository.UpdateCustomer(1, dto);

                var customer = customerRepository.FindById(1);


                // Assert
                Assert.Equal("Mathew", (customer.Result).FirstName);
                // Assert
                Assert.Equal("Martin", (customer.Result).LastName);
            }
        }

        /// <summary>
        /// The DeleteCustomer_DeleteCustomerByIdOne
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task DeleteCustomer_DeleteCustomerByIdTwo()
        {
            var connectionStringBuilder =
               new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connection = new SqliteConnection(connectionStringBuilder.ToString());
            var options = new DbContextOptionsBuilder<CustomerDBContext>()
                .UseSqlite(connection)
                .Options;

            using (var context = new CustomerDBContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                context.Customers.AddRange(new[]
                {
                    new Customer(){
                        FirstName = "Salman",
                        LastName = "Rasheed",
                        DateOfBirth = DateTime.Now.AddYears(-15)
                    },
                    new Customer(){
                        FirstName = "Raheem",
                        LastName = "Khan",
                        DateOfBirth = DateTime.Now.AddYears(-21)
                    },
                    new Customer(){
                        FirstName = "Baba",
                        LastName = "Ganouj",
                        DateOfBirth = DateTime.Now.AddYears(-63)
                    }
                });

                context.SaveChanges();

                var customerRepository = new CustomerRepo(context);
                
                await customerRepository.DeleteCustomer(2);

                var customers = customerRepository.GetCustomers();
                // Assert
                Assert.Equal(2, (customers.Result).Count);
            }
        }

        /// <summary>
        /// The SearchCustomer_SearchByJohn
        /// </summary>
        [Fact]
        public void SearchCustomer_SearchByKhan()
        {
            var connectionStringBuilder =
               new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connection = new SqliteConnection(connectionStringBuilder.ToString());
            var options = new DbContextOptionsBuilder<CustomerDBContext>()
                .UseSqlite(connection)
                .Options;

            using (var context = new CustomerDBContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                context.Customers.AddRange(new[]
                {
                    new Customer(){
                        FirstName = "Salman",
                        LastName = "Khan",
                        DateOfBirth = DateTime.Now.AddYears(-15)
                    },
                    new Customer(){
                        FirstName = "Raheem",
                        LastName = "Khan",
                        DateOfBirth = DateTime.Now.AddYears(-21)
                    },
                    new Customer(){
                        FirstName = "Baba",
                        LastName = "Ganouj",
                        DateOfBirth = DateTime.Now.AddYears(-63)
                    }
                });


                context.SaveChanges();

                var customerRepository = new CustomerRepo(context);

            
                var customers = customerRepository.FindCustomerByName("Khan");
                // Assert
                Assert.Equal(2, customers.Result.Count);
            }
        }
    }
}