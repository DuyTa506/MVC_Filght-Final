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
})