$(document).ready(ready);

function drawDateSelection()
{
    var datePicker = new DatePicker();
    datePicker.initialize(
        "#daterange-btn",
        function (start, end)
        {
            drawCharts(start, end);
        }
    );
}

function drawCharts(initialDate, finalDate)
{
    SalesByCategoryChart.display("#purchases_by_category_chart", initialDate, finalDate, 5);
    TopCostumersTable.display("#top_suppliers", initialDate, finalDate, 10);
    NetSalesChart.display("#net_purchases_chart", initialDate, finalDate, "month");
}

function ready()
{
    var initialDate = moment().subtract(1, 'year').startOf('year');
    var finalDate = moment().subtract(1, 'year').endOf('year');

    // Draw charts:
    drawCharts(initialDate, finalDate);

    // Load date selection:
    drawDateSelection();
}