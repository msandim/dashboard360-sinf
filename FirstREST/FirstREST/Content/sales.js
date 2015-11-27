$(document).ready(ready);

function loadDateSelection()
{
    $('#daterange-btn').daterangepicker(
        {
            ranges: {
                'Last Year': [moment().subtract(1, 'year').startOf('year'), moment().subtract(1, 'year').endOf('year')],
                'Last 3 Years Until Now': [moment().subtract(3, 'year').startOf('year'), moment()],
            },
            startDate: moment().subtract(1, 'year').startOf('year'),
            endDate: moment().subtract(1, 'year').endOf('year')
        },
        function (start, end) {
            drawCharts(start, end);
        }
    );
}

function drawCharts(initialDate, finalDate)
{
    SalesByCategoryChart.display("#sales_by_category_chart", initialDate, finalDate, 5);
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
    loadDateSelection();
}