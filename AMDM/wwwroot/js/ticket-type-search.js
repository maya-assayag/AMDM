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

            $('.tickets-sections').html('');

                    var template = $('#hidden-template-search-resulte').html();

                    $.each(data, function (i, val) {

                        var temp = template;

                        console.log(val);

                        $.each(val, function (key, value) {
                            temp = temp.replaceAll('{' + key + '}', value);
                        });

                        $('.tickets-sections').append(temp);

                    });

        });

    });
});