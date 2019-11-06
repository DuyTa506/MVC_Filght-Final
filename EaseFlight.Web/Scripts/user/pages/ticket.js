$(document).ready(function () {

});

function returnTicket(id) {
    var parent = $(event.target).parents('tr');
    parent.find('.td-status').html('<div class="lds-ellipsis"><div></div><div></div><div></div><div></div></div>');
    parent.find('.td-refund').html('<div class="lds-ellipsis"><div></div><div></div><div></div><div></div></div>');

    $.ajax({
        url: '/Ticket/RefundTicket',
        data: {
            ticketId: id
        },
        type: 'post',
        success: function (response) {
            var data = JSON.parse(response);

            if (data.type == 'success') {
                parent.find('.td-status').html('<p class="status-return">Return ($' + data.refund + ')</p>');
                parent.find('.td-status').attr('class', '').addClass('td-status-width');
                parent.find('.td-refund').remove();
                ToastSuccess('Your ticket is return successfully! Refund $' + data.refund + ', please check your Paypal');
            }
        }
    });
}