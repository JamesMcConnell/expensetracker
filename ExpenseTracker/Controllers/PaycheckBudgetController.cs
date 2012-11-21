using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ExpenseTracker.Framework.Data;
using ExpenseTracker.Framework.Models;
using ExpenseTracker.Framework.ViewModels;

namespace ExpenseTracker.Controllers
{
    public class PaycheckBudgetController : ApiController
    {
		private readonly IPaycheckBudgetRepository _repo;

		public PaycheckBudgetController(IPaycheckBudgetRepository repo)
		{
			_repo = repo;
		}

        // GET api/paycheckbudget
        public IEnumerable<PaycheckBudget> Get()
        {
			return _repo.FetchAll();
        }

        // GET api/paycheckbudget/5
        public PaycheckBudget Get(int id)
        {
            return _repo.FetchById(id);
        }

        // POST api/paycheckbudget
        public IEnumerable<PaycheckBudget> Post(PaycheckBudgetViewModel budgetViewModel)
        {
            var paycheckBudget = new PaycheckBudget
            {
                PaycheckBudgetId = budgetViewModel.PaycheckBudgetId,
                PaycheckBudgetAmount = budgetViewModel.PaycheckBudgetAmount,
                PaycheckBudgetDate = Convert.ToDateTime(budgetViewModel.PaycheckBudgetDate)
            };

            var paycheckBudgetItems = budgetViewModel.Items.Select(x => new PaycheckBudgetItem
            {
                PaycheckBudgetId = x.PaycheckBudgetId,
                Amount = x.Amount,
                Description = x.Description,
                IsPaid = x.IsPaid,
                PaidDate = Convert.ToDateTime(x.DueDate)
            });

			_repo.Update(paycheckBudget);
			_repo.UpdateAll(paycheckBudget.PaycheckBudgetId, paycheckBudgetItems);

			return Get();
        }

        // DELETE api/paycheckbudget/5
        public void Delete(int id)
        {
        }
    }
}
