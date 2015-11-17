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

function load_net_sales() {
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
            create_net_sales(data);
        },
        failure: function () {
            alert('Failed to get sales values');
        }
    });
}


function ready() {
    load_sales_by_category();
    load_net_sales();
    create_top_customers();
}

function create_net_sales(data) {
    var ctx = $("#net_sales_chart").get(0).getContext("2d");

    var data = {
        labels: ["January", "February", "March", "April", "May", "June", "July"],
        datasets: [
            {
                label: "My First dataset",
                fillColor: "rgba(220,220,220,0.2)",
                strokeColor: "rgba(220,220,220,1)",
                pointColor: "rgba(220,220,220,1)",
                pointStrokeColor: "#fff",
                pointHighlightFill: "#fff",
                pointHighlightStroke: "rgba(220,220,220,1)",
                data: [65, 59, 80, 81, 56, 55, 40]
            },
            {
                label: "My Second dataset",
                fillColor: "rgba(151,187,205,0.2)",
                strokeColor: "rgba(151,187,205,1)",
                pointColor: "rgba(151,187,205,1)",
                pointStrokeColor: "#fff",
                pointHighlightFill: "#fff",
                pointHighlightStroke: "rgba(151,187,205,1)",
                data: [28, 48, 40, 19, 86, 27, 90]
            }
        ]
    };

    var myLineChart = new Chart(ctx).Line(data);
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

