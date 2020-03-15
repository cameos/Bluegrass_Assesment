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


    });
});