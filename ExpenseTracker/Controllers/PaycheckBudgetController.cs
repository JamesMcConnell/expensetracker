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

        private PaycheckBudgetResponse GetBudgets(DateTime offset)
        {
            var budgetResponse = new PaycheckBudgetResponse
            {
                PagingInfo = new PagingInfo(),
                Budgets = new List<PaycheckBudgetViewModel>()
            };

            var budgets = _docSession.Query<PaycheckBudget>().ToList();
            var currentBudgets = budgets.Where(b => b.Date.Date >= offset.Date).OrderBy(b => b.Date).Take(2).ToList();
            budgetResponse.PagingInfo.HasPrevious = budgets.Any(b => b.Date.Date < offset.Date);
            budgetResponse.PagingInfo.HasFuture = budgets.Any(b => b.Date.Date > currentBudgets[1].Date.Date);

            foreach (var budget in currentBudgets)
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
                budgetResponse.Budgets.Add(paycheckBudget);
            }

            return budgetResponse;
        }

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

        // GET api/paycheckbudget/getallbudgets
        [HttpGet]
        [ActionName("GetCurrentBudgets")]
        public PaycheckBudgetResponse GetCurrentBudgets()
        {
            return GetBudgets(DateTime.Now);
        }

        // POST api/paycheckbudget/getpagedbudgets
        [HttpPost]
        [ActionName("GetPreviousBudgets")]
        public PaycheckBudgetResponse GetPreviousBudgets(PaycheckBudgetViewModel budgetViewModel)
        {
            var dateOffset = budgetViewModel.Date.AddDays(-14); // We want to go back 2 weeks from the given date
            return GetBudgets(dateOffset);
        }

        [HttpPost]
        [ActionName("GetFutureBudgets")]
        public PaycheckBudgetResponse GetFutureBudgets(PaycheckBudgetViewModel budgetViewModel)
        {
            return GetBudgets(budgetViewModel.Date);
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
            return paycheckBudget;
        }

        // POST api/paycheckbudget/addbudget
        [HttpPost]
        [ActionName("AddBudget")]
        public PaycheckBudgetResponse AddBudget(PaycheckBudgetViewModel budgetViewModel)
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

            return GetCurrentBudgets();
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
