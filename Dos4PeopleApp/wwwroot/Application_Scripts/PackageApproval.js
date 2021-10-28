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
    if (confirm("Are you sure want to Approve the package?") == true) {
        if ($('#hidPackageRequestId').val() == '0' || $('#hidPackageRequestId').val() == null) {
            toastr.warning("Select Package")
        }
        else {
            var objPackageReq = {
                UserPackageRequestId: $('#hidPackageRequestId').val()               
            };
            $.ajax({
                url: "/PackageApproval/UserPackageRequestApprove",
                data: JSON.stringify(objPackageReq),
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
    if (confirm("Are you sure want to reject the package?") == true) {
        if ($('#hidPackageRequestId').val() == '0' || $('#hidPackageRequestId').val() == null) {
            toastr.warning("Select Package")
        }
        else {
            var objPackageReq = {
                UserPackageRequestId: $('#hidPackageRequestId').val()
            };
            $.ajax({
                url: "/PackageApproval/UserPackageRequestReject",
                data: JSON.stringify(objPackageReq),
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
    $('#hidPackageRequestId').val('0');
    $('#txtPaymentType').val('');
    $('#txtPaymentNumber').val('');
    $('#txtAmount').val('');
    $('#txtReference').val('');
    $('#txtRemark').val(''); 
}

function LoadPackageRequestList() {
    var table = $('#ListTableId').DataTable();
    table.destroy();
    $('#ListTableId').DataTable({        
        "ajax":
        {
            "url": '/PackageApproval/GetUserPackageRequestPendingList',
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
                        return '<button type="button" onclick = "GetPackageRequestInfoById(' + data.userPackageRequestId + ')" class="btn info"><i class="fa fa-pencil"></i></a>'
                    }

                },               

                { "data": "stringCreateDate", "autoWidth": true },
                { "data": "userFullName", "autoWidth": true },
                { "data": "amount", "autoWidth": true },
                { "data": "paymentMethodTypeName", "autoWidth": true }, 
                { "data": "packageName", "autoWidth": true },               
                { "data": "paymentMethodName", "autoWidth": true } ,
                { "data": "reference", "autoWidth": true }               
            ],       
        "order": [1, "asc"],
        "processing": "true",
        "language": {
            "processing": "processing... please wait"
        },


    });
}

function GetPackageRequestInfoById(id) {
    $.get('/PackageApproval/GetUserPackageRequestById/' + id, function (data) {
        $('#hidPackageRequestId').val(data.data.userPackageRequestId);       
        $('#txtPaymentType').val(data.data.paymentMethodTypeName);
        $('#txtPaymentNumber').val(data.data.paymentMethodName);
        $('#txtAmount').val(data.data.amount);
        $('#txtReference').val(data.data.reference);
        $('#txtRemark').val(data.data.remarks);
      
        $('#btnApprove').show();
        $('#btnReject').show();
        $('#btnList').show();
        
        $('#MasterDetailId').show();
        $('#ListId').hide();
    });
}