function BalanceSheetTable()
{
}

BalanceSheetTable.initialize = function (tableId, data)
{
    this.tableId = tableId;
    this.state = 1;
    /*
    this.total_non_current_assets = data.metrics.total_non_current_assets.years_data;
    this.total_current_assets = data.metrics.total_current_assets.years_data;
    this.total_current_liabilities = data.metrics.total_current_liabilities.years_data;
    this.total_long_term_debt = data.metrics.total_long_term_debt.years_data;
    */

    this.total_assets = data.metrics.total_assets.years_data;
    this.cash = data.metrics.cash.years_data;
    this.sales = data.metrics.sales.years_data;
    this.accounts_receivable = data.metrics.accounts_receivable.years_data;
    this.total_liabilities = data.metrics.total_liabilities.years_data;
    this.accounts_payable = data.metrics.accounts_payable.years_data;
    this.cost_of_goods_sold = data.metrics["custo merc. vend"].years_data;
    this.unknown = data.metrics["estados e outros entes"].years_data;
    this.investments = data.metrics["emprestimos obtidos"].years_data;
    this.years = [];

    this.labels = [
        { value: "Assets", size: "big_label" }, { value: "Cash", size: "medium_label" },
        { value: "Sales", size: "medium_label" }, { value: "Accounts Receivable", size: "medium_label" },
        { value: "Liabilities", size: "big_label" }, { value: "Accounts Payable", size: "medium_label" },
        { value: "Cost of goods sold", size: "medium_label" }, { value: "Unknown", size: "medium_label" },
        { value: "Investments", size: "medium_label" }];

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

    BalanceSheetTable.loadRowsMonths(0, this.total_assets[index].months, "small_label");
    BalanceSheetTable.loadRowsMonths(1, this.cash[index].months, "small_label");
    BalanceSheetTable.loadRowsMonths(2, this.sales[index].months, "small_label");
    BalanceSheetTable.loadRowsMonths(3, this.accounts_receivable[index].months, "small_label");
    BalanceSheetTable.loadRowsMonths(4, this.total_liabilities[index].months, "small_label");
    BalanceSheetTable.loadRowsMonths(5, this.accounts_payable[index].months, "small_label");
    BalanceSheetTable.loadRowsMonths(6, this.cost_of_goods_sold[index].months, "small_label");
    BalanceSheetTable.loadRowsMonths(7, this.unknown[index].months, "small_label");
    BalanceSheetTable.loadRowsMonths(8, this.investments[index].months, "small_label");
}

BalanceSheetTable.loadTableYears = function () {
    this.columnLabels = [];
    this.rows = [];

    this.columnLabels.push("Metrics");
    for (var i = 0; i < this.years.length; i++) {
        this.columnLabels.push(this.years[i]);
    }

    BalanceSheetTable.loadRowsYears(0, this.total_assets, "small_label");
    BalanceSheetTable.loadRowsYears(1, this.cash, "small_label");
    BalanceSheetTable.loadRowsYears(2, this.sales, "small_label");
    BalanceSheetTable.loadRowsYears(3, this.accounts_receivable, "small_label");
    BalanceSheetTable.loadRowsYears(4, this.total_liabilities, "small_label");
    BalanceSheetTable.loadRowsYears(5, this.accounts_payable, "small_label");
    BalanceSheetTable.loadRowsYears(6, this.cost_of_goods_sold, "small_label");
    BalanceSheetTable.loadRowsYears(7, this.unknown, "small_label");
    BalanceSheetTable.loadRowsYears(8, this.investments, "small_label");
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

BalanceSheetTable.thClicked = function(index) {
    if (BalanceSheetTable.state == 1) {
        if (index >= 1) {
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
        header += '<th onclick="BalanceSheetTable.thClicked(' + i + ')">' + this.columnLabels[i] + '</th>';
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