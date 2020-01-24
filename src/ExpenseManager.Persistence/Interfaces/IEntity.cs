namespace ExpenseManager.Persistence.Interfaces
{
	using System;

	public interface IEntity<T>
		where T : IComparable
	{
		T Id { get; set; }
	}
}
