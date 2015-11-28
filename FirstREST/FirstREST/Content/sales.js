$(document).ready(ready);

function drawDateSelection() {
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
    SalesByCategoryChart.display("#sales_by_category_chart", "#sales_by_category_legend", initialDate, finalDate, 5);
    TopCostumersTable.display("#top_customers", initialDate, finalDate, 10);
    NetSalesChart.display("#net_sales_chart", initialDate, finalDate, "month");
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