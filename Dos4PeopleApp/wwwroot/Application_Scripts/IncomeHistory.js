(function () {

    const loadSearchData = () => {
        var table = $('#ListTableId').DataTable({
            'ajax': `${$('#searchUrlId').val()}?fromDate=${$('#fromDateId').val()}&toDate=${$('#toDateId').val()}`,
            "order": [],
            "responsive": true,
            /*"columnDefs": [{ orderable: false, targets: [4] }],*/
            "destroy": true,
            "columns": [

                { "data": "transactionDate", width: '12%' },
                { "data": "transactionAmt", width: '8%' },
                { "data": "remarks", width:'44%' },
                { "data": "joiningDate", width: '12%' },
                { "data": "currentDuration", width: '8%'  },
                { "data": "expiryDate", width: '12%'  },
            ],
            footerCallback: function (row, data, start, end, display) {
                var api = this.api();
                var allValues = data.map(x => x.transactionAmt)
                var total = allValues.reduce((prevObj, newObj) => {
                    return prevObj + newObj;
                },0)
                $(api.column(1).footer()).html(total);
            }
        });
    }
    $('#fromDatePickerId').datetimepicker({
        useCurrent: false,
        format: 'DD-MMM-YYYY'
    });
    $('#toDatePickerId').datetimepicker({
        useCurrent: false,
        format: 'DD-MMM-YYYY'
    });
    $('#btnSearchId').click();
    $(document).ready(() => {
        loadSearchData();
    })

    
    $('#btnSearchId').click(() => {
        loadSearchData();

    });
})();

