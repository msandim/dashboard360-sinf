$(document).ready(ready);

function drawCashChart(data) {
    var years_data = data.metrics.cash.years_data;
    CashChart.displayChart("#cash_chart", years_data);
}

function drawCharts(data) {
    //console.log(data);
    drawCashChart(data);
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