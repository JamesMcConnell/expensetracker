using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExpenseTracker.Framework.Data;
using ExpenseTracker.Framework.ViewModels;

namespace ExpenseTracker.Controllers
{
	public class HomeController : Controller
	{
		private IPaycheckBudgetRepository _paycheckBudgetRepo;

		public HomeController(IPaycheckBudgetRepository paycheckBudgetRepo)
		{
			_paycheckBudgetRepo = paycheckBudgetRepo;
		}

		public ActionResult Index()
		{
			return View();
		}
	}
}
