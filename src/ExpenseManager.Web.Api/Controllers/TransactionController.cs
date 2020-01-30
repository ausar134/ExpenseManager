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

	[Route("api/transaction")]
	public class TransactionController : BaseController<TransactionController>
	{
		public TransactionController(ILogger<TransactionController> logger, ExpenseManagerContext context)
		: base(logger, context)
		{
		}

		[HttpGet("")]
		public async Task<ActionResult<List<Transaction>>> RetrieveAllTransactions()
		{
			var transactions = await DbContext.Transactions
				.TagWith("Retrieving all transactions")
				.Where(x => !x.IsDeleted)
				.ToListAsync();

			return Ok(transactions);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Transaction>> RetrieveTransaction(long id)
		{
			var transaction = await DbContext.Transactions
				.TagWith("Retrieving single transaction!")
				.Where(x => !x.IsDeleted)
				.SingleOrDefaultAsync(x => x.Account.Id == id);

			if (transaction == null)
			{
				Logger.LogError(LogMessages.EntityNotFound, nameof(Transaction), id);
				return NotFound("No such transaction!");
			}

			return Ok(transaction);
		}

		[HttpPost("")]
		public async Task<ActionResult<Transaction>> CreateTransaction(Transaction trans)
		{
			var transaction = new Transaction { Account = trans.Account, Category = trans.Category, 
				Amount = trans.Amount, TransactionType = trans.TransactionType, Id = trans.Id,
				TransactionTime = trans.TransactionTime, Comments = trans.Comments};

			DbContext.Transactions.Add(transaction);

			await DbContext.SaveChangesAsync();

			return CreatedAtAction(nameof(RetrieveTransaction), new { id = transaction.Id }, transaction);
		}

		[HttpPut("{id}")]
		public async Task<ActionResult<Transaction>> UpdateTransaction(long id, Transaction trans)
		{
			var transaction = await DbContext.Transactions
				.TagWith("Transaction retrieval for update")
				.Where(x => !x.IsDeleted)
				.SingleOrDefaultAsync(x => x.Account.Id == id);

			if (transaction == null)
			{
				Logger.LogError(LogMessages.EntityNotFound, nameof(Transaction), id);
				return NotFound("No such transaction!");
			}

			transaction.Amount = trans.Amount;
			transaction.Account = trans.Account;
			transaction.Category = trans.Category;
			transaction.TransactionType = trans.TransactionType;
			transaction.Id = trans.Id;
			transaction.TransactionTime = trans.TransactionTime;
			transaction.Comments = trans.Comments;

			await DbContext.SaveChangesAsync();

			return Ok(transaction);
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteTransaction(long id)
		{
			var transaction = await DbContext.Transactions
				.TagWith("Retrieving single transaction!")
				.Where(x => !x.IsDeleted)
				.SingleOrDefaultAsync(x => x.Account.Id == id);

			if (transaction == null)
			{
				Logger.LogError(LogMessages.EntityNotFound, nameof(Transaction), id);
				return NotFound("No such transaction!");
			}

			transaction.IsDeleted = true;

			await DbContext.SaveChangesAsync();

			return Ok();
		}
	}
}
