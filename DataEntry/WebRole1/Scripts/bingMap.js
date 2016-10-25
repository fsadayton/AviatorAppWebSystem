/* Bing Map setup*/
var bingMap = (function() {
    var searchManager;
    var providers;
    var map = null;
    var pinCallback = null;
    var distanceUpdate = null;
    var allPushpins = [];
    var lastZoomLevel = null;
    var directionsLinkId = null;

    /*
        SUMMARY: Entry point for setting up the map.  

        mapID: The id of the wrapper div
        providerList: Array of all providers to map
        pinClickedCallback: Function to call when a pin is clicked. Used to highlight the correspoinding provider.
        distanceUpdateCallback:  Callback to give a distance for each provider once it is calculated.  Is called for each provider.
    */
    function setupMap(mapId, providerList, pinClickedCallback, distanceUpdateCallback) {
        getMap(mapId);
        providers = providerList;
        pinCallback = pinClickedCallback;
        distanceUpdate = distanceUpdateCallback;
        allPushpins = [];
    }

    /*
        SUMMARY: Entry point for setting up the map.  

        mapID: The id of the wrapper div
        provider: provider to show on the map
        linkToDirectionsId: ID of the directions link.  
    */
    function setupSinglePointMap(mapId, provider, linkToDirectionsId) {
        getMap(mapId);
        providers = [];
        providers.push(provider);
        pinCallback = null;
        distanceUpdate = null;
        allPushpins = [];
        directionsLinkId = linkToDirectionsId;
    }

    /*
        Set up the map with Microsoft
    */
    function getMap(mapId) {

        if (map === null) {
            map = new Microsoft.Maps.Map(document.getElementById(mapId), {
                credentials: bingMapsKey,
                showMapTypeSelector: false,
                enableClickableLogo: false,
                showScalebar: false,
                enableSearchLogo: false
            });
            map.setView({ mapTypeId: Microsoft.Maps.MapTypeId.road });
        }

        loadSearchModule();

        Microsoft.Maps.Events.addHandler(map, 'viewchangeend', viewChanged);

    }

    /*
        Load the search manager if needed and start loading the locations.
    */
    function loadSearchModule() {
        if (searchManager) {
            findLocations();
        }
        else {
            Microsoft.Maps.loadModule('Microsoft.Maps.Search', { callback: findLocations });
        }
    }

    /* 
        Clears all the old locations and starts getting all the locations.
    */
    function findLocations() {
        allPushpins = [];
        map.entities.clear();

        //Don't start getting all the provider locations until we know our current location.
        //This is so we can calculate the distance to the providers.
        getLocation();
    }

    /*
        Send out all the geocode requests for each provider.
    */
    function geocodeAllProviders() {
        
        providers.forEach(function (provider) {
            geocodeRequest(provider);
        });

    }

    /* Send a geocode request for a provider */
    function geocodeRequest(provider) {
        createSearchManager();
        var where = provider.address;
        var userData = provider;
        var request =
        {
            where: where,
            count: 5,
            bounds: map.getBounds(),
            callback: onGeocodeSuccess,
            errorCallback: onGeocodeFailed,
            userData: userData
        };
        searchManager.geocode(request);
    }

    /* 
        Create the search manager for the map
    */
    function createSearchManager() {
        map.addComponent('searchManager', new Microsoft.Maps.Search.SearchManager(map));
        searchManager = map.getComponent('searchManager');
    }

    /*
        When the data is geocded succesfully, a pushpin is createed at that point on the map.
    */
    function onGeocodeSuccess(result, providerData) {
        if (result) {
            var topResult = result.results && result.results[0];
            if (topResult) {
                var distance = null;
                if (curLat !== null) {
                    distance = getDistance(curLat, curLon, topResult.location.latitude, topResult.location.longitude).toFixed(1);
                }

                var pushpin = new Microsoft.Maps.Pushpin(topResult.location, {
                    anchor: new Microsoft.Maps.Point(12, 24),
                    width: 200, 
                    height: 32,
                    typeName: 'Pushpins',
                    icon: applicationRoot + '/Content/Images/map-marker.png'
                });
                

                pushpin.title = providerData.name;
                pushpin.description = distance;
                pushpin.selected = false;

                if (distanceUpdate != null  && distance !== null) {
                    distanceUpdate(providerData.serviceProviderId + '_' + providerData.locationId, distance);
                }

                pushpin.getProviderIDLocationID  = function() {
                    return  providerData.serviceProviderId + '_' + providerData.locationId;
                }


                if (providers.length > 1) {
                    // Add handler for the pushpin click event.
                    Microsoft.Maps.Events.addHandler(pushpin, 'click', pinClicked);
                }

                if (providers.length === 1) {
                    setPinOptions(pushpin, true);
                    $("#" + directionsLinkId).attr('href', "http://bing.com/maps/default.aspx?rtp=adr.My%20Location~adr." + providerData.address);
                }

                map.setView({ center: topResult.location, zoom: 10 });
                map.entities.push(pushpin);

                allPushpins.push(pushpin);
            }
        }
    }

    /* 
        Geocode failed event
    */
    function onGeocodeFailed(result, userData) {
        console.error('Geocode failed');
    }

    /* 
        Get's the user's current position.
    */
    function getLocation() {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(showPosition, function () {
                curLat = null;
                curLon = null;
                geocodeAllProviders();
            });
        }
        else {
            curLat = null;
            curLon = null;
            geocodeAllProviders();
            //x.innerHTML = "Geolocation is not supported by this browser.";
        }
    }

    /*
        Set's the current lat long from the given position.
    */
    function showPosition(position) {
        curLat = position.coords.latitude;
        curLon = position.coords.longitude;
        geocodeAllProviders();
    }

    /* 
        Pin Clicked event.  Makes only the clicked pin have an orange marker.  
        Hides the other pin's titles.
    */
    function pinClicked(e) {
        if (e.targetType === 'pushpin') {
            if (pinCallback != null) {
                pinCallback(e.target.getProviderIDLocationID());
            }

            for (var i = 0; i < allPushpins.length; i++) {
                allPushpins[i].setOptions({ icon: applicationRoot + 'Content/Images/map-marker.png', text: '', width:24, height:30 });
                allPushpins[i].selected = false;
            }

            e.target.setOptions({ icon: applicationRoot +  'Content/Images/map-marker_orange.png', text: e.target.title, width: 200, height: calculateHeightForPin(e.target.title) });
            e.target.selected = true;
        }
    }

    /*
        Tracks the zoom level of the map.  Shows the provider's name if the map is zoomed in enough
    */
    function viewChanged(e) {
        if (providers.length > 1 && lastZoomLevel !== map.getZoom()) {
            lastZoomLevel = map.getZoom();

            for (var i = 0; i < allPushpins.length; i++) {
                setPinOptions(allPushpins[i], lastZoomLevel >= 14 || allPushpins[i].selected);
            }
        }
    }

    /*
        Sets the height, width, text for a pin
    */
    function setPinOptions(pin, isTextShown) {
        if (isTextShown) {
            pin.setOptions({ text: pin.title, width: 200, height: calculateHeightForPin(pin.title) });
        } else {
            pin.setOptions({ text: '', width: 24, height: 30 });
        }
    }

    /* 
        Calcuates the height the div will need to be to hold the provider's name
    */
    function calculateHeightForPin(text) {
        var height = (text.length / 15) * 40;
        return (height < 40) ? 40 : height;
    }

    /*  
        Gets the distance between two lat longs 
    */
    function getDistance(lat1, lon1, lat2, lon2, unit) {
        var radlat1 = Math.PI * lat1 / 180;
        var radlat2 = Math.PI * lat2 / 180;
        var theta = lon1 - lon2;
        var radtheta = Math.PI * theta / 180;
        var dist = Math.sin(radlat1) * Math.sin(radlat2) + Math.cos(radlat1) * Math.cos(radlat2) * Math.cos(radtheta);
        dist = Math.acos(dist);
        dist = dist * 180 / Math.PI;
        dist = dist * 60 * 1.1515;
        if (unit == "K") { dist = dist * 1.609344 }
        if (unit == "N") { dist = dist * 0.8684 }
        return dist;
    }

    return {
        setupMap: setupMap,
        setupSinglePointMap: setupSinglePointMap
    };
})();