$(function () {
    $(document).ready(function () {
        $.ajax({
            url: '/RSSFeed/BringTheNews',
            data: {
               
            }
        }).done(function (data) {
            console.log(data);
            var template = $('#hidden-template-news-title').html();
            

            $.each(data, function (i, item) {
                var temp = template;
                $.each(item, function (key, value) {
                    
                    console.log(typeof value.pubDate);
                    temp = temp.replaceAll('{' + key + '}', value);
                });
                $('.news-section').append(temp);
            });
            


        }).fail(function (error) {
            console.log(error);
            alert(error);
        });

    });
});