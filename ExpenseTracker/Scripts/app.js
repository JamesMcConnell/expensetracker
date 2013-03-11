var app = app || {};
app.utilities = (function (app) {
	"use strict";
	var formatCurrency = function (value) {
		return '$' + value.toFixed(2);
	};

	var formatDate = function (value) {
		var d = Date.parse(value);
		return d.toString("M/d/yyyy");
	};

	return {
		formatCurrency: formatCurrency,
		formatDate: formatDate
	};
})(app);