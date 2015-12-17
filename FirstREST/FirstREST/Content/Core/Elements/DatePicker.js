function DatePicker()
{
}

DatePicker.prototype.initialize = function (buttonId, changeHandler, initialDate, finalDate, ranges)
{
    var startLastMonth = moment().subtract(1, 'month').startOf('month');
    var endLastMonth = moment().subtract(1, 'month').endOf('month');

    if (!initialDate)
        initialDate = startLastMonth;

    if (!finalDate)
        finalDate = endLastMonth;

    if (!ranges)
        ranges = DatePicker.defaultRange1;

    $(buttonId).daterangepicker(
        {
            ranges: ranges,
            startDate: initialDate,
            endDate: finalDate,
            format: "DD/MM/YYYY"
        },
        function (initialDate, finalDate) {
            DatePicker.setInfo(initialDate, finalDate);
            changeHandler(initialDate, finalDate);
        }
    );

    DatePicker.setInfo(initialDate, finalDate);
};

DatePicker.setInfo = function(initialDate, finalDate) {
    $("#DatePickerInfo").text("Displaying values from " + DateUtils.formatDate(initialDate) + " to " + DateUtils.formatDate(finalDate));
};

DatePicker.defaultDate1 = [moment().subtract(2, 'month').startOf('month'), moment()];
DatePicker.defaultDate2 = [moment().startOf('year'), moment()];
DatePicker.defaultDate3 = [moment().subtract(1, 'year').startOf('year'), moment().subtract(1, 'year').endOf('year')];
DatePicker.defaultDate4 = [moment().subtract(3, 'year').startOf('year'), moment()];
DatePicker.defaultDate5 = [moment(), moment()];

DatePicker.defaultRange1 = {
    'Last 3 Months': DatePicker.defaultDate1,
    'Current Year': DatePicker.defaultDate2,
    'Last Year': DatePicker.defaultDate3,
    'Last 3 Years Until Now': DatePicker.defaultDate4
};