$(document).ready(function () {
    loadInitialization();
    LoadPackageRequestList();
});

function loadInitialization() {   
    Clear();   
    $('#btnApprove').hide();
    $('#btnReject').hide();
    $('#btnList').hide();
    $('#ListId').show();
    $('#MasterDetailId').hide();
};

$('#btnList').click(function () {
    $('#btnApprove').hide();
    $('#btnReject').hide();
    $('#btnList').hide();   
    $('#MasterDetailId').hide();
    $('#ListId').show();
    LoadPackageRequestList();
   
});

$('#btnApprove').click(function () {
    if (confirm("Are you sure want to Approve the Withdraw?") == true) {
        if ($('#hidWithdrawalId').val() == '0' || $('#hidWithdrawalId').val() == null) {
            toastr.warning("Select Withdrawal Information")
        }
        else {
            var objVmWithdrawal = {
                WithdrawId: $('#hidWithdrawalId').val()               
            };
            $.ajax({
                url: "/WithdrawalApproval/WithdrawalApprove",
                data: JSON.stringify(objVmWithdrawal),
                type: "POST",
                contentType: "application/json;charset=utf-8",
                success: function (response) {
                    debugger;
                    if (response.status == true) {
                        toastr.success(response.message);
                        loadInitialization();
                        LoadPackageRequestList();
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
$('#btnReject').click(function () {
    if (confirm("Are you sure want to reject the Withdraw?") == true) {
        if ($('#hidWithdrawalId').val() == '0' || $('#hidWithdrawalId').val() == null) {
            toastr.warning("Select Withdrawal Information")
        }
        else {
            var objVmWithdrawal = {
                WithdrawId: $('#hidWithdrawalId').val()
            };
            $.ajax({
                url: "/WithdrawalApproval/WithdrawalReject",
                data: JSON.stringify(objVmWithdrawal),
                type: "POST",
                contentType: "application/json;charset=utf-8",
                success: function (response) {
                    debugger;
                    if (response.status == true) {
                        toastr.success(response.message);
                        loadInitialization();
                        LoadPackageRequestList();
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
    $('#hidWithdrawalId').val('0');
    $('#txtUserName').val('');
    $('#txtBalanceType').val('');
    $('#txtReceivePaymnetMethod').val('');
    $('#txtWalletAddress').val('');
    $('#txtWithdrawAmount').val('');
    $('#txtRemark').val('');
}

function LoadPackageRequestList() {
    var table = $('#ListTableId').DataTable();
    table.destroy();
    $('#ListTableId').DataTable({        
        "ajax":
        {
            "url": '/WithdrawalApproval/GetWithdrawalPendingList',
            "type": "POST",
            "datatype": "json"
        },
        "responsive": true,
        //"processing": true,
        //"serverSide": true,
        "columns":
            [
                {
                    "data": null,
                    'width': '5%',
                    "className": "center",
                    render: function (data, type, row) {
                        return '<button type="button" onclick = "GetUserPackageRequestById(' + data.withdrawId + ')" class="btn info"><i class="fa fa-pencil"></i></a>'
                    }

                },               

                { "data": "stringCreateDate", "autoWidth": true },
                { "data": "userFullName", "autoWidth": true },
                { "data": "withdrawBalanceType", "autoWidth": true },
                { "data": "withdrawAmount", "autoWidth": true }, 
                { "data": "paymentMethod", "autoWidth": true },               
                { "data": "walletAddress", "autoWidth": true } ,
                { "data": "remarks", "autoWidth": true }               
            ],       
        "order": [1, "asc"],
        "processing": "true",
        "language": {
            "processing": "processing... please wait"
        },


    });
}

function GetUserPackageRequestById(id) {
    $.get('/WithdrawalApproval/GetWithdrawalInfoById/' + id, function (data) {
        $('#hidWithdrawalId').val(data.data.withdrawId); 
        $('#txtUserName').val(data.data.userFullName);
        $('#txtBalanceType').val(data.data.withdrawBalanceType);
        $('#txtReceivePaymnetMethod').val(data.data.paymentMethod);
        $('#txtWalletAddress').val(data.data.walletAddress);
        $('#txtWithdrawAmount').val(data.data.withdrawAmount);
        $('#txtRemark').val(data.data.remarks);
      
        $('#btnApprove').show();
        $('#btnReject').show();
        $('#btnList').show();
        
        $('#MasterDetailId').show();
        $('#ListId').hide();
        GetUserWiseWithdrawBalance(id);
    });
}

function GetUserWiseWithdrawBalance(id) {
    $.get('/WithdrawalApproval/GetUserWiseWithdrawBalance/' + id, function (data) {
        $('#lblMinBalance').text(data.data.availableTaskEarn);
        $('#lblCommission').text(data.data.availableCommissionEarn);
    });
}