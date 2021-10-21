/*=========================================================================================
    File Name: column.js
    Description: Chartjs column chart
    ----------------------------------------------------------------------------------------
    Item Name: Stack - Responsive Admin Theme
    Version: 3.2
    Author: PIXINVENT
    Author URL: http://www.themeforest.net/user/pixinvent
==========================================================================================*/

// Column chart
// ------------------------------
$(window).on("load", function(){

    //Get the context of the Chart canvas element we want to select
    var ctx = $("#column-chart");
    var ctxMoney = $("#column-money-chart");

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
        responsiveAnimationDuration:500,
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
    var chartData = {
        labels: ["12", "15", "16", "17", "18"],
        datasets: [{
            label: "DIRECT REFERRALS COUNT",
            data: [0, 0, 42.5, 0, 0],
            backgroundColor: "#5F5AF0",
            hoverBackgroundColor: "rgba(22,211,154,.9)",
            borderColor: "transparent"
        }]
    };

    var config = {
        type: 'bar',

        // Chart Options
        options : chartOptions,

        data : chartData
    };

    // Create the chart
    var lineChart = new Chart(ctx, config);
    var chartDataForMoney = {
        labels: ["12", "15", "16", "17", "18"],
        datasets: [{
            label: "MONEY CREDITED TO YOUR ACCOUNT",
            data: [22, 0, 50, 0, 0],
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
    var lineChartForMoney = new Chart(ctxMoney, configForMoney);
});