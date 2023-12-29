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
    public class EftTransactionController : ControllerBase
    {
        private readonly VbDbContext dbContext;
        private readonly IMapper mapper;

        public EftTransactionController(VbDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<List<EftTransactionDto>> Get()
        {
            var eftTransactions = await dbContext.EftTransactions.ToListAsync();
            return mapper.Map<List<EftTransactionDto>>(eftTransactions);
        }

        [HttpGet("{id}")]
        public async Task<EftTransactionDto> Get(int id)
        {
            var eftTransaction = await dbContext.EftTransactions
                .FirstOrDefaultAsync(x => x.Id == id);

            return mapper.Map<EftTransactionDto>(eftTransaction);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EftTransactionDto eftTransactionDto)
        {
            var eftTransaction = mapper.Map<EftTransaction>(eftTransactionDto);

            if (eftTransaction == null)
            {
                return BadRequest("Mapping from EftTransactionDto to EftTransaction failed.");
            }

            dbContext.EftTransactions.Add(eftTransaction);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = eftTransaction.Id }, eftTransaction);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] EftTransactionDto eftTransactionDto)
        {
            var existingEftTransaction = await dbContext.EftTransactions.FindAsync(id);

            if (existingEftTransaction == null)
            {
                return NotFound();
            }

            // Update other properties as needed
            mapper.Map(eftTransactionDto, existingEftTransaction);

            if (existingEftTransaction == null)
            {
                return BadRequest("Mapping from EftTransactionDto to existing EftTransaction failed.");
            }

            await dbContext.SaveChangesAsync();

            return Ok(mapper.Map<EftTransactionDto>(existingEftTransaction));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var eftTransaction = await dbContext.EftTransactions.FindAsync(id);

            if (eftTransaction == null)
            {
                return NotFound();
            }

            eftTransaction.IsActive = false;
            await dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
