using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Customers.DTO;
using Customers.Model;
using Customers.Repo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CustomersAPI.Service.Controllers
{

    /// <summary>
    /// Customer Web Api
    /// </summary>
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/CustomerManagement/")]
    public class CustomerController : ControllerBase
    {
        //inject the DBContext and logger into the controller...
        private readonly CustomerRepo _repo;

        private readonly ILogger _logger;

        /// <summary>
        /// C-tor
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="logger"></param>
        public CustomerController(CustomerRepo repository, ILogger logger)
        {

            _repo = repository;
            _logger = logger;
        }

       

        /// <summary>
        /// Gets customer by id 
        /// </summary>
        /// <param name="id">id </param>
        /// <returns></returns>
        [ProducesResponseType(typeof(CustomerDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(Exception), 500)]
        [HttpGet("{id}",Name= "GetCustomerById")]
        public ActionResult<Customer> GetCustomerById(int id)
        {
            var customer = _repo.FindById(id);
            if (customer.Result == null)
            {
                _logger.LogError($"Customer with id: {id}, hasn't been found in db.");
                return NotFound($"Customer with id: {id} not found");
            } 
            
            _logger.LogInformation($"Returned customer with id: {id}");
            return Ok(customer.Result);
            
        }

        /// <summary>
        /// Add Customer 
        /// </summary>
        /// <param name="dto">Customer DTO</param>
        [ProducesResponseType(typeof(CustomerDto), 201)]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(Exception), 500)]
        [HttpPut("Customers/AddCustomer")]
        public async Task<IActionResult> Post(CustomerDto customerDto)
        {
            try
            {

                if (customerDto == null)
                {
                    _logger.LogError("Customer object sent from client is null.");
                    return BadRequest("Customer object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid Customer object sent from client.");
                    return BadRequest("Invalid model object");
                }

                bool customerExists = _repo.DoesCustomerAlreadyExists(customerDto);

                if (!customerExists)
                {
                    var result = await _repo.AddCustomer(customerDto);
                    _logger.LogInformation($"customer Added with First Name : {customerDto.FirstName} and Last Name : {customerDto.LastName}");
                    return CreatedAtRoute("GetCustomerById", new { id = result.Id }, customerDto);
                }
                else
                    _logger.LogError($"customer already exists with the given First Name : {customerDto.FirstName} and Last Name : {customerDto.LastName}");
                return StatusCode(200, $"customer already exists with the given First Name : {customerDto.FirstName} and Last Name : {customerDto.LastName}");

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Add Customer action: {ex.Message}");
                return StatusCode(500, "Internal server error :" + ex.Message);
            }

        }

        /// <summary>
        /// Updates Customer 
        /// </summary>
        /// <param name="id">Customer Id</param>
        /// <param name="dto">Customer DTO</param>
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(Exception), 500)]
        [HttpPut("Customers/{id:int}")]
        public async Task<IActionResult> UpdateCustomer(int id, CustomerDto customerDto)
        {

            try
            {
                if (customerDto == null)
                {
                    _logger.LogError("Customer object sent from client is null.");
                    return BadRequest("Customer object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid Customer object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var customer = _repo.FindById(id);
                if (customer.Result == null)
                {
                    _logger.LogError($"Customer with id: {id}, hasn't been found in db.");
                    return NotFound($"Customer with Id {id} not found.");
                }
               
                bool customerExists = _repo.DoesCustomerAlreadyExists(customerDto);
                if (!customerExists)
                {
                    await _repo.UpdateCustomer(id, customerDto);
                    _logger.LogInformation($"Customer with id: {id}, Updated.First Name : {customerDto.FirstName} and Last Name : {customerDto.LastName}");
                    return StatusCode(200, "Customer updated successfully.");
                }
                else
                    _logger.LogError($"Update will conflict with First Name : {customerDto.FirstName} and Last Name : {customerDto.LastName}");
                return StatusCode(200, $"Update will conflict with First Name : {customerDto.FirstName} and Last Name : {customerDto.LastName}");



            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Add Customer action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

        }


        /// <summary>
        /// Deletes Customer by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(Exception), 500)]
        [HttpDelete("Customers/{id:int}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                var customer = _repo.FindById(id);
                if (customer.Result == null)
                {
                    _logger.LogError($"Customer with id: {id}, hasn't been found in db.");
                    return NotFound($"Customer with Id {id} not found.");
                }
                await _repo.DeleteCustomer(id);
                _logger.LogInformation($"Customer with id: {id}, Deleted successfully.");
                return StatusCode(200, "Customer Deleted successfully.");

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Add Customer action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

        }

        /// <summary>
        /// Search Customer based on First Name and Last Name 
        /// </summary>
        /// <param name="filterBy">search by</param>
        /// <returns>list of Customers</returns>
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(List<CustomerDto>), 200)]
        [ProducesResponseType(typeof(Exception), 500)]
        [HttpGet("Customers/GetCustomers")]
        
        public async Task<IActionResult> SearchCustomers(string filterBy)
        {

            if (!string.IsNullOrEmpty(filterBy))
            {
                var customer = _repo.FindCustomerByName(filterBy);

                if (customer.Result == null)
                {
                    _logger.LogError($"Customer with  First Name or Last Name: {filterBy}, Not Found.");
                    return NotFound($"Customer with First Name or Last Name: {filterBy}, Not Found.");
                }

                _logger.LogInformation($"All Customers with First Name or Last Name Like {filterBy}, Returned");
                return Ok(await customer);

            }

            try
            {
                _logger.LogInformation($"Returned all Customers from database.");
                return Ok(await _repo.GetCustomers());

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong {nameof(SearchCustomers)} action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

        }
    }
}