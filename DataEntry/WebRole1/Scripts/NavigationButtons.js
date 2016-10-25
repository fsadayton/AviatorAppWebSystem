(function ($, viewport) {
    $(document).ready(function () {
        if (viewport.is('>=sm')) {
            $(function () {
                $(".menuButton").mouseover(function () {
                   if (viewport.is('>=sm')) {
                        $('[data-toggle="popover"]').popover({ trigger: "hover" });
                    }
                });
            });
        }

        // Execute code each time window size changes
        $(window).resize(
            viewport.changed(function () {
                if (viewport.is('>=sm')) {
                        $('[data-toggle="popover"]').popover({ trigger: "hover" });
                    $(".buttonDescription").hide();
                  
                }
                else {
                    $(".buttonDescription").show();
                    $("[data-toggle='popover']").popover('destroy');
                }
            })
        );
    });
})(jQuery, ResponsiveBootstrapToolkit);
$(document).ready(function (e) {
    $('[data-toggle="popover"]').popover();
});