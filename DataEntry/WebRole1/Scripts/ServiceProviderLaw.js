$(document).ready(function () {
    serviceProviderDisplay.populate();

    
});

var serviceProviderDisplay = (function () {
    var specificCategories;
    var allCounties;
    var categoryList;
    var categoryTitle;
    var headerList = new Array();
    var providerList = new Array() ;
    //The html for the list item
    
    var providerText =  "<li class='list-group-item clearfix'>"
        + "<div class='pull-left' style='width: 80%'>"
        + "  <div style='margin-bottom: 10px'>"
        + "    <div><a href='{detailsUrl}'><h4>{providerName}</h4></div>"
        + "    <div> {locationName} </div>"
        + "  </div>"
        + "</div>"
        + "<div class='pull-right distance'><span id=''>12 mi</span></div>"
        + "</li>";
    function populatePage() {
        $("#spinner").fadeIn();
        $("#providerList").hide();
        getCategories(getProviders);
        $("#providerList").show();
    }
    /* Summary: Gets all the categories in the database
               Gets the family information if needed */
    function getCategories(callback) {
        $.ajax({
            url: getCategoryLookupIndexPath,
            cache: false,
            success: function(data) {
                categoryList = data;
                    callback();
            }
        });
    }

    /*Summary:  gets the Providers that are associated with the given 
                categories and shows them in the list */
    function getProviders() {
        $.ajax({
            url: getLCProviderPath,
            cache: false,
            data: {
                "counties": 57
            },
            success: function (data) {
                //For each provider, fill in the variables and add to the list.
                $.each(data, function(i, provider) {
                    var providerCategories = $.map(provider.categories, function(categoryId) {
                        var category = findCategory(categoryId);
                        return (category !== undefined && category !== null) ? category.name : null;
                    });

                   
                    for (var i = 0; i < providerCategories.length; i++) {
                      if ($.inArray(providerCategories[i],headerList)) {
                          headerList.push(providerCategories[i]);
                      }   
                    }

                    providerList = data;

                });
            },
            complete: function () {
                buildLegalList();
                $("#spinner").fadeOut(function () {
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
    /*Build html for legal */
    function buildLegalList() {
        headerList = uniq(headerList);
        for (var j = 0; j < headerList.length; j++) {

            categoryTitle = headerList[j].toString();
            var categoryHtml = "<li> <h3 class='providerCategoryHeader'>"
             +categoryTitle+ "</h3>";
            if (j === 0) {
                $('#providerList').prepend(categoryHtml);
            } else {
                $('#providerList').append(categoryHtml);
            }
            for (var i = 0; i < providerList.length; i++) {
                
                var provider = providerList[i];
                var providerCatName = findCategory(provider.categories);
                if (providerCatName.name === headerList[j]){
                var locationName = (provider.locationName === undefined || provider.locationName === null) ? '' : provider.locationName;
                var detailsUrl = getLCDetails + "/" + provider.serviceProviderId + "?location=" + provider.locationId;
                $("#providerList").append(providerText.replace("{providerName}", provider.name)
                    .replace("{locationName}", locationName)
                    .replace("{detailsUrl}", detailsUrl));
                }
            }
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


    return {
        populate: populatePage
    };
    
})();