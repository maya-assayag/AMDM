$(function () {
    $('.register').click(function() {

        var traineeID = $(this).attr('trainee-id');

    var form = $('#__AjaxAntiForgeryForm');
    var token = $('input[name="__RequestVerificationToken"]', form).val();

            $.ajax({
                method :'post',
                //method: 'get',
                url: '/Trainings/Register',
               
                data: {
                   '__RequestVerificationToken': token,
                        'trainingID': traineeID }

            }).done(function (data) {
                console.log(data);
                alert(data);
            }).fail(function (error) {
                console.log(error);
                alert(error);
            });
        });
    


});
