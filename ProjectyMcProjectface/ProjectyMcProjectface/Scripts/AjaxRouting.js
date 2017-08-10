
$(document).ready(function () {

    $(".partial").on('click', function () {
        $.ajax({
            type: "GET",
            url: $(this).attr("value")+"?isPartial=true",
            dataType: "html",
            success: function (data) {
                $("#bodyContent").html(data);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("Status: " + textStatus); alert("Error: " + errorThrown);
            }
        });
    });
    $("#bodyContent").on('click', ".linkToTable", function () {
        $(".LoadingOverlay").css("display", "block");
        $.ajax({
            type: "GET",
            url: $(this).attr("value") + "?internalDbName=" +
            $(this).attr("internalDbName") + "&schema=" +
            $(this).attr("schema") + "&name=" +
            $(this).attr("name") + "&isPartial=true",
            dataType: "html",
            success: function (data) {
                $("#bodyContent").html(data);
                $(".LoadingOverlay").css("display", "none");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("Status: " + textStatus); alert("Error: " + errorThrown);
                $(".LoadingOverlay").css("display", "none");
            }
        });
    });
    $("#dropDownDatabases").on('click', ".dropDownDatabaseItem", function () {
        $(".LoadingOverlay").css("display", "block");
        $.ajax({
            type: "GET",
            url: $(this).attr("value"),
            success: function (data) {
                $("#bodyContent").html(data);
                $(".LoadingOverlay").css("display", "none");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("Status: " + textStatus); alert("Error: " + errorThrown);
                $(".LoadingOverlay").css("display", "none");
            }
        });
    });

    $("#bodyContent").on('click', "#TestConnectionButtonAddDatabase", function (e) {
        e.preventDefault();
        var model = {
            ConnectionString: $("#ConnectionString").val(),
            Name: $("#DatabaseName").val(),
            IsConnectionSuccessful: $("#FormHolderAddDatabase").attr("IsConnectionSuccessful")
        };

        $(".LoadingOverlay").css("display", "block");
        var serialize = $(this).serialize();
        var model = { ConnectionString: $("#ConnectionString").val(), Name : $("#DatabaseName").val() }
        $.ajax({
            type: "POST",
            url: "/Home/AddDatabase",
            dataType: "html",
            data: model,
            success: function (data) {
                $("#bodyContent").html(data);
                $(".LoadingOverlay").css("display", "none");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("Status: " + textStatus); alert("Error: " + errorThrown);
                $(".LoadingOverlay").css("display", "none");
            }
        });
    });
    $("#bodyContent").on('click', "#AddDatabaseButton", function (e) {
        e.preventDefault();
        var model = {
            ConnectionString: $("#ConnectionString").val(),
            Name: $("#DatabaseName").val()
        };

        $(".LoadingOverlay").css("display", "block");
        var serialize = $(this).serialize();
        var model = { ConnectionString: $("#ConnectionString").val(), Name: $("#DatabaseName").val() }
        $.ajax({
            type: "POST",
            url: "/Home/RegisterDatabase",
            dataType: "html",
            data: model,
            success: function (data) {
                $("#bodyContent").html(data);
                $(".LoadingOverlay").css("display", "none");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("Status: " + textStatus); alert("Error: " + errorThrown);
                $(".LoadingOverlay").css("display", "none");
            }
        });
    });

    $("#bodyContent").on('click', ".linkToDatabase", function (e) {
        e.preventDefault();
        $(".LoadingOverlay").css("display", "block");
        $.ajax({
            type: "GET",
            url: $(this).attr("value"),
            success: function (data) {
                $("#bodyContent").html(data);
                $(".LoadingOverlay").css("display", "none");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("Status: " + textStatus); alert("Error: " + errorThrown);
                $(".LoadingOverlay").css("display", "none");
            }
        });
    });

});

