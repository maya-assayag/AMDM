$('.round').click(function (e) {
    e.preventDefault();
    e.stopPropagation();
    $('.arrow').toggleClass('bounceAlpha');
});
$(".landing-page-arrow").click(function (e) {
    var url = 'https://localhost:44361/TrainingTypes/Index';
    $(location).prop('href', url);
});