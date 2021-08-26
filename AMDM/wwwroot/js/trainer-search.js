$(function () {

    $('form').on('submit',function (e) {
        e.preventDefault();
        var query = $('#query').val();
       
      


        $.ajax({
            //method :'post',
            url: '/Trainers/Search',
            data: {
                'query': query
                
            }
        }).done(function (data) {
            
            console.log(data);
            $('tbody').html('');

            var template = $('#hidden-template-search-resulte').html();

            $.each(data, function (i, val) {
                console.log(val);
                if (val.trainerGender == 0) {
                    val.trainerGender = 'Female';

                } else {
                    val.trainerGender = 'Male';
                }
                var temp = template;

                val.dateOfBirth = new Date(val.dateOfBirth).toLocaleDateString('en-GB')

                console.log(val);

                $.each(val, function (key, value) {
                    temp = temp.replaceAll('{' + key + '}', value);
                });

                $('tbody').append(temp);

            });



        });



    });
});