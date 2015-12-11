function DateUtils()
{
}

DateUtils.formatDate = function (date) {
    return date.format("YYYY-MM-DD");
};

DateUtils.formatLabel = function (dateText, intervalType) {
    return moment(dateText).format(intervalType === 'month' ? 'MMMM YYYY' : 'YYYY');
};

DateUtils.formatLabelYearMonth = function (year, month) {
    return moment(new Date(year, month, 1)).format("MMMM YYYY");
};

DateUtils.formatLabelMonth = function (month) {
    return moment(new Date(2015, month, 1)).format("MMMM");
};