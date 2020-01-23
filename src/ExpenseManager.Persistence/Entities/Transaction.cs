namespace ExpenseManager.Persistence.Entities
{
	using System;

	using ExpenseManager.Models.Enums;
	using ExpenseManager.Persistence.Base;

	public class Transaction : Entity<Guid>
	{
		public Account Account { get; set; }

		public TransactionCategory Category { get; set; }

		public TransactionType TransactionType { get; set; }

		public decimal Amount { get; set; }

		public DateTimeOffset TransactionTime { get; set; }

		public string Comments { get; set; }
	}
}
