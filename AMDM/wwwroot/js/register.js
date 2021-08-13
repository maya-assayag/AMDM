﻿$(function () {
    $('.register').click(function() {

        var trainingId = $(this).attr('training-id');

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
                console.log(data);
                alert(data);
            }).fail(function (error) {
                console.log(error);
                alert(error);
            });
    });
    


});
