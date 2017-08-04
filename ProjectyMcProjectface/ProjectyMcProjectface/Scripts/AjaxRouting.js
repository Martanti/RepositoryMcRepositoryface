
$(document).ready(function () {

    $(".partial").on('click', function () {
        $.ajax({
            type: "GET",
            url: $(this).attr("value")+"?isPartial=true",
            dataType: "html",
            success: function (data) {
                $("#bodyContent").html(data);
            }
        });
    });


});

