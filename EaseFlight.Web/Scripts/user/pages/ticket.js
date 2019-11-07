$(document).ready(function () {

});

function returnTicket(id) {
    $('#confirm-return').modal('hide');
    var parent = $('#ticket-' + id);
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

function openReturnTicketModal(id) {
    $('#confirm-return').find('.a-confirm').attr('onclick', 'returnTicket("' + id + '")');
    $('#confirm-return').modal('show');
}