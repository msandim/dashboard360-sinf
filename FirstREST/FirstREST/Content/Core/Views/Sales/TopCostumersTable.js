function TopCostumersTable()
{
}

TopCostumersTable.display = function (tableId, initialDate, finalDate, limit)
{
    $.ajax({
        url: 'http://localhost:49822/api/sales/top_costumers',
        type: 'Get',
        data: {
            initialDate: DateUtils.formatDate(initialDate),
            finalDate: DateUtils.formatDate(finalDate),
            limit: limit
        },
        beforeSend: function ()
        {
            $(tableId).closest("div .box").append("<div class=\"overlay\"><i class=\"fa fa-refresh fa-spin\"></i></div>");
        },
        success: function (data)
        {
            TopCostumersTable.displayTable(tableId, data);
            $(tableId).closest("div .box").children("div .overlay").remove();
        },
        failure: function ()
        {
            alert('Failed to get sales values');
        }
    });
};
TopCostumersTable.displayTable = function (tableId, data)
{
    var table = new Table();
    table.initialize();

    table.addColumnLabel("Client ID");
    table.addColumnLabel("Client Name");
    table.addColumnLabel("Total Value", "small_label");

    for (var i = 0; i < data.length; i++)
        table.addRow(data[i].ClientId, data[i].ClientName, CurrencyUtils.format(data[i].Total, "EUR"));

    table.display(tableId);
};