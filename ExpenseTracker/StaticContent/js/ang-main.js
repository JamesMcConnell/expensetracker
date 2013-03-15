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
            isPaid: budget.budgetItems[i].isPaid,
            datePaid: budget.budgetItems[i].datePaid
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
    service.budgets = [];

    service.prepForBroadcast = function (budgets) {
        this.budgets = budgets;
        this.broadcastItem();
    };

    service.broadcastItem = function () {
        $rootScope.$broadcast('budgetsLoaded');
    };

    return service;
});

var CalendarCtrl = function ($scope, pbService) {
    $scope.events = [];
    $scope.eventExists = function (newEvent) {
        angular.forEach($scope.events, function (value, key) {
            if (value.title == newEvent.title) {
                return true;
            }
        });

        return false;
    };

    $scope.$on('budgetsLoaded', function () {
        angular.forEach(pbService.budgets, function (budget, key) {
            var paydayEvent = {
                title: 'Payday - ' + budget.amount,
                allDay: true,
                start: budget.date,
                editable: false,
                textColor: '#3a87ad',
                backgroundColor: '#d9edf7',
                borderColor: '#bce8f1'
            };

            var paydayEventExists = $scope.eventExists(paydayEvent);
            if (!paydayEventExists) {
                $scope.events.push(paydayEvent);
            }

            angular.forEach(budget.budgetItems, function (budgetItem, key) {
                var itemEvent = {
                    title: budgetItem.description + ' - ' + budgetItem.amount,
                    allDay: true,
                    start: budgetItem.dueDate,
                    editable: false
                };

                var itemEventExists = $scope.eventExists(itemEvent);
                if (!itemEventExists) {
                    $scope.events.push(itemEvent);
                }
            });
        });
    });
}

var PaycheckBudgetCtrl = function ($scope, $http, pbService) {
    $scope.selectedBudget = undefined;
    $scope.hasData = false;

    $http({ method: 'GET', url: 'api/paycheckbudget' }).success(function (data, status) {
        $scope.budgets = data;
        $scope.hasData = true;
        if ($scope.budgets.length > 0) {
            pbService.prepForBroadcast($scope.budgets);
        }
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
    };

    $scope.updateStatus = function (budget, budgetItem) {
        budgetItem.isPaid = true;
        budgetItem.datePaid = new Date();
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
            $http.post('api/paycheckbudget', budget);
            $scope.budgets.push(budget);
        } else {
            $http.put('api/paycheckbudget/' + budget.id, budget);
            $scope.updateBudget(budget);
        }
        $scope.shouldBeOpen = false;
        $scope.selectedBudget = undefined;
        pbService.prepForBroadcast($scope.budgets);
    };

    $scope.updateBudget = function (budget) {
        for (var i = 0; i < $scope.budgets; i++) {
            if ($scope.budgets[i].id == budget.id) {
                $scope.budgets[i] = budget;
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