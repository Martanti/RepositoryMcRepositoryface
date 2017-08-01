$(document).ready(function () {
    $("#UserLogInForm").validate({
        rules: {
            Email: "required",
            Password: "required"
        }
    });
});