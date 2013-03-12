using System.Collections.Generic;
using System.Web.Http;
using ExpenseTracker.Framework.ViewModels;
using Raven.Client;

namespace ExpenseTracker.Controllers
{
    public class PaycheckBudgetItemController : ApiController
    {
        private IDocumentSession _docSession;

		public PaycheckBudgetItemController(IDocumentSession docSession)
		{
            _docSession = docSession;
		}

        // GET api/paycheckbudgetitem
        public IEnumerable<PaycheckBudgetItemViewModel> Get()
        {
			return new List<PaycheckBudgetItemViewModel>();
        }

        // GET api/paycheckbudgetitem/5
        public PaycheckBudgetItemViewModel Get(int id)
        {
			return new PaycheckBudgetItemViewModel();
        }

        // POST api/paycheckbudgetitem
        public void Post(PaycheckBudgetItemViewModel budgetItem)
        {
        }

        // PUT api/paycheckbudgetitem/5
        public void Put(int id, PaycheckBudgetItemViewModel budgetItem)
        {
        }

        // DELETE api/paycheckbudgetitem/5
        public void Delete(int id)
        {
        }
    }
}
