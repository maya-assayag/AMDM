$(function () {

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
                console.log(data);
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


            }).fail(function (error) {
                console.log(error);
                alert(error);
            });
        }
       
    });

    
});