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

    $(document).on("submit", "#adlogin", function (e) {
        e.preventDefault();
        e.stopImmediatePropagation();


        console.log("inside the login ajax");

        var form = new FormData(document.getElementById("adlogin"));
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

    //add country from the admin controller
    $(document).on("submit", "#formCountry", function (e) {
        e.preventDefault();
        e.stopImmediatePropagation();

        var form = new FormData(document.getElementById("formCountry"));
        $.ajax({
            method: "POST",
            url: "https://localhost:44331/admin/country",
            dataType: "json",
            data: form,
            contentType: false,
            processData: false,
            cache: false,
            success: function (error_message) {
                if (typeof error_message == 'string' || error_message instanceof String) {
                    if (error_message.indexOf("error") !== -1) {
                        $("#countryError").removeClass("hideError").addClass("showError");
                    }
                    else {
                        window.location.href = "https://localhost:44331" + error_message;
                    }
                }
            }
        });
    });

    $(document).on('click', '#nav-tab a[href="#nav-province"]', function (e) {
        e.preventDefault();

        $.ajax({
            method: "GET",
            url: "https://localhost:44331/admin/countries",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            cache: false,
            success: function (countries) {
                console.log(countries);
                var cs = '';
                $.each(countries, function (key, value) {
                    cs += '<option value="' + value.CountryId + '">' + value.CountryName + '</option>';
                });
                var target = $("#adminCSelect");
                target.append(cs);
            }
        });
    });

    $(document).on("submit", "#formProvince", function (e) {
        e.preventDefault();
        e.stopImmediatePropagation();

        var form = new FormData(document.getElementById("formProvince"));
        $.ajax({
            method: "POST",
            url: "https://localhost:44331/admin/province",
            dataType: "json",
            data: form,
            contentType: false,
            processData: false,
            cache: false,
            success: function (error_message) {
                if (typeof error_message == 'string' || error_message instanceof String) {
                    if (error_message.indexOf("error") !== -1) {
                        $("#provinceError").removeClass("hideError").addClass("showError");
                    }
                    else {
                        window.location.href = "https://localhost:44331" + error_message;
                    }
                }
            }
        });
    });

    $(document).on('click', '#nav-tab a[href="#nav-city"]', function (e) {
        e.preventDefault();

        $.ajax({
            method: "GET",
            url: "https://localhost:44331/admin/countries",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            cache: false,
            success: function (countries) {
                console.log(countries);
                var cs = '';
                $.each(countries, function (key, value) {
                    cs += '<option value="' + value.CountryId + '">' + value.CountryName + '</option>';
                });
                var target = $("#adminCoSelect");
                target.append(cs);
            }
        });
    });

    $(document).on('change', '#adminCoSelect', function (e) {
        e.preventDefault();

        $.ajax({
            method: "POST",
            url: "https://localhost:44331/admin/provinces",
            dataType: "json",
            data: JSON.stringify({ CountryId: $(this).val() }),
            contentType: "application/json; charset=utf-8",
            processData: false,
            cache: false,
            success: function (provinces) {
                console.log("Select provinces:"+provinces);
                var ps = '';
                $.each(provinces, function (key, value) {
                    ps += '<option value="' + value.ProvinceId + '">' + value.ProvinceName + '</option>';
                });
                var target = $("#adminPrSelect");
                target.append(ps);
            }
        });

    });

    $(document).on("submit", "#formCity", function (e) {
        e.preventDefault();
        e.stopImmediatePropagation();

        var form = new FormData(document.getElementById("formCity"));
        $.ajax({
            method: "POST",
            url: "https://localhost:44331/admin/city",
            dataType: "json",
            data: form,
            contentType: false,
            processData: false,
            cache: false,
            success: function (error_message) {
                if (typeof error_message == 'string' || error_message instanceof String) {
                    if (error_message.indexOf("error") !== -1) {
                        $("#cityError").removeClass("hideError").addClass("showError");
                    }
                    else {
                        window.location.href = "https://localhost:44331" + error_message;
                    }
                }
            }
        });
    });




});