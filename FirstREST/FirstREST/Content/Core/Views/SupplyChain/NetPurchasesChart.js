function NetPurchasesChart()
{
}

NetPurchasesChart.display = function (canvasId, initialDate, finalDate, timeInterval)
{
    $.ajax({
        url: 'http://localhost:49822/api/purchases/net_purchases_by_interval',
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
            NetPurchasesChart.displayChart(canvasId, data, timeInterval);
            $(canvasId).closest("div .box").children("div .overlay").remove();
        },
        failure: function ()
        {
            alert('Failed to get sales values');
        }
    });
};
NetPurchasesChart.displayChart = function (canvasId, data, timeInterval)
{

    if (!NetPurchasesChart.chart)
    {
        NetPurchasesChart.chart = new LineChart();
    } else {
        NetPurchasesChart.chart.shutdown();
    }

    NetPurchasesChart.chart.initialize();

    for (var i = 0; i < data.length; i++)
    {
        var element = data[i];
        NetPurchasesChart.chart.addValue(DateUtils.formatLabel(element.Date, timeInterval), element.Total);
    }
     
    NetPurchasesChart.chart.display(canvasId);
};