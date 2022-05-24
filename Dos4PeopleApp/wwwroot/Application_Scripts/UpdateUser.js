
$(document).ready(() => {
    loadCountries();
    loadUser();
});

const formatCountry = (country) => {
    if (!country.id) { return country.text; }
    var $country = $(
        '<span class="flag-icon flag-icon-' + country.id.toLowerCase() + ' flag-icon-squared"></span>' +
        '<span class="flag-text">' + country.text + "</span>"
    );
    return $country;
};
const loadCountries = () => {
    $.get('/User/GetCountries', function (data) {
        let countries = data.data;
        $("#CountryId").select2({
            templateResult: formatCountry,
            data: countries
        });


    });
}
const loadUser = () => {
    $.get($('#loadUserUrlId').val(), (data) => {
        debugger;
        $('#UserNameId').val(data.data.userName);
        $('#FullNameId').val(data.data.fullName);
        $('#EmailId').val(data.data.email);
        $('#MobileId').val(data.data.mobile);
        $('#IsSendEmailId').prop('checked', data.data.isSendEmail);
        $('#MobileId').val(data.data.mobile);
        if (data.data.countryId == null || data.data.countryId == undefined) {
            //$.get('http://ip-api.com/json', function (res) {
            //    $("#CountryId").val(res.countryCode).trigger('change');
            //})
            $.get('https://ipwho.is/', function (data) {
                $("#CountryId").val(data.country_code).trigger('change');;
            })
        }
        else {
            $("#CountryId").val(data.data.countryId).trigger('change');
        }
        $('#PackageId').val(data.data.package);
        $('#JoinDateId').val(data.data.joinDate);
        $('#DurationId').val(data.data.duration);
        $('#ExpireId').val(data.data.expire);

    })
}
let file = null;
const uploadImage = (fileInput) => {
    const files = fileInput.files;
    if (files) {
        file = files[0];
        var img = document.getElementById("UserImage");
        img.file = file;
        var reader = new FileReader();
        reader.onload = (function (aImg) {
            return function (e) {
                aImg.src = e.target.result;
            };
        })(img);
        reader.readAsDataURL(file);
    }
}
const submitUserInfo = () => {
    if (file != null) {
        if (Math.round(file.size / (1024 * 1024)) > 1) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Image Size should be less than 1 MB'
            });
            return;
        }
    }

    let user = {
        FullName: $('#FullNameId').val(),
        Email: $('#EmailId').val(),
        Mobile: $('#MobileId').val(),
        CountryId: $('#CountryId').val(),
        IsSendEmail: $('#IsSendEmailId').is(':checked')
    }
    const fm = new FormData();
    fm.append('FullName', user.FullName);
    if (user.Email) {
        fm.append('Email', user.Email)
    }
    if (user.Mobile) {
        fm.append('Mobile', user.Mobile);
    }
    if (user.CountryId) {
        fm.append('CountryId', user.CountryId);
    }
    if (user.IsSendEmail) {
        fm.append('IsSendEmail', user.IsSendEmail);
    }
    if (file) {
        fm.append('Image', file);
    }
    $.ajax({
        url: $('#submitUrlId').val(),
        data: fm,
        type: "POST",
        enctype: 'multipart/form-data',
        processData: false,
        contentType: false,
        success: function (response) {
            if (response.success == true) {
                Swal.fire({
                    icon: 'success',
                    title: 'Success',
                    text: "User Information Updated Successfully"
                })

            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: response.message
                })
            }
        },
        error: function (response) {
            toastr.error("error!");
        }
    });

}
$('#submitId').click(() => {
    submitUserInfo();
})