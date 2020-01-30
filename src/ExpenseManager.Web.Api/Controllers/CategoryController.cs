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

	[Route("api/category")]
    public class CategoryController : BaseController<CategoryController>
	{
        public CategoryController(ILogger<CategoryController> logger, ExpenseManagerContext context)
		: base(logger, context)
        {
		}

		[HttpGet("")]
		public async Task<ActionResult<List<TransactionCategory>>> RetrieveAllCategories()
		{
			var categories = await DbContext.Categories
				.TagWith("Retrieving all categories")
				.Where(x => !x.IsDeleted)
				.ToListAsync();

			return Ok(categories);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<TransactionCategory>> RetrieveCategory(long id)
		{
			var category = await DbContext.Categories
				.TagWith("Retrieving single category!")
				.Where(x => !x.IsDeleted)
				.SingleOrDefaultAsync(x => x.Id == id);

			if (category == null)
			{
				Logger.LogError(LogMessages.EntityNotFound, nameof(TransactionCategory), id);
				return NotFound("No such category!");
			}

			return Ok(category);
		}

		[HttpPost("")]
		public async Task<ActionResult<TransactionCategory>> CreateCategory(TransactionCategory cat)
		{
			var category = new TransactionCategory {Title = cat.Title };

			DbContext.Categories.Add(category);

			await DbContext.SaveChangesAsync();

			return CreatedAtAction(nameof(RetrieveCategory), new { id = category.Id }, category);
		}

		[HttpPut("{id}")]
		public async Task<ActionResult<TransactionCategory>> UpdateCategory(long id, TransactionCategory cat)
		{
			var category = await DbContext.Categories
				.TagWith("Category retrieval for update")
				.Where(x => !x.IsDeleted)
				.SingleOrDefaultAsync(x => x.Id == id);

			if (category == null)
			{
				Logger.LogError(LogMessages.EntityNotFound, nameof(Account), id);
				return NotFound("No such category!");
			}

			category.Title = cat.Title;

			await DbContext.SaveChangesAsync();

			return Ok(category);
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteCategory(long id)
		{
			var category = await DbContext.Categories
				.TagWith("Retrieving single category!")
				.Where(x => !x.IsDeleted)
				.SingleOrDefaultAsync(x => x.Id == id);

			if (category == null)
			{
				Logger.LogError(LogMessages.EntityNotFound, nameof(TransactionCategory), id);
				return NotFound("No such category!");
			}

			category.IsDeleted = true;

			await DbContext.SaveChangesAsync();

			return Ok();
		}
	}
}
