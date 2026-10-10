$(document).ready(function () {
    toastr.options = {
        closeButton: true,
        progressBar: true,
        positionClass: "toast-top-right",
        timeOut: 3000
    };

    const alertBox = $("#validationAlert");

    if (alertBox.length) {
        const errors = alertBox.find("li");

        errors.each(function () {
            toastr.error($(this).text());
        });
    }
});
