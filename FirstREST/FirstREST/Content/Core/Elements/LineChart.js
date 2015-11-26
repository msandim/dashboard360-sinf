function LineChart()
{    
}

LineChart.prototype.initialize = function (animation, responsive, maintainAspectRatio)
{
    this.labels = [];
    this.values = [];

    // Create the options object:
    this.options = {
        animation: !animation ? false : animation,
        responsive: !responsive ? true : responsive,
        maintainAspectRatio: !maintainAspectRatio ? true : maintainAspectRatio
    };
};

LineChart.prototype.addValue = function (label, value)
{
    this.labels.push(label);
    this.values.push(value);
};

LineChart.prototype.display = function (canvasId)
{
    // Create context:
    var context = $(canvasId).get(0).getContext("2d");

    var data =
        {
            labels: this.labels,
            datasets:
                [
                    {
                        label: "My Second dataset",
                        fillColor: "rgba(151,187,205,0.2)",
                        strokeColor: "rgba(151,187,205,1)",
                        pointColor: "rgba(151,187,205,1)",
                        pointStrokeColor: "#fff",
                        pointHighlightFill: "#fff",
                        pointHighlightStroke: "rgba(151, 187, 205, 1)",
                        data: this.values
                    }
                ]
        };

    new Chart(context).Line(data, this.options);
};

LineChart.prototype.setAnimation = function (value)
{
    this.options.animation = value;
};
LineChart.prototype.setResponsive = function (value)
{
    this.options.responsive = value;
};
LineChart.prototype.setMaintainAspectRatio = function (value)
{
    this.options.maintainAspectRatio = value;
};