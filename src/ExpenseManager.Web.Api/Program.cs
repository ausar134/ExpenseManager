namespace ExpenseManager.Web.Api
{
	using System;
	using System.Linq;
	using System.Threading.Tasks;

	using ExpenseManager.Models.Enums;
	using ExpenseManager.Persistence;
	using ExpenseManager.Persistence.Entities;

	using Microsoft.AspNetCore.Hosting;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Hosting;

	public sealed class Program
	{
		public static async Task Main(string[] args)
		{
			var opts = new DbContextOptionsBuilder<ExpenseManagerContext>()
				.UseSqlite("Data Source=context.db3");
			using var ctx = new ExpenseManagerContext(opts.Options);

			var t = await ctx.Transactions
				.Include(x => x.Account)
				.Include(x => x.Category)
				.SingleOrDefaultAsync();

			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}
