namespace ExpenseManager.Web.Api.Helpers
{
	using ExpenseManager.Persistence;

	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.Logging;

	[ApiController]
	public abstract class BaseController<T> : ControllerBase
		where T : BaseController<T>
	{
		// ReSharper disable once ContextualLoggerProblem : Inheritance hack used
		protected BaseController(ILogger<T> logger, ExpenseManagerContext context)
		{
			Logger = logger;
			DbContext = context;
		}

		protected ILogger<T> Logger { get; }

		protected ExpenseManagerContext DbContext { get; }
	}
}
