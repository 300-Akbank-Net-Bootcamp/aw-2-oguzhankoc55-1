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
    public class MappingProfile : ControllerBase
    {
        private readonly VbDbContext dbContext;
        private readonly IMapper mapper;

        public MappingProfile(VbDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<List<AccountTransactionDto>> Get()
        {
            var transactions = await dbContext.AccountTransactions.ToListAsync();
            return mapper.Map<List<AccountTransactionDto>>(transactions);
        }

        [HttpGet("{id}")]
        public async Task<AccountTransactionDto> Get(int id)
        {
            var transaction = await dbContext.AccountTransactions
                .FirstOrDefaultAsync(x => x.Id == id);

            return mapper.Map<AccountTransactionDto>(transaction);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AccountTransactionDto accountTransactionDto)
        {
            var accountTransaction = mapper.Map<AccountTransaction>(accountTransactionDto);

            dbContext.AccountTransactions.Add(accountTransaction);
            await dbContext.SaveChangesAsync();

            return Ok(mapper.Map<AccountTransactionDto>(accountTransaction));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] AccountTransactionDto accountTransactionDto)
        {
            var existingTransaction = await dbContext.AccountTransactions.FindAsync(id);

            if (existingTransaction == null)
            {
                return NotFound();
            }

            // Update other properties as needed
            mapper.Map(accountTransactionDto, existingTransaction);

            await dbContext.SaveChangesAsync();

            return Ok(mapper.Map<AccountTransactionDto>(existingTransaction));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var transaction = await dbContext.AccountTransactions.FindAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            transaction.IsActive = false;
            await dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
