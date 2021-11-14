$(document).ready(function () { 
    GetTotalNumberOfMembers();
    LoadMemberList();
});

function GetTotalNumberOfMembers() {   
    $.get('/MemberList/GetTotalNumberOfUsers', function (data) {        
        $('#lblTotalNumberOfMembers').text(data.data);
    });
}

function LoadMemberList() {
    var table = $('#ListTableId').DataTable();
    table.destroy();
    $('#ListTableId').DataTable({    
        "lengthMenu": [[5, 10, 25, 50, -1], [5, 10, 25, 50, "All"]],
        //"responsive": true,
        "scrollX": true,       
        "serverSide": true,
        "processing": "true",  
        "sort": true,
        "language": {
            "processing": "processing... please wait"
        },
        "ajax":
        {
            "url": '/MemberList/GetMemberList',
            "type": "POST",
            "datatype": "json"
        },      
        "columns":
            [
                { "data": "fullName", "autoWidth": true, "orderable": true },
                { "data": "userName", "autoWidth": true },
                { "data": "email", "autoWidth": true },
                { "data": "mobile", "autoWidth": true }, 
                { "data": "sponsored", "autoWidth": true },               
                { "data": "package", "autoWidth": true } ,
                { "data": "joinDate", "autoWidth": true },
                { "data": "duration", "autoWidth": true },
                { "data": "expaire", "autoWidth": true },
                 { "data": "status", "autoWidth": true }
            ], "order": [[0, "desc"]]  
          
    });
}

