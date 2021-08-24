$(function () {
    $('#tweet-submit').submit(function () {

        /*$('#all-tweets').load();*/




        $.ajax({
            url: '/Home/GetAllTweets',
        }).done(function (data) {
            console.log(data);


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
       
});