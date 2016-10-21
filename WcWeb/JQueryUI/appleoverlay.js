/* =========
========== */

//do this - only one event attached to onload
$(function () {

    // Setup a basic iframe for use inside overlays.
    var theframe = $('<iframe id="frameoverlay" allowTransparency="true" frameborder="0" scrolling="auto"></iframe>');

    $("a.ov-trigger").overlay({

        close: "#overlay-bundle .close",
        closeOnClick: false, //make modal
        closeOnEsc: true,
        mask: '#666666',
        effect: 'apple',
        oneInstance: true,

        onBeforeLoad: function () {

            var link = this.getTrigger().attr("href");

            //determine browser

            var ver = $.browser.version;
            if ($.browser.msie && (ver == "6.0" || ver == "7.0")) {
                location.replace(link + '&browser=down');
            }
            else {

                // grab wrapper element inside content
                var wrap = this.getOverlay().find(".contentWrap");

                //Add the link and style attributes to the basic iframe
                $(theframe).attr({ src: link, style: 'height:440px; width:100%; border:none;' });

                //Write the iframe into the wrap
                wrap.html(theframe);

                //construct the close button and assign text and a click function
                $("<a class=\"close\"></a>").insertBefore(wrap);
                $("<a class=\"close\"></a>").appendTo(wrap);
                $("#overlay-bundle .close").text("Close Panel").click(function () { posterback(); });
            }

        },

        onBeforeClose: posterback

    });


});


function posterback() {

    //todo find correct way around this hack
    __doPostBack('ctl00$MainContent$ctl00', 'rebindcart');

}