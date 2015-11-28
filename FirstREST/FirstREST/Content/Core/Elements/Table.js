function Table()
{
}

Table.prototype.initialize = function ()
{
    this.columnLabels = [];
    this.rows = [];
};

Table.prototype.addColumnLabel = function (columnName)
{
    this.columnLabels.push(columnName);
};

Table.prototype.addRow = function ()
{
    // Create an empty row:
    var row = [];

    // Add all arguments to the row:
    var args = arguments;
    for (var i = 0; i < args.length; i++)
        row.push(args[i]);

    // Add row to the array:
    this.rows.push(row);
};

Table.prototype.display = function (tableId)
{
    var table = $(tableId);
    table.html('');

    // Header:
    var header = '<thead><tr role="row">';
    for (var i = 0; i < this.columnLabels.length; i++)
        header += '<th>' + this.columnLabels[i] + '</th>';
    header += '</tr></thead>';
    table.append(header);

    // Body:
    var body = '<tbody>';
    for (var j = 0; j < this.rows.length; j++)
    {
        body += '<tr role="row" class="odd">';
        var row = this.rows[j];
        for (var k = 0; k < row.length; k++)
            body += '<td>' + row[k] + '</td>';
        body += '</tr>';
    }
    body += '</tbody>';
    table.append(body);
};