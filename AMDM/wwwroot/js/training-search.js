$(function () {

    $(document).ready(function () {
        $.ajax({
            url: '/TrainingTypes/GetAllTrainingTypes',
        }).done(function (data) {
            //$('#type-filter').html('');
            

            var template = $('#hidden-template-trainings-types-filter').html();
            
            $.each(data, function (i, val) {

                var temp = template;

                $.each(val, function (key, value) {
                    temp = temp.replaceAll('{' + key + '}', value);
                });

                $('#type-filter').append(temp);

            });
        });
    });

    $(document).ready(function () {
        $.ajax({
            url: '/Trainers/GetAllTrainersNames',
        }).done(function (data) {

            var template = $('#hidden-template-trainers-names-filter').html();

            $.each(data, function (i, val) {

                var temp = template;

                $.each(val, function (key, value) {
                    temp = temp.replaceAll('{' + key + '}', value);
                });

                $('#trainer-filter').append(temp);

            });
        });
    });

    $('form').on('submit',function (e) {
        e.preventDefault();
        var query = $('#query').val();

        var userType = $('#query').attr('user-type');
        var traineeId = '';
        if (userType == "Trainee") {
            traineeId = $('#query').attr('user-id');
        }
       
        var selectDate = document.getElementById('date-filter');
        var dateFilter = selectDate.options[selectDate.selectedIndex].value;

        var selectType = document.getElementById('type-filter');
        var typeFilter = selectType.options[selectType.selectedIndex].value;

        var selectTrainer = document.getElementById('trainer-filter');
        var trainerFilter = selectTrainer.options[selectTrainer.selectedIndex].value;


        $.ajax({
            //method :'post',
            url: '/Trainings/Search',
            data: {
                'query': query,
                'dateFilter': dateFilter,
                'typeFilter': typeFilter,
                'trainerFilter': trainerFilter
            }
        }).done(function (data) {
            
            console.log(data);

            $('.trainings-section').html('');
            
            var template = $('#hidden-template-search-resulte').html();

            $.each(data, function (i, val) {
                var flag = false;
                console.log(val.id);
                if (userType == "Trainee") {
                    //var allWrappers = document.getElementsByClassName("registration-after-search");
                    //var allp = document.getElementsByClassName("already-registered-message-after-search");
                    $.ajax({
                        //method :'post',
                        url: '/Trainings/GetAllTraineesIdOfTraining',
                        data: {
                            'trainingId': val.id,
                        }
                    }).done(function (traineesIdOnTraining) {
                        console.log(traineesIdOnTraining);
                        $.each(traineesIdOnTraining, function (i, trainee) {
                            if (trainee == traineeId) {
                                flag = true;
                                $(".display-registration-after-search-"+val.id).addClass("unregister");
                                $(".display-registration-after-search-"+val.id).attr("name", "unregister");
                                $(".display-registration-after-search-"+val.id).text("Unregister");
                                $(".already-registered-message-after-search-" + val.id).show();
                            }
                        });
                        if (flag == false) {
                            $(".display-registration-after-search-" + val.id).addClass("register");
                            $(".display-registration-after-search-" + val.id).attr("name", "register");
                            $(".display-registration-after-search-" + val.id).text("Register");
                            $(".already-registered-message-after-search-" + val.id).hide();
                        }
                        flag = false;

                    }).fail(function (error) {
                        console.log(eroor);
                    });
                }
                





                var temp = template;

                        val.time = new Date(val.time).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })
                        val.date = new Date(val.date).toLocaleDateString('en-GB')
                        console.log(val);

                $.each(val, function (key, value) {
                    temp = temp.replaceAll('{' + key + '}', value);
                });

                $('.trainings-section').append(temp);

            });



        });



    });
});