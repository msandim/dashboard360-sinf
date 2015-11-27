function SalesByCategoryChart()
{
}

SalesByCategoryChart.display = function(canvasId, initialDate, finalDate, limit) {
    $.ajax(
        {
            url: 'http://localhost:49822/api/sales/sales_by_category',
            type: 'Get',
            data: {
                initialDate: DateUtils.formatDate(initialDate),
                finalDate: DateUtils.formatDate(finalDate),
                limit: limit
            },
            success: function (data) {
                SalesByCategoryChart.displayChart(canvasId, data);
            },
            failure: function() {
                alert('Failed to get sales values');
            }
        }
    );
};
SalesByCategoryChart.displayChart = function (canvasId, data) {

    if (!SalesByCategoryChart.chart) {
        SalesByCategoryChart.chart = new PieChart();
    }
    else {
        SalesByCategoryChart.chart.shutdown();
    }

    SalesByCategoryChart.chart.initialize(canvasId);

    // Add sections:
    for (var i = 0; i < data.length; i++)
    {
        var element = data[i];

        SalesByCategoryChart.chart.addSection(element.FamilyDescription, element.Total.toFixed(2));
    }

    // Display:
    SalesByCategoryChart.chart.display();
};