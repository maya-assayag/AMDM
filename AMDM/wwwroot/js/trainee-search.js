$(function () {

    $('form').on('submit',function (e) {
        e.preventDefault();
        var query = $('#query').val();
       
        //var selectDate = document.getElementById('date-filter');
        //var dateFilter = selectDate.options[selectDate.selectedIndex].value;

        //var selectType = document.getElementById('type-filter');
        //var typeFilter = selectType.options[selectType.selectedIndex].value;

        //var selectTrainer = document.getElementById('trainer-filter');
        //var trainerFilter = selectTrainer.options[selectTrainer.selectedIndex].value;


        $.ajax({
            //method :'post',
            url: '/Trainees/Search',
            data: {
                'query': query
                //'dateFilter': dateFilter,
                //'typeFilter': typeFilter,
                //'trainerFilter': trainerFilter
            }
        }).done(function (data) {

            console.log(data);
            $('tbody').html('');

            var template = $('#hidden-template-search-resulte').html();

            $.each(data, function (i, val) {

                var temp = template;

                console.log(val);

                $.each(val, function (key, value) {
                    temp = temp.replaceAll('{' + key + '}', value);
                });

                $('tbody').append(temp);

            });



        });



    });
});