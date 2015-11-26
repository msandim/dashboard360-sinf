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
SalesByCategoryChart.displayChart = function (canvasId, data)
{
    // Create pie chart:
    var pieChart = new PieChart();
    pieChart.initialize(canvasId);

    // Add sections:
    for (var i = 0; i < data.length; i++)
    {
        var element = data[i];

        pieChart.addSection(element.FamilyDescription, element.Total.toFixed(2));
    }

    // Display:
    pieChart.display();
};