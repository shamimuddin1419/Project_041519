$(document).ready(function () {
    loadInitialization();
    LoadPackageList();
});

function loadInitialization() {   
    Clear();   
    $('#btnSave').hide();
    $('#btnReject').hide();
    $('#btnList').hide();
    $('#ListId').show();
    $('#MasterDetailId').hide();
};


$('#btnList').click(function () {
    $('#btnSave').hide();
    $('#btnReject').hide();
    $('#btnList').hide();   
    $('#MasterDetailId').hide();
    $('#ListId').show();
    LoadPackageList();
   
});







$('#btnDelete').click(function () {
    if (confirm("Are you sure want to delete the package?") == true) {
        if ($('#hidPackageId').val() == '0' || $('#hidPackageId').val() == null) {
            toastr.warning("Select Package")
        }
        else {
            var objPackage = {
                PackageId: $('#hidPackageId').val()               
            };
            $.ajax({
                url: "/Package/DeletePackage",
                data: JSON.stringify(objPackage),
                type: "POST",
                contentType: "application/json;charset=utf-8",
                success: function (response) {
                    debugger;
                    if (response.status == true) {
                        toastr.success(response.message);
                        NewButtonAction();
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



$('#btnSave').click(function () {
    if ($('#ddlPackageCategory').val() == null || $('#ddlPackageCategory').val() == '0') {
        toastr.error('Provide Package Category');
    }
    else if ($('#txtPackageName').val() == '') {
        toastr.warning("Provide Package Name")
    }
    else if ($('#txtPackageValue').val() == '') {
        toastr.warning("Provide Package Value")
    }
    else if ($('#txtPlanDuration').val() == '') {
        toastr.warning("Provide Plan Duration")
    }
    else if ($('#txtDailyTaskCount').val() == '') {
        toastr.warning("Provide Daily Task Count")
    }
    else if ($('#txtActionworth').val() == '') {
        toastr.warning("Provide Action worth")
    }
    else if ($('#txtDailyEarn').val() == '') {
        toastr.warning("Provide Daily Earn")
    }
    else if ($('#txtWeeklyEarn').val() == '') {
        toastr.warning("Provide Weekly Earn")
    }
    else if ($('#txtMonthlyEarn').val() == '') {
        toastr.warning("Provide Monthly Earn")
    }
    else if ($('#txtYearlyEarn').val() == '') {
        toastr.warning("Provide Yearly Earn")
    }
    else if ($('#txtReferralEarnlevel').val() == '') {
        toastr.warning("Provide Referral Earn Label")
    }
    else if ($('#txtWorkComissionlevel').val() == '') {
        toastr.warning("Provide Work Comission Label")
    }
    else if ($('#txtPotentialReferralEarn').val() == '') {
        toastr.warning("Provide Potential Referral Earn")
    }
    else if ($('#txtTargetPotentialYearlyIncome').val() == '') {
        toastr.warning("Provide Target Potential Yearly Income")
    }
    else if ($('#txtTCBOntheMainInvestment').val() == '') {
        toastr.warning("Provide TCB On the Main Investment")
    }
    else if ($('#txtPotentialYearlyMinIncome').val() == '') {
        toastr.warning("Provide Potential Yearly Min Income")
    }
    else if ($('#txtRemark').val() == '') {
        toastr.warning("Provide Remark")
    }
    else {
        var objPackage = {
            PackageCategoryId: $('#ddlPackageCategory').val(),
            PackageName: $('#txtPackageName').val(),
            PackageValue: $('#txtPackageValue').val(),
            PackageDurationDays: $('#txtPlanDuration').val(),
            DailyTaskCount: $('#txtDailyTaskCount').val(),
            PerClickValue: $('#txtActionworth').val(),
            DailyValue: $('#txtDailyEarn').val(),
            WeeklyValue: $('#txtWeeklyEarn').val(),
            MonthlyValue: $('#txtMonthlyEarn').val(),
            YearlyValue: $('#txtYearlyEarn').val(),
            ReferralEarn: $('#txtReferralEarnlevel').val(),
            WorkCommission: $('#txtWorkComissionlevel').val(),
            PotentialReferralEarn: $('#txtPotentialReferralEarn').val(),
            TargetPotentialYearlyIncome: $('#txtTargetPotentialYearlyIncome').val(),
            TCBOnMainInvestPer: $('#txtTCBOntheMainInvestment').val(),
            PotentialYearlyIncome: $('#txtPotentialYearlyMinIncome').val(),
            Remarks: $('#txtRemark').val(),
            IsActive: $('#IsActive').is(":checked"),
        };
        $.ajax({
            url: "/Package/InsertPackage",
            data: JSON.stringify(objPackage),
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
    $('#txtPaymentType').val('');
    $('#txtPaymentNumber').val('');
    $('#txtRemark').val('');
    $('#txtReference').val('');    
}

function LoadPackageList() {
    var table = $('#ListTableId').DataTable();
    table.destroy();
    $('#ListTableId').DataTable({        
        "ajax":
        {
            "url": '/PackageApproval/GetUserPackageRequestPendingList',
            "type": "POST",
            "datatype": "json"
        },
        //"responsive": true,
        //"processing": true,
        //"serverSide": true,
        "columns":
            [
                {
                    "data": null,
                    'width': '5%',
                    "className": "center",
                    render: function (data, type, row) {
                        return '<button type="button" onclick = "GetPackageById(' + data.userPackageRequestId + ')" class="btn info"><i class="fa fa-pencil"></i></a>'
                    }

                },               

                { "data": "stringCreateDate", "autoWidth": true },
                { "data": "userFullName", "autoWidth": true },
                { "data": "packageName", "autoWidth": true },
                { "data": "paymentMethodName", "autoWidth": true }, 
                { "data": "paymentMethodTypeName", "autoWidth": true } ,
                { "data": "reference", "autoWidth": true },
                { "data": "remarks", "autoWidth": true } 
            ],       
        "order": [1, "asc"],
        "processing": "true",
        "language": {
            "processing": "processing... please wait"
        },


    });
}

function GetPackageById(id) {
    $.get('/Package/GetPackageInfoById/' + id, function (data) {
        $('#hidPackageId').val(data.data.packageId);
        $('#ddlPackageCategory').val(data.data.packageCategoryId);
        $('#txtPackageName').val(data.data.packageName);
        $('#txtPackageValue').val(data.data.packageValue);
        $('#txtPlanDuration').val(data.data.packageDurationDays);
        $('#txtDailyTaskCount').val(data.data.dailyTaskCount);
        $('#txtActionworth').val(data.data.perClickValue);
        $('#txtDailyEarn').val(data.data.dailyValue);
        $('#txtWeeklyEarn').val(data.data.weeklyValue);
        $('#txtMonthlyEarn').val(data.data.monthlyValue);
        $('#txtYearlyEarn').val(data.data.yearlyValue);
        $('#txtReferralEarnlevel').val(data.data.referralEarn);
        $('#txtWorkComissionlevel').val(data.data.workCommission);
        $('#txtPotentialReferralEarn').val(data.data.potentialReferralEarn);
        $('#txtTargetPotentialYearlyIncome').val(data.data.potentialReferralEarn);
        $('#txtTCBOntheMainInvestment').val(data.data.targetPotentialYearlyIncome);
        $('#txtPotentialYearlyMinIncome').val(data.data.potentialYearlyIncome);
        $('#txtRemark').val(data.data.remarks);
        $('.select2').trigger('change');   
        $('#IsActive').prop('checked', data.data.isActive),
       $('.switchery').trigger('click');
        $('#btnSave').hide();
        $('#btnNew').show();
        $('#btnDelete').show();
        if (data.data.isPublished == true) {
            $('#btnPublish').hide();
        } else {
            $('#btnPublish').show();
        }
        $('#MasterDetailId').show();
        $('#ListId').hide();
    });
}