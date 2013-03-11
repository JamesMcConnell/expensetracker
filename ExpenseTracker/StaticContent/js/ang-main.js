var app = angular.module('ExpenseTracker', []);
app.controller('PaycheckBudgetCtrl', function ($scope, $http) {
    $scope.budgets = [{
        id: 1,
        date: "3/1/2013",
        amount: 2350,
        budgetItems: [{
            itemId: 1,
            budgetId: 1,
            description: "Home Choice",
            amount: 100,
            paidDate: "3/5/2013",
            isPaid: false
        },
        {
            itemId: 3,
            budgetId: 1,
            description: "Bug's Tuition",
            amount: 275,
            paidDate: "3/5/2013",
            isPaid: false
        },
        {
            itemId: 4,
            budgetId: 1,
            description: "Rent",
            amount: 800,
            paidDate: "3/5/2013",
            isPaid: true
        }],
        remaining: 1175
    }];

//    $http({ method: 'GET', url: 'api/paycheckbudget' }).success(function (data, status) {
//        console.log(JSON.stringify(data));
//        $scope.budgets = data;
//    });

    $scope.editBudget = function (budget) {
        $scope.selectedBudget = budget;
    }

    $scope.updateStatus = function (budgetItem) {
        console.log(budgetItem);
    }

    $scope.isPaid = function (budgetItem) {
        return budgetItem.isPaid;
    }

    $scope.formatCurrency = function (amount) {
        return '$' + amount.toFixed(2);
    }
});