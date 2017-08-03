$(document).ready(function () {

    $.validator.addMethod("cust_required", $.validator.methods.required, "Please, fill this field");
    $.validator.addMethod("cust_email", $.validator.methods.email, "It looks like this email is invalid");
    $.validator.addMethod("cust_minlength", $.validator.methods.minlength, $.validator.format("Sorry, but it should be at least {0} characters long"));
    $.validator.addMethod("cust_maxlength", $.validator.methods.maxlength, $.validator.format("Sorry, but it exeeded the {0} character limit"));

    $.validator.addClassRules({
        valid_req: {
            cust_required: true
        },

        valid_email: {
            cust_email: true
        },

        valid_min_len_4: {
            cust_minlength: 4
        },

        valid_max_len_128: {
            cust_maxlength: 128
        },

        valid_max_len_100: {
            cust_maxlength: 100
        },

        valid_max_len_36: {
            cust_maxlength: 36
        },

        valid_equal: {
            equalTo: function() {
                if ($(".valid_equal").attr("id") === undefined) {
                    return undefined;
            }
                else {
                    var retunValue = "#" + $(".valid_equal").attr("id").replace("Re", "");
                    return retunValue;
            }
        }
        }


    });
    $(".FormsToValidate").validate();
});