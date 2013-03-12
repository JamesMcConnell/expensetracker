using System;
using System.Collections.Generic;

namespace ExpenseTracker.Framework.Models
{
    public class PaycheckBudget
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public List<PaycheckBudgetItem> BudgetItems { get; set; }
    }

    public class PaycheckBudgetItem
    {
        public int Id { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Amount { get; set; }
        public bool IsPaid { get; set; }
        public string Description { get; set; }
    }
}
