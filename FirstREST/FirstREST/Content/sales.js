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

        url: 'http://localhost:49822/api/sales/sales_by_category',
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
        url: 'http://localhost:49822/api/primavera/sale',
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
    var first_three_years_ago = new Date(today.getFullYear() - 3, 0, 1);
    var last_three_years_ago = new Date(today.getFullYear() - 2, 0, 0);
    var first_two_years_ago = new Date(today.getFullYear() - 2, 0, 1);
    var last_two_years_ago = new Date(today.getFullYear() - 1, 0, 0);
    var first_one_year_ago = new Date(today.getFullYear() - 1, 0, 1);
    var last_one_year_ago = new Date(today.getFullYear(), 0, 0);
    var first_this_year = new Date(today.getFullYear(), 0, 1);

    var three_years_ago_value = add_net_sales(get_load_sales(first_three_years_ago, last_three_years_ago));
    var two_years_ago_value = add_net_sales(get_load_sales(first_two_years_ago, last_two_years_ago));
    var one_year_ago_value = add_net_sales(get_load_sales(first_one_year_ago, last_one_year_ago));
    var today_value = add_net_sales(get_load_sales(first_this_year, today));

    var line_data = [];
    line_data[first_three_years_ago.getFullYear()] = three_years_ago_value;
    line_data[first_two_years_ago.getFullYear()] = two_years_ago_value;
    line_data[first_one_year_ago.getFullYear()] = one_year_ago_value;
    line_data[today.getFullYear()] = today_value;

    var data = {
        labels: Object.keys(line_data),
        datasets: [
            {
                label: "My Second dataset",
                fillColor: "rgba(151,187,205,0.2)",
                strokeColor: "rgba(151,187,205,1)",
                pointColor: "rgba(151,187,205,1)",
                pointStrokeColor: "#fff",
                pointHighlightFill: "#fff",
                pointHighlightStroke: "rgba(151,187,205,1)",
                data: [three_years_ago_value.toFixed(2), two_years_ago_value.toFixed(2), one_year_ago_value.toFixed(2), today_value.toFixed(2)]
            }
        ]
    };

    var options = {
        animation: false,
        responsive: true,
        maintainAspectRatio: true
    };

    var lineChart = new Chart(ctx).Line(data, options);
   
    $("#net_sales_chart").get(0).onclick = function (event) {
        var activePoints = lineChart.getPointsAtEvent(event);
        var year = activePoints[0].label;
        lineChart.destroy();
        create_net_sales_year(year);
    }
}

function create_net_sales_year(year) {
    var ctx = $("#net_sales_chart").get(0).getContext("2d");

    var january = new Date(year, 0, 1);
  
    var january_value = add_net_sales(get_load_sales(new Date(year, 0, 1), new Date(year, 1, 0)));
    var february_value = add_net_sales(get_load_sales(new Date(year, 1, 1), new Date(year, 2, 0)));
    var march_value = add_net_sales(get_load_sales(new Date(year, 2, 1), new Date(year, 3, 1)));
    var april_value = add_net_sales(get_load_sales(new Date(year, 3, 1), new Date(year, 4, 1)));
    var may_value = add_net_sales(get_load_sales(new Date(year, 4, 1), new Date(year, 5, 1)));
    var june_value = add_net_sales(get_load_sales(new Date(year, 5, 1), new Date(year, 6, 1)));
    var july_value = add_net_sales(get_load_sales(new Date(year, 6, 1), new Date(year, 7, 1)));
    var august_value = july_value + add_net_sales(new Date(year, 7, 1), new Date(year, 8, 1));
    var september_value = add_net_sales(get_load_sales(new Date(year, 8, 1), new Date(year, 9, 1)));
    var october_value = add_net_sales(get_load_sales(new Date(year, 9, 1), new Date(year, 10, 1)));
    var november_value = add_net_sales(get_load_sales(new Date(year, 10, 1), new Date(year, 11, 1)));
    var december_value = add_net_sales(get_load_sales(new Date(year, 11, 1), new Date(january.getFullYear() + 1, 0, 0)));

    var line_data = [];
    line_data["January"] = january_value;
    line_data["February"] = february_value;
    line_data["March"] = march_value;
    line_data["April"] = april_value;
    line_data["May"] = may_value;
    line_data["June"] = june_value;
    line_data["July"] = july_value;
    line_data["August"] = august_value;
    line_data["September"] = september_value;
    line_data["October"] = october_value;
    line_data["November"] = november_value;
    line_data["December"] = december_value;

    var data = {
        labels: Object.keys(line_data),
        datasets: [
            {
                label: "My Second dataset",
                fillColor: "rgba(151,187,205,0.2)",
                strokeColor: "rgba(151,187,205,1)",
                pointColor: "rgba(151,187,205,1)",
                pointStrokeColor: "#fff",
                pointHighlightFill: "#fff",
                pointHighlightStroke: "rgba(151,187,205,1)",
                data: [january_value.toFixed(2), february_value.toFixed(2), 
                    march_value.toFixed(2), april_value.toFixed(2), may_value.toFixed(2), june_value.toFixed(2),
                    july_value.toFixed(2), august_value.toFixed(2), september_value.toFixed(2), october_value.toFixed(2),
                    november_value.toFixed(2), december_value.toFixed(2)]
            }
        ]
    };

    var options = {
        animation: false,
        responsive: true,
        maintainAspectRatio: true

    };

    var lineChart = new Chart(ctx).Line(data, options);
}

