$(function () {
    $('form').submit(function (e) {
        e.preventDefault();
        var query = $('#query').val();


        // $('tbody').load('/Trainings/Search?query=' + query);


        $.ajax({
            //method :'post',
            url: '/Trainings/Search',
            data: { 'query': query }
                }).done(function (data) {

                    //$('tbody').html('');
                    //for (var i = 0; i < data.length; i++)
                    //{
                    //    var template = '<tr><td>' + data[i].title + '</td><td>' + data[i].body + '</td></tr>';
                    //    $('tbody').append(template);
                    //}

                    $('tbody').html('');

                    var template = $('#hidden-template').html();

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