$(document).ready(ready);

function drawCashChart(data) {
    var years_data = data.metrics.cash.years_data;
    CashChart.displayChart("#cash_chart", years_data);
}

function drawAccountsChart(data) {
    var accounts_receivable = data.metrics.accounts_receivable.years_data;
    var accounts_payable = data.metrics.accounts_payable.years_data;
    AccountsChart.displayChart("#accounts_chart", "#accounts_legend", accounts_receivable, accounts_payable);
    AccountsChart.displayLegends();
}

function drawBalanceSheetTable(data) {
    BalanceSheetTable.initialize("#balance_sheet_table", data);
    BalanceSheetTable.displayTable();
}


function drawCharts(data) {
    console.log(data);
    drawCashChart(data);
    drawAccountsChart(data);
    drawBalanceSheetTable(data);
}

function getBalanceSheet() {
    $.ajax({
        url: 'http://localhost:49822/api/financial',
        type: 'Get',
        beforeSend: function () {
            $("#cash_chart").closest("div .box").append("<div class=\"overlay\"><i class=\"fa fa-refresh fa-spin\"></i></div>");
            $("#accounts_chart").closest("div .box").append("<div class=\"overlay\"><i class=\"fa fa-refresh fa-spin\"></i></div>");
            $("#balance_sheet_table").closest("div .box").append("<div class=\"overlay\"><i class=\"fa fa-refresh fa-spin\"></i></div>");
        },
        success: function (data) {
            drawCharts(data);
            $("#cash_chart").closest("div .box").children("div .overlay").remove();
            $("#accounts_chart").closest("div .box").children("div .overlay").remove();
            $("#balance_sheet_table").closest("div .box").children("div .overlay").remove();
        },
        failure: function () {
            alert('Failed to get financial values');
        }
    });
}

function ready() {
    getBalanceSheet();
}

function drillDown(year) {
    CashChart.onDrillDown(year);
    AccountsChart.onDrillDown(year);
    BalanceSheetTable.onDrillDown(year);
}

function drillUp() {
    CashChart.onDrillUp();
    AccountsChart.onDrillUp();
    BalanceSheetTable.onDrillUp();
}