$(function () {


    $('form').on('submit',function (e) {
        e.preventDefault();
        var query = $('#query').val();

        var traineeId = $('#trainee-id-query').val();
       
        var selectPurchaseDate = document.getElementById('purchase-date-filter');
        var purchaseDateFilter = selectPurchaseDate.options[selectPurchaseDate.selectedIndex].value;

        var selectExpiredDate = document.getElementById('expired-date-filter');
        var expiredDateFilter = selectExpiredDate.options[selectExpiredDate.selectedIndex].value;

        


        $.ajax({
            //method :'post',
            url: '/Tickets/Search',
            data: {
                'query': query,
                'traineeId': traineeId,
                'purchaseDateFilter': purchaseDateFilter,
                'expiredDateFilter': expiredDateFilter
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