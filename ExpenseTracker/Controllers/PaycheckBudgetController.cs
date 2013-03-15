using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ExpenseTracker.Framework.Models;
using ExpenseTracker.Framework.ViewModels;
using Raven.Client;
using System.Net;using System.Net.Http;

namespace ExpenseTracker.Controllers
{
    public class PaycheckBudgetController : ApiController
    {
        private readonly IDocumentSession _docSession;

		public PaycheckBudgetController(IDocumentSession docSession)
		{
            _docSession = docSession;
		}

        #region Custom Routes
        // GET api/paycheckbudget/getbudgetitemsascalendarevents
        [HttpGet]
        [ActionName("GetBudgetItemsAsCalendarEvents")]
        public IEnumerable<BudgetItemAsEvent> GetBudgetItemsAsCalendarEvents()
        {
            var budgets = _docSession.Query<PaycheckBudget>().ToList().OrderBy(pb => pb.Date);
            List<BudgetItemAsEvent> events = new List<BudgetItemAsEvent>();
            foreach (var budget in budgets)
            {
                var paydayEvent = new BudgetItemAsEvent
                {
                    EventTitle = string.Format("Payday - {0}", budget.Amount),
                    AllDayEvent = true,
                    EventStart = budget.Date,
                    IsEditable = false,
                    TextColor = "#3a87ad",
                    BackgroundColor = "#d9edf7",
                    BorderColor = "#bce8f1"
                };
                events.Add(paydayEvent);

                var itemEvents = budget.BudgetItems.Select(x => new BudgetItemAsEvent
                {
                    EventTitle = string.Format("{0} - {1}", x.Description, x.Amount),
                    AllDayEvent = true,
                    EventStart = x.DueDate,
                    IsEditable = false,
                    TextColor = "#ffffff",
                    BackgroundColor = "#6F8BC4",
                    BorderColor = "#3366CC",
                    CssClass = (x.IsPaid) ? "event-paid" : ""
                });
                events.AddRange(itemEvents);
            }

            return events;
        }
        #endregion

        // GET api/paycheckbudget/getallbudgets
        [HttpGet]
        [ActionName("GetAllBudgets")]
        public IEnumerable<PaycheckBudgetViewModel> GetAllBudgets()
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

        // GET api/paycheckbudget/getbudget/5
        [HttpGet]
        [ActionName("GetBudget")]
        public PaycheckBudgetViewModel GetBudget(int id)
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

        // POST api/paycheckbudget/addbudget
        [HttpPost]
        [ActionName("AddBudget")]
        public int AddBudget(PaycheckBudgetViewModel budgetViewModel)
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

            return paycheckBudget.Id;
        }

        // PUT api/paycheckbudget/updatebudget/5
        [HttpPut]
        [ActionName("UpdateBudget")]
        public HttpResponseMessage UpdateBudget(int id, PaycheckBudgetViewModel budgetViewModel)
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

            return new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
        }
    }
}
