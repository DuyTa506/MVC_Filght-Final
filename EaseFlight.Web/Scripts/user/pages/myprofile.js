$(document).ready(function () {
    //Set datetimepicker for birthday
    $('.dob').datetimepicker({
        format: 'DD/MM/YYYY'
    });

    //Set datetimepicker for passport
    $('.passport-expiry').datetimepicker({
        format: 'DD/MM/YYYY'
    });
});