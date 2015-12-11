function CashChart() {
}

CashChart.displayChart = function (canvasId, data) {
    this.canvasId = canvasId;
    this.data = data;

    if (!CashChart.chart) {
        CashChart.chart = new LineChart();
    } else {
        CashChart.chart.shutdown();
    }

    CashChart.chart.initialize();

    for (var i = 0; i < data.length; i++) {
        var element = data[i];
        CashChart.chart.addValue(element.year, element.total);
    }

    CashChart.chart.display(canvasId);

    $(canvasId).get(0).onclick = function (event) {
        var activePoints = CashChart.chart.chart.getPointsAtEvent(event);
        if (activePoints[0] != null) {
            var year = activePoints[0].label;
            //CashChart.onDrillDown(year);
            drillDown(year);
        }
    }
};

CashChart.onDrillDown = function (year) {
    CashChart.chart.shutdown();
    CashChart.chart.initialize();

    for (var i = 0; i < this.data.length; i++) {
        var element = this.data[i];
        if (element.year == year) {
            for (var j = 0; j < element.months.length; j++) {
                var label = DateUtils.formatLabelYearMonth(year, j);
                var month = element.months[j];

                CashChart.chart.addValue(label, month);
            }
            break;
        }
    }
    
    //Drill Up
    $(this.canvasId).get(0).onclick = function (event) {
        //CashChart.onDrillUp();
        drillUp();
    }

    CashChart.chart.display(this.canvasId);
}

CashChart.onDrillUp = function () {
    CashChart.chart.shutdown();
    CashChart.chart.initialize();

    for (var i = 0; i < this.data.length; i++) {
        var element = this.data[i];
        CashChart.chart.addValue(element.year, element.total);
    }

    $(this.canvasId).get(0).onclick = function (event) {
        var activePoints = CashChart.chart.chart.getPointsAtEvent(event);
        if (activePoints[0] != null) {
            var year = activePoints[0].label;
            //CashChart.onDrillDown(year);
            drillDown(year);
        }
    }

    CashChart.chart.display(this.canvasId);
}