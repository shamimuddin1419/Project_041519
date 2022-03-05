(function () {

    //$('#fromDatePickerId').datetimepicker();
    $('#fromDatePickerId').datetimepicker({
        useCurrent: false,
        format: 'DD-MMM-YYYY'
    });
    $('#toDatePickerId').datetimepicker({
        useCurrent: false,
        format: 'DD-MMM-YYYY'
    });
    $('#btnSearchId').click(() => {
        var table = $('#ListTableId').DataTable({
            'ajax': `${$('#searchUrlId').val()}?fromDate=${$('#fromDateId').val()}&toDate=${$('#toDateId').val()}`,
            "order": [] ,
            "responsive": true,
            /*"columnDefs": [{ orderable: false, targets: [4] }],*/
            "destroy": true,
            "columns": [

                { "data": "transactionDate", "autoWidth": true },
                { "data": "transactionAmt", "autoWidth": true },
                { "data": "remarks", "autoWidth": true },
                { "data": "joiningDate", "autoWidth": true },
                { "data": "currentDuration", "autoWidth": true },
                { "data": "expiryDate", "autoWidth": true },
            ]
        });

    });
})();

