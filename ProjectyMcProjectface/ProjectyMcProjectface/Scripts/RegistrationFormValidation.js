$(document).ready(function () {
    $("#UserRegistrationForm").validate({
        rules: {
            Email: "required",
            Username: {
                required: true,
                minlength: 4,
                maxlength:36
            },

            Password: {
                required: true,
                minlength: 4,
                maxlength:100
            },

            RepeatedPassword: {
                required: true,
                equalTo: "#PasswordInput"
            }
        },

        messages: {
            Email: "Plase enter your email",
            Username: {
                required: "Please enter your desired username",
                minlength: "Your username bust at least 4 characcters long",
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