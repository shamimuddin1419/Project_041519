$(document).ready(function () {

});
//var planListRender = function () {
//    $.ajax({
//        url: "/User/InsertUser",
//        data: JSON.stringify(objVmUser),
//        type: "POST",
//        contentType: "application/json;charset=utf-8",
//        success: function (response) {
//            debugger;
//            if (response.status == true) {
//                //toastr.success(response.message);
//                $('#UserName').val('');
//                $('#Email').val('');
//                $('#Password').val('');
//                $('#ConfirmPassword').val('');
//                $('#Referrer').val('');
//                $('#successAlert').show();
//            } else {
//                toastr.error(response.message);
//            }
//        },
//        error: function (response) {
//            toastr.error("error!");
//        }
//}