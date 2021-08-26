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
            
                    //$('tbody').html('');
                    //for (var i = 0; i < data.length; i++)
                    //{
                    //    var template = '<tr><td>' + data[i].title + '</td><td>' + data[i].body + '</td></tr>';
                    //    $('tbody').append(template);
                    //}

                    $('tbody').html('');

                    var template = $('#hidden-template-search-resulte').html();

                    $.each(data, function (i, val) {

                        var temp = template;

                        val.time = new Date(val.time).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })
                        val.date = new Date(val.date).toLocaleDateString('en-GB')
                        console.log(val);

                        $.each(val, function (key, value) {
                            temp = temp.replaceAll('{' + key + '}', value);
                        });

                        $('tbody').append(temp);

                    });



                });



    });
});