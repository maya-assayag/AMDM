
$(function () {
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
});
