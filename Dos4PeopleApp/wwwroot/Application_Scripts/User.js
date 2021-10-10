
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
    else if ($('#Email').val() == '') {
        toastr.warning("Provide Email")
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
    else {
        var objVmUser = {
            UserName: $('#UserName').val(),
            Email: $('#Email').val(),
            Password: $('#Password').val(),
            ConfirmPassword: $('#ConfirmPassword').val(),
            ReferrelUserName: $('#Referrer').val(),
        };
        $.ajax({
            url: "/User/InsertUser",
            data: JSON.stringify(objVmUser),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            success: function (response) {
                debugger;
                if (response.status == true) {
                    toastr.success(response.message);
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
