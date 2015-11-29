function TopAbsencesChart() {
}

TopAbsencesChart.display = function (tableId, initialDate, finalDate, limit) {
    $.ajax({
        url: 'http://localhost:49822/api/humanResources/absence_count',
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
            TopAbsencesChart.displayTable(tableId, data);
            $(tableId).closest("div .box").children("div .overlay").remove();
        },
        failure: function () {
            alert('Failed to get absences values');
        }
    });
};
TopAbsencesChart.displayTable = function (tableId, data) {
    var table = new Table();
    table.initialize();

    table.addColumnLabel("Employee ID");
    table.addColumnLabel("Employee Name");
    table.addColumnLabel("Value");

    for (var i = 0; i < data.length; i++)
        table.addRow(data[i].EmployeeId, data[i].EmployeeName, data[i].Count);

    table.display(tableId);
};