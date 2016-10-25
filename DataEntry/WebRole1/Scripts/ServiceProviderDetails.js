$(document).ready(function () {
    $(function () {
        $("#toggleCounties").on("click", function () {
            if ($("#counties").css("display") === "none") {
                $("#counties").css("display", "block");
                document.getElementById("toggleCounties").innerHTML = "Hide covered areas";
            } else {
                $("#counties").css("display", "none");
                document.getElementById("toggleCounties").innerHTML = "Show covered areas";
            }
        });
    });
});