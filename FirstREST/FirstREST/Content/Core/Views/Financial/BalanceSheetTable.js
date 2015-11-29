function BalanceSheetTable()
{
}

BalanceSheetTable.initialize = function (tableId, data)
{
    this.tableId = tableId;

    this.total_non_current_assets = data.metrics.total_non_current_assets.years_data;
    this.total_current_assets = data.metrics.total_current_assets.years_data;
    this.total_assets = data.metrics.total_assets.years_data;
    this.total_current_liabilities = data.metrics.total_current_liabilities.years_data;
    this.total_long_term_debt = data.metrics.total_long_term_debt.years_data;
    this.total_liabilities = data.metrics.total_liabilities.years_data;

    this.table = new Table();
    this.table.initialize();
};

BalanceSheetTable.displayTable = function ()
{
    this.table.addColumnLabel("Client ID");
    this.table.addColumnLabel("Client Name");
    this.table.addColumnLabel("Value");

    /*
    for (var i = 0; i < data.length; i++)
        table.addRow(data[i].ClientId, data[i].ClientName, CurrencyUtils.format(data[i].Total) + " €");
    */

   

    this.table.display(this.tableId);
};