
var ServiceProviderTypeAhead = (function() {
    var serviceProviders;

    //Sets the provider id and the contact email from the selection
    function setProviderIdAndEmail(providerName) {
        var foundProvider = null;
        for (var i = 0 ; i < serviceProviders.length; i++) {
            if (serviceProviders[i].Name === providerName) {
                foundProvider = serviceProviders[i];
                break;
            }
        }

        if (foundProvider != null) {
            $("#ServiceProviderId").val(foundProvider.Id);
            if (foundProvider.Locations != undefined && foundProvider.Locations[0] != undefined) {
                $("#InviteeEmailAddress").val(foundProvider.Locations[0].Contact.Email);
            }
        }
        else {
            $("#ServiceProviderId").val(0);
            $("#InviteeEmailAddress").val('');
        }
    }

    // Set up the typeahead box.  Set the engine and initialization parameters.
    function setUpTypeAhead() {
        var engine = new Bloodhound({
            local: serviceProviders,
            identify: function (obj) { return obj.Id; },
            datumTokenizer: Bloodhound.tokenizers.obj.whitespace('Name'),
            queryTokenizer: Bloodhound.tokenizers.whitespace
        });

        $('#serviceProvider .typeahead').typeahead({
            hint: true,
            highlight: true,
            minLength: 1
        },
        {
            name: 'ServiceProviders',
            display: 'Name',
            displayKey: 'Name',
            source: engine,
            templates: {
                empty: [
                    '<div class="tt-emptyMessage">',
                    'unable to find any Service Providers',
                    '</div>'
                ].join('\n'),
                suggestion: function (data) {
                    return '<p><strong>' + data.Name + '</strong> <br> ' + data.Description + '</p>';
                }
            }
        })
        .on('typeahead:selected', function (e, sug) {
            //User clicked one of the options
            setProviderIdAndEmail(sug.Name);
         })
        .on('typeahead:autocomplete', function (e, sug) {
            //User tabbed out and autocompleted
            setProviderIdAndEmail(sug.Name);
        })
        .on('typeahead:change', function (e) {
            //User left the textbox.  Need this incase they found no valid provider.
            if ($("#ServiceProviderName").val() !== '') {
                setProviderIdAndEmail($("#ServiceProviderName").val());
            }
        });
    };

    // Loads all the service providers into the typeahead.  Hides the box until its ready to go.
    function getServiceProviders() {
        $("#spinner").fadeIn();
        $.ajax({
            cache: false,
            type: "GET",
            url: getAllServiceProvidersPath,  //Found in layout
            success: function (data) {
                serviceProviders = data;
                setUpTypeAhead();
                $("#spinner").fadeOut();
            }
        });
    };    

    return {
        getServiceProviders: getServiceProviders
    };
})();
