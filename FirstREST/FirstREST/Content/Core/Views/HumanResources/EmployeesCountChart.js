function EmployeesCountChart()
{
}

EmployeesCountChart.display = function (canvasId, initialDate, finalDate, timeInterval)
{
    $.ajax({
        url: 'http://localhost:49822/api/humanResources/employee_count_by_interval',
        type: 'Get',
        data: {
            initialDate: DateUtils.formatDate(initialDate),
            finalDate: DateUtils.formatDate(finalDate),
            timeInterval: timeInterval
        },
        beforeSend: function ()
        {
            $(canvasId).closest("div .box").append("<div class=\"overlay\"><i class=\"fa fa-refresh fa-spin\"></i></div>");
        },
        success: function (data)
        {
            EmployeesCountChart.displayChart(canvasId, data, timeInterval);
            $(canvasId).closest("div .box").children("div .overlay").remove();
        },
        failure: function ()
        {
            alert('Failed to get human resources values');
        }
    });
};
EmployeesCountChart.displayChart = function (canvasId, data, timeInterval)
{

    if (!EmployeesCountChart.chart)
    {
        EmployeesCountChart.chart = new LineChart();
    } else
    {
        EmployeesCountChart.chart.shutdown();
    }

    EmployeesCountChart.chart.initialize();

    for (var i = 0; i < data.length; i++)
    {
        var element = data[i];
        EmployeesCountChart.chart.addValue(DateUtils.formatLabel(element.Date, timeInterval), element.Count);
    }

    EmployeesCountChart.chart.display(canvasId);
};