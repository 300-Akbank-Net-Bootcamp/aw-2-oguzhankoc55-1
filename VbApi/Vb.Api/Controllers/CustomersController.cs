using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vb.Business;
using Vb.Data;
using Vb.Data.Entity;

namespace VbApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly VbDbContext dbContext;
        private readonly IMapper mapper;

        public CustomersController(VbDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var customers = await dbContext.Customers.ToListAsync();
            var customerDtos = mapper.Map<List<CustomerDto>>(customers);
            return Ok(customerDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var customer = await dbContext.Customers
                .Include(x => x.Accounts)
                .Include(x => x.Addresses)
                .Include(x => x.Contacts)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (customer == null)
            {
                return NotFound();
            }

            var customerDto = mapper.Map<CustomerDto>(customer);

            return Ok(customerDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CustomerDto customerDto)
        {
            var customer = mapper.Map<Customer>(customerDto);

            if (customer == null)
            {
                return BadRequest("Mapping from CustomerDto to Customer failed.");
            }

            dbContext.Customers.Add(customer);
            await dbContext.SaveChangesAsync();

            var createdCustomerDto = mapper.Map<CustomerDto>(customer);

            return CreatedAtAction(nameof(Get), new { id = customer.Id }, createdCustomerDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] CustomerDto customerDto)
        {
            var existingCustomer = await dbContext.Customers.FindAsync(id);

            if (existingCustomer == null)
            {
                return NotFound();
            }

            // Update other properties as needed
            mapper.Map(customerDto, existingCustomer);

            await dbContext.SaveChangesAsync();

            var updatedCustomerDto = mapper.Map<CustomerDto>(existingCustomer);

            return Ok(updatedCustomerDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingCustomer = await dbContext.Customers.FindAsync(id);

            if (existingCustomer == null)
            {
                return NotFound();
            }

            existingCustomer.IsActive = false;
            await dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
