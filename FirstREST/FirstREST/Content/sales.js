function formatDate(date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;

    return [year, month, day].join('-');
}

function load_sales_by_category() {
    var today = new Date();
    var five_years_ago = new Date(new Date().getFullYear() - 5, 0, 1)

    $.ajax({

        url: 'http://localhost:49822/api/sale',
        type: 'Get',
        data: {
            initialDate: formatDate(five_years_ago),
            finalDate: formatDate(today),
            DocumentType: 'ECL'
        },
        success: function (data) {
            create_sales_by_category(data);
        },
        failure: function () {
            alert('Failed to get sales values');
        }
    });
}



function get_load_sales(initDate, finalDate) {
    var result = "failed";
    $.ajax({
        url: 'http://localhost:49822/api/sale',
        type: 'Get',
        async: false,
        data: {
            initialDate: formatDate(initDate),
            finalDate: formatDate(finalDate),
            DocumentType: 'ECL'

        },
        success: function (data) {
            result = data;
        },
        failure: function () {
            alert('Failed to get sales values');
        }
    });
    return result;
}

function add_net_sales(data) {
    var total = 0;

    if (data == "failed")
        return -1;

    for (var i = 0; i < data.length; i++)
    {
        total += data[i]["Value"]["Value"];
    }
    return total;
}

function create_net_sales() {
    var ctx = $("#net_sales_chart").get(0).getContext("2d");

    var today = new Date();
    var four_years_ago = new Date(new Date().getFullYear() - 4, 0, 1);
    var three_years_ago = new Date(new Date().getFullYear() - 3, 0, 1);
    var two_years_ago = new Date(new Date().getFullYear() - 2, 0, 1);
    var one_year_ago = new Date(new Date().getFullYear() - 1, 0, 1);
    var three_years_ago_value = add_net_sales(get_load_sales(four_years_ago, three_years_ago));
    var two_years_ago_value = three_years_ago_value + add_net_sales(get_load_sales(three_years_ago, two_years_ago));
    var one_year_ago_value = two_years_ago_value + add_net_sales(get_load_sales(two_years_ago, one_year_ago));
    var today_value = one_year_ago_value + add_net_sales(get_load_sales(one_year_ago, today));

    var pie_data = [];
    pie_data[three_years_ago.getFullYear()] = three_years_ago_value;
    pie_data[two_years_ago.getFullYear()] = two_years_ago_value;
    pie_data[one_year_ago.getFullYear()] = one_year_ago_value;
    pie_data[today.getFullYear()] = today_value;


    var data = {
        labels: Object.keys(pie_data),
        datasets: [
            {
                label: "My Second dataset",
                fillColor: "rgba(151,187,205,0.2)",
                strokeColor: "rgba(151,187,205,1)",
                pointColor: "rgba(151,187,205,1)",
                pointStrokeColor: "#fff",
                pointHighlightFill: "#fff",
                pointHighlightStroke: "rgba(151,187,205,1)",
                data: [three_years_ago_value, two_years_ago_value, one_year_ago_value, today_value]
            }
        ]
    };

    var options = {
        animation: false,
        responsive: true,
        maintainAspectRatio: true
    };

    var myLineChart = new Chart(ctx).Line(data, options);
}

function create_top_customers() {

}

function create_sales_by_category(data) {
    var ctx = $("#sales_by_category_chart").get(0).getContext("2d");
    /*
    $.each(data, function () {
        $.each(this, function (k, v) {
            alert(k + "  " + v);
        });
    });
    */
    var pie_data = [
        {
            value: data[0]["Value"]["Value"],
            color: "yellow",
            label: data[0]["Product"]["FamilyId"]
        }
        
    ];

    var options = {
        animation: false,
        responsive: true,
        maintainAspectRatio: true
    };

    var piechart = new Chart(ctx).Pie(pie_data, options);
}

$(document).ready(ready);

function ready() {
    load_sales_by_category();
    create_net_sales();
    create_top_customers();
}
