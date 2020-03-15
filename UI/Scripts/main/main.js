var $ = jQuery.noConflict();
$(document).ready(function () {


    function check_admin_registration() {
        if ($("#adminName").length === 0 || $("#adminName").val() === "") {
            return;
        }
        if ($("#adminLastName").length === 0 || $("#adminLastName").val() === "") {
            return;
        }
        if ($("#adminEmail").length === 0 || $("#adminEmail").val() === "" || ($("#adminEmail") !== $("#adminReEmail"))) {
            return;
        }

        if ($("#adminPassword").length === 0 || $("#adminPassword").val() === "" || ($("#adminPassword") !== $("#adminRePassword"))) {
            return;
        }
    }

    $(document).on("submit", "#adminRegister", function (e) {
        e.preventDefault();
        e.stopImmediatePropagation();

        check_admin_registration();
        var form = new FormData(document.getElementById("adminRegister"));
        $.ajax({
            method: "POST",
            url: "https://localhost:44331/login/new",
            dataType: "json",
            data: form,
            contentType: false,
            processData: false,
            cache: false,
            success: function (error_message) {
                if (typeof error_message == 'string' || error_message instanceof String) {
                    if (error_message.indexOf("error") !== -1) {
                        $("#registerError").removeClass("hideError").addClass("showError");
                    }
                    else {
                        window.location.href = "https://localhost:44331" + error_message;
                    }
                }
            }
        });
    });

    $(document).on("submit", "#admin_login", function (e) {
        e.preventDefault();
        e.stopImmediatePropagation();

        var form = new FormData(document.getElementById("admin_login"));
        $.ajax({
            method: "POST",
            url: "https://localhost:44331/login/signin",
            dataType: "json",
            data: form,
            contentType: false,
            processData: false,
            cache: false,
            success: function (error_message) {
                if (typeof error_message == 'string' || error_message instanceof String) {
                    if (error_message.indexOf("error") !== -1) {
                        $("#loginError").removeClass("hideError").addClass("showError");
                    }
                    else {
                        window.location.href = "https://localhost:44331" + error_message;
                    }
                }
            }
        });
    });
});