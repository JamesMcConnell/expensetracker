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
			var paycheckBudgets = _repo.FetchAll().ToList();
			var paycheckBudgetViewModels = new List<PaycheckBudgetViewModel>();

			foreach (var paycheckBudget in paycheckBudgets)
			{
				var items = _repo.FetchAllItemsForPaycheckBudget(paycheckBudget.PaycheckBudgetId).ToList();
				var itemTotal = items.Sum(pbi => pbi.Amount);

				var vm = new PaycheckBudgetViewModel
				{
					PaycheckBudgetId = paycheckBudget.PaycheckBudgetId,
					PaycheckBudgetDate = paycheckBudget.PaycheckBudgetDate.ToShortDateString(),
					PaycheckBudgetAmount = paycheckBudget.PaycheckBudgetAmount,
					Remaining = paycheckBudget.PaycheckBudgetAmount - itemTotal,
					Items = items.Select(pbi => new PaycheckBudgetItemViewModel
					{
                        PaycheckBudgetItemId = pbi.PaycheckBudgetItemId,
                        PaycheckBudgetId = pbi.PaycheckBudgetId,
						Description = pbi.Description,
						Amount = pbi.Amount,
						DueDate = pbi.PaidDate.ToShortDateString(),
                        IsPaid = pbi.IsPaid
					})
				};

				paycheckBudgetViewModels.Add(vm);
			}

			return paycheckBudgetViewModels;
        }

        // GET api/paycheckbudget/5
        public PaycheckBudgetViewModel Get(int id)
        {
            var dbPaycheckBudget = _repo.FetchById(id);
            var items = _repo.FetchAllItemsForPaycheckBudget(id);
            var itemTotal = items.Sum(x => x.Amount);

            var vm = new PaycheckBudgetViewModel
            {
                PaycheckBudgetId = id,
                PaycheckBudgetDate = dbPaycheckBudget.PaycheckBudgetDate.ToShortDateString(),
                PaycheckBudgetAmount = dbPaycheckBudget.PaycheckBudgetAmount,
                Remaining = dbPaycheckBudget.PaycheckBudgetAmount - itemTotal,
                Items = items.Select(x => new PaycheckBudgetItemViewModel
                {
                    PaycheckBudgetId = id,
                    PaycheckBudgetItemId = x.PaycheckBudgetItemId,
                    Description = x.Description,
                    Amount = x.Amount,
                    DueDate = x.PaidDate.ToShortDateString(),
                    IsPaid = x.IsPaid
                })
            };

            return vm;
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
