namespace ExpenseManager.Persistence
{
	using System;

	using ExpenseManager.Persistence.Entities;

	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Logging;

	public class ExpenseManagerContext : DbContext
	{
		private readonly ILoggerFactory loggerFactory;

		public ExpenseManagerContext(DbContextOptions<ExpenseManagerContext> options, ILoggerFactory factory)
			: base(options)
			=> loggerFactory = factory;

		public DbSet<Account> Accounts { get; private set; }

		public DbSet<Transaction> Transactions { get; private set; }

		public DbSet<TransactionCategory> Categories { get; private set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (optionsBuilder == null)
			{
				throw new ArgumentNullException(nameof(optionsBuilder));
			}

			if (loggerFactory != null)
			{
				optionsBuilder.UseLoggerFactory(loggerFactory);
			}

			base.OnConfiguring(optionsBuilder);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			if (modelBuilder == null)
			{
				throw new ArgumentNullException(nameof(modelBuilder));
			}

			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Transaction>(e =>
			{
				e.HasOne(t => t.Account)
					.WithMany(a => a.Transactions);

				e.HasOne(t => t.Category)
					.WithMany();
			});
		}
	}
}
