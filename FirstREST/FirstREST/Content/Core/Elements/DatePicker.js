function DatePicker()
{
}

DatePicker.prototype.initialize = function (buttonId, changeHandler)
{
    var startLastMonth = moment().subtract(1, 'month').startOf('month');
    var endLastMonth = moment().subtract(1, 'month').endOf('month');

    $(buttonId).daterangepicker(
        {
            ranges: {
                'Last Month': [startLastMonth, endLastMonth],
                'Current Year': [moment().startOf('year'), moment()],
                'Last Year': [moment().subtract(1, 'year').startOf('year'), moment().subtract(1, 'year').endOf('year')],
                'Last 3 Years Until Now': [moment().subtract(3, 'year').startOf('year'), moment()]
            },
            startDate: startLastMonth,
            endDate: endLastMonth,
            format: "DD/MM/YYYY"
        },
        function (initialDate, finalDate) {
            DatePicker.setInfo(initialDate, finalDate);
            changeHandler(initialDate, finalDate);
        }
    );

    DatePicker.setInfo(startLastMonth, endLastMonth);
};

DatePicker.setInfo = function(initialDate, finalDate) {
    $("#DatePickerInfo").text("Displaying values from " + DateUtils.formatDate(initialDate) + " to " + DateUtils.formatDate(finalDate));
};