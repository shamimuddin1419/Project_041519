$(document).ready(() => {
    $('#successAlert').hide();
    const urlParams = new URLSearchParams(window.location.search);
    const myParam = urlParams.get('referrer');
    $('#Referrer').val(myParam);
    $.get('/User/GetCountries', function (data) {
        let countries = data.data;
        $("#CountryId").select2({
            templateResult: formatCountry,
            data: countries
        }); 
        
        $.get('https://ipwho.is/', function (data) {
            $("#CountryId").val(data.country_code).trigger('change');;
        })
        //$.get('http://ip-api.com/json', function (data) {
        //    $("#CountryId").val(data.countryCode).trigger('change');;
        //})
        
    });
    
});

function hasWhiteSpace(s) {
    return s.indexOf(' ') >= 0;
}
function formatCountry(country) {
    if (!country.id) { return country.text; }
    var $country = $(
        '<span class="flag-icon flag-icon-' + country.id.toLowerCase() + ' flag-icon-squared"></span>' +
        '<span class="flag-text">' + country.text + "</span>"
    );
    return $country;
};
function validateEmail() {
   let email= $('#Email').val()
    var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
    return emailReg.test(email);
}

function CheckEmailValidation(){    
    if (!validateEmail()) {
        toastr.warning("Provide valid email address")       
    }
}
function MatchPassword() {
    if ($('#Password').val() != $('#ConfirmPassword').val()) {
        toastr.warning("Password and it's confirmation doesn't match")
    } 
}

$('#btnSave').click(function () {
    if ($('#UserName').val() == '')
    {
        toastr.warning("Provide Username")
    }
    else if (hasWhiteSpace($('#UserName').val())) {
        toastr.warning('Space in UserName is not allowed');
    }
    else if ($('#Email').val() == '') {
        toastr.warning("Provide Email")
    }  
    else if ($('#Mobile').val() == '') {
        toastr.warning("Provide Mobile")
    } 
    else if ($('#Password').val() == '') {
        toastr.warning("Provide Password")
    } 
    else if ($('#ConfirmPassword').val() == '') {
        toastr.warning("Provide Confirm Password")
    } 
    else if ($('#Password').val() != $('#ConfirmPassword').val()) {
        toastr.warning("Password and it's confirmation doesn't match")
    }
    else if ($('#CountryId').val() == '0') {
        toastr.warning("Provide Country")
    }
    else if (!$('#TermsConditionId').is(':checked')) {
        toastr.warning("Tou have to agree with terms and conditions to register")
    }
    else {
        var objVmUser = {
            UserName: $('#UserName').val(),
            Email: $('#Email').val(),
            Mobile: $('#Mobile').val(),
            FullName: $('#FullName').val(),
            Password: $('#Password').val(),
            ConfirmPassword: $('#ConfirmPassword').val(),
            ReferrelUserName: $('#Referrer').val(),
            CountryId: $('#CountryId').val(),
            IsSendEmail: $('#IsSendEmailId').is(':checked')
        };
        $.ajax({
            url: "/User/InsertUser",
            data: JSON.stringify(objVmUser),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            success: function (response) {
                debugger;
                if (response.status == true) {
                    //toastr.success(response.message);
                    $('#UserName').val('');
                    $('#Email').val('');
                    $('#Mobile').val('');
                    $('#FullName').val('');
                    $('#Password').val('');
                    $('#ConfirmPassword').val('');
                    $('#Referrer').val('');
                    $('#successAlert').show();
                    $('#TermsConditionId').prop('checked', false);
                    $('#IsSendEmailId').prop('checked', false);
                    
                } else {
                    toastr.error(response.message);
                }
            },
            error: function (response) {              
                toastr.error("error!"); 
            }
        });
    }
});
