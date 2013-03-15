using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ExpenseTracker.Framework.Models;
using ExpenseTracker.Framework.ViewModels;
using Raven.Client;

namespace ExpenseTracker.Controllers
{
    public class PaycheckBudgetController : ApiController
    {
        private readonly IDocumentSession _docSession;

		public PaycheckBudgetController(IDocumentSession docSession)
		{
            _docSession = docSession;
		}

        // GET api/paycheckbudget
        public IEnumerable<PaycheckBudgetViewModel> Get()
        {
            var allBudgets = _docSession.Query<PaycheckBudget>().ToList().OrderBy(pb => pb.Date);
            List<PaycheckBudgetViewModel> paycheckBudgets = new List<PaycheckBudgetViewModel>();
            foreach (var budget in allBudgets)
            {
                var paycheckBudget = new PaycheckBudgetViewModel
                {
                    Id = budget.Id,
                    Amount = budget.Amount,
                    Date = budget.Date,
                    BudgetItems = budget.BudgetItems.Select(x => new PaycheckBudgetItemViewModel
                    {
                        Amount = x.Amount,
                        Description = x.Description,
                        IsPaid = x.IsPaid,
                        DueDate = x.DueDate,
                        DatePaid = x.DatePaid
                    }).ToList()
                };

                paycheckBudget.Remaining = paycheckBudget.Amount - paycheckBudget.BudgetItems.Sum(pbi => pbi.Amount);
                paycheckBudgets.Add(paycheckBudget);
            }

            return paycheckBudgets;
        }

        // GET api/paycheckbudget/5
        public PaycheckBudgetViewModel Get(int id)
        {
            var budget = _docSession.Load<PaycheckBudget>(id);
            var paycheckBudget = new PaycheckBudgetViewModel
            {
                Id = budget.Id,
                Amount = budget.Amount,
                Date = budget.Date,
                BudgetItems = budget.BudgetItems.Select(x => new PaycheckBudgetItemViewModel
                {
                    Amount = x.Amount,
                    Description = x.Description,
                    IsPaid = x.IsPaid,
                    DueDate = x.DueDate,
                    DatePaid = x.DatePaid
                }).ToList()
            };

            paycheckBudget.Remaining = paycheckBudget.Amount - paycheckBudget.BudgetItems.Sum(pbi => pbi.Amount);

            return paycheckBudget;
        }

        // POST api/paycheckbudget
        public void Post(PaycheckBudgetViewModel budgetViewModel)
        {
            var paycheckBudget = new PaycheckBudget
            {
                Amount = budgetViewModel.Amount,
                Date = budgetViewModel.Date,
                BudgetItems = budgetViewModel.BudgetItems.Select(x => new PaycheckBudgetItem
                {
                    Amount = x.Amount,
                    Description = x.Description,
                    IsPaid = x.IsPaid,
                    DueDate = x.DueDate,
                    DatePaid = x.DatePaid
                }).ToList()
            };

            _docSession.Store(paycheckBudget);
            _docSession.SaveChanges();
        }

        // PUT api/paycheckbudget/5
        public void Put(int id, PaycheckBudgetViewModel budgetViewModel)
        {
            var dbBudget = _docSession.Load<PaycheckBudget>(id);
            dbBudget.Amount = budgetViewModel.Amount;
            dbBudget.Date = budgetViewModel.Date;
            dbBudget.BudgetItems = budgetViewModel.BudgetItems.Select(x => new PaycheckBudgetItem
            {
                Amount = x.Amount,
                Description = x.Description,
                IsPaid = x.IsPaid,
                DueDate = x.DueDate,
                DatePaid = x.DatePaid
            }).ToList();

            _docSession.SaveChanges();
        }

        // DELETE api/paycheckbudget/5
        public void Delete(int id)
        {
        }
    }
}
