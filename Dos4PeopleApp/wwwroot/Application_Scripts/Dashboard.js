$(document).ready(function () {
    GetDashboardFirstCardData();
    GetDashboardGraphData();
    
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
$('#withdrawBtn').click(function () {   
    toastr.info("Please Check Your Withdrawal Amount And Withdrawal Date & Time.");
})

var GetDashboardFirstCardData = function () {
    var url = $('#GetDashboardFirstCardDataURLId').val()
    $.get(url, function (data) {
        debugger;
        if (data.success) {
            $('#ReferrelEarnValId').text(data.data.totalReferrelCommission);
            $('#WorkCommissionValId').text(data.data.totalWorkCommission);
            $('#TaskCommissionValId').text(data.data.totalTaskEarn);

            $('#CurrBalValId').text(data.data.currentBalance);
            $('#TodayEarnValId').text(data.data.todaysEarn);
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