function create_sales_by_category(data) {
    var ctx = $("#sales_by_category_chart").get(0).getContext("2d");

    var values = [];
    var label = "";
    var value = "";
    $.each(data, function () {
        $.each(this, function (k, v) {
            if (k == "Product")
                label = v["FamilyId"];
            else if (k == "Value")
                value = v["Value"];
        });

        if (label in Object.keys(values)) 
            values[label] += value;
        else 
            values[label] = value;
    });

    var pie_data = [];
    var keys = Object.keys(values);
    var i = 0;
    for (var key in keys) {
        pie_data[i] = {
            value: values[keys[key]].toFixed(2),
            color: "#"+((1<<24)*Math.random()|0).toString(16).slice(-6),
            label: keys[key]
        }
        i++;
    }

    var options = {
        animation: false,
        responsive: true,
        maintainAspectRatio: true
    };

    var piechart = new Chart(ctx).Pie(pie_data, options);
}

function load_top_customers() {
    var today = new Date();
    var five_years_ago = new Date(new Date().getFullYear() - 5, 0, 1)

    $.ajax({
        url: 'http://localhost:49822/api/primavera/sale',
        type: 'Get',
        data: {
            initialDate: formatDate(five_years_ago),
            finalDate: formatDate(today),
            DocumentType: 'ECL'
        },
        success: function (data) {
            create_top_customers(data);
        },
        failure: function () {
            alert('Failed to get sales values');
        }
    });
}

function create_top_customers(data) {
    var table = $("#top_customers");

    //Table header
    table.append('<thead><tr role="row">').
                        append('<th>ClientId</th><th>ClientName</th><th>Value</th>').
                        append('</tr></thead>');

    //Table body
    table.append('<tbody>');

    //Rows
    var clients = [];
    var value = 0;
    var clientId = "";
    var clientName = "";
    var found = false;
    $.each(data, function () {
        $.each(this, function (k, v) {
            if (k == "ClientId")
                clientId = v;
            else if (k == "ClientName")
                clientName = v;
            else if (k == "Value")
                value = v["Value"];
        });


        found = false;
        for (var i = 0; i < clients.length; i++) {
            //Client in array
            if (clients[i].clientId == clientId) {
                clients[i].value += value;
                found = true;
                break;
            }
        }
        
        //Client not in array
        if (!found)
            clients.push({ clientId: clientId, clientName: clientName, value: value });
    });

    //Sort
    clients.sort(function (a, b) {
        return parseFloat(b.value) - parseFloat(a.value);
    });


    //Add top 10 rows
    for (var i = 0; i < 10 && i < clients.length; i++) {
        table.append('<tr role="row" class="odd"><td>' + clients[i].clientId + '</td><td>' + clients[i].clientName + '</td><td>' + clients[i].value.toFixed(2) + '</td></tr>');
    }

    //Table end body
    table.append('</tbody>');
}

$(document).ready(ready);

function ready() {
    load_sales_by_category();
    create_net_sales();
    load_top_customers();
}