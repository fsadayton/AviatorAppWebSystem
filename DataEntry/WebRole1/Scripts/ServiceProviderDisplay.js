
var serviceProviderDisplay = (function () {
    var familyCategories;
    var familyList;
    var allCounties;
    var categoryList;
    var shownCategories;
    var countyList = countyDefault.getId();
    var filterCategories = new Array();
    var includedCategories = new Array();
    var filterCounter = 0;
    var categoryPressed = 0;
    var familyIdPassed;
    var isShowingSpecialPop = false;
    function populatePage(isSpecialPopulations) {

        isShowingSpecialPop = (isSpecialPopulations === undefined) ? false : isSpecialPopulations;

        $("#spinner").fadeIn();
        $("#providerList").hide();

        getFilterCategories();
        getCategories(function () { getProviders(countyList) });
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
    //gets categories for filter
    function getFilterCategories() {
        familyIdPassed = getUrlParameter('family');
        $.ajax({
            url: getFamilyCategories + "?familyId=" + familyIdPassed,
            cache: false,
            success: function (data) {
                var tempfilterCategories = data;
                for (var f = 0; f < data.length; f++) {
                    filterCategories[f] = tempfilterCategories[f].Name;
                    includedCategories[f] = filterCategories[f];
                }

            }

        });
    }

    //Reads in a parameter from the URL
    function getUrlParameter(sParam) {
        var sPageURL = decodeURIComponent(window.location.search.substring(1)),
            sURLVariables = sPageURL.split('&'),
            sParameterName,
            i;

        for (i = 0; i < sURLVariables.length; i++) {
            sParameterName = sURLVariables[i].split('=');

            if (sParameterName[0] === sParam) {
                return sParameterName[1] === undefined ? true : sParameterName[1];
            }
        }
    };
    /* Summary: Gets all the categories in the database
                Gets the family information if needed */
    function getCategories(callback) {
        $.ajax({
            url: getCategoryLookupIndexPath,
            cache: false,
            success: function (counties) {
                allCounties = counties;
                var familyIdParam = getParameterByName('family');
                var categoryIdParam = getParameterByName('category');
                //GET all the categories
                $.ajax({
                    url: getCategoryLookupIndexPath,
                    cache: false,
                    success: function (data) {
                        categoryList = data;
                        if (familyIdParam !== undefined && familyIdParam !== null) {
                            //Get the categories associated with the given family
                            var familyPath = (isShowingSpecialPop) ? getSpecialPopulationFamiliesPath : getFamilyPath;
                            $.ajax({
                                url: familyPath,
                                cache: false,
                                success: function (data) {
                                    familyList = data;
                                    var family = findFamily(familyIdParam);
                                    $("#sectionName").html(family.name);
                                    familyCategories = family.categoryIds;
                                    shownCategories = familyCategories;
                                    callback();
                                }
                            });
                        }
                        else if (categoryIdParam) {
                            //Get the crime categories
                            $.ajax({
                                url: getCrimeTypes,
                                cache: false,
                                success: function (data) {
                                    familyCategories = [categoryIdParam];
                                    shownCategories = [];
                                    $.each(data, function (i, familyCategory) {
                                        shownCategories.push(familyCategory.id);
                                    });
                                    var selectedCategory = findCategory(categoryIdParam);
                                    $("#sectionName").html(selectedCategory.name);
                                    callback();
                                }
                            });
                        }
                    }
                });
            }
        });
    }
    //filters headerList to make a set 
    function uniq(a) {
        var seen = {};
        return a.filter(function (item) {
            return seen.hasOwnProperty(item) ? false : (seen[item] = true);
        });
    }
    /*Summary:  gets the Providers that are associated with the given 
                categories and shows them in the list */
    function getProviders(countyList) {

        uniq(familyCategories);
        $.ajax({
            url: getServiceProvidersApiPath,
            cache: false,
            data: {
                "counties": countyList,
                "categories": familyCategories
            },
            success: function (data) {

                document.getElementById("providerList").innerHTML = "";

                //The html for the list item
                var providerText = "<a href='{detailsUrl}' ><li class='list-group-item list-group-item-hover clearfix' id='item_{listItemId}'>"
                    + ""
                    + "<div class='pull-left' style='width: 80%'>"
                    + "  <div style='margin-bottom: 10px'>"
                    + "    <div><h4>{providerName}</h4></div>"
                    + "    <div> {locationName} </div>"
                    + "  </div>"
                    + "  <div style='color: green; font-size: .8em;'>{categoryList}</div>"
                    + "</div>"
                    + "<div class='pull-right distance'><span id='distance_{listItemId}'></span></div>"
                    + "</li></a>";

                var filteredList = [];

                // For each provider, fill in the variables and add to the list.
                $.each(data, function (i, provider) {
                    var locationName = (provider.locationName === undefined || provider.locationName === null) ? '' : provider.locationName;
                    var resourcePath = (isShowingSpecialPop) ? getSpecialPopulationsDetailsPath : getPersonalResourcesDetailsPath;
                    var detailsUrl = resourcePath + "/" + provider.serviceProviderId + "?location=" + provider.locationId;
                    var listItemId = provider.serviceProviderId + '_' + provider.locationId;
                    var familyCategoryNames = [];
                    uniq(shownCategories);
                    // only show the categories that either fall under the family selected or fall under crime
                    for (var j = 0; j < shownCategories.length; j++) {
                        if (provider.categories.indexOf(shownCategories[j]) >= 0) {
                            var category = findCategory(shownCategories[j]);
                            if (category !== undefined && category !== null) {
                                familyCategoryNames.push(category.name);
                            }
                        }
                    }
                    uniq(familyCategoryNames);
                    // Fill in all the variables.
                    var providerCategories = familyCategoryNames.join(', ');
                    var categorycheck = providerCategories.split(',');
                    var shouldInclude = false;
                    for (var i =0; i<categorycheck.length;i++){
                        if (contains(includedCategories, categorycheck[i])) {
                            shouldInclude = true;
                        }
                    }
                    var checkTypeCategory = window.location.href;
                    if (checkTypeCategory.search("category") > -1) {
                        shouldInclude = true;
                        
                    } else {
                        
                    }
                    if (shouldInclude) {
                        filteredList.push(provider);
                        $("#providerList").append(
                            providerText.replace("{providerName}", provider.name)
                            .replace("{locationName}", locationName)
                            .replace("{categoryList}", providerCategories)
                            .replace("{detailsUrl}", detailsUrl)
                            .replace(/{listItemId}/g, listItemId));
                    }

                });
                if (document.getElementById("providerList").innerHTML === "") {
                    document.getElementById("providerList").innerHTML = "Sorry, no results found. Please update filter";
                }

                bingMap.setupMap('myMap', filteredList, mapPinClickCallback, setDistanceCallback);
            },
            complete: function () {
                $("#spinner").fadeOut(function () {
                    if (filterCounter === 0) {
                        buildFilter();
                        bindFilterButton();
                        getCounties();
                        bindApplyFilterButton();
                        setSearchForCountiesBar();
                        filterCounter = 1;
                    }
                    $("#providerList").fadeIn();
                });
            }
        });

    }

    /* 
        Callback for the map to set the distance to a provider.  ID is providerId_locationId
    */
    function setDistanceCallback(id, distance) {
        $("#distance_" + id).html(distance + " miles");
    }

    function contains(a, obj) {
        for (var i = 0; i < a.length; i++) {
            if (a[i] === obj) {
                return true;
            }
        }
        return false;
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
            var checkTypeCategory = window.location.href;
            if (checkTypeCategory.search("category") > -1) {
                $("#categoriesFilterText").hide();
                document.getElementById("filterModalTitle").innerHTML = "Counties";
                $("#categoriesButton").css({ "visibility": "hidden" });
                $("#countiesButton").css({"visibility":"hidden"});
                $("#countiesFilterButtonText").show();
            }
        });
    }

    function buildFilter() {
        uniq(filterCategories);
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
    /* Summary: find a family ids in array of famliy objects */
    function findFamily(familyId) {
        for (var i = 0; i < familyList.length; i++) {
            if (familyList[i].id == familyId) {
                return familyList[i];
            }
        }
        return null;
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
    //Builds county filter
    function setCountiesFilter() {
        $("#categoriesFilterText").hide();
        $("#countiesFilterButtonText").show();
        document.getElementById("filterModalTitle").innerHTML = "Counties";
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
            if ($("#categoriesStatement").html().trim() == "All") {
                includedCategories = copy(filterCategories);
            }
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
            $("#providerList").innerHTML = "";
            getProviders(countiestoget);
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

                }
            } else {

                var updateCountyStatement = $("#countiesStatement").html();
                if (updateCountyStatement.trim() === "All") {
                    $("#countiesStatement").html("");
                    $("#countiesStatement").append(labelToAdd + " ");
                } else {
                    $("#countiesStatement").append(" , " + labelToAdd);
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
                if ($("#categoriesStatement").html().trim() === "") {
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

    /*  
        Callback function for the map.  When a pin is clicked, it will call this function
        with the providerId_locationId
    */
    function mapPinClickCallback(providerLocationId) {
        $(".list-group-item").removeClass('list-group-item-highlighted');
        var item = $('#item_' + providerLocationId);
        item.addClass('list-group-item-highlighted');
        $('#providerList').animate({
            scrollTop: item.offset().top - 200
        }, 500);

    }
    return {
        populate: populatePage,
        mapPinClick: mapPinClickCallback
    };

})();

