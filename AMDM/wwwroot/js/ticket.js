$(function() {

    $(document).ready(function () {
        $('.small-weight-icone').hide();
        var userType = $('.ticket-section').attr('user-type');
        if (userType == "Trainee") {
            var traineeId = $('.display-trainee-ticket').attr('trainee-id');
            $.ajax({
                url: '/Tickets/DetailsByTraineeId',
                data: {
                    'traineeId': traineeId,
                }
            }).done(function (data) {
                $(".purchase-new-ticket-btn").hide();
                if (data.expiredDate<Date.now() || data.remainingPunchingHoles==0) {
                    $(".purchase-new-ticket-btn").show();
                }
                data.expiredDate = new Date(data.expiredDate).toLocaleDateString('en-GB')
                var template = $('#hidden-template-trainee-ticket').html();

                var temp = template;

                $.each(data, function (key, value) {
                    temp = temp.replaceAll('{' + key + '}', value);
                });

                $('.ticket-body').append(temp);

                if (data.remainingPunchingHoles < 10) {
                    for (var i = 1; i <= data.remainingPunchingHoles; i++) {
                        $('.icone-'+i).show();
                    }  
                }
                if (data.remainingPunchingHoles == 0) {
                    $('.alert-ticket-section').html('');

                    var template = $('#hidden-template-ticket-no-punches-left-alert').html();



                    var temp = template;


                    temp = temp.replaceAll('{' + "message" + '}', "Your ticket date is valid, But you don't have any punches left");


                    $('.alert-ticket-section').append(temp);
                }


            }).fail(function (error) {
                console.log(error);
                alert(error);
            });
        }
       
    });

    
});