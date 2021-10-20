

function MatchPassword() {
    if ($('#NewPassword').val() != $('#ConfirmPassword').val()) {
        toastr.warning("New Password and it's confirmation doesn't match")
    } 
}
function CheckExistingPassword() {
    if ($('#CurrentPassword').val() == '') {
        toastr.warning("Provide Password")
    }
    else {
        var objVmUser = {
            Password: $('#CurrentPassword').val()
        };
        $.ajax({
            url: "/ChangePassword/CheckExistingPassword",
            data: JSON.stringify(objVmUser),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            success: function (response) {
                debugger;
                if (response.status == true) {

                } else {
                    $('#CurrentPassword').val('');
                    toastr.error(response.message);
                }
            },
            error: function (response) {
                toastr.error("error!");
            }
        });
    }
}


$('#btnChangePassword').click(function () {
   
    if ($('#CurrentPassword').val() == '') {
        toastr.warning("Provide Current Password")
    } 
    else if ($('#NewPassword').val() == '') {
        toastr.warning("Provide New Password")
    }
    else if ($('#ConfirmPassword').val() == '') {
        toastr.warning("Provide Confirm Password")
    } 
    else if ($('#NewPassword').val() != $('#ConfirmPassword').val()) {
        toastr.warning("New Password and it's confirmation doesn't match")
    } 
    else {
        var objVmUser = {
            CurrentPassword: $('#CurrentPassword').val(),
            Password: $('#NewPassword').val(),
            ConfirmPassword: $('#ConfirmPassword').val()           
        };
        $.ajax({
            url: "/ChangePassword/ChangeCurrentPassword",
            data: JSON.stringify(objVmUser),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            success: function (response) {
                debugger;
                if (response.status == true) {
                    toastr.success(response.message);
                    Clear();
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

function Clear() {
    $('#CurrentPassword').val('');
    $('#NewPassword').val('');
    $('#ConfirmPassword').val('');   
}