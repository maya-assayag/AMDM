$('.round').click(function (e) {
    e.preventDefault();
    e.stopPropagation();
    $('.arrow').toggleClass('bounceAlpha');
});
$(".landing-page-arrow").click(function (e) {
    var url = 'https://localhost:44361/TrainingTypes/Index';
    $(location).prop('href', url);
});

$(".admin-page-double-arrow").click(function (e) {
    $('#exampleModal').modal();
});

$("#close-modal-double-arrow").click(function (e) {
    $('#exampleModal').remove();
    location.reload();
});