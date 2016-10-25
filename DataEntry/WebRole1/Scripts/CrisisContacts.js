
var crisisContacts = (function() {
    var results = $("#results");

    /* Summary: Sets up the search functionality*/
    function searchSetup() {
        results.hide();
        bindSearchButton();
        $("#searchSpinner").hide();

        formSubmitted();
    }

    /* Summary: Binds the search button for searching.*/
    function bindSearchButton() {
        $("#searchBtn").click(function () {
            formSubmitted();
        });

        $("#searchForm").submit(function () {
            formSubmitted();
            return false;
        });
    }

    /* Summary: Search the contacts when the form is submitted */
    function formSubmitted() {
        var searchText = $("#Search").val();
        $.ajax({
            cache: false,
            type: "GET",
            url: getCrisisContactSearchPath,  //Found in layout
            data: {
                "searchText": searchText
            },
            success: function (data) {
                $("#searchSpinner").fadeOut();
                if (data.trim() === "") {
                    data = "<li class='list-group-item'>No crisis contacts found. </li>";
                }
                $("#contactList").html(data);

                results.fadeIn();
                $("#confirm-delete").on("show.bs.modal", function (e) {
                    bindDeleteContact(e);
                });
            },
            error: function (xhr, ajaxOptions, thrownError) {
                $("#searchSpinner").fadeOut();
            }
        });

        /* Summary: Bind the delete contract button in the modal */
        function bindDeleteContact(e) {
            console.log($(e.relatedTarget).data("href"));
            $("#deleteContactButton").click(function () {
                $("#deleteContactButton").hide();
                $.ajax({
                    cache: false,
                    type: 'GET',
                    url: $(e.relatedTarget).data("href"),
                    success: function (data) {
                        location.reload();
                    }
                });
                $("#deleteContactButton").show();
                $("#confirm-delete").hide();
            });
        }
    }

    return {
        setupSearch: searchSetup
    };
})();