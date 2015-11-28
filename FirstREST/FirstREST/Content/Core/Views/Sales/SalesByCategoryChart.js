function SalesByCategoryChart()
{
}

SalesByCategoryChart.display = function(canvasId, legendsId, initialDate, finalDate, limit) {
    $.ajax(
        {
            url: 'http://localhost:49822/api/sales/sales_by_category',
            type: 'Get',
            data: {
                initialDate: DateUtils.formatDate(initialDate),
                finalDate: DateUtils.formatDate(finalDate),
                limit: limit
            },
            beforeSend: function ()
            {
                $(canvasId).closest("div .box").append("<div class=\"overlay\"><i class=\"fa fa-refresh fa-spin\"></i></div>");
            },
            success: function (data) {
                SalesByCategoryChart.displayChart(canvasId, legendsId, data);
                $(canvasId).closest("div .box").children("div .overlay").remove();
            },
            failure: function() {
                alert('Failed to get sales values');
            }
        }
    );
};
SalesByCategoryChart.displayChart = function (canvasId, legendsId, data) {

    if (!SalesByCategoryChart.chart) {
        SalesByCategoryChart.chart = new PieChart();
    }
    else {
        SalesByCategoryChart.chart.shutdown();
    }

    SalesByCategoryChart.chart.initialize(canvasId, legendsId);

    // Add sections:
    for (var i = 0; i < data.length; i++)
    {
        var element = data[i];

        SalesByCategoryChart.chart.addSection(element.FamilyDescription, element.Total.toFixed(2));
    }

    // Display:
    SalesByCategoryChart.chart.display();
};