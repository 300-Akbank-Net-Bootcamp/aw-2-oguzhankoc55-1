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
    public class AddressController : ControllerBase
    {
        private readonly VbDbContext dbContext;
        private readonly IMapper mapper;

        public AddressController(VbDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<List<AddressDto>> Get()
        {
            var addresses = await dbContext.Addresses.ToListAsync();
            return mapper.Map<List<AddressDto>>(addresses);
        }

        [HttpGet("{id}")]
        public async Task<AddressDto> Get(int id)
        {
            var address = await dbContext.Addresses
                .FirstOrDefaultAsync(x => x.Id == id);

            return mapper.Map<AddressDto>(address);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddressDto addressDto)
        {
            var address = mapper.Map<Address>(addressDto);

            dbContext.Addresses.Add(address);
            await dbContext.SaveChangesAsync();

            return Ok(mapper.Map<AddressDto>(address));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] AddressDto addressDto)
        {
            var existingAddress = await dbContext.Addresses.FindAsync(id);

            if (existingAddress == null)
            {
                return NotFound();
            }

            // Update other properties as needed
            mapper.Map(addressDto, existingAddress);

            await dbContext.SaveChangesAsync();

            return Ok(mapper.Map<AddressDto>(existingAddress));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var address = await dbContext.Addresses.FindAsync(id);

            if (address == null)
            {
                return NotFound();
            }

            address.IsActive = false;
            await dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
