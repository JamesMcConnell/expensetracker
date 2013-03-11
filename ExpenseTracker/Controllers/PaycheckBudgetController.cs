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
        public IEnumerable<PaycheckBudgetViewModel> Get()
        {
            var allBudgets = _repo.FetchAll();
            List<PaycheckBudgetViewModel> paycheckBudgets = new List<PaycheckBudgetViewModel>();
            foreach (var budget in allBudgets)
            {
                var paycheckBudget = new PaycheckBudgetViewModel
                {
                    PaycheckBudgetId = budget.PaycheckBudgetId,
                    PaycheckBudgetAmount = budget.PaycheckBudgetAmount,
                    PaycheckBudgetDate = budget.PaycheckBudgetDate.ToShortDateString(),
                    Items = new List<PaycheckBudgetItemViewModel>()
                };

                foreach (var budgetItem in budget.PaycheckBudgetItems)
                {
                    var paycheckBudgetItem = new PaycheckBudgetItemViewModel
                    {
                        PaycheckBudgetId = budget.PaycheckBudgetId,
                        PaycheckBudgetItemId = budgetItem.PaycheckBudgetItemId,
                        Amount = budgetItem.Amount,
                        Description = budgetItem.Description,
                        IsPaid = budgetItem.IsPaid,
                        PaidDate = budgetItem.PaidDate.ToShortDateString()
                    };

                    paycheckBudget.Items.Add(paycheckBudgetItem);
                }

                paycheckBudget.Remaining = paycheckBudget.PaycheckBudgetAmount - paycheckBudget.Items.Sum(pbi => pbi.Amount);
                paycheckBudgets.Add(paycheckBudget);
            }

            return paycheckBudgets;
        }

        // GET api/paycheckbudget/5
        public PaycheckBudgetViewModel Get(int id)
        {
            var budget = _repo.FetchById(id);
            var paycheckBudget = new PaycheckBudgetViewModel
            {
                PaycheckBudgetId = budget.PaycheckBudgetId,
                PaycheckBudgetAmount = budget.PaycheckBudgetAmount,
                PaycheckBudgetDate = budget.PaycheckBudgetDate.ToShortDateString(),
                Items = new List<PaycheckBudgetItemViewModel>()
            };

            foreach (var budgetItem in budget.PaycheckBudgetItems)
            {
                var paycheckBudgetItem = new PaycheckBudgetItemViewModel
                {
                    PaycheckBudgetId = budget.PaycheckBudgetId,
                    PaycheckBudgetItemId = budgetItem.PaycheckBudgetItemId,
                    Amount = budgetItem.Amount,
                    Description = budgetItem.Description,
                    IsPaid = budgetItem.IsPaid,
                    PaidDate = budgetItem.PaidDate.ToShortDateString()
                };

                paycheckBudget.Items.Add(paycheckBudgetItem);
            }

            paycheckBudget.Remaining = paycheckBudget.PaycheckBudgetAmount - paycheckBudget.Items.Sum(pbi => pbi.Amount);

            return paycheckBudget;
        }

        // POST api/paycheckbudget
        public IEnumerable<PaycheckBudgetViewModel> Post(PaycheckBudgetViewModel budgetViewModel)
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
                PaidDate = Convert.ToDateTime(x.PaidDate)
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
