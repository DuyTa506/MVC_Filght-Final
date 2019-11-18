$(document).ready(function () {
    $('.dashboard-menu').addClass('menu-active');

    var ticketChartCanvas = $('#ticketChart').get(0).getContext('2d');
    var monthArr = [];

    switch (new Date().getMonth() + 1) {
        case 12: monthArr.push("December");
        case 11: monthArr.push("November");
        case 10: monthArr.push("October");
        case 9: monthArr.push("September");
        case 8: monthArr.push("August");
        case 7: monthArr.push("July");
        case 6: monthArr.push("June");
        case 5: monthArr.push("May");
        case 4: monthArr.push("April");
        case 3: monthArr.push("March");
        case 2: monthArr.push("Febnuary");
        case 1: monthArr.push("January");
    }

    var ticketChartData = {
        labels: monthArr.reverse(),
        datasets: [
            {
                label: 'TicketSuccess',
                borderColor: '#28a745',
                pointRadius: false,
                pointColor: '#3b8bba',
                pointStrokeColor: 'rgba(60,141,188,1)',
                pointHighlightFill: '#fff',
                pointHighlightStroke: 'rgba(60,141,188,1)',
                data: ticketSuccess
            },
            {
                label: 'TicketReturn',
                borderColor: '#dc3545',
                pointRadius: false,
                pointColor: 'rgba(210, 214, 222, 1)',
                pointStrokeColor: '#c1c7d1',
                pointHighlightFill: '#fff',
                pointHighlightStroke: 'rgba(220,220,220,1)',
                data: ticketReturn
            },
            {
                label: 'Roundtrip',
                borderColor: '#6c757d',
                pointRadius: false,
                pointColor: 'rgba(210, 214, 222, 4)',
                pointStrokeColor: '#c1c7d1',
                pointHighlightFill: '#fff',
                pointHighlightStroke: 'rgba(60,220,220,1)',
                data: ticketRoundtrip
            }
        ]
    };

    var ticketChartOptions = {
        maintainAspectRatio: false,
        responsive: true,
        legend: {
            display: false
        },
        scales: {
            xAxes: [{
                gridLines: {
                    display: false,
                }
            }],
            yAxes: [{
                gridLines: {
                    display: false,
                }
            }]
        }
    };

    // This will get the first returned node in the jQuery collection.
    var ticketChart = new Chart(ticketChartCanvas, {
        type: 'line',
        data: ticketChartData,
        options: ticketChartOptions
    });
})