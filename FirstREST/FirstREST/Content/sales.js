$(document).ready(ready);

function loadDateSelection()
{
    $('#daterange-btn').daterangepicker(
        {
            ranges: {
                'Last 3 Years': [moment().subtract(3, 'year').startOf('year'), moment()],
                'Last Year': [moment().subtract(1, 'year').startOf('year'), moment().subtract(1, 'year').endOf('year')]
            },
            startDate: moment().subtract(3, 'year').startOf('year'),
            endDate: moment()
        },
        function (start, end) {
            drawCharts(start, end);
            //alert(start.format('YYYY/MM/DD') + ' - ' + end.format('YYYY/MM/DD'));
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
    var finalDate = moment();
    var initialDate = moment().subtract(1, 'year').startOf('year');

    // Draw charts:
    drawCharts(initialDate, finalDate);

    // Load date selection:
    loadDateSelection();
}