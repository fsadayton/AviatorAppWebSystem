

var providerSearch = (function () {
    var searchText;
    var page = 0;
    var categoryId = null;
    var countyId = null;
    var countyFilter = $("#countyFilter");
    var categoryFilter = $("#categoryFilter");
    var results = $("#results");
    var lastPage;

    function searchSetup() {
        results.hide();
        bindSearchButton();
        fillDropdown(countyFilter, getCountiesByStateIdPath, { stateId: 34 });
        fillDropdown(categoryFilter, getCategoryLookupIndexPath);
        $("#searchSpinner").hide();

        $(window).scroll(function () {
            if ($(window).scrollTop() === $(document).height() - $(window).height()) {
                if (page >= 0) {
                    searchProviders(searchText);
                }
            }
        });
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

    function formSubmitted() {
        page = -1;
        lastPage = -1;
        searchText = $("#Search").val();
        countyId = (countyFilter.val() !== "All") ? countyFilter.val() : null;
        categoryId = (categoryFilter.val() !== "All") ? categoryFilter.val() : null;

        $.ajax({
            cache: false,
            type: "GET",
            url: getServiceProviderSearchCountPath, //Found in layout
            data: {
                "searchText": searchText,
                "countyId": countyId,
                "categoryId": categoryId
            },
            success: function(data) {
                results.show();
                $("#searchCount").html(data);
            }
        });

        searchProviders($("#Search").val());
        $("#providerList").html("");
    }

    /* Summary: Finds providers for given search text.  Fills in found items div. */
    function searchProviders(searchText) {
        $("#searchSpinner").show();
        page++;
        if (page === lastPage) {
            return;
        }
        $.ajax({
            cache: false,
            type: "GET",
            url: getServiceProviderSearchPath,  //Found in layout
            data: {
                "searchText": searchText,
                "page": page,
                "countyId": countyId,
                "categoryId": categoryId
            },
            success: function (data) {
                $("#searchSpinner").fadeOut();
                if (data.trim() !== "") {
                    $("#providerList").append(data);
                    lastPage = page;
                }
                else {
                    page = -1;
                }

                $("#confirm-delete").on("show.bs.modal", function (e) {
                    deleteProviderListener(e);
                    
                });
                
            },
            error: function (xhr, ajaxOptions, thrownError) {
                $("#searchSpinner").fadeOut();
            }
        });
    }
    function deleteProviderListener(e) {
        console.log($(e.relatedTarget).data("href"));
        $("#deleteProviderButton").click(function() {
            $("#deleteProviderButton").hide();
            $.ajax({
                cache: false,
                type: 'GET',
                url: $(e.relatedTarget).data("href"),
                success: function(data) {
                    location.reload();
                }
            });
            $("#deleteProviderButton").show();
            $("#confirm-delete").hide();
            
        });

    }
    function fillDropdown(dropdownFilter, url, paramaters) {
        $.ajax({
            cache: false,
            type: "GET",
            url: url,
            data: paramaters,
            success: function (data) {
                dropdownFilter.html("");
                $.each(data, function (id, option) {
                    dropdownFilter.append($("<option></option>").val(option.id).html(option.name));
                });

                dropdownFilter.html(dropdownFilter.find("option").sort(function (a, b) {
                    return a.text === b.text ? 0 : a.text < b.text ? -1 : 1;
                }));

                dropdownFilter.prepend($("<option></option>").val("All").html("All"));
                dropdownFilter.val('All');
            }
        });
    }

    return {
        setup: searchSetup
    };
})();

function serviceProviderPageSetup() {
    //Initial setup when page is loaded.
    bindCloseLocation();
    bindCountryDropdowns();
    bindStateDropdowns();
    bindCoverageCountryDropdowns();
    bindCoverageStateDropdowns();
    bindCoverageAddButtons();
    bindContactPersonInfo();
    buildCategoriesList();
    bindDeleteRows();
    bindClearCoverage();
    checkCoverages();

    // Default the country selection and fill in the states.
    $(".coverageCountrySelection").trigger('change');
    
    /* Summary: When a star is hovered over, fill in the appropriate stars. */
    $(".ranking").hover(function () {
        getAndShowRankingFromStar(this);
    });

    /* Summary: When a star is clicked, set the hidden field to the appropriate number. */
    $(".ranking").click(function() {
        $("#DisplayRank").val(getAndShowRankingFromStar(this));
    });

    /* Summary: When moused out, return the number of filled in stars to the actual selection */
    $(".rankGrouping").mouseout(function() {
        showRanking($("#DisplayRank").val());
    });

    // Prepopulate the filled in stars.
    showRanking($("#DisplayRank").val());

    /* Summary: Add the fields for another location. */
    $("#addLocation").click(function () {
        $.ajax({
            url: this.href,
            cache: false,
            success: function(html) {
                $("#LocationFields").append(html);
                bindCountryDropdowns();
                bindStateDropdowns();
                bindCoverageCountryDropdowns();
                bindCoverageStateDropdowns();
                bindCoverageAddButtons();
                bindContactPersonInfo();
                bindCloseLocation();
            }
        });
        return false;
    });
   
    /* Summary: When the user is done with the categories modal, fill in the hidden form field with
                the user's selections */
    $('#categoriesModal').on('hide.bs.modal', function(e) {
        buildCategoriesList();
    });
    /* Summary: Categories are cleared when a user selects a new type */
    $('#Type').change(function () {
        $('input[type="checkbox"][name="Services.ServiceAreas"]:checked');
        for (var i = 0; i < $("#categoriesList").length ; i++) {
            $('input[type="checkbox"][name="Services.ServiceAreas"]:checked').prop('checked', false);
        }
        $("#selectedCategories").html('');
        $("#noCategories").show();
    });
    /* Summary: Using the entire row to select a category.  Ignore checkbox clicks. */
    $(".categoryRow").find('input[type="checkbox"]').click(function (event) {
        //Turn off checking via the checkbox and allow the entire row to check the box.
        event.stopPropagation();
    });

    /* Summary: Select the corresponding checkbox when anywhere on the row is clicked */
    $(".categoryRow").click(function () {
        event.stopPropagation();
        var categoryId = $(this).attr('id').replace('categoryRow_', '');
        var checkbox = $("#category_" + categoryId);
        checkbox.prop('checked', !checkbox.prop('checked'));
    });

    /* Summary: Show the categories modal when the categories list is clicked. */
    $("#categoriesList").click(function () {
        $('input[type="checkbox"][name="Services.ServiceAreas"]:checked');  
        var providerselected = $("#Type").val();
        $(".categoryRow").hide();
        $(".serviceType" + providerselected).show();
        $("#categoriesModal").modal('show');
    });

}

/* Summary: Given star, get the rank that it corresponds to and fill in the corresponding stars.*/
function getAndShowRankingFromStar(star) {
    var id = $(star).attr('id');
    var count = id.replace("rank", "");

    showStars(count);

    if (count === "3") {
        return "1";
    }
    else if (count === "2") {
        return "2";
    }
    else {
        return "3";
    }
}

/* Summary: Shows the stars based on ranking.*/
function showRanking(rank) {
    if (rank === "3") {
        showStars("1");
    }
    else if (rank === "2") {
        showStars("2");
    }
    else {
        showStars("3");
    }
}

/* Summary: Fills in the number of stars for a given number and show description. */
function showStars(count) {
    var index;
    for (index = 1 ; index <= 3; index++) {
        if (index <= count) {
            $("#rank" + index).removeClass("fa-star-o").addClass("fa-star");
        }
        else {
            $("#rank" + index).removeClass('fa-star').addClass('fa-star-o');
        }
    }
    $(".ranking").removeClass("ranking-high").removeClass("ranking-medium").removeClass("ranking-low");
    
    if (count === "3") {
        $("#rankDescription").html("High");
        $(".fa-star").addClass("ranking-high");
    }
    else if (count === "2") {
        $("#rankDescription").html("Medium");
        $(".fa-star").addClass("ranking-medium");
    }
    else {
        $("#rankDescription").html("Low");
        $(".fa-star").addClass("ranking-low");
    }
}

/* Summary: Checks if any coverages have been added to a location.  
            Shows a warning if none have been included. */
function checkCoverages() {
    $(".coverageList").each(function(index, value) {
        var foundRows = $(value).find(".row");
        var warning = $(value).find('.alert-warning');
        if (foundRows.length === 0) {
            $(warning).show();
        }
        else {
            $(warning).hide();
        }
    });
}

/* Summary: Binds the country dropdowns so that they will get the corresponding states for the selected country */
function bindCountryDropdowns() {

    //When the country is changed, fill in the corresponding states for that country.
    $(".countrySelection").unbind('change').change(function () {
        var selectedItem = $(this).val();
        var prefix = $(this).attr('id').replace('CountryId', '');
        var stateDdl = $("#" + prefix + "StateIdString");

        //Preset the coverage dropdown
        var coverageCountry = $("#" + prefix + "coverageCountry");
        coverageCountry.val(coverageCountry.val());
        coverageCountry.trigger('change');

        getStatesAjax(selectedItem, stateDdl);
        return false;
    });
}

/* Summary: When the location state dropdown is changed, set the coverage state selection state 
            dropdown to be the same. */
function bindStateDropdowns() {
    $(".stateSelection").unbind('change').change(function () {
        var selectedItem = $(this).val();
        var prefix = $(this).attr('id').replace('StateIdString', '');

        //Preset the coverage dropdown
        var coverageState = $("#" + prefix + "coverageState");
        coverageState.val(selectedItem);
        coverageState.trigger('change');
    });
}

/* Summary: Show the contact person info fields when the show contact person link is clicked. */
function bindContactPersonInfo() {
    $(".showContactPersonLink").click(function () {
        var contactInfoName = $(this).attr('id').replace('ContactPersonLink', 'ContactPersonInfo');
        $("#" + contactInfoName).removeClass('hide');
        $(this).addClass('hide');
    });
}

/* Summary: Bind the close button on locations*/
function bindCloseLocation() {
    showHideLocationCloseButtons();
    $(".button-closeLocation").click(function () {
        //Remove the location completely.
        var locationFieldsName = $(this).attr('id').replace('RemoveLocation', 'LocationFields');
        $("#" + locationFieldsName).slideUp(500, function() {
            $("#" + locationFieldsName).remove();
            showHideLocationCloseButtons();
        });
    });
}
/*Summary: Bind close all on locations*/
function bindClearCoverage() {
    $("#clearCoverage").click(function() {
        $(".coverageRowLocation").text("");
        $("#noCoverage").show();
    });

}
/* Summary: Only show the location close buttons if there is more than one location */
function showHideLocationCloseButtons() {
    //Don't show the close button if only location is left.
    if ($(".button-closeLocation").length > 1) {
        $(".button-closeLocation").show();
    }
    else {
        $(".button-closeLocation").hide();
    }
}

/* Summary: Show all the categories that have been selected in the categories modal. 
            Show a warning if there are none selected. */
function buildCategoriesList()
{
    var categories = $('input[type="checkbox"][name="Services.ServiceAreas"]:checked');
    $("#selectedCategories").html('');
    $("#noCategories").hide();
    for (var i = 0; i < categories.length; i++) {
        $("#selectedCategories").append("<div>" + $(categories[i]).next('span').text() + "</div>");
    }
    if (categories.length === 0) {
        $("#noCategories").show();
    }
}

/* Summary: When the coverage country dropdown is changed, get the corresponding states.*/
function bindCoverageCountryDropdowns() {
    $(".coverageCountrySelection").unbind('change').change(function () {
        var selectedItem = $(this).val();
        var prefix = $(this).attr('id').replace('coverageCountry', '');
        var stateDdl = $("#" + prefix + "coverageState");

        $(stateDdl).prop('disabled', false);
        getStatesAjax(selectedItem, stateDdl);

        return false;
    });
}

/* Summary: Gets the states for the given country id and fills in the given state dropdown */
function getStatesAjax(countryId, stateDropdown) {
    //Go get the states and load them into the states dropdown
    $.ajax({
        cache: false,
        type: "GET",
        url: GetStatesByCountryIDPath, //found in layout
        data: { "countryId": countryId },
        success: function (data) {
            stateDropdown.html('<option value>Select State</option>');
            $.each(data, function (id, option) {
                stateDropdown.append($('<option></option>').val(option.id).html(option.name));
            });
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
}


/* Summary: Get the counties for the given selected state when the state is changed. */
function bindCoverageStateDropdowns() {
    $(".coverageStateSelection").unbind('change').change(function () {
        var selectedItem = $(this).val();
        var prefix = $(this).attr('id').replace('coverageState', '');
        var countyDdl = $("#" + prefix + "coverageCounty");

        $(countyDdl).prop('disabled', false);

        //Go get the counties and add them to the county dropdown
        $.ajax({
            cache: false,
            type: "GET",
            url: getCountiesByStateIdPath,  //Found in layout
            data: { "stateId": selectedItem },
            success: function (data) {
                countyDdl.html('<option value>Select County</option>');
                countyDdl.append($('<option></option>').val('All').html('All'));
                $.each(data, function (id, option) {
                    countyDdl.append($('<option></option>').val(option.id).html(option.name));
                });
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
        return false;
    });
}

/* Summary: When x on a coverage row is clicked remove the entire row. */
function bindDeleteRows() {
    $(".deleteRow").unbind('click').click(function(event) {
        var rowId = $(this).attr('id').replace('_delete', '_row');
        $("#" + rowId).remove();
        //Check if there are any coverages left.
        checkCoverages();
    });
}

/* Summary: Adds a coverage to the location.*/
function bindCoverageAddButtons() {
    $(".addCoverage").unbind('click').click(function (event) {
        event.preventDefault();

        //Get all the dropdowns for the current location.
        var countryDropdown = $(this).parent().find('.coverageCountrySelection');
        var stateDropdown = $(this).parent().find('.coverageStateSelection');
        var countyDropdown = $(this).parent().find('.coverageCountySelection');
        var selectedCoverages = $(this).parent().parent().find('.coverageRowLocation');
        var countyId = countyDropdown.val();
        
        // If the county isn't selected, do nothing.
        if (countyId === "" || countyId === null) {
            return false;
        }

        var stateName = stateDropdown.children("option").filter(":selected").text();
        var countryName = countryDropdown.children("option").filter(":selected").text();

        var selectedCoverageInputs = $(this).parent().parent().find('.coverageID');

        var pageUrl = this.href;

        //Get the selected values.
        var countyName = countyDropdown.children("option").filter(":selected").text();
        if (countyName == "All") {
            // other behavior 
            var countyDropdownvalue = countyDropdown.children('option').not(':selected').valueOf();
            for (var i = 1; i < (countyDropdownvalue.length) ; i++) {
                addACoverage($(countyDropdownvalue[i]).text(), countryDropdown, stateDropdown, selectedCoverages, $(countyDropdownvalue[i]).val(), countryName, stateName, selectedCoverageInputs, pageUrl);
               }
        }
        else {
            addACoverage(countyName, countryDropdown, stateDropdown, selectedCoverages, countyId, countryName, stateName, selectedCoverageInputs, pageUrl);
        }
return false;
    });
}
function addACoverage(countyName, countryDropdown, stateDropdown, selectedCoverages, countyId, countryName, stateName, selectedCoverageInputs, pageUrl) {

    var locationPrefix = $(countryDropdown).attr('id').split('_')[1];
    //Don't allow user to add same county twice.
    var found = false;
    $.each(selectedCoverageInputs, function (index, value) {
        if ($(value).val() === countyId) {
            found = true;
        }
    });

    if (found) {
        return false;
    }

    //Go get the html for a new coverage row.
    $.ajax({
        url: pageUrl,
        cache: false,
        data: {
            "countyId": countyId,
            "CountryName": countryName,
            "StateName": stateName,
            "CountyName": countyName
        },
        success: function (html) {
            var inputFieldHtml = $(html).find('.coverageID');
            var id = $(inputFieldHtml).attr('id');
            selectedCoverages.append(html);

            var inputField = $("#" + id);

            //Fix prefixes for nested list in the model.
            var name = $(inputField).attr('name');
            $(inputField).attr('name', 'locations[' + locationPrefix + "]." + name);
            $(inputField).attr('id', 'locations_' + locationPrefix + "__" + id);

            //Rename the hidden field with the correct prefix.
            var hiddenIndex = $(selectedCoverages).find('[name = ".coverage.index"]');
            var hiddenIndexName = $(hiddenIndex).attr('name');
            $(hiddenIndex).attr('name', 'locations[' + locationPrefix + "]" + hiddenIndexName);

            bindDeleteRows();
            //$(countyDropdown).val('');

            //Hide the no coverage warning if needed.
            checkCoverages();
        }
    });

}

