function AccountsChart() {
}

AccountsChart.displayChart = function (canvasId, legendsId, accounts_receivable, accounts_payable) {
    this.canvasId = canvasId;
    this.accounts_receivable = accounts_receivable;
    this.accounts_payable = accounts_payable;
    this.legendsId = legendsId;

    if (!AccountsChart.chart) {
        AccountsChart.chart = new LineChart();
    } else {
        AccountsChart.chart.shutdown();
    }

    AccountsChart.chart.initialize();

    //Accounts Receivable
    for (var i = 0; i < accounts_receivable.length; i++) {
        var element = accounts_receivable[i];
        AccountsChart.chart.addValue(element.year, element.total, 0);
    }

    //Accounts Payable
    for (var i = 0; i < accounts_payable.length; i++) {
        var element = accounts_payable[i];
        AccountsChart.chart.addValue(element.year, element.total, 1);
    }

    AccountsChart.chart.display(canvasId);

    $(canvasId).get(0).onclick = function (event) {
        var activePoints = AccountsChart.chart.chart.getPointsAtEvent(event);
        if (activePoints[0] != null) {
            var year = activePoints[0].label;
            AccountsChart.onDrillDown(year);
        }
    }
};

AccountsChart.onDrillDown = function (year) {
    AccountsChart.chart.shutdown();
    AccountsChart.chart.initialize();

    for (var i = 0; i < this.accounts_receivable.length; i++) {
        var element_receivable = this.accounts_receivable[i];
        if (element_receivable.year == year) {
            var element_payable = this.accounts_payable[i];
            for (var j = 0; j < element_receivable.months.length; j++) {
                var label = DateUtils.formatLabelYearMonth(year, j);
                var month_receivable = element_receivable.months[j];
                var month_payable = element_payable.months[j];

                AccountsChart.chart.addValue(label, month_receivable, 0);
                AccountsChart.chart.addValue(label, month_payable, 1);
            }

            break;
        }
    }

    //Drill Up
    $(this.canvasId).get(0).onclick = function (event) {
        //AccountsChart.onDrillUp();
        drillUp();
    }

    AccountsChart.chart.display(this.canvasId);
}

AccountsChart.onDrillUp = function () {
    AccountsChart.chart.shutdown();
    AccountsChart.chart.initialize();

    //Accounts Receivable
    for (var i = 0; i < this.accounts_receivable.length; i++) {
        var element = this.accounts_receivable[i];
        AccountsChart.chart.addValue(element.year, element.total, 0);
    }

    //Accounts Payable
    for (var i = 0; i < this.accounts_payable.length; i++) {
        var element = this.accounts_payable[i];
        AccountsChart.chart.addValue(element.year, element.total, 1);
    }


    $(this.canvasId).get(0).onclick = function (event) {
        var activePoints = AccountsChart.chart.chart.getPointsAtEvent(event);
        if (activePoints[0] != null) {
            var year = activePoints[0].label;
            //AccountsChart.onDrillDown(year);
            drillDown(year);
        }
    }

    AccountsChart.chart.display(this.canvasId);
}

AccountsChart.displayLegends = function () {
    $(this.legendsId).append("<li><i class=\"fa fa-square-o\"></i> Accounts Receivable</li>");
    $(this.legendsId).append("<li><i class=\"fa fa-square-o\"></i> Accounts Payable</li>");
};