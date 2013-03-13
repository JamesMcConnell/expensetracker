using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;

namespace ExpenseTracker.Framework.ViewModels
{
	[DataContract]
	public class PaycheckBudgetViewModel
	{
		[DataMember(Name = "id")]
		public int Id { get; set; }

		[DataMember(Name = "date")]
		public DateTime Date { get; set; }

		[DataMember(Name = "amount")]
		public decimal Amount { get; set; }

		[DataMember(Name = "budgetItems")]
		public List<PaycheckBudgetItemViewModel> BudgetItems { get; set; }

		[DataMember(Name = "remaining")]
		public decimal Remaining { get; set; }
	}

	[DataContract]
	public class PaycheckBudgetItemViewModel
	{
		[DataMember(Name = "description")]
		public string Description { get; set; }

		[DataMember(Name = "amount")]
		public decimal Amount { get; set; }

		[DataMember(Name = "dueDate")]
		public DateTime DueDate { get; set; }

		[DataMember(Name = "isPaid")]
		public bool IsPaid { get; set; }
	}
}
