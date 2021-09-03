
$(function () {

    //const client = new signalR.HubConnectionBuilder()
    //    .withUrl("/traininghub")
    //    .build();

    //client.on("UpdatePlaceLeft", training => {
    //    updatePlaceLeft(training)
    //});

    $('.registration-rap').ready(function () {
        //client.start();
        var allWrappers = document.getElementsByClassName("registration");
        var alla = document.getElementsByClassName("display-registration");
        var allp = document.getElementsByClassName("already-registered-message");
        for (var i = 0; i < alla.length; i++) {
            if (allWrappers[i].getAttribute("registration-flag") == "False") {
                alla[i].classList.add("register");
                alla[i].classList.add("btn-outline-primary");
                alla[i].setAttribute("name", "register");
                alla[i].textContent="Register";
                allp[i].style.display = 'none';
            } else {
                alla[i].classList.add("unregister");
                alla[i].classList.add("btn-outline-danger");
                alla[i].setAttribute("name", "unregister");
                alla[i].textContent="Unregister";
                allp[i].style.display = 'block';
            }
        }         
    });

   

    function updatePlaceLeft(training) {

        $('.update-' + trainingID).text(training.TotalTraineesLeft+1);
    }

    $(document).on('click', '.register', function () {
        var trainingId = $(this).attr('training-id');
        var a = $(this);
        var p = $('.already-registered-message-' + trainingId.toString());//.show();


        var form = $('#__AjaxAntiForgeryForm');
        var token = $('input[name="__RequestVerificationToken"]', form).val();


        $.ajax({
            method: 'post',
            //method: 'get',
            url: '/Trainings/Register',

            data: {
                '__RequestVerificationToken': token,
                'trainingID': trainingId
            }

        }).done(function (data) {
            a.attr('name', 'unregister');
            a.attr('class', 'btn btn-outline-danger unregister display-registration display-registration-'+trainingId);
            a.text('Unregister');
            p.show();
            $('.alert-section').html('');

            var template = $('#hidden-template-registration-success-alert').html();



            var temp = template;

            
            temp = temp.replaceAll('{' + "message" + '}', "The registration was successful");
            

            $('.alert-section').append(temp);


            $.ajax({
                //method: 'post',
                //method: 'get',
                url: '/Trainings/GetPlaceLeft',

                data: {
                    'trainingID': trainingId
                }

            }).done(function (placeLeft) {
                $('.update-training-place-left-' + trainingId).html('');
                
                $('.update-training-place-left-' + trainingId).append('<strong>Place left:</strong>' + placeLeft);
                
            });


            var userType = $('.ticket-section').attr('user-type');
            if (userType == "Trainee") {
                var traineeId = $('.display-trainee-ticket').attr('trainee-id');
                $.ajax({
                    //method: 'post',
                    //method: 'get',
                    url: '/Tickets/DetailsByTraineeId',

                    data: {
                        'traineeId': traineeId
                    }

                }).done(function (ticket) {
                    $('.update-ticket-punches-left').html('');

                    $('.update-ticket-punches-left').append('You have left: ' + ticket.remainingPunchingHoles + ' ticket punches');
                    $('.small-weight-icone').hide();
                    if (ticket.remainingPunchingHoles < 10) {
                        for (var i = 1; i <= ticket.remainingPunchingHoles; i++) {
                            $('.icone-' + i).show();
                        }
                    }
                });
            }
            


            


            /*location.reload();*/

        }).fail(function (error) {
            console.log(error.responseJSON);
            /*alert(error.responseJSON.error);*/
            $('.alert-section').html('');

            var template = $('#hidden-template-registration-error-alert').html();

            

            var temp = template;

            $.each(error.responseJSON, function (key, value) {
                temp = temp.replaceAll('{' + key + '}', value);
            });

            $('.alert-section').append(temp);

            



        });
    });
    $(document).on('click', '.unregister', function () {

        var trainingId = $(this).attr('training-id');
        var a = $(this);
        var p = $('.already-registered-message-' + trainingId.toString());


        var form = $('#__AjaxAntiForgeryForm');
        var token = $('input[name="__RequestVerificationToken"]', form).val();

        $.ajax({
            method: 'post',
            //method: 'get',
            url: '/Trainings/Unregister',

            data: {
                '__RequestVerificationToken': token,
                'trainingID': trainingId
            }

        }).done(function (data) {
            a.attr('name', 'register');
            a.attr('class', 'register btn btn-outline-primary display-registration  display-registration-' + trainingId);
            a.text('Register');
            p.hide();
            //location.reload();



           
        }).fail(function (error) {
            console.log(error);
            $('.alert-section').html('');
            var template = $('#hidden-template-registration-error-alert').html();
            var temp = template;
            $.each(error.responseJSON, function (key, value) {
                temp = temp.replaceAll('{' + key + '}', value);
            });
            $('.alert-section').append(temp);
        });
    });
});
