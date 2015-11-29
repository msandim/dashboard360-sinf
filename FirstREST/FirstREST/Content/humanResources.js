$(document).ready(ready);

function drawDateSelection() {
    var datePicker = new DatePicker();
    datePicker.initialize(
        "#daterange-btn",
        function (start, end) {
            drawCharts(start, end);
        }
    );
}

function drawCharts(initialDate, finalDate) {
    PurchasesByCategoryChart.display("#purchases_by_category_chart", "#purchases_by_category_legend", initialDate, finalDate, 5);
    TopSuppliersTable.display("#top_suppliers", initialDate, finalDate, 10);
    NetPurchasesChart.display("#net_purchases_chart", initialDate, finalDate, "month");
}

function ready() {
    var initialDate = moment().subtract(1, 'year').startOf('year');
    var finalDate = moment().subtract(1, 'year').endOf('year');

    // Draw charts:
    drawCharts(initialDate, finalDate);

    // Load date selection:
    drawDateSelection();
}