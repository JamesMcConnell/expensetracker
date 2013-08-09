using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ExpenseTracker.Framework.Models;
using ExpenseTracker.Framework.ViewModels;
using System.Net;
using System.Net.Http;

namespace ExpenseTracker.Controllers
{
    public class PaycheckBudgetController : ApiController
    {
		private readonly ExpenseTrackerEntities _db;

        public PaycheckBudgetController()
		{
			_db = new ExpenseTrackerEntities();
		}

        private PaycheckBudgetResponse GetBudgets(DateTime offset)
        {
            var budgetResponse = new PaycheckBudgetResponse
            {
                PagingInfo = new PagingInfo(),
                Budgets = new List<PaycheckBudgetViewModel>()
            };

			var budgets = _db.Budgets.ToList();
            var currentBudgets = budgets.Where(b => b.BudgetDate >= offset.Date).OrderBy(b => b.BudgetDate).Take(2).ToList();
            budgetResponse.PagingInfo.HasPrevious = budgets.Any(b => b.BudgetDate.Date < offset.Date);
			if (currentBudgets.Count > 1)
			{
				budgetResponse.PagingInfo.HasFuture = budgets.Any(b => b.BudgetDate.Date > currentBudgets[1].BudgetDate.Date);
			}
			else
			{
				budgetResponse.PagingInfo.HasFuture = false;
			}

            foreach (var budget in currentBudgets)
            {
                var paycheckBudget = new PaycheckBudgetViewModel
                {
                    Id = budget.BudgetId,
                    Amount = budget.BudgetAmount,
                    Date = budget.BudgetDate,
                    BudgetItems = budget.BudgetItems.Select(x => new PaycheckBudgetItemViewModel
                    {
						Id = x.BudgetItemId,
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
			var budgets = _db.Budgets.OrderBy(b => b.BudgetDate);
            List<BudgetItemAsEvent> events = new List<BudgetItemAsEvent>();
            foreach (var budget in budgets)
            {
                var paydayEvent = new BudgetItemAsEvent
                {
                    EventTitle = string.Format("Payday - {0}", budget.BudgetAmount),
                    AllDayEvent = true,
                    EventStart = budget.BudgetDate,
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
			var budget = _db.Budgets.FirstOrDefault(b => b.BudgetId == id);
            var paycheckBudget = new PaycheckBudgetViewModel
            {
                Id = budget.BudgetId,
                Amount = budget.BudgetAmount,
                Date = budget.BudgetDate,
                BudgetItems = budget.BudgetItems.Select(x => new PaycheckBudgetItemViewModel
                {
					Id = x.BudgetItemId,
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
			var budget = new Budget
			{
				BudgetAmount = budgetViewModel.Amount,
				BudgetDate = budgetViewModel.Date
			};

			foreach (var budgetItem in budgetViewModel.BudgetItems)
			{
				budget.BudgetItems.Add(new BudgetItem
				{
					Amount = budgetItem.Amount,
					Description = budgetItem.Description,
					IsPaid = budgetItem.IsPaid,
					DueDate = budgetItem.DueDate,
					DatePaid = budgetItem.DueDate // When adding a new budget, the date paid has to have a valid date, so give it the due date.
				});
			}

			_db.Budgets.AddObject(budget);
			_db.SaveChanges();

            return GetCurrentBudgets();
        }

        // PUT api/paycheckbudget/updatebudget/5
        [HttpPut]
        [ActionName("UpdateBudget")]
        public HttpResponseMessage UpdateBudget(int id, PaycheckBudgetViewModel budgetViewModel)
        {
			var dbBudget = _db.Budgets.FirstOrDefault(b => b.BudgetId == id);
            dbBudget.BudgetAmount = budgetViewModel.Amount;
            dbBudget.BudgetDate = budgetViewModel.Date;

			foreach (var budgetItem in budgetViewModel.BudgetItems)
			{
				var dbBudgetItem = dbBudget.BudgetItems.FirstOrDefault(bi => bi.BudgetItemId == budgetItem.Id);
				dbBudgetItem.Amount = budgetItem.Amount;
				dbBudgetItem.Description = budgetItem.Description;
				dbBudgetItem.IsPaid = budgetItem.IsPaid;
				dbBudgetItem.DueDate = budgetItem.DueDate;
				dbBudgetItem.DatePaid = budgetItem.DatePaid;
			}

            _db.SaveChanges();

            return new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
        }
    }
}
