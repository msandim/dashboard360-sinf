function ColorGenerator() {
    ColorGenerator.baseHue = 0;
}

ColorGenerator.generateChars = function (numColors)
{
    var colors = [];
    for (var i = 0; i < numColors; i++)
    {
        var hue = Math.round(ColorGenerator.baseHue + (240 / numColors) * i) % 240;
        colors.push(hue);
    }

    return colors;
};