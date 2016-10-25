$(document).on("ready", function() {
    calculateHeights();

    $(window).scroll(function() {
        if ($(document).scrollTop() > 300) {
            $("header #navigation").addClass("sticky");
        } else {
            $("header #navigation").removeClass("sticky");
        }
    });
});

$(window).resize(function () {
    // Reset height to automatic function
    $(".application-feature").css({ "height": "auto" });
    calculateHeights();
});

function calculateHeights() {
    var heights = $(".application-feature").map(function () {
        return $(this).height();
    });
    var maxHeight = Math.max.apply(null, heights);
    $(".application-feature").height(maxHeight);
}