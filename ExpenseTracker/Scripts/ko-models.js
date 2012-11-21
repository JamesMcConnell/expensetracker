function formatCurrency(value) {
	return '$' + value.toFixed(2);
}

function formatDate(value) {
	var d = Date.parse(value);
	return d.toString("M/d/yyyy");
}

var PaycheckBudgetItem = function () {
	var self = this;
	self.PaycheckBudgetItemId = ko.observable();
	self.PaycheckBudgetId = ko.observable();
	self.Description = ko.observable();
	self.Amount = ko.observable();
	self.PaidDate = ko.observable();
	self.IsPaid = ko.observable();

	self.loadData = function (data) {
		self.PaycheckBudgetItemId(data.PaycheckBudgetItemId);
		self.PaycheckBudgetId(data.PaycheckBudgetId);
		self.Description(data.Description);
		self.Amount(data.Amount);
		self.PaidDate(data.PaidDate);
		self.IsPaid(data.IsPaid);
	};

	self.updateStatus = function (item) {
		item.IsPaid(true);
		app.dataservice.updatePaycheckBudgetItem(item.PaycheckBudgetItemId(), ko.toJSON(item));
	};
};

var PaycheckBudget = function () {
	var self = this;
	self.PaycheckBudgetId = ko.observable();
	self.PaycheckBudgetDate = ko.observable();
	self.PaycheckBudgetAmount = ko.observable();
	self.PaycheckBudgetItems = ko.observableArray([]);
	self.RemainingAmount = ko.computed(function () {
		if (self.PaycheckBudgetItems().length > 0) {
			var itemSum = 0;
			ko.utils.arrayForEach(self.PaycheckBudgetItems(), function (item) {
				itemSum += item.Amount();
			});
			return self.PaycheckBudgetAmount() - itemSum;
		} else {
			return 0;
		}
	});

	self.loadData = function (data) {
		self.PaycheckBudgetId(data.PaycheckBudgetId);
		self.PaycheckBudgetDate(formatDate(data.PaycheckBudgetDate));
		self.PaycheckBudgetAmount(data.PaycheckBudgetAmount);
		self.PaycheckBudgetItems(ko.utils.arrayMap(data.PaycheckBudgetItems, function (item) {
			var budgetItem = new PaycheckBudgetItem();
			budgetItem.loadData(item);
			return budgetItem;
		}));
	};

	self.addItem = function () {
		self.Items.push(new PaycheckBudgetItem());
		// This attaches the jQuery UI datepicker to the dynamically created input.
		$('.item-duedate:not(.hasDatePicker)').datepicker({
			showOn: 'button',
			buttonImage: 'images/calendar.gif',
			buttonImageOnly: true
		});
	};

	self.removeItem = function (item) {
		self.Items.remove(item);
	};
};

var BudgetListViewModel = function (data) {
	var self = this;
	self.budgets = ko.observableArray(ko.utils.arrayMap(data, function (item) {
		var budget = new PaycheckBudget();
		budget.loadData(item);
		return budget;
	}));

	self.selectedBudget = ko.observable(new PaycheckBudget());

	self.editPaycheck = function (pb) {
		//$('#edit-paycheckbudget-modal').modal('show');
		self.selectedBudget(pb);
	};

	self.addPaycheck = function () {
		//$('#edit-paycheckbudget-modal').modal('show');
		self.selectedBudget(new PaycheckBudget());
	};

	self.submitData = function (item) {
		app.dataservice.submitPaycheckBudget(
            ko.toJSON(item),
            function (result) {
            	$('#edit-paycheckbudget-modal').modal('hide');
            },
            function (xhr) {
            	alert(xhr.statusText + " " + xhr.repsonseText);
            }
        );
	};
};