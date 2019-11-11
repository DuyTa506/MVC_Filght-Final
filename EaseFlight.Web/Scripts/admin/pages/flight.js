$(document).ready(function () {
    //Init datatable
    $("#flightTable").DataTable();

    ////Add create button
    var parent = $('.dataTables_filter').parent();
    parent.append('<button title="Create flight" data-toggle="modal" data-target="#flightModal" type="button" class="btn btn-block btn-success btn-sm btn-create"><i class="fas fa-plus"></i></button>');

    //Add context Menu
    $.contextMenu({
        selector: 'tr',
        callback: function (key, options) {

            if (key == 'status') { //Change status option

            } else if (key == 'edit') { //Edit option

            } else if (key == 'delete') { //Delete option

            }
        },
        items: {
            "status": { name: "Change status", icon: "paste"},
            "sep1": "---------",
            "edit": { name: "Edit", icon: "edit" },
            "delete": { name: "Delete", icon: "delete" }
        }
    });  

    //Select plane onchange
    $('select[name="plane"]').change(function () {
        planeSelectOnchange($(this).val());
    });

    planeSelectOnchange($('select[name="plane"]').val());
})

function planeSelectOnchange(planeId) {
    $.ajax({
        url: '/Admin/Flight/GetAirport',
        type: 'post',
        data: {
            planeId: planeId
        },
        success: function (response) {
            var data = JSON.parse(response).airport;
            var depart = $('select[name="depart"]');
            var arrival = $('select[name="arrival"]');

            depart.empty(); arrival.empty();

            for (var i = 0; i < data.length; ++i) {
                var airport = data[i].split(':');
                depart.append('<option value="' + airport[0] + '">' + airport[1] + '</option>');
                arrival.append('<option value="' + airport[0] + '">' + airport[1] + '</option>');
            }

            depart.trigger('change');
            arrival.trigger('change');
        }
    });
}