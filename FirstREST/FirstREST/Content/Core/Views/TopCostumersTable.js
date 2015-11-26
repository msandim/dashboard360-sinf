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
        success: function (data)
        {
            TopCostumersTable.displayTable(tableId, data);
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

    table.addColumnLabel("ClientId");
    table.addColumnLabel("ClientName");
    table.addColumnLabel("Value");

    for (var i = 0; i < data.length; i++)
        table.addRow(data[i].ClientId, data[i].ClientName, data[i].Total.toFixed(2));

    table.display(tableId);
};