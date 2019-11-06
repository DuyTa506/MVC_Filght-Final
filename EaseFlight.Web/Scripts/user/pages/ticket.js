$(document).ready(function () {

});

function returnTicket(id) {
    $.ajax({
        url: '/Ticket/RefundTicket',
        data: {
            ticketId: id
        },
        type: 'post',
        success: function (response) {
            var data = JSON.parse(response);

            if (data.type == 'success') {
                ToastSuccess('Your ticket is return successfully! Refund $' + data.refund + ', please check your Paypal');
            }
        }
    });
}