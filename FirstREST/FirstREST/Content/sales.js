$(document).ready(ready);

function ready() 
{
    var finalDate = new Date();
    var initialDate = new Date(finalDate.getFullYear() - 5, 0, 1);

    // Draw charts:
    SalesByCategoryChart.display("#sales_by_category_chart", initialDate, finalDate, 5);
    TopCostumersTable.display("#top_customers", initialDate, finalDate, 10);
    NetSalesChart.display("#net_sales_chart", initialDate, finalDate, "year");
}