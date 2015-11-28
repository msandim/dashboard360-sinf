function ColorGenerator()
{
}

ColorGenerator.hsvToRgb = function (h, s, v) {
    var r, g, b;

    if (s === 0) {
        r = g = b = v;
        return { r: r, g: g, b: b };
    }

    h /= 60;			// sector 0 to 5
    var i = Math.floor(h);
    var f = h - i;			// factorial part of h
    var p = v * ( 1 - s );
    var q = v * ( 1 - s * f );
    var t = v * ( 1 - s * ( 1 - f ) );
    
    switch (i) 
    {
        case 0:
            r = v;
            g = t;
            b = p;
            break;
        case 1:
            r = q;
            g = v;
            b = p;
            break;
        case 2:
            r = p;
            g = v;
            b = t;
            break;
        case 3:
            r = p;
            g = q;
            b = v;
            break;
        case 4:
            r = t;
            g = p;
            b = v;
            break;
        default:		// case 5:
            r = v;
            g = p;
            b = q;
            break;
    }

    return { r: r, g: g, b: b };
}

ColorGenerator.rgbToHex = function (r, g, b)
{
    return "#" + Math.round(((1 << 24) + (r << 16) + (g << 8) + b)).toString(16).slice(1);
}

ColorGenerator.generateColors = function (numColors)
{
    var colors = [];

    var baseHue = 240;
    var deltaHue = 360.0 / numColors;
    var saturation = 0.8;
    var value = 0.8;
    for (var i = 0; i < numColors; i++)
    {
        var hue = (baseHue + deltaHue * i) % 240;
        var rgb = ColorGenerator.hsvToRgb(hue, saturation, value);

        colors.push(ColorGenerator.rgbToHex(rgb.r * 255, rgb.g * 255, rgb.b * 255));
    }

    return colors;
};