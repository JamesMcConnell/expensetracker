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
		public int PaycheckBudgetId { get; set; }

		[DataMember(Name = "date")]
		public string PaycheckBudgetDate { get; set; }

		[DataMember(Name = "amount")]
		public decimal PaycheckBudgetAmount { get; set; }

		[DataMember(Name = "budgetItems")]
		public List<PaycheckBudgetItemViewModel> Items { get; set; }

		[DataMember(Name = "remaining")]
		public decimal Remaining { get; set; }
	}

	[DataContract]
	public class PaycheckBudgetItemViewModel
	{
		[DataMember(Name = "itemId")]
		public int PaycheckBudgetItemId { get; set; }

        [DataMember(Name = "budgetId")]
        public int PaycheckBudgetId { get; set; }

		[DataMember(Name = "description")]
		public string Description { get; set; }

		[DataMember(Name = "amount")]
		public decimal Amount { get; set; }

		[DataMember(Name = "paidDate")]
		public string PaidDate { get; set; }

		[DataMember(Name = "isPaid")]
		public bool IsPaid { get; set; }
	}
}
