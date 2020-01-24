namespace ExpenseManager.Web.Api.Controllers
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	using ExpenseManager.Models;
	using ExpenseManager.Persistence;
	using ExpenseManager.Persistence.Entities;
	using ExpenseManager.Web.Api.Helpers;

	using Microsoft.AspNetCore.Mvc;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Logging;

	[Route("api/account")]
	public class AccountController : BaseController<AccountController>
	{
		public AccountController(ILogger<AccountController> logger, ExpenseManagerContext context)
			: base(logger, context)
		{
		}

		[HttpGet("")]
		public async Task<ActionResult<List<Account>>> RetrieveAllAccounts()
		{
			var accounts = await DbContext.Accounts
				.TagWith("Retrieving all accounts")
				.Where(x => !x.IsDeleted)
				.ToListAsync();

			return Ok(accounts);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Account>> RetrieveAccount(long id)
		{
			var account = await DbContext.Accounts
				.TagWith("Retrieving single account!")
				.Where(x => !x.IsDeleted)
				.SingleOrDefaultAsync(x => x.Id == id);

			if (account == null)
			{
				Logger.LogError(LogMessages.EntityNotFound, nameof(Account), id);
				return NotFound("No such account!");
			}

			return Ok(account);
		}

		[HttpPost("")]
		public async Task<ActionResult<Account>> CreateAccount(Account acc)
		{
			var account = new Account { Amount = acc.Amount, Title = acc.Title, };

			DbContext.Accounts.Add(account);

			await DbContext.SaveChangesAsync();

			return CreatedAtAction(nameof(RetrieveAccount), new { id = account.Id }, account);
		}

		[HttpPut("{id}")]
		public async Task<ActionResult<Account>> UpdateAccount(long id, Account acc)
		{
			var account = await DbContext.Accounts
				.TagWith("Account retrieval for update")
				.Where(x => !x.IsDeleted)
				.SingleOrDefaultAsync(x => x.Id == id);

			if (account == null)
			{
				Logger.LogError(LogMessages.EntityNotFound, nameof(Account), id);
				return NotFound("No such account!");
			}

			account.Amount = acc.Amount;
			account.Title = acc.Title;

			await DbContext.SaveChangesAsync();

			return Ok(account);
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteAccount(long id)
		{
			var account = await DbContext.Accounts
				.TagWith("Retrieving single account!")
				.Where(x => !x.IsDeleted)
				.SingleOrDefaultAsync(x => x.Id == id);

			if (account == null)
			{
				Logger.LogError(LogMessages.EntityNotFound, nameof(Account), id);
				return NotFound("No such account!");
			}

			account.IsDeleted = true;

			await DbContext.SaveChangesAsync();

			return Ok();
		}
	}
}
