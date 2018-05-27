// Buttons for creating Customers, Apartments and Orders
$(document).on("click", ".createButton", function () {
    var url = $(this).data("url");
    $("#myModalBody").load(url, function () {
        $("#myModal").modal("show");
    });
});

// Navbar buttons for editing Customers and Apartments
$(document).on("click", ".editLink", function (e) {
    e.preventDefault();
    var url = $(this).data("url") + "/" + $(this).data("id");
    $("#myModalBody").load(url, function (data) {
        $("#myModal").modal("show");
    });
});

// Details, Edit, Delete Buttons on Orders Index page 
$(document).on("click", ".orderButton", function () {
    var url = $(this).data("url") + "/" + $(this).data("id");
    $("#myModalBody").load(url, function (data) {
        $("#myModal").modal("show");
    });
});

// HANDLING NAVBAR LOGIN FORM

$(document).on("submit", "#loginForm", function (e) {
    e.preventDefault();
    $.ajax({
        method: "POST",
        url: $(this).attr("action"),
        data: $(this).serialize(),
        dataType: "json",
        success: function (data) {
            if (data.status == "OK") {
                location.href = "/Home";
            } else if (data.status == "LOGIN FAILED") {
                $("#loginFailedModal").modal("show");
            }
        }
    });
});

// HANDLING FORMS FOR CREATING AND EDITING CUSTOMERS

$(document).on("submit", "#createCustomerForm", function (e) {
    e.preventDefault();
    $.ajax({
        method: "POST",
        url: $(this).attr("action"),
        data: $(this).serialize(),
        dataType: "json",
        success: function (data) {
            if (data.status == "OK") {
                $("#myModal").modal("hide");
                location.href = "/Home";
            } else if (data.status == "DUPLICATE") {
                $("span.field-validation-valid").hide();
                $errorSpan = $("span[data-valmsg-for='Email']");
                $errorSpan.html("Sähköpostiosoite on jo käytössä");
                $errorSpan.show();
            } else if (data.status == "FAILED") {
                $("span.field-validation-valid").hide();
                $.each(data.errorMessages, function (key, value) {
                    $errorSpan = $("span[data-valmsg-for='" + key + "']");
                    $errorSpan.html(value);
                    $errorSpan.show();
                });
            }
        }
    });
});

$(document).on("submit", "#editCustomerForm", function (e) {
    e.preventDefault();
    $.ajax({
        method: "POST",
        url: $(this).attr("action"),
        data: $(this).serialize(),
        dataType: "json",
        success: function (data) {
            if (data.status == "OK") {
                $("#myModal").modal("hide");
            } else if (data.status == "FAILED") {
                $("span.field-validation-valid").hide();
                $.each(data.errorMessages, function (key, value) {
                    $errorSpan = $("span[data-valmsg-for='" + key + "']");
                    $errorSpan.html(value);
                    $errorSpan.show();
                });
            }
        }
    });
});

// HANDLING FORMS FOR CREATING AND EDITING APARTMENTS

$(document).on("submit", "#createApartmentForm", function (e) {
    e.preventDefault();
    $.ajax({
        method: "POST",
        url: $(this).attr("action"),
        data: $(this).serialize(),
        dataType: "json",
        success: function (data) {
            if (data.status == "OK") {
                $("#myModal").modal("hide");
                location.href = "/Home";
            } else if (data.status == "FAILED") {
                $("span.field-validation-valid").hide();
                $.each(data.errorMessages, function (key, value) {
                    $errorSpan = $("span[data-valmsg-for='" + key + "']");
                    $errorSpan.html(value);
                    $errorSpan.show();
                });
            }
        }
    });
});

$(document).on("submit", "#editApartmentForm", function (e) {
    e.preventDefault();
    $.ajax({
        method: "POST",
        url: $(this).attr("action"),
        data: $(this).serialize(),
        dataType: "json",
        success: function (data) {
            if (data.status == "OK") {
                $("#myModal").modal("hide");
            } else if (data.status == "FAILED") {
                $("span.field-validation-valid").hide();
                $.each(data.errorMessages, function (key, value) {
                    $errorSpan = $("span[data-valmsg-for='" + key + "']");
                    $errorSpan.html(value);
                    $errorSpan.show();
                });
            }
        }
    });
});

// HANDLING FORMS FOR CREATING, EDITING AND DELETING ORDERS

$(document).on("submit", "#createOrderForm", function (e) {
    e.preventDefault();
    $.ajax({
        method: "POST",
        url: $(this).attr("action"),
        data: $(this).serialize(),
        dataType: "json",
        success: function (data) {
            if (data.status == "OK") {
                $("#myModal").modal("hide");
                location.href = "/Orders";
            } else if (data.status == "FAILED") {
                $("span.field-validation-valid").hide();
                $.each(data.errorMessages, function (key, value) {
                    $errorSpan = $("span[data-valmsg-for='" + key + "']");
                    $errorSpan.html(value);
                    $errorSpan.show();
                });
            }
        }
    });
});

$(document).on("submit", "#editOrderForm", function (e) {
    e.preventDefault();
    $.ajax({
        method: "POST",
        url: $(this).attr("action"),
        data: $(this).serialize(),
        dataType: "json",
        success: function (data) {
            if (data.status == "OK") {
                $("#myModal").modal("hide");
                location.href = "/Orders";
            } else if (data.status == "FAILED") {
                $("span.field-validation-valid").hide();
                $.each(data.errorMessages, function (key, value) {
                    $errorSpan = $("span[data-valmsg-for='" + key + "']");
                    $errorSpan.html(value);
                    $errorSpan.show();
                });
            }
        }
    });
});

$(document).on("submit", "#deleteOrderForm", function (e) {
    e.preventDefault();
    $.ajax({
        method: "POST",
        url: $(this).attr("action"),
        data: $(this).serialize(),
        dataType: "json",
        success: function (data) {
            if (data.status == "OK") {
                $("#myModal").modal("hide");
                location.href = "/Orders";
            }
        }
    });
});