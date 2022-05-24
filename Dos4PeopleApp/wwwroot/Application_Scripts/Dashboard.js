$(document).ready(function () {
    GetDashboardFirstCardData();
    GetDashboardGraphData();
    //GetIndividualUnseenChatListByReceiverId();
    
})

$('#copyBtn').click(function () {
    debugger;
    /* Get the text field */
    var copyText = document.getElementById("refferelId");
    copyText.select();
    document.execCommand("Copy")

    /* Copy the text inside the text field */
    //navigator.clipboard.writeText(copyText);

    /* Alert the copied text */
    toastr.info("Copied");
})
//$('#withdrawBtn').click(function () {   
//    toastr.info("Please Check Your Withdrawal Amount And Withdrawal Date & Time.");
//})

var GetDashboardFirstCardData = function () {
    var url = $('#GetDashboardFirstCardDataURLId').val()
    $.get(url, function (data) {
        debugger;
        if (data.success) {
            $('#ReferrelEarnValId').text(parseFloat(data.data.totalReferrelCommission).toFixed(2));
            $('#WorkCommissionValId').text(parseFloat(data.data.totalWorkCommission).toFixed(2));
            $('#TaskCommissionValId').text(parseFloat(data.data.totalTaskEarn).toFixed(2));

            $('#CurrBalValId').text(parseFloat(data.data.currentBalance).toFixed(2));
            $('#TodayEarnValId').text(parseFloat(data.data.todaysEarn).toFixed(2));
            $('#CurrentPackageNameId').text(data.data.currentPackageName);
            $('#CurrPackValId').text(data.data.currentPackageValue);

        }
    });
}
var GetDashboardGraphData = function () {
    var referrelChartId = $("#direct-referrel-column-chart");
    var moneyCreditedChartId = $("#money-credited-column-chart");

    // Chart Options
    var chartOptions = {
        // Elements options apply to all of the options unless overridden in a dataset
        // In this case, we are setting the border of each bar to be 2px wide and green
        elements: {
            rectangle: {
                borderWidth: 2,
                borderColor: 'rgb(0, 255, 0)',
                borderSkipped: 'bottom'
            }
        },
        scales: {
            yAxes: [{
                scaleLabel: {
                    display: true,
                    labelString: 'probability'
                }
            }]
        },
        responsive: true,
        maintainAspectRatio: false,
        responsiveAnimationDuration: 500,
        legend: {
            position: 'top',
        },
        scales: {
            xAxes: [{
                display: true,
                gridLines: {
                    color: "#5F5AF0",
                    drawTicks: false,
                },
                scaleLabel: {
                    display: true,
                }
            }],
            yAxes: [{
                display: true,
                gridLines: {
                    color: "#5F5AF0",
                    drawTicks: false,
                },
                scaleLabel: {
                    display: true,
                }
            }]
        },

    };

    // Chart Data
    var chartDataForReferrel = {
        labels: ["12", "15", "16", "17", "18"],
        datasets: [{
            label: "DIRECT REFERRALS COUNT",
            data: [0, 0, 42.5, 0, 0],
            backgroundColor: "#5F5AF0",
            hoverBackgroundColor: "rgba(22,211,154,.9)",
            borderColor: "transparent"
        }]
    };

    $.get($('#GetDashboardGraphURLId').val(), function (data) {
        if (data.success) {
            var chartDataForReferrel = {
                labels: data.data.map(result => result.transactionDate),
                datasets: [{
                    label: "DIRECT REFERRALS COUNT",
                    data: data.data.map(result => result.referralCount),
                    backgroundColor: "#5F5AF0",
                    hoverBackgroundColor: "rgba(22,211,154,.9)",
                    borderColor: "transparent"
                }]
            };
            var configForReferrel = {
                type: 'bar',
                // Chart Options
                options: chartOptions,

                data: chartDataForReferrel
            };
            // Create the chart
            var referrelChart = new Chart(referrelChartId, configForReferrel);
            var chartDataForMoney = {
                labels: data.data.map(result => result.transactionDate),
                datasets: [{
                    label: "MONEY CREDITED TO YOUR ACCOUNT",
                    data: data.data.map(result => result.transactionAmt),
                    backgroundColor: "#5F5AF0",
                    hoverBackgroundColor: "rgba(22,211,154,.9)",
                    borderColor: "transparent"
                }]
            };
            var configForMoney = {
                type: 'bar',
                // Chart Options
                options: chartOptions,

                data: chartDataForMoney
            };
            var moneyChart = new Chart(moneyCreditedChartId, configForMoney);
        }
    })
}

//var GetIndividualUnseenChatListByReceiverId = function () {
//    $.get('/AdminIndividualChat/GetIndividualUnseenChatListByReceiverId', function (response) {
//        var ChatHistory = '';
//        if (response.status == true) {
//            var itemRender
//            var result = response.data;
//            $('#lblNumberOfMessage').text(result.length);
//            $('#lblNumberOfMessageForMobile').text(result.length);
//            $('#lblSubNumberOfMessage').text(result.length);
//            $.each(result, function (index, value) {
//                if (value.isAdmin) {
//                    itemRender =`<a href="/AdminIndividualChat">
//                                            <div class="media">
//                                                <div class="media-left align-self-center"><i class="ft-plus-square icon-bg-circle bg-cyan"></i></div>
//                                                <div class="media-body">                                                   
//                                                    <p class="notification-text font-small-3 text-muted"> ${value.messageBody}</p><small>
//                                                        <time class="media-meta text-muted" datetime="2015-06-11T18:29:20+08:00"> ${value.createdDateString}</time>
//                                                    </small>
//                                                </div>
//                                            </div>
//                                        </a>`
//                ChatHistory = ChatHistory + itemRender;
//            }else {
//                    itemRender = `<a href="/UserIndividualChat">
//                                            <div class="media">
//                                                <div class="media-left align-self-center"><i class="ft-plus-square icon-bg-circle bg-cyan"></i></div>
//                                                <div class="media-body">                                                   
//                                                    <p class="notification-text font-small-3 text-muted"> ${value.messageBody}</p><small>
//                                                        <time class="media-meta text-muted" datetime="2015-06-11T18:29:20+08:00"> ${value.createdDateString}</time>
//                                                    </small>
//                                                </div>
//                                            </div>
//                                        </a>`
//                    ChatHistory = ChatHistory + itemRender;
//            }

//            });

//            $('#ChatHistory').html(ChatHistory);

//        } else {
//            toastr.error(response.message);
//        }

//    });
//}