
/*Summary: Functions dealing with the default county for filtering */
var countyDefault = (function () {
    /*Summary: Reads the given cookie's value from the browser
      cookieName: The name of the cookie.    */
    function readCookie(cookieName) {
        var name = cookieName + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) === ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) === 0) {
                return c.substring(name.length, c.length);
            }
        }
        return "";
    }

    /*Summary: Gets the selected county's ID from a cookie. */
    function getSelectedCountyId() {
        return readCookie("countyId");
    }

    /*Summary: Gets the selected county's name from a cookie. */
    function getSelectedCountyName() {
       return readCookie("countyNamecookie");
    }

    return {
        getId: getSelectedCountyId,
        getName: getSelectedCountyName
    };
})();


$(document).ready(function () {
    getCounties();
    switchCounty();
    $(".nameCounty").append(countyDefault.getName()+" County");

    function switchCounty() {
        $(".differentCounty").click(function () {
            var countyId = countyDefault.getId();

            var deleteCookie = function (name) {
                document.cookie = name + '=;expires=Thu, 01 Jan 1970 00:00:01 GMT;';
            };
            deleteCookie("countyId");
            getCounties(countyId);

        });
    }
    
    function initiateAviator(selectedCounty){
     if (!checkLocationCookieNull()) {
        disableSelectCounty();
        makeLocationCookie();

         $(".specialDropdown").select2({
             dropdownParent: $("#countySelectionModal"),
             placeholder: "Select a county"
         });
         if (selectedCounty != undefined) {
             $(".specialDropdown").val(selectedCounty).trigger('change');
         } else {
             $("#selectCounty").prop("disabled", true);
         }

         $('#countySelectionModal').modal({
            backdrop: 'static',
            keyboard: false
        });
        $('#countySelectionModal').modal('show');
        
    }
    }
    //gets counties for filter
    function getCounties(selectedCountyId) {
        var addCounty = "<option value={countyName}>{county}</option>";
        var addSelectedCounty = "<option value={countyName} selected='selected'>{county}</option>";
        var countyList;
        $.ajax({
            url: getCountiesPath,
            cache: false,
            success: function (data) {
                countyList = data;
                $("#e2").html('');
                $.each(countyList, function (i, county) {
                   if (i !== selectedCountyId) {
                        $("#e2").append(addCounty.replace("{countyName}", i).replace("{county}", county));
                    } else {
                       $("#e2").append(addSelectedCounty.replace("{countyName}", i).replace("{county}", county));
                    }
                });

                initiateAviator(selectedCountyId);
            }
        });
    }
    function disableSelectCounty() {
        $(".specialDropdown").on('change', function() {
            $("#selectCounty").prop('disabled', false);
        });
    }
    function makeLocationCookie() {
        $("#selectCounty").click(function() {
            document.cookie = "countyId" + "=" + $(".specialDropdown").val();
            document.cookie = "countyNamecookie" + "=" + $("#e2 option:selected").text();
            location.reload();
        });
    }
   
    function checkLocationCookieNull() {
        var cookieNull =  countyDefault.getId();
        if (cookieNull === ""||cookieNull === "null") {
            return false;
        } else {
            return true;
        }
    }

    $(function() {
        // Turn on tooltips
        $('[data-toggle="tooltip"]').tooltip();
    });
});
