function NetSalesChart()
{
}

NetSalesChart.display = function (canvasId, initialDate, finalDate, timeInterval)
{
    $.ajax({
        url: 'http://localhost:49822/api/sales/net_income_by_interval',
        type: 'Get',
        data: {
            initialDate: DateUtils.formatDate(initialDate),
            finalDate: DateUtils.formatDate(finalDate),
            timeInterval: timeInterval
        },
        beforeSend: function()
        {
            $(canvasId).closest("div .box").append("<div class=\"overlay\"><i class=\"fa fa-refresh fa-spin\"></i></div>");
        },
        success: function (data)
        {
            NetSalesChart.displayChart(canvasId, data, timeInterval);
            $(canvasId).closest("div .box").children("div .overlay").remove();
        },
        failure: function ()
        {
            alert('Failed to get sales values');
        }
    });
};
NetSalesChart.displayChart = function (canvasId, data, timeInterval) {

    if (!NetSalesChart.chart) {
        NetSalesChart.chart = new LineChart();
    } else {
        NetSalesChart.chart.shutdown();
    }

    NetSalesChart.chart.initialize();

    for (var i = 0; i < data.length; i++)
    {
        var element = data[i];
        NetSalesChart.chart.addValue(DateUtils.formatLabel(element.Date, timeInterval), element.Total);
    }
     
    NetSalesChart.chart.display(canvasId);
};