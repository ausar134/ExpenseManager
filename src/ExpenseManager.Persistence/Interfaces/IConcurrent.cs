namespace ExpenseManager.Persistence.Interfaces
{
	public interface IConcurrent
	{
#pragma warning disable CA1819 // Properties should not return arrays
		byte[] RowVersion { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays
	}
}
