function CurrencyUtils()
{
}

CurrencyUtils.format = function (number, currency) {
    return number.toLocaleString("en-US", { style: 'currency', currency: currency });
};