//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ExpenseTracker.Framework.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PaycheckBudget
    {
        public PaycheckBudget()
        {
            this.PaycheckBudgetItems = new HashSet<PaycheckBudgetItem>();
        }
    
        public int PaycheckBudgetId { get; set; }
        public System.DateTime PaycheckBudgetDate { get; set; }
        public decimal PaycheckBudgetAmount { get; set; }
    
        public virtual ICollection<PaycheckBudgetItem> PaycheckBudgetItems { get; set; }
    }
}