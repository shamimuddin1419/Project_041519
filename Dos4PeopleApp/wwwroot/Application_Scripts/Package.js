$(document).ready(function () {
    loadInitialization();
});

function loadInitialization() {   
    Clear();
    LoadPackageCategory();  
    $('#btnDelete').hide();
    $('#btnNew').hide();
    $('#btnPublish').hide();    
    $('#PackageListId').hide();
};

function LoadPackageCategory() {
    $('#ReferralEarnlabel').text('');
    $('#WorkComissionlabel').text('');
    var Packages = []
    $.get('/Package/GetPackageCategoryList', function (data) {
        Packages = data.data;
        $('#ddlPackageCategory').select2({
            data: Packages
        });
    });
}

$('#btnList').click(function () {
    $('#btnSave').hide();
    $('#btnDelete').hide();
    $('#btnPublish').hide();   
    $('#btnNew').show();
    $('#PackageMasterDetailId').hide();
    $('#PackageListId').show();
    LoadPackageList();
   
});

$('#btnNew').click(function () {
    NewButtonAction();
});

function NewButtonAction() {
    $('#btnSave').show();
    $('#btnDelete').hide();
    $('#btnPublish').hide();
    $('#btnNew').hide();
    $('#PackageMasterDetailId').show();
    $('#PackageListId').hide();
    Clear();
} 

$("#ddlPackageCategory").change(function () {
    $('#ReferralEarnlabel').text('');
    $('#WorkComissionlabel').text('');

    var objPackageCategory = {
        PackageCategoryId: $('#ddlPackageCategory').val()
    };
    $.ajax({
        url: "/Package/GetPackageCateMaxLevel",
        data: JSON.stringify(objPackageCategory),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        success: function (response) {
            debugger;
            if (response.status == true) {
                $('#ReferralEarnlabel').text(response.data.maxLevel);
                $('#WorkComissionlabel').text(response.data.maxLevel);

            } else {
                toastr.error(response.message);
            }
        },
        error: function (response) {
            toastr.error("error!");
        }
    });
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

$('#btnPublish').click(function () {
    if (confirm("Are you sure want to publish the package?") == true) {
        if ($('#hidPackageId').val() == '0' || $('#hidPackageId').val() == null) {
            toastr.warning("Select Package")
        }
        else {
            var objPackage = {
                PackageId: $('#hidPackageId').val(),
                IsActive: $('#IsActive').is(":checked")               
            };
            $.ajax({
                url: "/Package/UpdatePackage",
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
                    LoadPackageCategory();
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
    $('#hidPackageId').val("0");
    $('#ddlPackageCategory').val('0');
    $('#txtPackageName').val('');
    $('#txtPackageValue').val('0');
    $('#txtPlanDuration').val('0');
    $('#txtDailyTaskCount').val('0');
    $('#txtActionworth').val('0');
    $('#txtDailyEarn').val('0');
    $('#txtWeeklyEarn').val('0');
    $('#txtMonthlyEarn').val('0');
    $('#txtYearlyEarn').val('0');
    $('#txtReferralEarnlevel').val('0');
    $('#txtWorkComissionlevel').val('0');
    $('#txtPotentialReferralEarn').val('0');
    $('#txtTargetPotentialYearlyIncome').val('0');
    $('#txtTCBOntheMainInvestment').val('0');
    $('#txtPotentialYearlyMinIncome').val('0');
    $('#txtRemark').val('');
    $('.select2').trigger('change');   
    $('#IsActive').prop('checked', true);
    $('.switchery').trigger('click')
}

function LoadPackageList() {
    var table = $('#PackageListTableId').DataTable();
    table.destroy();
    $('#PackageListTableId').DataTable({        
        "ajax":
        {
            "url": '/Package/GetPackageList',
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
                        return '<button type="button" onclick = "GetPackageById(' + data.packageId + ')" class="btn info"><i class="fa fa-pencil"></i></a>'
                    }

                },               

                { "data": "packageCategory", "autoWidth": true },
                { "data": "packageName", "autoWidth": true },
                { "data": "packageValue", "autoWidth": true },
                { "data": "isPublished", "autoWidth": true }, 
                { "data": "isActive", "autoWidth": true } 
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
        $('#PackageMasterDetailId').show();
        $('#PackageListId').hide();
    });
}