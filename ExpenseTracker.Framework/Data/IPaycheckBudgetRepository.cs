using System.Collections.Generic;
using ExpenseTracker.Framework.Models;
using System.Linq;

namespace ExpenseTracker.Framework.Data
{
	public interface IPaycheckBudgetRepository
	{
		IEnumerable<PaycheckBudget> FetchAll();
		PaycheckBudget FetchById(int paycheckBudgetId);

		void Create(PaycheckBudget paycheckBudget);
		void Update(PaycheckBudget paycheckBudget);

		IEnumerable<PaycheckBudgetItem> FetchAllItemsForPaycheckBudget(int paycheckBudgetId);

		void Create(PaycheckBudgetItem paycheckBudgetItem);
		void Update(PaycheckBudgetItem paycheckBudgetItem);
        void UpdateAll(int paycheckBudgetId, IEnumerable<PaycheckBudgetItem> paycheckBudgetItems);
	}
}
