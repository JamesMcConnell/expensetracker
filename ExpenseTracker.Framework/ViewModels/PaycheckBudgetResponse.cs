using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ExpenseTracker.Framework.ViewModels
{
    [DataContract]
    public class PaycheckBudgetResponse
    {
        [DataMember(Name = "budgets")]
        public List<PaycheckBudgetViewModel> Budgets { get; set; }

        [DataMember(Name = "pagingInfo")]
        public PagingInfo PagingInfo { get; set; }
    }
}
