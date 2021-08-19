$(function () {
    $('form').submit(function (e) {
        e.preventDefault();
        var query = $('#query').val();
        /*var filterDate = $('#dateFilter').val();*/
        var select = document.getElementById('date-filter');
        var dateFilter = select.options[select.selectedIndex].value;

        // $('tbody').load('/Trainings/Search?query=' + query);


        $.ajax({
            //method :'post',
            url: '/Trainings/Search',
            data: {
                'query': query,
                'dateFilter': dateFilter
            }}).done(function (data) {

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