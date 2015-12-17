$(document).ready(ready);

function drawDateSelection(initialDate, finalDate)
{
    var datePicker = new DatePicker();
    datePicker.initialize(
        "#daterange-btn",
        function (start, end)
        {
            drawCharts(start, end);
        },
        initialDate,
        finalDate,
        DatePicker.defaultRange1
    );
}

function drawCharts(initialDate, finalDate)
{
    PurchasesByCategoryChart.display("#purchases_by_category_chart", "#purchases_by_category_legend", initialDate, finalDate, 5);
    TopSuppliersTable.display("#top_suppliers", initialDate, finalDate, 10);
    TopProductsTable.display("#top_products", initialDate, finalDate, 10);
    NetPurchasesChart.display("#net_purchases_chart", initialDate, finalDate, "month");
}

function ready()
{
    var initialDate = DatePicker.defaultDate1[0];
    var finalDate = DatePicker.defaultDate1[1];

    // Draw charts:
    drawCharts(initialDate, finalDate);

    // Load date selection:
    drawDateSelection(initialDate, finalDate);
}