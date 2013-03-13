function copyBudget(budget) {
    var budgetCopy = {
        id: budget.id,
        date: budget.date,
        amount: budget.amount,
        budgetItems: new Array()
    };

    for (var i = 0; i < budget.budgetItems.length; i++) {
        budgetCopy.budgetItems.push({
            description: budget.budgetItems[i].description,
            amount: budget.budgetItems[i].amount,
            dueDate: budget.budgetItems[i].dueDate,
            isPaid: budget.budgetItems[i].isPaid
        });
    }

    return budgetCopy;
}

var app = angular.module('expenseTracker', ['ui.bootstrap']);
app.directive('datepicker', function ($parse) {
    return function (scope, element, attrs, controller) {
        var ngModel = $parse(attrs.ngModel);
        $(function () {
            element.datepicker({
                dateFormat: 'MM dd, yy',
                onSelect: function (dateText, inst) {
                    scope.$apply(function (scope) {
                        ngModel.assign(scope, dateText);
                    });
                }
            });
        });
    }
});
var PaycheckBudgetCtrl = function ($scope, $http) {
    $http({ method: 'GET', url: 'api/paycheckbudget' }).success(function (data, status) {
        $scope.budgets = data;
    });

    $scope.selectedBudget = undefined;

    /** Click events **/
    $scope.editBudget = function (budget) {
        $scope.selectedBudget = copyBudget(budget);
        $scope.shouldBeOpen = true;
        $scope.isEditing = true;
    };

    $scope.addBudget = function () {
        $scope.selectedBudget = defaultBudget;
        $scope.shouldBeOpen = true;
        $scope.isEditing = false;
    };

    $scope.addBudgetItem = function (budget) {
        budget.budgetItems.push({});
    };

    $scope.removeBudgetItem = function (budget, item) {
        for (var i in budget.budgetItems) {
            if (budget.budgetItems[i] == item) {
                budget.budgetItems.splice(i, 1);
                break;
            }
        }
    };

    $scope.updateStatus = function (budget, budgetItem) {
        budgetItem.isPaid = true;
        $scope.isEditing = true;
        $scope.submitData(budget);
    };

    $scope.pagePrevious = function () {
        var firstBudget = $scope.budgets[0];

    };

    $scope.pageNext = function () {
        var lastBudget = $scope.budgets[$scope.budgets.length - 1];
    };

    /** Click events **/

    $scope.isPaid = function (budgetItem) {
        return budgetItem.isPaid;
    };

    $scope.closeModal = function () {
        $scope.shouldBeOpen = false;
    };

    $scope.submitData = function (budget) {
        if (!$scope.isEditing) {
            $http.post('api/paycheckbudget', budget).success(function (data, status) {
                $scope.budgets = data;
            });
        } else {
            $http.put('api/paycheckbudget/' + budget.id, budget).success(function (data, status) {
                $scope.budgets = data;
            });
        }
        $scope.shouldBeOpen = false;
    };

    $scope.opts = {
        backdropFade: true,
        dialogFade: true
    };
}

var defaultBudget = {
    id: null,
    date: null,
    amount: null,
    budgetItems: new Array()
};

//    $scope.budgets = [{
//        id: 1,
//        date: "3/1/2013",
//        amount: 2350,
//        budgetItems: [{
//            itemId: 1,
//            budgetId: 1,
//            description: "Home Choice",
//            amount: 100,
//            paidDate: "3/5/2013",
//            isPaid: false
//        },
//        {
//            itemId: 3,
//            budgetId: 1,
//            description: "Bug's Tuition",
//            amount: 275,
//            paidDate: "3/5/2013",
//            isPaid: false
//        },
//        {
//            itemId: 4,
//            budgetId: 1,
//            description: "Rent",
//            amount: 800,
//            paidDate: "3/5/2013",
//            isPaid: true
//        }],
//        remaining: 1175
//    }];