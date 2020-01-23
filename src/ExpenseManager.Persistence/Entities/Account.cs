namespace ExpenseManager.Persistence.Entities
{
	using System.Collections.Generic;

	using ExpenseManager.Persistence.Base;

	public class Account : Entity<long>
	{
		public string Title { get; set; }

		public decimal Amount { get; set; }

		public virtual List<Transaction> Transactions { get; set; }
	}
}
