function NetChart()
{
}

NetChart.getNetPurchasesAsync = function(initialDate, finalDate, timeInterval) {
    $.ajax({
        url: 'http://localhost:49822/api/purchases/net_purchases_by_interval',
        type: 'Get',
        data: {
            initialDate: DateUtils.formatDate(initialDate),
            finalDate: DateUtils.formatDate(finalDate),
            timeInterval: timeInterval
        },
        success: function (data)
        {
            NetChart.setNetPurchases(data);
        },
        failure: function ()
        {
            alert('Failed to get net purchases values');
        }
    });
};
NetChart.setNetPurchases = function (data)
{
    NetChart.netPurchases = data;
    NetChart.netPurchasesReceived = true;
    NetChart.handleValues();
}

NetChart.getNetSalesAsync = function (initialDate, finalDate, timeInterval)
{
    $.ajax({
        url: 'http://localhost:49822/api/sales/net_sales_by_interval',
        type: 'Get',
        data: {
            initialDate: DateUtils.formatDate(initialDate),
            finalDate: DateUtils.formatDate(finalDate),
            timeInterval: timeInterval
        },
        success: function (data)
        {
            NetChart.setNetSales(data);
        },
        failure: function ()
        {
            alert('Failed to get net sales values');
        }
    });
};
NetChart.setNetSales = function (data)
{
    NetChart.netSales = data;
    NetChart.netSalesReceived = true;
    NetChart.handleValues();
};

NetChart.handleValues = function ()
{
    if (NetChart.netSalesReceived && NetChart.netPurchasesReceived) {

        NetChart.addElementsToChart(NetChart.netPurchases, 0);
        NetChart.addElementsToChart(NetChart.netSales, 1);
        NetChart.chart.display(NetChart.canvasId);
        NetChart.chart.displayLegends("Net Purchases");
        NetChart.chart.displayLegends("Net Sales");
    }
};

NetChart.addElementsToChart = function (data, dataset)
{
    for (var i = 0; i < data.length; i++)
    {
        var element = data[i];
        NetChart.chart.addValue(DateUtils.formatLabel(element.Date, NetChart.timeInterval), element.Total, dataset);
    }
}

NetChart.displayChartAsync = function (initialDate, finalDate, timeInterval)
{
    NetChart.netPurchasesReceived = false;
    NetChart.netSalesReceived = false;
    
    NetChart.getNetPurchasesAsync(initialDate, finalDate, timeInterval);
    NetChart.getNetSalesAsync(initialDate, finalDate, timeInterval);
}

NetChart.displayChart = function (canvasId, legendsId, initialDate, finalDate, timeInterval)
{
    NetChart.canvasId = canvasId;
    NetChart.timeInterval = timeInterval;

    if (!NetChart.chart)
    {
        NetChart.chart = new LineChart();
    }
    else
    {
        NetChart.chart.shutdown();
    }

    NetChart.chart.initialize(legendsId);
    NetChart.displayChartAsync(initialDate, finalDate, timeInterval);
}