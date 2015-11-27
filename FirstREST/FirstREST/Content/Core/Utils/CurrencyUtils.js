function CurrencyUtils()
{
}

CurrencyUtils.format = function (number)
{
    var p = number.toFixed(2).split(".");
    return p[0].split("").reverse().reduce(
        function (acc, number, i, orig)
        {
            return number + (i && !(i % 3) ? "," : "") + acc;
        },
        ""
        ) + "." + p[1];
};