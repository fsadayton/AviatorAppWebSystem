$(document).ready(function () {
    $("#spinner").fadeIn();
    $("#hotLine").fadeOut();

    $.ajax({
        url: getHotLineProviders,
        cache: false,
        success: function (data) {
            $.each(data, function (i, provider) {

                var helpLine = "<li class='list-group-item' style='margin-bottom:-20px;' ><h3>{Name}</h3><div style='text-align: left;'>{location}</div><div style='text-align: left; color:#009577 ; font-size: large;'><a  style='text-align: left; color:#009577 !important; font-size: large;' href='tel:{phone}'>Call: {Number}</a></div></li></br>";
                $("#hotLine").append(helpLine.replace("{Name}", provider.name).replace("{Number}", provider.phoneNumber).replace("{phone}", provider.phoneNumber).replace("{location}", " "));
            });
            $("#spinner").fadeOut();
            $("#hotLine").fadeIn();
        }
    });
});