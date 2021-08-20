
$(function () {
    $('.register').click(function() {

        var trainingId = $(this).attr('training-id');
        var a = $(this);
        var registeredMessage = $('.already-registered-message');

        var form = $('#__AjaxAntiForgeryForm');
        var token = $('input[name="__RequestVerificationToken"]', form).val();

            $.ajax({
                method :'post',
                //method: 'get',
                url: '/Trainings/Register',
               
                data: {
                   '__RequestVerificationToken': token,
                    'trainingID': trainingId
                }

            }).done(function (data) {
                a.attr('name', 'unregister');
                a.attr('class', 'unregister');
                a.text('Unregister');
                console.log(registeredMessage);
                registeredMessage.attr('show','yes')
            }).fail(function (error) {
                console.log(error);
                alert(error);
            });
    });
    


});
