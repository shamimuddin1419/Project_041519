
$('#btnGenerateDailyIncome').click(function () {     
    $.ajax({
            url: "/GenerateDailyTaskIncome/GenerateDailyTaskIncome",           
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
});
