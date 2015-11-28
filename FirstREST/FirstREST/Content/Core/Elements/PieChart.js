function PieChart()
{    
}

PieChart.prototype.initialize = function (canvasId, animation, responsive, maintainAspectRatio) {
    this.chart = null;

    // Get context:
    this.context = $(canvasId).get(0).getContext("2d");

    // Create array to hold pie data:
    this.pieData = [];

    // Create array to hold pie legends data:
    this.lengends = [];

    this.numColors = 5;
    this.colors = ColorGenerator.generateColors(this.numColors);
    this.colorIndex = 0;

    // Create the options object:
    this.options = {
        animation: !animation ? false : animation,
        responsive: !responsive ? true : responsive,
        maintainAspectRatio: !maintainAspectRatio ? true : maintainAspectRatio
    };
};
PieChart.prototype.shutdown = function ()
{
    if (this.chart != null)
        this.chart.destroy();
}

PieChart.prototype.addSection = function (label, value, color)
{
    // If color is not defined, assign a random one:
    if (!color) {
        color = this.colors[this.colorIndex];
        this.colorIndex = (this.colorIndex + 1) % this.numColors;
    }   

    // Add new section:
    this.pieData.push(
        {
            label: label,
            value: value,
            color: color
        }
    );

    //Add legend:
    this.lengends.push(label);
};

PieChart.prototype.displayLegends = function (ulId) {
    
    for(var label in this.lengends) {
        $(ulId).append("<li><i class=\"fa fa-circle-o\"></i> " + this.lengends[label] + "</li>");
    }
};

PieChart.prototype.display = function ()
{
    this.chart = new Chart(this.context).Pie(this.pieData, this.options);
};

PieChart.prototype.setAnimation = function (value)
{
    this.options.animation = value;
};
PieChart.prototype.setResponsive = function (value)
{
    this.options.responsive = value;
};
PieChart.prototype.setMaintainAspectRatio = function (value)
{
    this.options.maintainAspectRatio = value;
};