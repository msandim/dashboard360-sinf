function Table()
{
}

Table.prototype.initialize = function ()
{
    this.columnLabels = [];
    this.rows = [];
    this.columnLabelStyles = [];
};

Table.prototype.addColumnLabel = function (columnName, labelClass)
{
    this.columnLabels.push(columnName);
    this.columnLabelStyles.push(labelClass);
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

    // If there are any values, add the column labels:
    if (this.rows.length !== 0)
        for (var i = 0; i < this.columnLabels.length; i++)
            header += '<th>' + this.columnLabels[i] + '</th>';

    // If there aren't any values, then just add a message:
    else
        header += "<th>No values to display</td>";
    
    header += '</tr></thead>';
    table.append(header);

    // Body:
    var body = "<tbody>";
    for (var j = 0; j < this.rows.length; j++)
    {
        body += '<tr role="row" class="odd">';
        var row = this.rows[j];
        for (var k = 0; k < row.length; k++) {
            var styleClass = !this.columnLabelStyles[k] ? "" : ' class="' + this.columnLabelStyles[k] + '"';
            body += "<td" + styleClass + ">" + row[k] + "</td>";
        }
            
        body += "</tr>";
    }
    body += "</tbody>";
    table.append(body);
};