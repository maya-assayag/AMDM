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
                $('.alert-section').html('');

                var template = $('#hidden-template-purchase-success-alert').html();



                var temp = template;


                temp = temp.replaceAll('{' + "message" + '}', "The purchase was made successfully");


                $('.alert-section').append(temp);
                
               
            }).fail(function (error) {
                console.log(error);
                alert(error);

                $('.alert-section').html('');

                var template = $('#hidden-template-purchase-error-alert').html();



                var temp = template;

                $.each(error.responseJSON, function (key, value) {
                    temp = temp.replaceAll('{' + key + '}', value);
                });

                $('.alert-section').append(temp);
            });
        });
    


});
