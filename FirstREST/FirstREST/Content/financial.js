$(document).ready(ready);

function drawCashChart(data) {
    var years_data = data.metrics.cash.years_data;
    CashChart.displayChart("#cash_chart", years_data);
}

function drawAcountsChart(data) {
    var accounts_receivable = data.metrics.accounts_receivable.years_data;
    var accounts_payable = data.metrics.accounts_payable.years_data;
    AccountsChart.displayChart("#accounts_chart", "#accounts_legend", accounts_receivable, accounts_payable);
    AccountsChart.displayLegends();
}


function drawCharts(data) {
    //console.log(data);
    drawCashChart(data);
    drawAcountsChart(data);
}

function getBalanceSheet() {
    $.ajax({
        url: 'http://localhost:49822/api/financial',
        type: 'Get',
        success: function (data) {
            drawCharts(data);
        },
        failure: function () {
            alert('Failed to get financial values');
        }
    });
}


function ready() {
    getBalanceSheet();
}