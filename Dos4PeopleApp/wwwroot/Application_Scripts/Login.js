
$('#btnLogin').click(function () {
    if ($('#UserName').val() == '')
    {
        toastr.warning("Provide Username or Email")
    }
    else if ($('#Password').val() == '') {
        toastr.warning("Provide Password")
    }    
    else {
        var objVmUser = {
            UserName: $('#UserName').val(),
            Password: $('#Password').val()
        };
        $.ajax({
            url: "/Login/UserLogin",
            data: JSON.stringify(objVmUser),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            success: function (response) {
                debugger;
                if (response.status == true) {
                    window.location.href = response.targetUrl;
                } else {
                    toastr.error("Account Not Found");
                }
            },
            error: function (response) {              
                toastr.error("error!"); 
            }
        });
    }
});


$('#lblLogout').click(function () {
        $.ajax({
            url: "/Login/Logout",           
            type: "POST",
            contentType: "application/json;charset=utf-8",
            success: function (response) {
                debugger;
                if (response.status == true) {
                    window.location.href = response.targetUrl;
                } else {
                    toastr.error(response.targetUrl);
                }
            },        
            error: function (response) {
                toastr.error("error!");
            }
        });    
});
