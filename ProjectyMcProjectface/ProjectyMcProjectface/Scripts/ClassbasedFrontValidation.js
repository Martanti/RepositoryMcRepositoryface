jQuery.validator.AddClassRules({
    valid_req:{
        required: true
    },

    valid_email: {
        email: true
    },

    valid_min_len_4: {
        minlength: 4
    },

    valid_max_len_128:{
        maxlength:128
    },

    valid_max_len_36: {
        maxlength:36
    },

    valid_equal: {
        equalTo: $(".valid_set_equality").id
    }


});