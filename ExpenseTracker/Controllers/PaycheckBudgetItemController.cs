using System;
using System.Collections.Generic;
using System.Web.Http;
using ExpenseTracker.Framework.Data;
using ExpenseTracker.Framework.Models;
using ExpenseTracker.Framework.ViewModels;

namespace ExpenseTracker.Controllers
{
    public class PaycheckBudgetItemController : ApiController
    {
		private readonly IPaycheckBudgetRepository _repo;

		public PaycheckBudgetItemController(IPaycheckBudgetRepository repo)
		{
			_repo = repo;
		}

        // GET api/paycheckbudgetitem
        public IEnumerable<PaycheckBudgetItemViewModel> Get()
        {
			return new List<PaycheckBudgetItemViewModel>();
        }

        // GET api/paycheckbudgetitem/5
        public PaycheckBudgetItemViewModel Get(int id)
        {
			return new PaycheckBudgetItemViewModel();
        }

        // POST api/paycheckbudgetitem
        public void Post(PaycheckBudgetItemViewModel budgetItem)
        {
        }

        // PUT api/paycheckbudgetitem/5
        public void Put(int id, PaycheckBudgetItemViewModel budgetItem)
        {
			var dbBudgetItem = new PaycheckBudgetItem
			{
				PaycheckBudgetItemId = budgetItem.PaycheckBudgetItemId,
				PaycheckBudgetId = budgetItem.PaycheckBudgetItemId,
				Amount = budgetItem.Amount,
				Description = budgetItem.Description,
				IsPaid = budgetItem.IsPaid,
				PaidDate = Convert.ToDateTime(budgetItem.DueDate)
			};

			_repo.Update(dbBudgetItem);
        }

        // DELETE api/paycheckbudgetitem/5
        public void Delete(int id)
        {
        }
    }
}
