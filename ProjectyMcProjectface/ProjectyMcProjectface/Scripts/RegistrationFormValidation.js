$(document).ready(function () {
    $("#UserRegistrationForm").validate({

        messages: {
            Email: {
                required: "Plase enter your email",
                email: "Please enter valid email address",
                maxlength: "Your email exeeds character limit of 128"
            },
            Username: {
                required: "Please enter your desired username",
                minlength: "Your username must be at least 4 characcters long",
                maxlength: "Your username exeeds character limit of 36"
            },

            Password: {
                required: "Please enter your password",
                minlength: "Your password must be at least 4 characters long",
                maxlength: "Your password exeeds character limit of 100"
            },

            RepeatedPassword: {
                required: "Please comfirm your password",
                equalTo: "Passwords did not match, please try again"
            }
        }

    });
});