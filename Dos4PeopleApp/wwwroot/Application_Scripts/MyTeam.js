(function () {
    $(document).ready(() => {
        LoadUserList();
    })
    const LoadUserList = ()=> {
        var table = $('#ListTableId').DataTable({
            'ajax': $('#searchUrlId').val(),
            "order": [],
            "responsive": true,
            /*"columnDefs": [{ orderable: false, targets: [4] }],*/
            "destroy": true,
            "columns": [

                { "data": "fullName", "autoWidth": true, "orderable": true },
                { "data": "userName", "autoWidth": true },
                { "data": "userLevel", "autoWidth": true },
                { "data": "email", "autoWidth": true },
                { "data": "mobile", "autoWidth": true },
                { "data": "sponsored", "autoWidth": true },
                { "data": "package", "autoWidth": true },
                { "data": "joinDate", "autoWidth": true },
                { "data": "duration", "autoWidth": true },
                { "data": "expire", "autoWidth": true },
                { "data": "status", "autoWidth": true }
            ]
        });
    }
}
)()