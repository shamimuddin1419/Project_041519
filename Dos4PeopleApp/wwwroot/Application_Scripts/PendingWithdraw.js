$(document).ready(function () {
    loadInitialization();
    GetWithdrawBalanceByUserId();
    GetWithdrawalListByUserId();
    GetServiceChargePercentage();
});
function loadInitialization() {
    Clear();
    LoadPaymentMethod();
};

function GetWithdrawBalanceByUserId() {
    $.get('/Withdrawal/GetWithdrawBalanceByUserId/', function (data) {
        $('#lblMinBalance').text(data.data.availableTaskEarn);
        $('#lblCommission').text(data.data.availableCommissionEarn);
    });
}

function GetServiceChargePercentage() {
    $.get('/Withdrawal/GetServiceChargePercentage/', function (data) {
        $('#lblMinBalanceSerCharge').text(data.data.mainBalanceServiceChargePer);
        $('#lblCommissionSerCharge').text(data.data.commissionBalanceServiceChargePer);
    });
}

function LoadPaymentMethod() {
    var Packages = []
    $.get('/BuyPlan/GetPaymentMethodTypeList', function (data) {
        Packages = data.data;
        $('#ddlReceivePaymentMethod').select2({
            data: Packages
        });

    });
}
//$("#ddlReceivePaymentMethod").change(function () {    
//    changeicon();
//});
function changeicon() {
    $('#IdPMImage').show();
    var PaymentType = $("#ddlReceivePaymentMethod option:selected").text();
    if (PaymentType == 'Payoneer') {
        $('#IdPMImage').attr('src', '/Content/Images/Payoneer.jpeg');
    } else if (PaymentType == 'Perfect Money') {
        $('#IdPMImage').attr('src', '/Content/Images/PerfectMoney.jpeg');
    }
    else if (PaymentType == 'Payeer') {
        $('#IdPMImage').attr('src', '/Content/Images/Payeer.jpeg');
    }
    else if (PaymentType == 'BTC') {
        $('#IdPMImage').attr('src', '/Content/Images/BTC.jpeg');
    } else {
        $('#IdPMImage').attr('src', '');
        $('#IdPMImage').hide();
    }
}

$("#txtWIthdrawAmount").blur(function () {

    if ($('#isMainBalance').is(":checked") == true) {
        if (parseFloat($('#lblMinBalance').text()) < parseFloat($('#txtWIthdrawAmount').val())) {
            toastr.warning('Withdraw Amount cannot be greater than Main Balance');
            $('#lblServiceCharge').text('');
            return;
        }
    }
    if ($('#isCommission').is(":checked") == true) {
        if (parseFloat($('#lblCommission').text()) < parseFloat($('#txtWIthdrawAmount').val())) {
            toastr.warning('Withdraw Amount cannot be greater than Commission Balance');
            $('#lblServiceCharge').text('');
            return;
        }
    }

    if ($('#isMainBalance').is(":checked") == false && $('#isCommission').is(":checked") == false) {
        toastr.warning('Please Select Withdrawal Balance Type.');
    }
    else if ($('#isMainBalance').is(":checked") == true && $('#isCommission').is(":checked") == true) {
        toastr.warning('Please Select Just Only One Withdrawal Balance Type.');
    }
    else if ($('#txtWIthdrawAmount').val() == null || $('#txtWIthdrawAmount').val() == '' || $('#txtWIthdrawAmount').val() == '0') {
        toastr.warning('Provide Withdraw Amount.');
        $('#lblServiceCharge').text('');
    }
    else {
        var objVmWithdrawal = {
            WithdrawAmount: $('#txtWIthdrawAmount').val(),
            isMainBalance: $('#isMainBalance').is(":checked"),
            isCommission: $('#isCommission').is(":checked")
        };
        $.ajax({
            url: "/Withdrawal/GetWithdrawServiceCharge",
            data: JSON.stringify(objVmWithdrawal),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            success: function (response) {
                $('#lblServiceCharge').text('Service charge Amount : ' + response.withdrawCharge);
            },
            error: function (response) {
                toastr.error("error!");
            }
        });
    }

});

