$(function () {
    $("[data-hide]").on("click", function () {
        $("." + $(this).attr("data-hide")).hide();
    });
});

var logItems = [];

function setupProviderActivity(inUrl) {
    $("#from").datepicker({
        defaultDate: "-1w",
        minDate: -31,
        maxDate: 0,
        changeMonth: true,
        numberOfMonths: 1,
        onClose: function (selectedDate) {
            $("#to").datepicker("option", "minDate", selectedDate);
        }
    });
    $("#to").datepicker({
        defaultDate: "0d",
        maxDate: 0,
        changeMonth: true,
        numberOfMonths: 1,
        onClose: function (selectedDate) {
            $("#from").datepicker("option", "maxDate", selectedDate);
        }
    });

    bindViewChangeBtn();
    getChangedProviderDropDown(inUrl);
}

function bindViewChangeBtn() {
    
    $('#sortProviderChanges').click(function (e) {
        loadReport();
    });

    $('#viewchangebtn').click(function (e) {
        loadReport();
    });
}
function getChangedProviderDropDown(inUrl) {
    $.ajax(
        {
            type: "GET",
            url: inUrl,
            dataType: "json",
            success: function (json) {
                logItems = json;
                var $el = $("#optionalProvider");
                $el.empty();
                $el.append($("<option></option>").attr("value", null).text("Select Provider (optional)"));
                var checkDuplicates = [];
                for (var i = 0; i < json.length; i++) {
                    var duplicate = checkDuplicates.indexOf(json[i].EditedProviderName);
                    if(duplicate === -1){
                        checkDuplicates.push(json[i].EditedProviderName);
                    }
                }

                checkDuplicates.sort(function (a, b) {
                    return a.toLowerCase().localeCompare(b.toLowerCase());
                });

                for (var i = 0; i < checkDuplicates.length; i++) {
                    $el.append($("<option></option>").attr("value", checkDuplicates[i]).text(checkDuplicates[i]));
                }
            }
        });
}

/// Summary: Get the filter settings and then builds the report based on the given filter.
function loadReport() {
    var showacalendardaystart = $('#from').val();
    var showacalendardayend = $('#to').val();
    if (showacalendardayend == "" || showacalendardaystart == "") {
        $("#errorMessage").html("Please select a from date and a to date.");
        $("#filterFlashMessage").show();
    }
    else {
        $("#filterFlashMessage").hide();
        var sortColumn = $("#sortProviderChanges option:selected").val();
        if (sortColumn != 'EditedDateTime') {
            logItems = _.sortBy(logItems, sortColumn);
        }
        else {
            logItems = _.sortBy(logItems, function(o) { return new Date(parseInt(o.EditedDateTime.substr(6))); });
        }
        $("#results tr").remove();
        showacalendardayend = new Date(showacalendardayend);
        showacalendardayend.setHours(23, 59, 59, 999);
        showacalendardaystart = new Date(showacalendardaystart);
        if (showacalendardaystart <= showacalendardayend) {
            var recordCount = 0;
            for (var j = 0; j < logItems.length; j++) {
                var readableTime = new Date(parseInt(logItems[j].EditedDateTime.substr(6)));
                if (showacalendardaystart <= readableTime && readableTime <= showacalendardayend) {
                    if (logItems[j].EditedProviderName === $("#optionalProvider option:selected").text()
                        || $("#optionalProvider option:selected").text() === "Select Provider (optional)") {

                        $("#results").append("<tr><td>" + logItems[j].EditedProviderName + "</td>" + "<td>" + logItems[j].Action + "</td>" + "<td>" + logItems[j].UserFirstName + "  " + logItems[j].UserLastName + "</td>" + "<td>" + readableTime.toLocaleDateString("en-US")  + ' ' +  readableTime.toLocaleTimeString()+ "</td></tr>");
                        recordCount++;
                    }
                }
            }

            if (recordCount === 0) {
                var blank = 'Nothing to display';
                $("#results").append("<tr><td> " + " " + "</td><td>" + blank + "</td><td></td><td></td></tr>");
            }
        }
    }
}