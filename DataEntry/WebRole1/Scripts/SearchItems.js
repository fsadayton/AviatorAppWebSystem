/* Summary: Binds the search button for searching.*/
function bindSearchButton(path) {
    $("#searchSpinner").hide();
    $("#searchBtn").click(function () {
        searchFamilies($("#Search").val(), path);
    });
}


/* Summary: Finds items for given search text.  Fills in found items div. */
function searchFamilies(searchText, path) {
    $("#searchBtn").prop("disabled", true);
    $("#allItems").fadeOut(function() {
        $("#foundItems").fadeOut(function() {
            $("#searchSpinner").fadeIn(function() {
                $.ajax({
                    cache: false,
                    type: "GET",
                    url: path, 
                    data: { "searchText": searchText },
                    success: function(data) {
                        $("#foundItems").html(data);
                    },
                    error: function(xhr, ajaxOptions, thrownError) {
                        $("#allItems").show();
                    },
                    complete: function() {
                        $("#searchSpinner").fadeOut(function() {
                            $("#foundItems").fadeIn();
                        });
                        $("#searchBtn").prop("disabled", false);
                    }
                });
            });
        });
    });
}