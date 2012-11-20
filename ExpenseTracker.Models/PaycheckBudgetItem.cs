using System;

namespace ExpenseTracker.Models
{
	public class PaycheckBudgetItem
	{
		public int PaycheckBudgetItemId { get; set; }
		public int PaycheckBudgetId { get; set; }
		public DateTime PaidDate { get; set; }
		public decimal Amount { get; set; }
		public bool IsPaid { get; set; }
	}
}
