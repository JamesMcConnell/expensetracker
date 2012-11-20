using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ExpenseTracker.Framework.Models;
using ExpenseTracker.Framework.ViewModels;
using ExpenseTracker.Framework.Data;
using ExpenseTracker.Framework.Data.Concrete;

namespace ExpenseTracker.Controllers
{
    public class PaycheckBudgetApiController : ApiController
    {
		private readonly IPaycheckBudgetRepository _repo;

		public PaycheckBudgetApiController()
		{
			_repo = new PaycheckBudgetRepository();
		}

        // GET api/paycheckbudgetapi
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
					Date = paycheckBudget.PaycheckBudgetDate.ToShortDateString(),
					Amount = paycheckBudget.PaycheckBudgetAmount,
					Remaining = paycheckBudget.PaycheckBudgetAmount - itemTotal,
					Items = items.Select(pbi => new PaycheckBudgetItemViewModel
					{
                        PaycheckBudgetItemId = pbi.PaycheckBudgetItemId,
                        PaycheckBudgetId = pbi.PaycheckBudgetId,
						Description = pbi.Description,
						Amount = pbi.Amount,
						DueDate = pbi.PaidDate.ToShortDateString(),
                        PaidStatus = pbi.IsPaid,
						IsPaid = (pbi.IsPaid) ? "Paid" : "Unpaid"
					})
				};

				paycheckBudgetViewModels.Add(vm);
			}

			return paycheckBudgetViewModels;
        }

        // GET api/paycheckbudgetapi/5
        public PaycheckBudgetViewModel Get(int id)
        {
            var dbPaycheckBudget = _repo.FetchById(id);
            var items = _repo.FetchAllItemsForPaycheckBudget(id);
            var itemTotal = items.Sum(x => x.Amount);

            var vm = new PaycheckBudgetViewModel
            {
                PaycheckBudgetId = id,
                Date = dbPaycheckBudget.PaycheckBudgetDate.ToShortDateString(),
                Amount = dbPaycheckBudget.PaycheckBudgetAmount,
                Remaining = dbPaycheckBudget.PaycheckBudgetAmount - itemTotal,
                Items = items.Select(x => new PaycheckBudgetItemViewModel
                {
                    PaycheckBudgetId = id,
                    PaycheckBudgetItemId = x.PaycheckBudgetItemId,
                    Description = x.Description,
                    Amount = x.Amount,
                    DueDate = x.PaidDate.ToShortDateString(),
                    PaidStatus = x.IsPaid,
                    IsPaid = (x.IsPaid) ? "Paid" : "Unpaid"
                })
            };

            return vm;
        }

        // POST api/paycheckbudgetapi
        public void Post(PaycheckBudgetViewModel budgetViewModel)
        {
            var paycheckBudget = new PaycheckBudget
            {
                PaycheckBudgetId = budgetViewModel.PaycheckBudgetId,
                PaycheckBudgetAmount = budgetViewModel.Amount,
                PaycheckBudgetDate = Convert.ToDateTime(budgetViewModel.Date)
            };

            var paycheckBudgetItems = budgetViewModel.Items.Select(x => new PaycheckBudgetItem
            {
                PaycheckBudgetId = x.PaycheckBudgetId,
                Amount = x.Amount,
                Description = x.Description,
                IsPaid = x.PaidStatus,
                PaidDate = Convert.ToDateTime(x.DueDate)
            });

            _repo.Update(paycheckBudget);
            _repo.UpdateAll(paycheckBudget.PaycheckBudgetId, paycheckBudgetItems);
        }

        // DELETE api/paycheckbudgetapi/5
        public void Delete(int id)
        {
        }
    }
}
