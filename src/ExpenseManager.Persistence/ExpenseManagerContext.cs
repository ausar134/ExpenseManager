namespace ExpenseManager.Persistence
{
	using System;

	using ExpenseManager.Persistence.Entities;

	using Microsoft.EntityFrameworkCore;

	public class ExpenseManagerContext : DbContext
	{
		public DbSet<Account> Accounts { get; private set; }

		public DbSet<Transaction> Transactions { get; private set; }

		public DbSet<TransactionCategory> Categories { get; private set; }

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
