var $ = jQuery.noConflict();
$(document).ready(function () {
    $("#suggestion-content-name").hide();



    $(document).on("load", "#insert_list", function (e) {
        e.preventDefault();
        e.stopImmediatePropagation();

        $.ajax({
            method: "GET",
            url: "https://localhost:44331/admin/contacts",
            dataType: "json",
            data: form,
            contentType: false,
            processData: false,
            cache: false,
            success: function (users) {
                console.log("this is users: " + users);
                var target = $("#insert_list");
                target.empty().html();
                target.append(users);
            }
        });

    });
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
                        var target_head = $("#title-back");
                        var target_body = $("#body-back");
                        target_head.empty().html();
                        target_body.empty().html();

                        target_head.append(" Added succesfully");
                        target_body.append(" Country added succesfully");
                        
                        $("#success-contact-modal").modal({ keyboard: false, backdrop: 'static' });
                        //window.location.href = "https://localhost:44331" + error_message;
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
                var target = $("#adminCSelect");
                target.empty().html();
                var cs = '';
                $.each(countries, function (key, value) {
                    cs += '<option value="' + value.CountryId + '">' + value.CountryName + '</option>';
                });
                
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
                        var target_head = $("#title-back");
                        var target_body = $("#body-back");
                        target_head.empty().html();
                        target_body.empty().html();

                        target_head.append(" Added succesfully");
                        target_body.append(" Province added succesfully");

                        $("#success-contact-modal").modal({ keyboard: false, backdrop: 'static' });
                        //window.location.href = "https://localhost:44331" + error_message;
                    }
                }
            }
        });
    });
    $(document).on('click', '#nav-tab a[href="#nav-city"]', function (e) {
        e.preventDefault();

        var target = $("#adminCoSelect");
        target.empty().html();

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

                target.append(cs);
            }
        });
    });
    $(document).on('change', '#adminCoSelect', function (e) {
        e.preventDefault();

        var target = $("#adminPrSelect");
        target.empty().html();


        $.ajax({
            method: "POST",
            url: "https://localhost:44331/admin/provinces",
            dataType: "json",
            data: JSON.stringify({ CountryId: $(this).val() }),
            contentType: "application/json; charset=utf-8",
            processData: false,
            cache: false,
            success: function (provinces) {
                console.log("Select provinces:" + provinces);
                var ps = '';
                $.each(provinces, function (key, value) {
                    ps += '<option value="' + value.ProvinceId + '">' + value.ProvinceName + '</option>';
                });

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
                        var target_head = $("#title-back");
                        var target_body = $("#body-back");
                        target_head.empty().html();
                        target_body.empty().html();

                        target_head.append(" Added succesfully");
                        target_body.append(" City added succesfully");

                        $("#success-contact-modal").modal({ keyboard: false, backdrop: 'static' });
                        //window.location.href = "https://localhost:44331" + error_message;
                    }
                }
            }
        });
    });
    $(document).on("change", ".change_image_article", function (e) {


        for (var i = 0; i < e.originalEvent.srcElement.files.length; i++) {

            var file = e.originalEvent.srcElement.files[i];

            var reader = new FileReader();
            reader.onloadend = function () {
                // $('#img-inside-small').attr('src', reader.result);
                $('.image_preview_article').html('<img src="' + reader.result + '" id="img-inside-small" class="news-back-imge"/>');
            }
            reader.readAsDataURL(file);
        }
    });
    $(document).on('click', '#conNew', function (e) {
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
                var target = $("#contactCountry");
                target.append(cs);
            }
        });
    });
    $(document).on('change', '#contactCountry', function (e) {
        e.preventDefault();

        var target = $("#contactProv");
        target.empty().html();


        $.ajax({
            method: "POST",
            url: "https://localhost:44331/admin/provinces",
            dataType: "json",
            data: JSON.stringify({ CountryId: $(this).val() }),
            contentType: "application/json; charset=utf-8",
            processData: false,
            cache: false,
            success: function (provinces) {
                console.log("Select provinces:" + provinces);
                var ps = '';
                $.each(provinces, function (key, value) {
                    ps += '<option value="' + value.ProvinceId + '">' + value.ProvinceName + '</option>';
                });

                target.append(ps);
            }
        });

    });
    $(document).on('change', '#contactProv', function (e) {
        e.preventDefault();

        var target = $("#contactCiti");
        target.empty().html();


        $.ajax({
            method: "POST",
            url: "https://localhost:44331/admin/cities",
            dataType: "json",
            data: JSON.stringify({ ProvinceId: $(this).val() }),
            contentType: "application/json; charset=utf-8",
            processData: false,
            cache: false,
            success: function (cities) {

                var ps = '';
                $.each(cities, function (key, value) {
                    ps += '<option value="' + value.CityId + '">' + value.CityName + '</option>';
                });

                target.append(ps);
            }
        });

    });
    $(document).on('submit', '#newContactForm', function (e) {
        e.preventDefault();
        e.stopImmediatePropagation();

        var form = new FormData(document.getElementById("newContactForm"));
        $.ajax({
            method: "POST",
            url: "https://localhost:44331/admin/contact",
            dataType: "json",
            data: form,
            contentType: false,
            processData: false,
            cache: false,
            success: function (error_message) {
                if (typeof error_message == 'string' || error_message instanceof String) {
                    if (error_message.indexOf("error") !== -1) {
                        $("#contactError").removeClass("hideError").addClass("showError");
                    }
                    else {
                        $("#new-contact-modal").modal("hide");

                        var target_head = $("#title-back");
                        var target_body = $("#body-back");
                        target_head.empty().html();
                        target_body.empty().html();

                        target_head.append(" Added succesfully");
                        target_body.append(" Contact added succesfully");

                        $("#success-contact-modal").modal({ keyboard: false, backdrop: 'static' });
                        //window.location.href = "https://localhost:44331" + error_message;
                    }
                }
            }
        });

    });
    $(document).on('submit', '.contactDelete', function (e) {
        e.preventDefault();
        e.stopImmediatePropagation();
        var formId = $(this).attr("id");
        console.log("id" + formId);
        var form = new FormData(document.getElementById(formId));

        $.ajax({
            method: "POST",
            url: "https://localhost:44331/admin/remove",
            dataType: "json",
            data: form,
            contentType: false,
            processData: false,
            cache: false,
            success: function (error_message) {
                if (typeof error_message == 'string' || error_message instanceof String) {
                    if (error_message.indexOf("error") !== -1) {
                        return;
                    }
                    else {
                        var target_head = $("#title-back");
                        var target_body = $("#body-back");
                        target_head.empty().html();
                        target_body.empty().html();

                        target_head.append(" Added succesfully");
                        target_body.append(" Contact deleted succesfully");

                        $("#success-contact-modal").modal({ keyboard: false, backdrop: 'static' });
                        //window.location.href = "https://localhost:44331" + error_message;
                    }
                }
            }
        });

    });
    $(document).on('click', '.fullContactInfo', function (e) {
        e.preventDefault();
        e.stopImmediatePropagation();




        var UserId = $(this).attr("id");
        console.log("clicked" + UserId);
        $.ajax({
            method: "POST",
            url: "https://localhost:44331/admin/find/user",
            data: JSON.stringify({ userId: UserId }),
            contentType: "application/json; charset=utf-8",
            cache: false,
            success: function (error_message) {
                console.log("success" + error_message);
            }
        });

    });
    $(document).on("submit", "#contactUpdate", function (e) {
        e.preventDefault();
        e.stopImmediatePropagation();

        console.log("inside");

        var form = new FormData(document.getElementById("contactUpdate"));
        $.ajax({
            method: "POST",
            url: "https://localhost:44331/admin/show/user",
            dataType: "json",
            data: form,
            contentType: false,
            processData: false,
            cache: false,
            success: function (error_message) {
                if (typeof error_message == 'string' || error_message instanceof String) {
                    if (error_message.indexOf("error") !== -1) {
                        $("#updateClickError").removeClass("hideError").addClass("showError");
                    }

                } else {
                    console.log("to be updte user" + error_message);
                    $("#updateContactHiddenField").val(error_message.UserId);
                    $("#updateContactFirst").val(error_message.FirstName);
                    $("#updateContactLast").val(error_message.LastName);
                    $("#updateContactIDnumber").val(error_message.ID);
                    $("#updateContactPhone").val(error_message.Phone);
                    $("#updateContactEmail").val(error_message.Email);

                    $("#update-contact-modal").modal({ keyboard: false, backdrop: 'static' });
                }
            }
        });
    });
    $(document).on("submit", "#updateContactForm", function (e) {
        e.preventDefault();
        e.stopImmediatePropagation();
        var form = new FormData(document.getElementById("updateContactForm"));
        $.ajax({
            method: "POST",
            url: "https://localhost:44331/admin/contact/update",
            dataType: "json",
            data: form,
            contentType: false,
            processData: false,
            cache: false,
            success: function (error_message) {
                if (typeof error_message == 'string' || error_message instanceof String) {
                    if (error_message.indexOf("error") !== -1) {
                        $("#updateConError").removeClass("hideError").addClass("showError");
                    }
                    else {

                        $("#update-contact-modal").modal("hide");

                        var target_head = $("#title-back-update");
                        var target_body = $("#body-back-update");
                        target_head.empty().html();
                        target_body.empty().html();

                        target_head.append(" Update success");
                        target_body.append(" Updated added succesfully");

                        $("#success-update-contact-modal").modal({ keyboard: false, backdrop: 'static' });
                        //window.location.href = "https://localhost:44331" + error_message;
                    }
                }
            }
        });

    });
    
    $(document).on("keyup", "#contactSearchFirst", function (e) {
        e.preventDefault();
        e.stopImmediatePropagation();
        var filt = {
            First: $("#contactSearchFirst").val(),
            CountryId: $("#searchCountryFilter").val(),
            ProvinceId: $("#searchProvinceFilter").val(),
            CityId: $("#searchCityFilter").val()
        };
        $.ajax({
            method: "POST",
            url: "https://localhost:44331/contact/predictive/first",
            dataType: "json",
            data: JSON.stringify(filt),
            contentType: "application/json; charset=utf-8",
            processData: false,
            cache: false,
            success: function (users) {
                $("#suggestion-content-name").show();
                              
                var target = $("#suggestion-content-name");
                target.empty().html();

                var cont = '<ul class="search-list">';
                $.each(users, function (key, value) {
                    cont += "<li class='userValues' id='" + value.UserId+"'>" + value.FirstName + "</li>";                   
                });
                cont += "</ul>";
                target.append(cont);
            }
        });

    });
    $(document).on("click", ".userValues", function (e) {
        e.preventDefault();
        e.stopImmediatePropagation();

        var id = $(this).attr("id");

        console.log(id);

        $.ajax({
            method: "POST",
            url: "https://localhost:44331/contact/get/user",
            dataType: "json",
            data: JSON.stringify({ UserId: id }),
            contentType: "application/json; charset=utf-8",
            processData: false,
            cache: false,
            success: function (user) {
                if (user !== null) {
                    $("#contactUserHiddenSearch").val(user.UserId);
                    $("#contactSearchFirst").val(user.FirstName);
                    $("#contactSearchLast").val(user.LastName);
                    $("#suggestion-content-name").hide();
                }
               
            }
        });
    });
    $(document).on('change', '#searchCountryFilter', function (e) {
        e.preventDefault();

        var target = $("#searchProvinceFilter");
        target.empty().html();

        $.ajax({
            method: "POST",
            url: "https://localhost:44331/contact/filter/provinces",
            dataType: "json",
            data: JSON.stringify({ CountryId: $(this).val() }),
            contentType: "application/json; charset=utf-8",
            processData: false,
            cache: false,
            success: function (provinces) {
                console.log("Select provinces:" + provinces);
                var ps = '';
                $.each(provinces, function (key, value) {
                    ps += '<option value="' + value.ProvinceId + '">' + value.ProvinceName + '</option>';
                });

                target.append(ps);
            }
        });

    });
    $(document).on('change', '#searchProvinceFilter', function (e) {
        e.preventDefault();

        var target = $("#searchCityFilter");
        target.empty().html();

        $.ajax({
            method: "POST",
            url: "https://localhost:44331/contact/filter/cities",
            dataType: "json",
            data: JSON.stringify({ ProvinceId: $(this).val() }),
            contentType: "application/json; charset=utf-8",
            processData: false,
            cache: false,
            success: function (cities) {

                var ps = '';
                $.each(cities, function (key, value) {
                    ps += '<option value="' + value.CityId + '">' + value.CityName + '</option>';
                });

                target.append(ps);
            }
        });

    });

    $(document).on("submit", "#contactUserFormSearch", function (e) {
        e.preventDefault();
        e.stopImmediatePropagation();

        //var form = new FormData(document.getElementById("contactUserFormSearch"));
        var filterFound = {
            contactUserHiddenSearch: $("#contactUserHiddenSearch").val(),
            contactSearchFirst: $("#contactSearchFirst").val(),
            contactSearchLast: $("#contactSearchLast").val()
        };
        $.ajax({
            method: "POST",
            url: "https://localhost:44331/contact/information/user",
            dataType: "json",
            data: JSON.stringify(filterFound),
            contentType: "application/json; charset=utf-8",
            processData: false,
            cache: false,
            success: function (user) {
                console.log("this is my user: " + user.FirstName);
                var target = $("#insert_list_found");
                target.empty().html();

                var target2 = $("#found_in");
                target2.empty().html();

                var found = '<table class="table table-borderless table-hover">';
                found += '<thead>';
                found += '<tr>';
                found += '<th scope="col">full name information</th>';
                found += '<th scope="col">email</th>';
                found += '<th scope="col">phone</th>';
                found += '<th scope="col">Status</th>';
                found += '<th scope="col">ID</th>';
                found += '<th scope="col">Gender</th>';
                found += '</tr>';
                found += '</thead>';

                found += '<tbody>';
                found += '<tr>';

                found += '<td>';
                found += '<a href="https://localhost:44331/contact/profile/ajax?contactId=' + user.UserId+'" style="text-decoration:none;">' + user.FirstName + ' ' + user.LastName + '</a>';
                found += '</td>';

                found += '<td>';
                found += user.Email;
                found += '</td>';

                found += '<td>';
                found += user.Phone;
                found += '</td>';


                found += '<td>';
                found += user.Status;
                found += '</td>';

                found += '<td>';
                found += user.ID;
                found += '</td>';

                found += '<td>';
                found += user.Gender;
                found += '</td>';

                found += '</tr>';

                found += '</tbody>';
                found += '</table>';

                target.append(found);
                target2.append(' Name found: '+user.FirstName + ' ' + user.LastName);
            }
        });
    });



});