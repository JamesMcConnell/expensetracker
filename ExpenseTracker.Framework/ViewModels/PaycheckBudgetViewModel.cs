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
		[DataMember]
		public int PaycheckBudgetId { get; set; }

		[DataMember]
		public string PaycheckBudgetDate { get; set; }

		[DataMember]
		public decimal PaycheckBudgetAmount { get; set; }

		[DataMember]
		public IEnumerable<PaycheckBudgetItemViewModel> Items { get; set; }

		[DataMember]
		public decimal Remaining { get; set; }
	}

	[DataContract]
	public class PaycheckBudgetItemViewModel
	{
		[DataMember]
		public int PaycheckBudgetItemId { get; set; }

        [DataMember]
        public int PaycheckBudgetId { get; set; }

		[DataMember]
		public string Description { get; set; }

		[DataMember]
		public decimal Amount { get; set; }

		[DataMember]
		public string DueDate { get; set; }

		[DataMember]
		public bool IsPaid { get; set; }
	}
}
