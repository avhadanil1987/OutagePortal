(function () {
    // overrides the jquery date validator method
    jQuery.validator.methods.date = function (value, element) {
        // We want to validate date and datetime
        var formats = ["DD-MM-YYYY", "DD-MM-YYYY HH:mm"];
        // Validate the date and return
        return moment(value, formats, true).isValid();
    };
})(jQuery, moment);