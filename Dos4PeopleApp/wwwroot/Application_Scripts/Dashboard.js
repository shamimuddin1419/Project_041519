$('#copyBtn').click(function () {
    debugger;
    /* Get the text field */
    var copyText = $('#refferelId').text();


    /* Copy the text inside the text field */
    navigator.clipboard.writeText(copyText);

    /* Alert the copied text */
    toastr.info("Copied");
})

$('#withdrawBtn').click(function () {  
    toastr.info("Please Check Your Withdrawal Amount And Withdrawal Date & Time");
})