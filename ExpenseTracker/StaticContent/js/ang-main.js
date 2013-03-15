function copyBudget(budget) {
    var budgetCopy = {
        id: budget.id,
        date: budget.date,
        amount: parseInt(budget.amount),
        budgetItems: new Array()
    };

    for (var i = 0; i < budget.budgetItems.length; i++) {
        budgetCopy.budgetItems.push({
            description: budget.budgetItems[i].description,
            amount: parseInt(budget.budgetItems[i].amount),
            dueDate: budget.budgetItems[i].dueDate,
            isPaid: budget.budgetItems[i].isPaid,
            datePaid: budget.budgetItems[i].datePaid
        });
    }

    return budgetCopy;
}

function prepBudgets(budgets) {
    for (var i = 0; i < budgets.length; i++) {
        budgets[i] = prepBudget(budgets[i]);
    }

    return budgets;
}

function prepBudget(budget) {
    var itemTotal = 0;
    for (var i = 0; i < budget.budgetItems.length; i++) {
        var budgetItem = budget.budgetItems[i];
        itemTotal += parseInt(budgetItem.amount);
    }
    budget.remaining = parseInt(budget.amount) - itemTotal;
    return budget;
}

var app = angular.module('expenseTracker', ['ui', 'ui.bootstrap']);
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

app.directive('pbCalendar', function ($parse) {
    return {
        require: 'ngModel',
        restrict: 'A',
        link: function (scope, element, attrs) {
            var baseOptions = {
                theme: true,
                header: {
                    left: 'prev,next today',
                    center: 'title',
                    right: 'month,agendaWeek,agendaDay'
                }
            };
            var sources = scope.$eval(attrs.ngModel);
            scope.calendar = element.html('');
            var view = scope.calendar.fullCalendar('getView');
            if (view) {
                view = view.name;
            }

            var expression, options = { defaultView: view, events: sources };
            if (attrs.pbCalendar) {
                expression = scope.$eval(attrs.uiCalendar);
            } else {
                expression = {};
            }
            angular.extend(options, baseOptions, expression);
            scope.calendar.fullCalendar(options);

            scope.$on('reloadEvents', function () {
                scope.calendar.fullCalendar('refetchEvents');
            });
        }
    };
});

app.factory('paycheckBudgetService', function ($rootScope, $http) {
    var service = {};
    service.budgets = [];

    service.reloadCalendarEvents = function () {
        $rootScope.$broadcast('reloadEvents');
    };

    return service;
});

var CalendarCtrl = function ($scope, pbService) {
    $scope.eventSource = 'api/paycheckbudget/GetBudgetItemsAsCalendarEvents';
}

var PaycheckBudgetCtrl = function ($scope, $http, pbService) {
    $scope.selectedBudget = undefined;
    $scope.hasData = false;

    $http({ method: 'GET', url: 'api/paycheckbudget/getcurrentbudgets' }).success(function (data, status) {
        $scope.budgets = prepBudgets(data.budgets);
        $scope.pagingInfo = data.pagingInfo;
    });

    /** Click events **/
    $scope.editBudget = function (budget) {
        $scope.selectedBudget = copyBudget(budget);
        $scope.shouldBeOpen = true;
        $scope.isEditing = true;
    };

    $scope.addBudget = function () {
        $scope.selectedBudget = {
            id: null,
            date: null,
            amount: null,
            budgetItems: new Array()
        };
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

        budget = prepBudget(budget);
    };

    $scope.updateStatus = function (budget, budgetItem) {
        budgetItem.isPaid = true;
        budgetItem.datePaid = new Date();
        $scope.isEditing = true;
        $scope.submitData(budget);
    };

    $scope.pagePrevious = function () {
        if ($scope.pagingInfo.hasPrevious) {
            var budget = $scope.budgets[0];
            $http.post('api/paycheckbudget/getpreviousbudgets', budget).success(function (data, status) {
                $scope.budgets = prepBudgets(data.budgets);
                $scope.pagingInfo = data.pagingInfo;
            });
        }
    };

    $scope.pageNext = function () {
        if ($scope.pagingInfo.hasFuture) {
            var budget = $scope.budgets[1];
            $http.post('api/paycheckbudget/getfuturebudgets', budget).success(function (data, status) {
                $scope.budgets = prepBudgets(data.budgets);
                $scope.pagingInfo = data.pagingInfo;
            });
        }
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
            $http.post('api/paycheckbudget/addbudget', budget).success(function (data, status) {
                $scope.budgets = prepBudgets(data.budgets);
                $scope.pagingInfo = data.pagingInfo;
                pbService.reloadCalendarEvents();
            });
        } else {
            $http.put('api/paycheckbudget/updatebudget/' + budget.id, budget).success(function (data, status) {
                $scope.updateBudget(budget);
                pbService.reloadCalendarEvents();
            });
        }

        $scope.shouldBeOpen = false;
        $scope.selectedBudget = undefined;
    };

    $scope.updateBudget = function (budget) {
        for (var i = 0; i < $scope.budgets.length; i++) {
            if ($scope.budgets[i].id == budget.id) {
                $scope.budgets[i] = prepBudget(budget);
            }
        }
    };

    $scope.opts = {
        backdropFade: true,
        dialogFade: true
    };
}

PaycheckBudgetCtrl.$inject = ['$scope', '$http', 'paycheckBudgetService'];
CalendarCtrl.$inject = ['$scope', 'paycheckBudgetService'];