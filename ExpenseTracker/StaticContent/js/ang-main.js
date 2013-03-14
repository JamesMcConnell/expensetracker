var defaultBudget = {
    id: null,
    date: null,
    amount: null,
    budgetItems: new Array()
};

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
            var tracker = 0;
            var getSources = function () {
                var equalsTracker = scope.$eval(attrs.equalsTracker);
                tracker = 0;
                angular.forEach(sources, function (value, key) {
                    if (angular.isArray(value)) {
                        tracker += value.length;
                    }
                });
                if (angular.isNumber(equalsTracker)) {
                    return tracker + sources.length + equalsTracker;
                } else {
                    return tracker + sources.length;
                }
            };
            function update() {
                scope.calendar = element.html('');
                var view = scope.calendar.fullCalendar('getView');
                if (view) {
                    view = view.name;
                }

                var expression, options = { defaultView: view, events: sources };
                if (attrs.pbCalendar) {
                    expression = scope.$eval(attrs.uiCalendar);
                }
                else {
                    expression = {};
                }
                angular.extend(options, baseOptions, expression);
                scope.calendar.fullCalendar(options);
            }
            update();
            scope.$watch(getSources, function (newVal, oldVal) {
                update();
            });
        }
    };
});

app.factory('paycheckBudgetService', function ($rootScope, $http) {
    var service = {};
    service.currentBudget = {};

    service.prepForBroadcast = function (budget) {
        this.currentBudget = budget;
        this.broadcastItem();
    };

    service.broadcastItem = function () {
        $rootScope.$broadcast('paycheckBudgetSelected');
    };

    return service;
});

var CalendarCtrl = function ($scope, pbService) {
    var date = new Date();
    var d = date.getDate();
    var m = date.getMonth();
    var y = date.getFullYear();

    $scope.events = [];

    $scope.$on('paycheckBudgetSelected', function () {
        $scope.events.push({
            title: 'Payday - ' + pbService.currentBudget.amount,
            allDay: true,
            start: pbService.currentBudget.date,
            editable: false,
            textColor: '#3a87ad',
            backgroundColor: '#d9edf7',
            borderColor: '#bce8f1'
        });

        angular.forEach(pbService.currentBudget.budgetItems, function (value, key) {
            $scope.events.push({
                title: value.description + ' - ' + value.amount,
                allDay: true,
                start: value.dueDate,
                editable: false
            });
        });
    });
}

var PaycheckBudgetCtrl = function ($scope, $http, pbService) {
    $scope.selectedBudget = undefined;

    $http({ method: 'GET', url: 'api/paycheckbudget' }).success(function (data, status) {
        $scope.budgets = data;
        pbService.prepForBroadcast($scope.budgets[0]);
    });

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

PaycheckBudgetCtrl.$inject = ['$scope', '$http', 'paycheckBudgetService'];
CalendarCtrl.$inject = ['$scope', 'paycheckBudgetService'];