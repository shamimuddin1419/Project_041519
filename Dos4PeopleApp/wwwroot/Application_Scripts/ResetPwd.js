
function validateEmail() {
    let email = $('#Email').val()
    var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
    return emailReg.test(email);
}

$('#btnSendEmail').click(function () {
    if ($('#Email').val() == '')
    {
        toastr.warning("Provide Email")
    } 
    else if (!validateEmail()) {
        toastr.warning("Provide valid email address")
    }
    else {
        var objVmUser = {           
            Email: $('#Email').val()
        };
        $.ajax({
            url: "/Resetpwd/SendEmail",
            data: JSON.stringify(objVmUser),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            success: function (response) {
                debugger;
                if (response.status == true) {
                    toastr.success(response.message);                   
                    $('#Email').val('');
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
