function SuppliesVsSalesChart() {
}

SuppliesVsSalesChart.displayChart = function (canvasId, legendsId, supplies, sales) {
    this.canvasId = canvasId;
    this.supplies = supplies;
    this.sales = sales;
    this.legendsId = legendsId;

    if (!SuppliesVsSalesChart.chart) {
        SuppliesVsSalesChart.chart = new LineChart();
    } else {
        SuppliesVsSalesChart.chart.shutdown();
    }

    SuppliesVsSalesChart.chart.initialize();

    //Supplies
    for (var i = 0; i < supplies.length; i++) {
        var element = supplies[i];
        SuppliesVsSalesChart.chart.addValue(element.year, element.total, 0);
    }

    //Sales
    for (var i = 0; i < sales.length; i++) {
        var element = sales[i];
        SuppliesVsSalesChart.chart.addValue(element.year, element.total, 1);
    }

    SuppliesVsSalesChart.chart.display(canvasId);

    $(canvasId).get(0).onclick = function (event) {
        var activePoints = SuppliesVsSalesChart.chart.chart.getPointsAtEvent(event);
        if (activePoints[0] != null) {
            var year = activePoints[0].label;
            //SuppliesVsSalesChart.onDrillDown(year);
            drillDown(year);
        }
    }
};

SuppliesVsSalesChart.onDrillDown = function (year) {
    SuppliesVsSalesChart.chart.shutdown();
    SuppliesVsSalesChart.chart.initialize();

    for (var i = 0; i < this.supplies.length; i++) {
        var element_supplies = this.supplies[i];
        if (element_supplies.year == year) {
            var element_sales = this.sales[i];
            for (var j = 0; j < element_supplies.months.length; j++) {
                var label = DateUtils.formatLabelYearMonth(year, j);
                var month_supplies = element_supplies.months[j];
                var month_sales = element_sales.months[j];

                SuppliesVsSalesChart.chart.addValue(label, month_supplies, 0);
                SuppliesVsSalesChart.chart.addValue(label, month_sales, 1);
            }

            break;
        }
    }

    //Drill Up
    $(this.canvasId).get(0).onclick = function (event) {
        //SuppliesVsSalesChart.onDrillUp();
        drillUp();
    }

    SuppliesVsSalesChart.chart.display(this.canvasId);
}

SuppliesVsSalesChart.onDrillUp = function () {
    SuppliesVsSalesChart.chart.shutdown();
    SuppliesVsSalesChart.chart.initialize();

    //Supplies
    for (var i = 0; i < this.supplies.length; i++) {
        var element = this.supplies[i];
        SuppliesVsSalesChart.chart.addValue(element.year, element.total, 0);
    }

    //Sales
    for (var i = 0; i < this.sales.length; i++) {
        var element = this.sales[i];
        SuppliesVsSalesChart.chart.addValue(element.year, element.total, 1);
    }


    $(this.canvasId).get(0).onclick = function (event) {
        var activePoints = SuppliesVsSalesChart.chart.chart.getPointsAtEvent(event);
        if (activePoints[0] != null) {
            var year = activePoints[0].label;
            //SuppliesVsSalesChart.onDrillDown(year);
            drillDown(year);
        }
    }

    SuppliesVsSalesChart.chart.display(this.canvasId);
}

SuppliesVsSalesChart.displayLegends = function () {
    $(this.legendsId).append("<li><i class=\"fa fa-square-o\"></i> Supplies Cost</li>");
    $(this.legendsId).append("<li><i class=\"fa fa-square-o\"></i> Sales</li>");
};