function DateUtils()
{
}

DateUtils.formatDate = function (date) {
    return date.format("YYYY-MM-DD");
};

DateUtils.formatLabel = function (dateText, intervalType) {
    return moment(dateText).format(intervalType === 'month' ? 'MMMM YYYY' : 'YYYY');
};