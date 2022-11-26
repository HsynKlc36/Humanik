$('[data-size="big"]').on('click', () => {
    $('body').toggleClass('sidebar-icon-only')
})

$(window).resize(function () {
    var width = $(document).width();
    if (width < 992) {
        $('body').removeClass('sidebar-icon-only')
    }
});

function showImage(src, target) {
    src.addEventListener("change", function () {
        target.style.backgroundImage = "url(" + URL.createObjectURL(event.target.files[0]) +")"
    });
}

var src = document.getElementsByClassName("profile-photo-input")[0];
var target = document.getElementById("photoUploadTarget");
showImage(src, target);
