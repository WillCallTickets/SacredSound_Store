

jQuery(function ($) {
    
    InitAge18Overlay();
});


var parmOvInstance = Sys.WebForms.PageRequestManager.getInstance();

parmOvInstance.add_endRequest(function () {
    //re-bind jquery events
    InitAge18Overlay();
});

//register modal   
function InitAge18Overlay() {

    var dComp18 = $("[id$='hdnDisplayComply18']").get(0);
    var bComp18 = $("[id$='hdnIsComply18']").get(0);

    if (dComp18 != undefined && bComp18 != undefined && dComp18.value === 'True' && bComp18.value === 'False') {

        // Setup a basic iframe for use inside overlays.
        var theframe = $('<iframe id="frameoverlay" allowTransparency="true" frameborder="0" scrolling="auto"></iframe>');

        $('#complianceoverlay').overlay({
            top: 200,
            fixed: true,
            mask: {
                color: '#222222',
                loadSpeed: 200,
                opacity: 0.9
            },

            // disable this for modal dialog-type of overlays
            closeOnClick: false,

            oneInstance: true,
            load: true,

            onBeforeClose: age18cancel,

            onBeforeLoad: function () {

                var link = '/Store/AgeVerification18.aspx';

                //determine browser

                var ver = $.browser.version;
                if ($.browser.msie && (ver == "6.0" || ver == "7.0")) {
                    location.replace(link + '&browser=down');
                }
                else {

                    // grab wrapper element inside content
                    var wrap = this.getOverlay().find(".contentWrap");

                    //Add the link and style attributes to the basic iframe
                    $(theframe).attr({ src: link, style: 'height:180px; width:100%; border:none;' });

                    //Write the iframe into the wrap
                    wrap.html(theframe);

                    //construct the close buttons and assign text and a click function
                    $("#complianceoverlay .close").text("Close").addClass('top-panel-close');
                    $("<a class=\"btnmed close bottom-panel-close\" ></a>").appendTo(wrap);
                    $('#complianceoverlay .close').text('Cancel').attr({ 'title' : 'cancel' }).css({ "cursor": "pointer" }).click(function () { age18cancel(); });
                }
            }
        });
        
        //$('#verify18_container .cancel').css({ "color": "red" }).click(function () { age18cancel(this); });
    }
}

function age18cancel() {

    //todo - find parent page control programatically

    //test for compliance in postbackevent
    __doPostBack('ctl00$MainContent$ctl00', 'complianceclose');

    return false;
}
