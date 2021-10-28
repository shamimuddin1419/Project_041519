$(document).ready(function () {
    loadInitialization();
});
function loadInitialization() {
    Clear();
    LoadPaymentMethod();
   
};

function LoadPaymentMethod() {    
    var Packages = []
    $.get('/BuyPlan/GetPaymentMethodTypeList', function (data) {
        Packages = data.data;
        $('#ddlPaymentMethodType').select2({
            data: Packages
        });
    });
}
$("#ddlPaymentMethodType").change(function () {
    let id = $('#ddlPaymentMethodType').val();
    var paymentMethods = []     
    $.get('/BuyPlan/GetPaymentMethodList/' + id, function (data) {
        paymentMethods = data.data;
        $('#ddlPaymentMethod').select2({           
            data: paymentMethods
        });
    });   
});

$('#btnRequestForPaymentAccept').click(function () {
    if (confirm("Are you sure want to request for payment accept ?") == true) {

        if ($('#ddlPaymentMethodType').val() == null || $('#ddlPaymentMethodType').val() == '0') {
            toastr.warning('Provide Payment Type');
        }
        else if ($('#ddlPaymentMethod').val() == null || $('#ddlPaymentMethod').val() == '0') {
            toastr.warning('Provide Payment Number');
        }
        else if ($('#txtAmount').val() == null || $('#txtAmount').val() == '') {
            toastr.warning('Provide Amount');
        }
        else if ($('#txtReference').val() == '') {
            toastr.warning('Provide Reference');
        }        
        else {
            var objUserPackageReq = {
                PaymentMethodId: $('#ddlPaymentMethod').val(),
                Amount: $('#txtAmount').val(),
                Reference: $('#txtReference').val(),
                Remarks: $('#txtRemark').val()
            };
            $.ajax({
                url: "/BuyPlan/RequestForPaymentAccept",
                data: JSON.stringify(objUserPackageReq),
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
    }

});

function Clear() {  
    
    $('#ddlPaymentMethodType').val('0');
    $('#ddlPaymentMethod').val('0');
    $('#txtRemark').val('');
    $('#txtReference').val('');  
    $('#txtAmount').val(''); 
    $('.select2').trigger('change');

}