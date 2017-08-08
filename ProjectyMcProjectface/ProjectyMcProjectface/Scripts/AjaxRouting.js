
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
    $("#bodyContent").on('click', ".linkToTable", function () {
        $.ajax({
            type: "GET",
            url: $(this).attr("value") + "?internalDbName=" +
                $(this).attr("internalDbName") + "&schema=" +
                $(this).attr("schema") + "&name=" +
                $(this).attr("name") + "&isPartial=true",
            dataType: "html",
            success: function (data) {
                $("#bodyContent").html(data);
            }
        });
    });

});

