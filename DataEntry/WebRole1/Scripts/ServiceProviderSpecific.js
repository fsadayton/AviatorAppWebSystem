
var categoryList;
var categoryTitle;
var headerList = new Array();
var providerList = new Array();
var countyList = new Array();
var filterCategories = new Array();
var categoryCheck = new Array();
var includedCategories = new Array();
var filterCounter = 0;
var categoryPressed = 0;
var hasResults = false;
var categoryPath;
var providerPath;
var detailsPath;

var serviceProviderDisplay = (function () {

    var localCook = getCookie("countyId");
    //The html for the list item

    function populatePage(givenCategoryPath, givenProviderPath, givenDetailsPath) {
        categoryPath = givenCategoryPath;
        providerPath = givenProviderPath;
        detailsPath = givenDetailsPath;
        $("#spinner").fadeIn();
        $("#providerList").hide();
        getFilterCategories();
        getCounties();
        setSearchForCountiesBar();
        $("#providerList").show();
    }
    //gets location cookie value to find providers based on user initial input 
    function getCookie(countyId) {
        var value = "; " + document.cookie;
        var parts = value.split("; " + countyId + "=");
        if (parts.length == 2) return parts.pop().split(";").shift();
    }

    /* Summary: Gets all the categories in the database
               Gets the family information if needed */
    function getCategories(callback) {
        $.ajax({
            url: getCategoryLookupIndexPath,
            cache: false,
            success: function (data) {
                categoryList = data;
                callback();
            }
        });
    }
    //gets categories for filter
    function getFilterCategories() {
        $.ajax({
            url: categoryPath,
            cache: false,
            success: function (data) {
                var tempfilterCategories = data;
                for (var f = 0; f < data.length; f++) {
                    filterCategories[f] = tempfilterCategories[f].Name;
                    includedCategories[f] = filterCategories[f];
                }
                getCategories(function () { getProviders(localCook) });

            }

        });
    }
    //gets counties for filter
    function getCounties() {
        $.ajax({
            url: getCountiesPath,
            cache: false,
            success: function (data) {
                countyList = data;
                setCountiesFilter();
            }
        });
    }

    /*Summary:  gets the Providers that are associated with the given 
                categories and shows them in the list */
    function getProviders(countiestoget) {
        $.ajax({
            url: providerPath,
            cache: false,
            data: {
                "counties": countiestoget
            },
            success: function (data) {
                if ($.isEmptyObject(data)) {
                    hasResults = false;
                } else {
                    hasResults = true;
                    //For each provider, fill in the variables and add to the list.
                    $.each(data, function (i, provider) {
                        var providerCategories = $.map(provider.categories, function (categoryId) {
                            var category = findCategory(categoryId);
                            return (category !== undefined && category !== null) ? category.name : null;
                        });
                        for (var j = 0; j < providerCategories.length; j++) {
                            if ($.inArray(providerCategories[j], headerList)) {
                                headerList.push(providerCategories[j]);
                            }
                        }

                    });

                    providerList = data;
                }
            },
            complete: function () {

                $("#spinner").fadeOut(function () {
                    $('#providerList').html('');

                    buildLegalList();
                    if (filterCounter === 0) {
                        buildFilter();
                        bindFilterButton();
                        filterCounter = 1;
                    }
                    bingMap.setupMap('myMap', providerList, mapPinClickCallback, setDistanceCallback);
                    $("#providerList").fadeIn();
                });
            }
        });

    }

    /* Summary: Find a category in the list by the category's ID*/
    function findCategory(categoryId) {
        for (var i = 0; i < categoryList.length; i++) {
            if (categoryList[i].id == categoryId) {
                return categoryList[i];
            }
        }

        return null;
    }
    //filters headerList to make a set 
    function uniq(a) {
        var seen = {};
        return a.filter(function (item) {
            return seen.hasOwnProperty(item) ? false : (seen[item] = true);
        });
    }
    //checks user category filter to see if it matches with provider categories
    function contains(a, obj) {
        var i = a.length;
        while (i--) {
            if (a[i] === obj) {
                return true;
            }
        }
        return false;
    }
    /*Build html for legal */
    function buildLegalList() {
        var blankCheck = 0;
        headerList = uniq(headerList);
        var providerText = "<li class='item_{listItemId} list-group-item list-group-item-hover clearfix'>"
        + "<div class='pull-left' style='width: 80%'>"
        + "  <div style='margin-bottom: 10px'>"
        + "    <div><a href='{detailsUrl}'><h4>{providerName}</h4></div>"
        + "    <div> {locationName} </div>"
        + "  </div>"
        + "</div>"
        + "<div class='pull-right distance'><span class='distance_{listItemId}'></span></div>"
        + "</li>";
        if (hasResults === false) {
            $('#providerList').html('')
            var blankCategories = "<li > <h3 class='providerCategoryHeader'>No results found. Please update your filter.</h3>";
            $('#providerList').prepend(blankCategories);
        } else {
            for (var j = 0; j < filterCategories.length; j++) {
                var hasProviders = 0;
                categoryTitle = filterCategories[j];
                var categoryHtml = "<li id='" + categoryTitle.replace(/[^\w]/gi, '') + "CategoryHtml'> <h3 class='providerCategoryHeader'>"
                    + categoryTitle + "</h3>";
                categoryTitle = categoryTitle.replace(/[^\w]/gi, '');

                for (var p = 0; p < includedCategories.length; p++) {
                    if (includedCategories[p] !== null) {
                        includedCategories[p].replace(/[^\w]/gi, '');
                    }
                }
                if (j === 0) {
                    $('#providerList').prepend(categoryHtml);
                } else {
                    $('#providerList').append(categoryHtml);
                }
                for (var i = 0; i < providerList.length; i++) {
                    var provider = providerList[i];
                    var listItemId = provider.serviceProviderId + '_' + provider.locationId;

                    for (var l = 0; l < provider.categories.length; l++) {
                        var providerCatName = findCategory(provider.categories[l]);
                        var isInIncludedCategories = contains(includedCategories, providerCatName.name);
                        if (providerCatName.name === filterCategories[j] && isInIncludedCategories === true) {
                            var locationName = (provider.locationName === undefined || provider.locationName === null) ? '' : provider.locationName;
                            var detailsUrl = detailsPath + "/" + provider.serviceProviderId + "?location=" + provider.locationId;
                            $("#providerList").append(providerText.replace("{providerName}", provider.name)
                                .replace("{locationName}", locationName)
                                .replace("{detailsUrl}", detailsUrl)
                                .replace(/{listItemId}/g, listItemId));
                            hasProviders = 1;
                            blankCheck = 1;
                        }
                    }
                }
                categoryTitle.replace(/[^\w]/gi, '');
                if (hasProviders === 0) {
                    $('#' + categoryTitle.replace(/[^\w]/gi, '') + "CategoryHtml").hide();
                }

            }
        }
        if (blankCheck === 0) {
            $('#providerList').html('')
            var blankCategorie = "<li > <h3 class='providerCategoryHeader'>No results found. Please update your filter.</h3>";
            $('#providerList').prepend(blankCategorie);
        }
    }
    /* Summary: get a url paramter by name.  Defaults to current window's url. */
    function getParameterByName(name, url) {
        if (!url) url = window.location.href;
        name = name.replace(/[\[\]]/g, "\\$&");
        var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
            results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';
        return decodeURIComponent(results[2].replace(/\+/g, " "));
    }
    //Builds category Filter
    function buildFilter() {
        $("#categoryFilter").hide();
        var categoryButton = "<div><label id='{labelId}'  class='btn categoryFilterButton pull-left'><input id='{id}' type='checkbox' name='{name}'value='{value}'style='opacity:0; position:absolute; left:9999px;'>{categoryName}</input></label></div><br><br>";

        for (var i = 0; i < filterCategories.length; i++) {
            var category = filterCategories[i];
            $("#categoryFilterButtonList").append(categoryButton.replace("{categoryName}", category).replace("{id}", category).replace("{name}", category).replace("{value}", category).replace("{labelId}", "label" + category.replace(/[^\w]/gi, '')));
            addListeners("label" + category.replace(/[^\w]/gi, ''), category);
        }

        $("#countiesFilterButtonText").hide();
        bindcountiesButton();
        bindCategoriesButton();
        bindApplyFilterButton();

    }
    //Builds county filter
    function setCountiesFilter() {
        var countiesButton = "<label id='{labelId}' value='{value}' class='btn categoryFilterButton pull-left'><input id='{id}' type='checkbox' name='{name}'style='opacity:0; position:absolute; left:9999px;'>{countyName}</input></label>";
        for (var j = 1; j < 89; j++) {
            $("#countiesFilter").append(countiesButton.replace("{countyName}", countyList[j]).replace("{id}", j + countyList[j]).replace("{value}", countyList[j]).replace("{labelId}", "label" + countyList[j].replace(" ", "")));
            addCountyListeners("label" + countyList[j], countyList[j]);
        }
    }

    // Updates provider list based on categories 
    function bindApplyFilterButton() {
        $("#applyFilter").unbind('click').click(function () {
            $("#spinner").fadeIn();
            $("#providerList").hide();
            var copyArrayList = copy(headerList);
            var tempHeaderList = new Array();
            var countiestoget = new Array();
            for (var j = 1; j < 89; j++) {
                if (document.getElementById(j + countyList[j]).checked) {
                    countiestoget.push(j);
                }
            }
            if (countiestoget.length === 0) {
                for (var i = 0; i < 89; i++) {
                    countiestoget.push(i);
                }
            }

            getProviders(countiestoget);

            for (var i = 0; i < headerList.length; i++) {
                var category = headerList[i];
                if (document.getElementById(category).checked) {
                    tempHeaderList.push(headerList[i]);
                }
            }
            headerList = tempHeaderList;

            if ((headerList != null) && (tempHeaderList.length > 0)) {
                $("#providerList").hide();
                $("#spinner").fadeIn();
                document.getElementById("providerList").innerHTML = "";
                buildLegalList();

            } else if ((tempHeaderList.length === 0)) {
                headerList = copyArrayList;
                $("#providerList").hide();
                $("#spinner").fadeIn();
                document.getElementById("providerList").innerHTML = "";
                if ($("#categoriesStatement").html() === "All") {
                    headerList = copy(filterCategories);
                    includedCategories = copy(filterCategories);
                }
                buildLegalList();

            }
            headerList = copy(copyArrayList);
        });
    }
    //click event for switching between categories -> counties on modal 
    function bindcountiesButton() {
        $('#countiesButton').click(function () {
            $("#categoriesFilterText").hide();
            $("#countiesFilterButtonText").show();
            document.getElementById("filterModalTitle").innerHTML = "Counties";
            $("#categoriesButton").css({ "visibility": "visible" });
            $("#countiesButton").css({ "visibility": "hidden" });
            if (categoryPressed === 0) {
                includedCategories = copy(filterCategories);
                categoryPressed = 1;
            }
        });
    }
    //click event for switching between counties -> categories on modal
    function bindCategoriesButton() {
        $("#categoriesButton").click(function () {
            $("#countiesFilterButtonText").hide();
            $("#categoriesFilterText").show();
            document.getElementById("filterModalTitle").innerHTML = "Categories";
            $("#countiesButton").css({ "visibility": "visible" });
            $("#categoriesButton").css({ "visibility": "hidden" });
        });
        for (var j = 0; j < includedCategories.length; j++) {
            includedCategories[j] = null;
        }
    }
    //Shows modal for filter button
    function bindFilterButton() {
        $("#filterButton").click(function () {
            $("#categoriesButton").css({ "visibility": "hidden" });
            $("#categoryFilter").modal('show');
            $("#countiesFilterButtonText").hide();
            $("#categoriesFilterText").show();
            document.getElementById("filterModalTitle").innerHTML = "Categories";
            $("#countiesButton").css({ "visibility": "visible" });
        });
    }
    //Changes statement based on what labels are active
    function addListeners(checkboxName, labelToAdd) {
        var selector = $("#" + checkboxName);
        selector.click(function (event) {
            if (selector.hasClass("active")) {
                for (var j = 0; j < includedCategories.length; j++) {
                    if (includedCategories[j] === (labelToAdd)) {
                        includedCategories.splice(j, 1);
                    }
                }
                var replaceWords = $("#categoriesStatement").html();
                var removeWord = labelToAdd.replace(/&/g, "&amp;").replace(/>/g, "&gt;").replace(/</g, "&lt;").replace(/"/g, "&quot;");

                replaceWords = replaceWords.replace(" , " + removeWord, "").replace(removeWord, "");
                replaceWords = replaceWords.trim();
                if (replaceWords.charAt(0) === ',') {
                    replaceWords = replaceWords.substring(1);
                }

                $("#categoriesStatement").html("");
                $("#categoriesStatement").append(replaceWords);
                if ($("#categoriesStatement").html().trim() == "") {
                    $("#categoriesStatement").append("All");
                    includedCategories = copy(filterCategories);
                }
            } else {
                categoryPressed = 1;
                var updateStatement = $("#categoriesStatement").html();
                if (updateStatement.trim() === "All") {
                    $("#categoriesStatement").html("");
                    $("#categoriesStatement").append(labelToAdd + " ");
                    includedCategories = [];
                    includedCategories.push(labelToAdd);
                } else {
                    $("#categoriesStatement").append(" , " + labelToAdd);
                    includedCategories.push(labelToAdd);
                }
            }
        });
    }

    //County listeners for modal filter 
    function addCountyListeners(checkboxName, labelToAdd) {
        var selector = $("#" + checkboxName);
        selector.click(function (event) {
            if (selector.hasClass("active")) {
                var replaceWords = $("#countiesStatement").html();
                var removeWord = labelToAdd.replace(/&/g, "&amp;").replace(/>/g, "&gt;").replace(/</g, "&lt;").replace(/"/g, "&quot;");
                $("#countiesStatement").html("");
                $("#countiesStatement").append(replaceWords.replace(" , ", "").replace(removeWord, ""));
                if ($("#countiesStatement").html().trim() == "") {
                    $("#countiesStatement").append("All");

                    categoryCheck = copy(filterCategories);
                }
            } else {

                var updateCountyStatement = $("#countiesStatement").html();
                if (updateCountyStatement.trim() == "All") {
                    $("#countiesStatement").html("");
                    $("#countiesStatement").append(labelToAdd + " ");
                    categoryCheck.push(labelToAdd.trim());
                } else {
                    $("#countiesStatement").append(" , " + labelToAdd);
                    categoryCheck.push(labelToAdd.trim);
                }
            }
        });
    }
    function copy(o) {
        var output, v, key;
        output = Array.isArray(o) ? [] : {};
        for (key in o) {
            v = o[key];
            output[key] = (typeof v === "object") ? copy(v) : v;
        }
        return output;
    }
    //updaters county filter based on search bar 
    function updateCountyButtons() {
        var substring = $('#filterSearchBar').val();
        for (var i = 1; i < 89; i++) {
            $('#label' + countyList[i].replace(" ", "")).hide();
            if (((countyList[i].replace(" ", "").toLowerCase()).indexOf(substring.toLowerCase()) > -1)) {
                $('#label' + countyList[i].replace(" ", "")).show();
            }
        }
    }

    //search bar listener on modal 
    function setSearchForCountiesBar() {
        $('#filterSearchBar').on('keyup', function () {
            updateCountyButtons();
        });
    }


    /*  
        Callback function for the map.  When a pin is clicked, it will call this function
        with the providerId_locationId
    */
    function mapPinClickCallback(providerLocationId) {

        var item = $('.item_' + providerLocationId);
        
        $(".list-group-item").removeClass('list-group-item-highlighted');

        var firstItem = (item.length > 0) ? item[0] : item;
        item.addClass('list-group-item-highlighted');

        $('#providerList').scrollTo(firstItem);
    }

    /* 
        Callback for the map to set the distance to a provider.  ID is providerId_locationId
    */
    function setDistanceCallback(id, distance) {
        $(".distance_" + id).html(distance + " miles");
    }

    return {
        populate: populatePage,
        mapPinClick: mapPinClickCallback
    };

})();
