Handlebars.registerHelper('jsonDate', function (jsonDate) {
    return moment(jsonDate).format('L');
});
Handlebars.registerHelper('jsonTime', function (jsonTime) {
    return moment(jsonTime).format('HH:mm:ss');
});
Handlebars.registerHelper('logLevel', function(level) {
    switch(level) {
        case 1:
            return "Debug";

        case 2:
            return "Information";

        case 3:
            return "Warning";

        case 4:
            return "Error";
    }

    return "Unkown";
});