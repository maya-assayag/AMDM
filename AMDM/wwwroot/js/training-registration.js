
$(function () {
    //$('form').submit(function () {
    //    var allWrappers = document.getElementsByClassName("registration");
    //    var alla = document.getElementsByClassName("display-registration");
    //    var allp = document.getElementsByClassName("already-registered-message");
    //    for (var i = 0; i < alla.length; i++) {
    //        if (allWrappers[i].getAttribute("registration-flag") == "False") {
    //            alla[i].classList.add("register");
    //            alla[i].setAttribute("name", "register");
    //            alla[i].textContent = "Register";
    //            allp[i].style.display = 'none';
    //        } else {
    //            alla[i].classList.add("unregister");
    //            alla[i].setAttribute("name", "unregister");
    //            alla[i].textContent = "Unregister";
    //            allp[i].style.display = 'block';
    //        }
    //    }         
    //});

    $('.registration-rap').ready(function () {
        var allWrappers = document.getElementsByClassName("registration");
        var alla = document.getElementsByClassName("display-registration");
        var allp = document.getElementsByClassName("already-registered-message");
        for (var i = 0; i < alla.length; i++) {
            if (allWrappers[i].getAttribute("registration-flag") == "False") {
                alla[i].classList.add("register");
                alla[i].setAttribute("name", "register");
                alla[i].textContent="Register";
                allp[i].style.display = 'none';
            } else {
                alla[i].classList.add("unregister");
                alla[i].setAttribute("name", "unregister");
                alla[i].textContent="Unregister";
                allp[i].style.display = 'block';
            }
        }         
    });
    $(document).on('click', '.register', function () {
        var trainingId = $(this).attr('training-id');
        var a = $(this);
        var p = $('.already-registered-message-' + trainingId.toString());//.show();


        var form = $('#__AjaxAntiForgeryForm');
        var token = $('input[name="__RequestVerificationToken"]', form).val();


        $.ajax({
            method: 'post',
            //method: 'get',
            url: '/Trainings/Register',

            data: {
                '__RequestVerificationToken': token,
                'trainingID': trainingId
            }

        }).done(function (data) {
            a.attr('name', 'unregister');
            a.attr('class', 'unregister');
            a.text('Unregister');
            p.show();
        }).fail(function (error) {
            console.log(error);
            alert(error);
        });
    });
    $(document).on('click', '.unregister', function () {

        var trainingId = $(this).attr('training-id');
        var a = $(this);
        var p = $('.already-registered-message-' + trainingId.toString());


        var form = $('#__AjaxAntiForgeryForm');
        var token = $('input[name="__RequestVerificationToken"]', form).val();

        $.ajax({
            method: 'post',
            //method: 'get',
            url: '/Trainings/Unregister',

            data: {
                '__RequestVerificationToken': token,
                'trainingID': trainingId
            }

        }).done(function (data) {
            a.attr('name', 'register');
            a.attr('class', 'register');
            a.text('Register');
            p.hide();
        }).fail(function (error) {
            console.log(error);
            alert(error);
        });
    });
});
