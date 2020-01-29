#pragma warning disable CA1819 // Properties should not return arrays
namespace ExpenseManager.Persistence.Base
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	using ExpenseManager.Persistence.Interfaces;

	public abstract class Entity<T> : IEntity<T>, IConcurrent, IDeletable
		where T : IComparable
	{
		/// <summary>
		/// The primary Id in database.
		/// </summary>
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public T Id { get; set; }

		/// <summary>
		/// Simple field to track Database Concurrency.
		/// </summary>
		[Timestamp]
		public byte[] RowVersion { get; set; }

		public bool IsDeleted { get; set; }
	}
}
