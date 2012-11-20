function formatCurrency(value) {
	return '$' + value.toFixed(2);
}

var PaycheckBudgetItem = function () {
    var self = this;
    self.PaycheckBudgetItemId = ko.observable();
    self.PaycheckBudgetId = ko.observable();
    self.Description = ko.observable();
    self.Amount = ko.observable();
    self.DueDate = ko.observable();
    self.IsPaid = ko.observable();
    self.PaidStatus = ko.observable();

    self.loadData = function (data) {
        self.PaycheckBudgetItemId(data.PaycheckBudgetItemId);
        self.PaycheckBudgetId(data.PaycheckBudgetId);
        self.Description(data.Description);
        self.Amount(data.Amount);
        self.DueDate(data.DueDate);
        self.IsPaid(data.IsPaid);
        self.PaidStatus(data.PaidStatus);
    };

    self.updateStatus = function (item) {
        item.PaidStatus(true);
        item.IsPaid('Paid');
        app.dataservice.updatePaycheckBudgetItem(item.PaycheckBudgetItemId(), ko.toJSON(item));
    };
};

var PaycheckBudget = function () {
    var self = this;
    self.PaycheckBudgetId = ko.observable();
    self.PaycheckBudgetDate = ko.observable();
    self.PaycheckBudgetAmount = ko.observable();
    self.Remaining = ko.observable();
    self.Items = ko.observableArray([]);

    self.loadData = function (data) {
        self.PaycheckBudgetId(data.PaycheckBudgetId);
        self.PaycheckBudgetDate(data.PaycheckBudgetDate);
        self.PaycheckBudgetAmount(data.PaycheckBudgetAmount);
        self.Remaining(data.Remaining);
        self.Items(ko.utils.arrayMap(data.Items, function (item) {
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

    self.selectedBudget = ko.observable();

    self.editPaycheck = function (item) {
        $('#edit-paycheckbudget-modal').modal('show');
        self.selectedBudget = item;
    };

    self.addPaycheck = function () {
        $('#edit-paycheckbudget-modal').modal('show');
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