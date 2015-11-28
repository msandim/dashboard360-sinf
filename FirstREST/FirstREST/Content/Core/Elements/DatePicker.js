function DatePicker()
{
}

DatePicker.prototype.initialize = function (buttonId, changeHandler)
{
    $(buttonId).daterangepicker(
        {
            ranges: {
                'Last Year': [moment().subtract(1, 'year').startOf('year'), moment().subtract(1, 'year').endOf('year')],
                'Last 3 Years Until Now': [moment().subtract(3, 'year').startOf('year'), moment()],
            },
            startDate: moment().subtract(1, 'year').startOf('year'),
            endDate: moment().subtract(1, 'year').endOf('year')
        },
        changeHandler
    );
};