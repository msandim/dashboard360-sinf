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
        success: function (data)
        {
            NetSalesChart.displayChart(canvasId, data);
        },
        failure: function ()
        {
            alert('Failed to get sales values');
        }
    });
};
NetSalesChart.displayChart = function (canvasId, data)
{
    var chart = new LineChart();
    chart.initialize();

    for (var i = 0; i < data.length; i++)
    {
        var element = data[i];
        chart.addValue(DateUtils.formatDate(element.InitialDate), element.Total);
    }

    chart.display(canvasId);
};