$(function () {


    $('form').on('submit',function (e) {
        e.preventDefault();
        var query = $('#query').val();

        $.ajax({
            //method :'post',
            url: '/TicketTypes/Search',
            data: {
                'query': query
            }
        }).done(function (data) {

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