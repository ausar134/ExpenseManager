namespace ExpenseManager.Models
{
	public static class LogMessages
	{
		public const string UnhandledException = "An unhandled exception occurred! Error: {Message}.";

		public const string EntityNotFound =
			"{Entity} identified by {Id} was not found or accessed with insufficient privileges.";

		public const string BadRequestMissingValue = "User did not provide required input.";
	}
}
