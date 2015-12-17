function LineChart()
{    
}

LineChart.prototype.initialize = function (legendsId, animation, responsive, maintainAspectRatio)
{
    this.labels = [];
    this.datasets = [];
    this.datasets2 = [];
    this.chart = null;
    this.legendsId = legendsId;

    // Create the options object:
    this.options = {
        animation: !animation ? false : animation,
        responsive: !responsive ? true : responsive,
        maintainAspectRatio: !maintainAspectRatio ? true : maintainAspectRatio,
        pointHitDetectionRadius: 0
    };
};
LineChart.prototype.shutdown = function() {
    if (this.chart != null)
        this.chart.destroy();

    if (this.legendsId) {
        $(this.legendsId).empty();
    }
}

LineChart.prototype.addValue = function (label, value, dataset)
{
    var index = dataset ? dataset : 0;

    if (index == 0) {
        this.labels.push(label);
        this.datasets.push(value);
    } else {
        this.datasets2.push(value);
    }
};

LineChart.prototype.display = function (canvasId)
{
    // Create context:
    var context = $(canvasId).get(0).getContext("2d");

    var data =
        {
            labels: this.labels,
            datasets: this.getDatasets()
        };

    this.chart = new Chart(context).Line(data, this.options);
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

LineChart.prototype.getDatasets = function () {
    var datasets = [
        {
            label: "My First Dataset",
            fillColor: "rgba(151,187,205,0.2)",
            strokeColor: "rgba(151,187,205,1)",
            pointColor: "rgba(151,187,205,1)",
            pointStrokeColor: "#fff",
            pointHighlightFill: "#fff",
            pointHighlightStroke: "rgba(151, 187, 205, 1)",
            data: this.datasets
        }
    ];
    if (this.datasets2.length > 0) {
        datasets.push({
            label: "My Second Dataset",
            fillColor: "rgba(220,0,0,0.1)",
            strokeColor: "rgba(220,0,0,1)",
            pointColor: "rgba(220,0,0,1)",
            pointStrokeColor: "#fff",
            pointHighlightFill: "#fff",
            pointHighlightStroke: "rgba(220,0,0,1)",
            data: this.datasets2
        });
    }
    return datasets;
};

LineChart.prototype.displayLegends = function ()
{
    if (!this.legendsId)
        return;

    for (var i = 0; i < arguments.length; i++) {
        $(this.legendsId).append("<li><i class=\"fa fa-square-o\"></i> " + arguments[i] + "</li>");
    }
};