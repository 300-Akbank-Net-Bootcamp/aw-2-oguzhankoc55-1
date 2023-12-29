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
    public class AccountController : ControllerBase
    {
        private readonly VbDbContext dbContext;
        private readonly IMapper mapper;

        public AccountController(VbDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var accounts = await dbContext.Accounts.ToListAsync();
            return Ok(accounts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var account = await dbContext.Accounts
                .Include(x => x.AccountTransactions)
                .Include(x => x.EftTransactions)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AccountDto _accountDto)
        {
            var account = mapper.Map<Account>(_accountDto);

            if (account == null)
            {
                return BadRequest("Mapping from AccountDto to Account failed.");
            }

            dbContext.Accounts.Add(account);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = account.Id }, mapper.Map<AccountDto>(account));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] AccountDto _accountDto)
        {
            var existingAccount = await dbContext.Accounts.FindAsync(id);

            if (existingAccount == null)
            {
                return NotFound();
            }

            mapper.Map(_accountDto, existingAccount);

            if (existingAccount == null)
            {
                return BadRequest("Mapping from AccountDto to existing Account failed.");
            }

            await dbContext.SaveChangesAsync();

            return Ok(mapper.Map<AccountDto>(existingAccount));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var account = await dbContext.Accounts.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            account.IsActive = false;
            await dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
