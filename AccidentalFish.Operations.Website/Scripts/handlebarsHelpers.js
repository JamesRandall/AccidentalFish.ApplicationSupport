Handlebars.registerHelper('jsonDate', function (jsonDate) {
    return moment(jsonDate).format('L');
});
Handlebars.registerHelper('jsonTime', function (jsonTime) {
    return moment(jsonTime).format('HH:mm:ss');
});
