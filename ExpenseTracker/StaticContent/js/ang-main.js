angular.module('expenseTracker', ['ui.bootstrap']);
var PaycheckBudgetCtrl = function ($scope, $http) {
    $http({ method: 'GET', url: 'api/paycheckbudget' }).success(function (data, status) {
        $scope.budgets = data;
    });

    $scope.editBudget = function (budget) {
        $scope.selectedBudget = budget;
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

    $scope.updateStatus = function (budgetItem) {
        console.log(budgetItem);
    };

    $scope.isPaid = function (budgetItem) {
        return budgetItem.isPaid;
    };

    $scope.closeModal = function () {
        $scope.shouldBeOpen = false;
    };

    $scope.submitData = function () {
        if (!$scope.isEditing) {
            $http.post('api/paycheckbudget', $scope.selectedBudget).success(function (data, status) {
                $scope.budgets = data;
            });
        } else {
            $http.put('api/paycheckbudget/' + $scope.selectedBudget.id, $scope.selectedBudget).success(function (data, status) {
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