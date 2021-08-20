$(function () {
    $('.unregister').click(function() {

        var trainingId = $(this).attr('training-id');
        var a = $(this);

    var form = $('#__AjaxAntiForgeryForm');
    var token = $('input[name="__RequestVerificationToken"]', form).val();

            $.ajax({
                method :'post',
                //method: 'get',
                url: '/Trainings/Unregister',
               
                data: {
                   '__RequestVerificationToken': token,
                        'trainingID': trainingId }

            }).done(function (data) {
                a.attr('name', 'register');
                a.attr('class', 'register');
                a.text('Register'); 
            }).fail(function (error) {
                console.log(error);
                alert(error);
            });
        });
    


});
