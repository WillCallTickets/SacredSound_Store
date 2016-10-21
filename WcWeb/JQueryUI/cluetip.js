/* ================================================================ 
This copyright notice must be kept untouched in the stylesheet at 
all times.

The original version of this script and the associated (x)html
is available at http://www.stunicholls.com/menu/skeleton.html
Copyright (c) 2005-2007 Stu Nicholls. All rights reserved.
This script and the associated (x)html may be modified in any 
way to fit your requirements.
=================================================================== */

merch_tooltip = function () {

    $(".bundle-img IMG ").tooltip({ effect: 'slide' });

}

//we need to do this because we can only have one event attached to onload
jQuery(function ($) {

    merch_tooltip();

//    InitAge18Overlay();
});

//var parmOvInstance = Sys.WebForms.PageRequestManager.getInstance();

//parmOvInstance.add_endRequest(function () {
//    //you need to re-bind your jquery events here
//    InitAge18Overlay();
//});


////register modal   
//function InitAge18Overlay() {

//    var dComp18 = $("[id$='hdnDisplayComply18']").get(0);
//    var bComp18 = $("[id$='hdnIsComply18']").get(0);

//    if (dComp18 != undefined && bComp18 != undefined && dComp18.value === 'True' && bComp18.value === 'False') {

//        // Setup a basic iframe for use inside overlays.
//        var theframe = $('<iframe id="frameoverlay" allowTransparency="true" frameborder="0" scrolling="auto"></iframe>');

//        $('#complianceoverlay').overlay({
//            top: 200,
//            fixed: true,
//            mask: {
//                color: '#222222',
//                loadSpeed: 200,
//                opacity: 0.8
//            },

//            // disable this for modal dialog-type of overlays
//            closeOnClick: false,

//            oneInstance: true,
//            load: true,

//            onBeforeLoad: function () {

//                var link = '/Store/AgeVerification18.aspx';

//                //determine browser

//                var ver = $.browser.version;
//                if ($.browser.msie && (ver == "6.0" || ver == "7.0")) {
//                    location.replace(link + '&browser=down');
//                }
//                else {

//                    // grab wrapper element inside content
//                    var wrap = this.getOverlay().find(".contentWrap");

//                    //Add the link and style attributes to the basic iframe
//                    $(theframe).attr({ src: link, style: 'height:440px; width:100%; border:none;' });

//                    //Write the iframe into the wrap
//                    wrap.html(theframe);

//                    //construct the close button and assign text and a click function
//                    //$("#complianceoverlay .close").text("Close Panel").click(function () { ageposterback(); });
//                    //$("<a class=\"btnmed close\"></a>").appendTo(wrap);
//                    //$("#complianceoverlay .close").css({'color':'red'}).click(function () { ageposterback(); });
//       
//                }

//            }
//        });
//    }
//}
