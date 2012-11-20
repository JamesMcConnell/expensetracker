﻿var app = app || {};
app.dataservice = (function (app) {
    "use strict";
    var getAllPaycheckBudgets = function (callback) {
        $.getJSON('/api/paycheckbudget', function (data) {
            callback(data);
        });
    };

    var updatePaycheckBudgetItem = function (id, data) {
        var url = '/api/paycheckbudgetitem/' + id;
        $.ajax({
            url: url,
            type: 'put',
            data: data,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    };

    var submitPaycheckBudget = function (data, success, error) {
        $.ajax({
            url: '/api/paycheckbudget',
            contentType: 'application/json; charset=utf-8',
            type: 'post',
            data: data,
            success: function (result) {
                success(result);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                error(xhr);
            }
        });
    };
    return {
        getAllPaycheckBudgets: getAllPaycheckBudgets,
        updatePaycheckBudgetItem: updatePaycheckBudgetItem,
        submitPaycheckBudget: submitPaycheckBudget
    };
})(app);