$(function () {

    $(document).ready(function () {
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

                $('.display-trainee-ticket').append(temp);


            }).fail(function (error) {
                console.log(error);
                alert(error);
            });
        }
       
    });

    
});