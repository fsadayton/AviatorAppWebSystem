

(function ($, viewport) {
    $(function () {
        // Execute code each time window size changes
            $(window).resize(
            viewport.changed(function () {
                if (viewport.is('>=sm')) {
                    $(".subMenuSelectionInfo").show();
                }
                else {
                    $(".subMenuSelectionInfo").hide();
                }
            })
        );

        //Summary: On a submenu, show the submenu's corresponding items when it is clicked.
        $(".subMenuItem").click(function() {

            var a = $(".menuPicture-subMenu");
            // Get the SVG document inside the Object tag
            for (var i = 0; i < a.length; i++) {
                var svgDoc = $(a)[i].contentDocument;
                var svgs = svgDoc.getElementsByClassName("subMenuIcon");
                svgs[0].setAttribute("class", "subMenuIcon");
            }

            $(".subMenuItem").removeClass("subMenuItem-selected");
            $(this).addClass("subMenuItem-selected");

            var aSelected = $(".subMenuItem-selected").find(".menuPicture-subMenu");
            // Get the SVG document inside the Object tag
            var svgDocSelected = aSelected[0].contentDocument;
            var svgsSelected = svgDocSelected.getElementsByClassName("subMenuIcon");
            svgsSelected[0].setAttribute("class", "subMenuIcon subMenuIcon-selected");

            var selectedItem = $(this).data("select");
            $(".subMenuSelection").hide();
            $("#" + selectedItem).show();
        });
    });

})(jQuery, ResponsiveBootstrapToolkit);