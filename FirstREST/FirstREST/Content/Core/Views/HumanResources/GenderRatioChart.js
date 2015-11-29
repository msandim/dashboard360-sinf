function GenderRatioChart() {
}

GenderRatioChart.display = function (canvasId, legendsId, initialDate, finalDate) {
    $.ajax(
        {
            url: 'http://localhost:49822/api/humanResources/gender_count',
            type: 'Get',
            data: {
                initialDate: DateUtils.formatDate(initialDate),
                finalDate: DateUtils.formatDate(finalDate),
            },
            beforeSend: function () {
                $(canvasId).closest("div .box").append("<div class=\"overlay\"><i class=\"fa fa-refresh fa-spin\"></i></div>");
            },
            success: function (data) {
                GenderRatioChart.displayChart(canvasId, legendsId, data);
                $(canvasId).closest("div .box").children("div .overlay").remove();
            },
            failure: function () {
                alert('Failed to get gender values');
            }
        }
    );
};
GenderRatioChart.displayChart = function (canvasId, legendsId, data) {

    if (!GenderRatioChart.chart) {
        GenderRatioChart.chart = new PieChart();
    }
    else {
        GenderRatioChart.chart.shutdown();
    }

    GenderRatioChart.chart.initialize(canvasId, legendsId);

    // Add sections:
    GenderRatioChart.chart.addSection("Male", data.Male);
    GenderRatioChart.chart.addSection("Female", data.Female);

    // Display:
    GenderRatioChart.chart.display();
};