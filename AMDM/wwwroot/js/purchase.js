$(function () {
    $('.purchase').click(function() {

        var traineeId = $(this).attr('trainee-id');
        var ticketTypeId = $(this).attr('ticketType-id');

        var form = $('#__AjaxAntiForgeryForm');
        var token = $('input[name="__RequestVerificationToken"]', form).val();

            $.ajax({
                method :'post',
                url: '/TicketTypes/Purchase',
               
                data: {
                   '__RequestVerificationToken': token,
                    //'ticketId': traineeId,
                    'ticketTypeId': ticketTypeId
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
