namespace ExpenseManager.Models.Enums
{
	public enum TransactionType
	{
		Unknown = 0,
		Income = 1,
		Expense = Income << 1,
		Transfer = Expense << 1,
	}
}
