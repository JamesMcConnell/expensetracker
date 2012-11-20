using System.Collections.Generic;
using System.Linq;
using ExpenseTracker.Framework.Models;

namespace ExpenseTracker.Framework.Data.Concrete
{
	public class PaycheckBudgetRepository : IPaycheckBudgetRepository
	{
		#region IPaycheckBudgetRepository Members
		public IEnumerable<PaycheckBudget> FetchAll()
		{
			using (var context = new ExpenseTrackerEntities())
			{
				return context.PaycheckBudgets.ToList();
			}
		}

		public PaycheckBudget FetchById(int paycheckBudgetId)
		{
			using (var context = new ExpenseTrackerEntities())
			{
				return context.PaycheckBudgets.FirstOrDefault(pb => pb.PaycheckBudgetId == paycheckBudgetId);
			}
		}

		public void Create(PaycheckBudget paycheckBudget)
		{
			using (var context = new ExpenseTrackerEntities())
			{
				context.PaycheckBudgets.Add(paycheckBudget);
				context.SaveChanges();
			}
		}

		public void Update(PaycheckBudget paycheckBudget)
		{
			using (var context = new ExpenseTrackerEntities())
			{
				var dbPaycheckBudget = context.PaycheckBudgets.FirstOrDefault(pb => pb.PaycheckBudgetId == paycheckBudget.PaycheckBudgetId);
				dbPaycheckBudget.PaycheckBudgetDate = paycheckBudget.PaycheckBudgetDate;
				dbPaycheckBudget.PaycheckBudgetAmount = paycheckBudget.PaycheckBudgetAmount;
				context.SaveChanges();
			}
		}

		public IEnumerable<PaycheckBudgetItem> FetchAllItemsForPaycheckBudget(int paycheckBudgetId)
		{
			using (var context = new ExpenseTrackerEntities())
			{
				return context.PaycheckBudgetItems.Where(pbi => pbi.PaycheckBudgetId == paycheckBudgetId).ToList();
			}
		}

		public void Create(PaycheckBudgetItem paycheckBudgetItem)
		{
			using (var context = new ExpenseTrackerEntities())
			{
				context.PaycheckBudgetItems.Add(paycheckBudgetItem);
				context.SaveChanges();
			}
		}

		public void Update(PaycheckBudgetItem paycheckBudgetItem)
		{
			using (var context = new ExpenseTrackerEntities())
			{
				var dbPaycheckBudgetItem = context.PaycheckBudgetItems.FirstOrDefault(pbi => pbi.PaycheckBudgetItemId == paycheckBudgetItem.PaycheckBudgetItemId);
				dbPaycheckBudgetItem.Description = paycheckBudgetItem.Description;
				dbPaycheckBudgetItem.PaidDate = paycheckBudgetItem.PaidDate;
				dbPaycheckBudgetItem.Amount = paycheckBudgetItem.Amount;
				dbPaycheckBudgetItem.IsPaid = paycheckBudgetItem.IsPaid;
				context.SaveChanges();
			}
		}

        public void UpdateAll(int paycheckBudgetId, IEnumerable<PaycheckBudgetItem> paycheckBudgetItems)
        {
            using (var context = new ExpenseTrackerEntities())
            {
                var dbPaycheckBudget = context.PaycheckBudgets.FirstOrDefault(x => x.PaycheckBudgetId == paycheckBudgetId);
                var dbItems = dbPaycheckBudget.PaycheckBudgetItems.ToList();

                foreach (var dbItem in dbItems)
                {
                    context.PaycheckBudgetItems.Remove(dbItem);
                }
                context.SaveChanges();

                foreach (var paycheckBudgetItem in paycheckBudgetItems)
                {
                    dbPaycheckBudget.PaycheckBudgetItems.Add(paycheckBudgetItem);
                }

                context.SaveChanges();
            }
        }
		#endregion
	}
}
