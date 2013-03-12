using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExpenseTracker.Framework.ViewModels;
using Raven.Client;
using ExpenseTracker.Framework.Models;

namespace ExpenseTracker.Controllers
{
	public class HomeController : Controller
	{
        private readonly IDocumentSession _docSession;

		public HomeController(IDocumentSession docSession)
		{
			_docSession = docSession;
		}

		public ActionResult Index()
		{
            return View();
		}
	}
}
