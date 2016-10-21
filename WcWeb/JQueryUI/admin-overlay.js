/* =========
========== */

//do this - only one event attached to onload
$(function () {

    InitAdminOverlay();

});



var prmOvInstance = Sys.WebForms.PageRequestManager.getInstance();

prmOvInstance.add_endRequest(function () {
    //you need to re-bind your jquery events here
    InitAdminOverlay();
});

function InitAdminOverlay() {

    // Setup a basic iframe for use inside overlays.
    var theframe = $('<iframe id="frameoverlay" allowTransparency="true" frameborder="0" scrolling="auto"></iframe>');

    $("INPUT.ov-trigger").overlay({

        close: ".close",
        closeOnClick: false, //make modal
        closeOnEsc: true,
        top: '5%',
        mask: '#666666',
        effect: 'apple',
        oneInstance: true,

        onBeforeLoad: function () {

            var link = this.getTrigger().attr("title");

            //determine browser
            var ver = $.browser.version;
            if ($.browser.msie && (ver == "6.0" || ver == "7.0")) {
                location.replace(link + '&browser=down');
            }
            else {

                // grab wrapper element inside content
                var wrap = this.getOverlay().find(".contentWrap");

                //fix firefox bug - force wysiwyg to display when parent is hidden
                wrap.parent().css('display', 'block');

                //Add the link and style attributes to the basic iframe
                $(theframe).attr({ src: link, style: 'height:700px; width:100%; border:none;' });

                //Write the iframe into the wrap
                wrap.html(theframe);

                //construct the close button and assign text and a click function
                $("<a class=\"close\"></a>").insertBefore(wrap);
                //                $("<a class=\"close\"></a>").appendTo(wrap);
                //$("#overlay-wysiwyg .close").text("Close Panel").click(function () { posterback(); });
                $("#overlay-wysiwyg .close").click(function () { posterback(); });
            }

        },

        onBeforeClose: posterback


    });
}

function posterback() {

    var trigger = $("INPUT.ov-trigger")[0];
    var link = $(trigger).attr("title");
    var parentControl = link.substring(link.indexOf('ctrl=') + 5);
    var parentPieces;
    if (parentControl.length == 0)
        parentControl = "ctl00$MainContent$ctl01";//FF hack

    var parentPieces = parentControl.split('&');
    var parent = parentPieces[0];
    //ctl00$MainContent$ctl00
    __doPostBack(parent, 'rebind');

}