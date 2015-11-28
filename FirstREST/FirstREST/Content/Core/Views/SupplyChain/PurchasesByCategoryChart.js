function PurchasesByCategoryChart()
{
}

PurchasesByCategoryChart.display = function (canvasId, legendsId, initialDate, finalDate, limit)
{
    $.ajax(
        {
            url: 'http://localhost:49822/api/purchases/purchases_by_category',
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
                PurchasesByCategoryChart.displayChart(canvasId, legendsId, data);
                $(canvasId).closest("div .box").children("div .overlay").remove();
            },
            failure: function() {
                alert('Failed to get purchases values');
            }
        }
    );
};
PurchasesByCategoryChart.displayChart = function (canvasId, legendsId, data)
{

    if (!PurchasesByCategoryChart.chart)
    {
        PurchasesByCategoryChart.chart = new PieChart();
    }
    else {
        PurchasesByCategoryChart.chart.shutdown();
    }

    PurchasesByCategoryChart.chart.initialize(canvasId, legendsId);

    // Add sections:
    for (var i = 0; i < data.length; i++)
    {
        var element = data[i];

        PurchasesByCategoryChart.chart.addSection(element.FamilyDescription, element.Total.toFixed(2));
    }

    // Display:
    PurchasesByCategoryChart.chart.display();
};