function BalanceSheetTable()
{
}

BalanceSheetTable.initialize = function (tableId, data)
{
    this.tableId = tableId;
    this.state = 1;
    this.total_non_current_assets = data.metrics.total_non_current_assets.years_data;
    this.total_current_assets = data.metrics.total_current_assets.years_data;
    this.total_assets = data.metrics.total_assets.years_data;
    this.total_current_liabilities = data.metrics.total_current_liabilities.years_data;
    this.total_long_term_debt = data.metrics.total_long_term_debt.years_data;
    this.total_liabilities = data.metrics.total_liabilities.years_data;
    this.years = [];

    this.labels = [
        { value: "Total Long-term Assets", size: "medium_label" }, { value: "Total Current Assets", size: "medium_label" },
        { value: "Total Assets", size: "big_label" }, { value: "Total Current Liabilities", size: "medium_label" },
        { value: "Total Long-term Debt", size: "medium_label" }, { value: "Total Liabilities", size: "big_label" }];

    // Add years
    for (var i = 0; i < this.total_assets.length; i++) {
        this.years.push(this.total_assets[i].year);
    }

    this.table = new Table();
    this.table.initialize();
};

BalanceSheetTable.displayTable = function ()
{
    this.table.addColumnLabel("Metrics");
    for (var i = 0; i < this.years.length; i++) {
        this.table.addColumnLabel(this.years[i]);
    }

    BalanceSheetTable.loadTableYears();

    BalanceSheetTable.display(this.tableId);
};

BalanceSheetTable.loadTableMonths = function (index) {
    this.columnLabels = [];
    this.rows = [];

    for (var i = 0; i < this.years.length; i++) {
        if (this.years[i] == index) {
            index = i
            break;
        }
    }

    this.columnLabels.push("Metrics");
    for (var i = 0; i < 12; i++) {
        this.columnLabels.push(DateUtils.formatLabelMonth(i));
    }

    console.log(index);
    BalanceSheetTable.loadRowsMonths(0, this.total_non_current_assets[index].months, "small_label");
    BalanceSheetTable.loadRowsMonths(1, this.total_current_assets[index].months, "small_label");
    BalanceSheetTable.loadRowsMonths(2, this.total_assets[index].months, "small_label");
    BalanceSheetTable.loadRowsMonths(3, this.total_current_liabilities[index].months, "small_label");
    BalanceSheetTable.loadRowsMonths(4, this.total_long_term_debt[index].months, "small_label");
    BalanceSheetTable.loadRowsMonths(5, this.total_liabilities[index].months, "small_label");
}

BalanceSheetTable.loadTableYears = function () {
    this.columnLabels = [];
    this.rows = [];

    this.columnLabels.push("Metrics");
    for (var i = 0; i < this.years.length; i++) {
        this.columnLabels.push(this.years[i]);
    }

    BalanceSheetTable.loadRowsYears(0, this.total_non_current_assets, "small_label");
    BalanceSheetTable.loadRowsYears(1, this.total_current_assets, "small_label");
    BalanceSheetTable.loadRowsYears(2, this.total_assets, "small_label");
    BalanceSheetTable.loadRowsYears(3, this.total_current_liabilities, "small_label");
    BalanceSheetTable.loadRowsYears(4, this.total_long_term_debt, "small_label");
    BalanceSheetTable.loadRowsYears(5, this.total_liabilities, "small_label");
}

BalanceSheetTable.loadRowsMonths = function (line, metric, size) {
    var rowElem = [];
    rowElem.push(this.labels[line]);
    for (var i = 0; i < 12; i++) {
        rowElem.push({ value: CurrencyUtils.format(metric[i]) + " €", size: size });
    }
    this.rows.push(rowElem);
}

BalanceSheetTable.loadRowsYears = function (line, metric, size) {
    var rowElem = [];
    rowElem.push(this.labels[line]);
    for (var i = 0; i < this.years.length; i++) {
        rowElem.push({ value: CurrencyUtils.format(metric[i].total) + " €", size: size });
    }
    this.rows.push(rowElem);
}

BalanceSheetTable.onDrillDown = function (year) {
    this.state = 0;
    $(this.tableId).empty();
    BalanceSheetTable.loadTableMonths(year);
    BalanceSheetTable.display(this.tableId);
}

BalanceSheetTable.onDrillUp = function () {
    this.state = 1;
    $(this.tableId).empty();
    BalanceSheetTable.loadTableYears();
    BalanceSheetTable.display(this.tableId);
}

function thClicked(index) {
    if (BalanceSheetTable.state == 1) {
        if (index > 1) {
            //BalanceSheetTable.onDrillDown(index);
            drillDown(this.years[index - 1]);
        }
    }
    else {
        //BalanceSheetTable.onDrillUp();
        drillUp();
    }
}

BalanceSheetTable.display = function (tableId) {
    var table = $(tableId);
    table.html('');

    // Header:
    var header = '<thead><tr role="row">';
    for (var i = 0; i < this.columnLabels.length; i++)
        header += '<th onclick="thClicked(' + i + ')">' + this.columnLabels[i] + '</th>';
    header += '</tr></thead>';
    table.append(header);

    // Body:
    var body = '<tbody>';
    for (var j = 0; j < this.rows.length; j++) {
        body += '<tr role="row" class="odd">';
        var row = this.rows[j];
        for (var k = 0; k < row.length; k++)
            body += '<td class=\"' + row[k].size + '\">' + row[k].value + '</td>';
        body += '</tr>';
    }
    body += '</tbody>';
    table.append(body);
};