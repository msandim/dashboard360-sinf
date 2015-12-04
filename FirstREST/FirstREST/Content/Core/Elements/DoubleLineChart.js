/// <reference path="DoubleLineChart.js" />
function DoubleLineChart()
{
}

DoubleLineChart.prototype.initialize = function (animation, responsive, maintainAspectRatio)
{
    this.labels = [];
    this.values1 = [];
    this.values2 = [];
    this.chart = null;

    // Create the options object:
    this.options = {
        animation: !animation ? false : animation,
        responsive: !responsive ? true : responsive,
        maintainAspectRatio: !maintainAspectRatio ? true : maintainAspectRatio
    };
};
DoubleLineChart.prototype.shutdown = function ()
{
    if (this.chart != null)
        this.chart.destroy();
}

DoubleLineChart.prototype.addValue = function (label, value1, value2)
{
    this.labels.push(label);
    this.values1.push(value1);
    this.values2.push(value2);
};

DoubleLineChart.prototype.display = function (canvasId)
{
    // Create context:
    var context = $(canvasId).get(0).getContext("2d");

    var data =
        {
            labels: this.labels,
            datasets:
                [
                    {
                        label: "My First dataset",
                        fillColor: "rgba(151,187,205,0.2)",
                        strokeColor: "rgba(151,187,205,1)",
                        pointColor: "rgba(151,187,205,1)",
                        pointStrokeColor: "#fff",
                        pointHighlightFill: "#fff",
                        pointHighlightStroke: "rgba(151, 187, 205, 1)",
                        data: this.values1
                    },
                    {
                        label: "My Second dataset",
                        fillColor: "rgba(151,187,205,0.2)",
                        strokeColor: "rgba(151,187,205,1)",
                        pointColor: "rgba(151,187,205,1)",
                        pointStrokeColor: "#fff",
                        pointHighlightFill: "#fff",
                        pointHighlightStroke: "rgba(151, 187, 205, 1)",
                        data: this.values2
                    }
                ]
        };

    this.chart = new Chart(context).Line(data, this.options);
};

DoubleLineChart.prototype.setAnimation = function (value)
{
    this.options.animation = value;
};
DoubleLineChart.prototype.setResponsive = function (value)
{
    this.options.responsive = value;
};
DoubleLineChart.prototype.setMaintainAspectRatio = function (value)
{
    this.options.maintainAspectRatio = value;
};