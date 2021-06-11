
function register(traineeID) {
        $.ajax({
            method: 'get',
            url: '/Trainings/Register',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: { 'trainingID': traineeID }

        }).done(function (data) {
            console.log(data);
            alert(data);
        }).fail(function (error) {
            console.log(error);
            alert(error);
        });
}

