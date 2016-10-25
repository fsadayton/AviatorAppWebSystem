/* Summary: Binds the search button for searching.*/
function bindSearchButton() {
    $("#searchSpinner").hide();
    $("#searchBtn").click(function () {
        searchUsers($("#Search").val());
    });

    $('#confirm-deactivate').on('show.bs.modal', function (e) {
        $('.debug-url').html('Delete URL: <strong>' + $(e.relatedTarget).data('href') + '</strong>');
        var deactivateLink = $(e.relatedTarget).data('href');
        var userId = $(e.relatedTarget).attr('id').replace('deactivate_', '');
        $(this).find('.btn-ok').click(function () {
            deactivate(userId);
        });
    });
}

/* Summary: Finds Users for given search text.  Fills in found items div. */
function searchUsers(searchText) {
    $("#foundItems").fadeOut(function () {
        $("#searchSpinner").fadeIn();
        $.ajax({
            cache: false,
            type: "GET",
            url: getUserSearchpath,  //Found in layout
            data: {"searchText": searchText },
            success: function (data) {
                $("#foundItems").html(data);
                $("#searchSpinner").fadeOut(function () {
                    $("#foundItems").fadeIn();
                });
                
                bindActivateButtons();
            },
            error: function (xhr, ajaxOptions, thrownError) {
                $("#searchSpinner").hide();
                $("#foundItems").show();
            }
        });
    });
}
//Sets an id for activating a user
function bindActivateButtons() {
    $(".activateButton").click(function (e) {
        var id = $(this).attr('id').replace('activate_', '');
        activate(id, this);
        e.preventDefault();
    });
}

//Activate a user 
function activate(userId, activateButton) {
    $.ajax({
        cache: false,
        type: "POST",
        url: getActivatePath,  //TODO: Found in layout
        data: { "id": userId },
        success: function (data) {
            //Hide the activate button
            $("#activate_" + userId).hide();
            //Show the deactivate button
            $("#deactivate_" + userId).show();
            $("#activeCheck_"+userId).find('input').attr('checked', true);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            $("#thrownError").show()
            setTimeout(function () { $("#thrownError").hide(); }, 5000);
        }
    });
}
//Deactivate
function deactivate(userId) {
    $.ajax({
        cache: false,
        type: "POST",
        url: getDeactivatePath,  //TODO: Found in layout
        data: { "id": userId },
        success: function (data) {
            
            //Hide the deactivate button
                $("#deactivate_" + userId).hide();
            //Show the activate button
                $("#activate_" + userId).show();
                $("#activeCheck_" + userId).find('input').attr('checked', false);
                $('#confirm-deactivate').modal('hide');
        },
        error: function (xhr, ajaxOptions, thrownError) {
            $("#thrownError").show()
            setTimeout(function () { $("#thrownError").hide(); }, 5000);
        }
    });
}