$(function () {

    $(document).ready(function () {
        var traineeId = $('.display-trainee-ticket').attr('trainee-id');
        $.ajax({
            url: '/Tickets/DetailsByTraineeId',
            data: {
                'traineeId': traineeId,
            }
        }).done(function (data) {
            console.log(data);
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
    });

    
});