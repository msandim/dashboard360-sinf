$(document).ready(ready);

function setButtonValues(data)
{
    $("#payables_value").text(CurrencyUtils.format(data.Payables, data.Currency));
    $("#receivables_value").text(CurrencyUtils.format(data.Receivables, data.Currency));
    $("#gross_purchases_value").text(CurrencyUtils.format(data.GrossPurchases, data.Currency));
    $("#gross_sales_value").text(CurrencyUtils.format(data.GrossSales, data.Currency));
    $("#net_purchases_value").text(CurrencyUtils.format(data.NetPurchases, data.Currency));
    $("#net_sales_value").text(CurrencyUtils.format(data.NetSales, data.Currency));
    $("#human_resources_value").text(CurrencyUtils.format(data.LaborCostValue, data.Currency));
}

function addRefreshSpin(canvasIds)
{
    for (var i = 0; i < canvasIds.length; i++)
    {
        $("#" + canvasIds[i]).closest("div .small-box").append("<div class=\"overlay\"><i class=\"fa fa-refresh fa-spin\"></i></div>");
    }
}

function removeRefreshSpin(canvasIds)
{
    for (var i = 0; i < canvasIds.length; i++)
    {
        $("#" + canvasIds[i]).closest("div .small-box").children("div .overlay").remove();
    }
}

function getButtonValuesAsync(initialDate, finalDate)
{
    var canvasIds = [
    "payables_value", "receivables_value", "gross_purchases_value", "gross_sales_value",
    "net_purchases_value", "net_sales_value", "human_resources_value"
    ];

    $.ajax(
        {
            url: 'http://localhost:49822/api/dashboard/button_values',
            type: 'Get',
            data: {
                initialDate: DateUtils.formatDate(initialDate),
                finalDate: DateUtils.formatDate(finalDate)
            }, 
            beforeSend: function ()
            {
                addRefreshSpin(canvasIds);
            },
            success: function (data) {
                setButtonValues(data);
                removeRefreshSpin(canvasIds);
            },
            failure: function ()
            {
                alert('Failed to get button values');
            }
        }
    );
}

function initializeDatePicker(initialDate, finalDate)
{
    var datePicker = new DatePicker();
    datePicker.initialize(
        "#daterange-btn",
        function (start, end)
        {
            getButtonValuesAsync(start, end);
            initializeNetChart(start, end);
        },
        initialDate,
        finalDate,
        DatePicker.defaultRange1
    );
}

function initializeNetChart(initialDate, finalDate)
{
    NetChart.displayChart("#net_chart", "#net_legend", initialDate, finalDate, "month");
};

function ready()
{
    var initialDate = DatePicker.defaultDate1[0];
    var finalDate = DatePicker.defaultDate1[1];

    getButtonValuesAsync(initialDate, finalDate);
    initializeDatePicker(initialDate, finalDate);
    initializeNetChart(initialDate, finalDate);
}