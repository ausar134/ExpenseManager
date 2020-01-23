namespace ExpenseManager.Persistence.Entities
{
	using ExpenseManager.Persistence.Base;

	public class TransactionCategory : Entity<long>
	{
		public string Title { get; set; }
	}
}
