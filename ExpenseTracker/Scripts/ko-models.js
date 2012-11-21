$(function () {
	// Budget Item view model
	app.PaycheckBudgetItem = function () {
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

		return {
			loadData: loadData,
			updateStatus: updateStatus
		};
	} ();

	app.PaycheckBudget = function () {
		var self = this;
		self.PaycheckBudgetId = ko.observable();
		self.PaycheckBudgetDate = ko.observable();
		self.PaycheckBudgetAmount = ko.observable();
		self.PaycheckBudgetItems = ko.observableArray([]);
		self.RemainingAmount = ko.observable();

		self.loadData = function (data) {
			self.PaycheckBudgetId(data.PaycheckBudgetId);
			self.PaycheckBudgetDate(app.utilities.formatDate(data.PaycheckBudgetDate));
			self.PaycheckBudgetAmount(data.PaycheckBudgetAmount);
			self.PaycheckBudgetItems(ko.utils.arrayMap(data.PaycheckBudgetItems, function (item) {
				return app.PaycheckBudgetItem.loadData(item);
			}));

			console.log(self.PaycheckBudgetItems);
			//			var itemSum = 0;
			//			ko.utils.arrayForEach(self.PaycheckBudgetItems(), function (item) {
			//				itemSum += item.Amount();
			//			});
			//			self.RemainingAmount(itemSum);
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

		return {
			loadData: loadData,
			addItem: addItem,
			removeItem: removeItem
		};
	} ();

	app.BudgetListViewModel = function () {
		var self = this;
		self.budgets = ko.observableArray([]);
		self.selectedBudget = ko.observable();

		self.loadData = function (data) {
			self.budgets(ko.utils.arrayMap(data, function (item) {
				var budget = app.PaycheckBudget.loadData(item);
				return budget;
			}));
		};

		self.editPaycheck = function (pb) {
			self.selectedBudget(pb);
		};

		self.addPaycheck = function () {
			self.selectedBudget(new app.PaycheckBudget());
		};

		self.submitData = function (item) {
			app.dataservice.submitPaycheckBudget(
				ko.toJSON(item),
				function (result) {
					$('#edit-paycheckbudget-modal').modal('hide');
				},
				function (xhr) {
					alert(xhr.statusText + " " + xhr.responseText);
				}
			);
		};

		return {
			budgets: budgets,
			selectedBudget: selectedBudget,
			loadData: loadData,
			editPaycheck: editPaycheck,
			addPaycheck: addPaycheck,
			submitData: submitData
		}
	} ();
});