$('#btnSubmitWithdrawalInfo').click(function () {
    if ($('#isMainBalance').is(":checked") == false && $('#isCommission').is(":checked") == false) {
        Swal.fire({
            icon: 'warning',
            title: 'Warning',
            text: 'Please Select Withdrawal Balance Type.',
        })
    }
    else if ($('#isMainBalance').is(":checked") == true && $('#isCommission').is(":checked") == true) {
        Swal.fire({
            icon: 'warning',
            title: 'Warning',
            text: 'Please Select Just Only One Withdrawal Balance Type.',
        })

    }
    else if ($('#ddlReceivePaymentMethod').val() == null || $('#ddlReceivePaymentMethod').val() == '0') {
        Swal.fire({
            icon: 'warning',
            title: 'Warning',
            text: 'Please Provide Receive Payment Method.',
        })
    }
    else if ($('#txtgWalletAddress').val() == null || $('#txtgWalletAddress').val() == '') {
        Swal.fire({
            icon: 'warning',
            title: 'Warning',
            text: 'Please Provide Wallet Address.',
        })
    }
    else if ($('#txtWIthdrawAmount').val() == null || $('#txtWIthdrawAmount').val() == '' || $('#txtWIthdrawAmount').val() == '0') {
        Swal.fire({
            icon: 'warning',
            title: 'Warning',
            text: 'Please Provide Withdraw Amount.',
        })
    }
    else if ($('#txtPassword').val() == '') {
        Swal.fire({
            icon: 'warning',
            title: 'Warning',
            text: 'Provide Valid Password.',
        })
    }
    else {
        Swal.fire({
            title: 'Are you sure want to request for withdraw ?',
            showCancelButton: true,
            confirmButtonText: 'Yes',
            denyButtonText: `Cancel`,
        }).then((result) => {
            /* Read more about isConfirmed, isDenied below */
            if (result.isConfirmed) {
                var objVmWithdrawal = {
                    PaymentMethod: $('#ddlReceivePaymentMethod').val(),
                    WalletAddress: $('#txtgWalletAddress').val(),
                    WithdrawAmount: $('#txtWIthdrawAmount').val(),
                    Password: $('#txtPassword').val(),
                    Remarks: $('#txtRemark').val(),
                    isMainBalance: $('#isMainBalance').is(":checked"),
                    isCommission: $('#isCommission').is(":checked")
                };
                $.ajax({
                    url: "/Withdrawal/WithdrawalRequest",
                    data: JSON.stringify(objVmWithdrawal),
                    type: "POST",
                    contentType: "application/json;charset=utf-8",
                    success: function (response) {
                        debugger;
                        if (response.status == true) {
                            GetWithdrawalListByUserId();
                           
                            let message = '';
                            let withdrawAmt = $('#txtWIthdrawAmount').val();
                            if ($('#isMainBalance').is(":checked") == true) {
                                message = `Your Requested Main Balance is $${withdrawAmt} Successful For Withdrawals,Thank you for participate with us.`
                            }
                            else {
                                message = `Your Requested Commission is $${withdrawAmt} Successful For Withdrawals,Thank you for participate with us.`
                            }
                            Swal.fire({
                                icon: 'success',
                                title: 'Success',
                                text: message
                            })
                            Clear();
                        } else {
                            Swal.fire({
                                icon: 'error',
                                title: 'Oops...',
                                text: response.message
                            })
                        }
                    },
                    error: function (response) {
                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...',
                            text: 'error!',
                        })
                    }
                });
            }
        })

    }
});
function isNumberKey(txt, evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode == 46) {
        if (txt.value.indexOf('.') === -1) {
            return true;
        } else {
            return false;
        }
    } else {
        if (charCode > 31 &&
            (charCode < 48 || charCode > 57))
            return false;
    }
    return true;
}
$('#isMainBalance').change(function () {
    if ($(this).is(":checked")) {
        $('#isCommission').prop('checked', false);
        $('#lblServiceCharge').text('');
        $('#txtWIthdrawAmount').val('');
    }

});
$('#isCommission').change(function () {
    if ($(this).is(":checked")) {
        $('#isMainBalance').prop('checked', false);
        $('#lblServiceCharge').text('');
        $('#txtWIthdrawAmount').val('');
    }

});
function Clear() {
    $('#ddlReceivePaymentMethod').val('0');
    $('#txtgWalletAddress').val('');
    $('#lblServiceCharge').text('');
    $('#txtWIthdrawAmount').val('');
    $('#txtPassword').val('');
    $('#txtRemark').val('');
    $('#isCommission').prop('checked', false);
    $('#isMainBalance').prop('checked', false);
    $('.select2').trigger('change');
}

function GetWithdrawalListByUserId() {
    var table = $('#ListTableId').DataTable();
    table.destroy();
    $('#ListTableId').DataTable({
        "ajax":
        {
            "url": '/Withdrawal/GetWithdrawalListByUserId',
            "type": "POST",
            "datatype": "json"
        },
        // "responsive": true,
        //"processing": true,
        //"serverSide": true,
        "columns":
            [
                { "data": "stringCreateDate", "autoWidth": true },
                { "data": "withdrawBalanceType", "autoWidth": true },
                { "data": "paymentMethod", "autoWidth": true },
                { "data": "walletAddress", "autoWidth": true },
                { "data": "withdrawAmount", "autoWidth": true },
                { "data": "withdrawStatus", "autoWidth": true }
            ],
        "order": [1, "asc"],
        "processing": "true",
        "language": {
            "processing": "processing... please wait"
        },
    });
}