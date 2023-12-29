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
    public class ContactController : ControllerBase
    {
        private readonly VbDbContext dbContext;
        private readonly IMapper mapper;

        public ContactController(VbDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<List<ContactDto>> Get()
        {
            var contacts = await dbContext.Contacts.ToListAsync();
            return mapper.Map<List<ContactDto>>(contacts);
        }

        [HttpGet("{id}")]
        public async Task<ContactDto> Get(int id)
        {
            var contact = await dbContext.Contacts
                .FirstOrDefaultAsync(x => x.Id == id);

            return mapper.Map<ContactDto>(contact);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ContactDto contactDto)
        {
            var contact = mapper.Map<Contact>(contactDto);

            dbContext.Contacts.Add(contact);
            await dbContext.SaveChangesAsync();

            return Ok(mapper.Map<ContactDto>(contact));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ContactDto contactDto)
        {
            var existingContact = await dbContext.Contacts.FindAsync(id);

            if (existingContact == null)
            {
                return NotFound();
            }

            // Update other properties as needed
            mapper.Map(contactDto, existingContact);

            await dbContext.SaveChangesAsync();

            return Ok(mapper.Map<ContactDto>(existingContact));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var contact = await dbContext.Contacts.FindAsync(id);

            if (contact == null)
            {
                return NotFound();
            }

            contact.IsActive = false;
            await dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
