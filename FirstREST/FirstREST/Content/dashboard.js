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
    // TODO
    for (var i = 0; i < canvasIds.length; i++)
    {
        $("#" + canvasIds[i]).closest("div .box").append("<div class=\"overlay\"><i class=\"fa fa-refresh fa-spin\"></i></div>");
    }
}

function removeRefreshSpin(canvasIds)
{
    // TODO
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

function ready()
{

    var initialDate = moment().subtract(1, 'month').startOf('month');
    var finalDate = moment().subtract(1, 'month').endOf('month');

    getButtonValuesAsync(initialDate, finalDate);
}