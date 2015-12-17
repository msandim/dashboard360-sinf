function TopProductsTable() {
}

TopProductsTable.display = function (tableId, initialDate, finalDate, limit) {
    $.ajax({
        url: 'http://localhost:49822/api/sales/top_products',
        type: 'Get',
        data: {
            initialDate: DateUtils.formatDate(initialDate),
            finalDate: DateUtils.formatDate(finalDate),
            limit: limit
        },
        beforeSend: function () {
            $(tableId).closest("div .box").append("<div class=\"overlay\"><i class=\"fa fa-refresh fa-spin\"></i></div>");
        },
        success: function (data) {
            TopProductsTable.displayTable(tableId, data);
            $(tableId).closest("div .box").children("div .overlay").remove();
        },
        failure: function () {
            alert('Failed to get sales values');
        }
    });
};
TopProductsTable.displayTable = function (tableId, data) {
    var table = new Table();
    table.initialize();

    table.addColumnLabel("Product ID");
    table.addColumnLabel("Product Name");
    table.addColumnLabel("Product Family");
    table.addColumnLabel("Value", "small_label");

    for (var i = 0; i < data.length; i++)
        table.addRow(data[i].ProductId, data[i].ProductName, data[i].ProductFamily, CurrencyUtils.format(data[i].Total, "EUR"));

    table.display(tableId);
};