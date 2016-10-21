/* ================================================================ 
This copyright notice must be kept untouched in the stylesheet at 
all times.

The original version of this script and the associated (x)html
is available at http://www.stunicholls.com/menu/skeleton.html
Copyright (c) 2005-2007 Stu Nicholls. All rights reserved.
This script and the associated (x)html may be modified in any 
way to fit your requirements.
=================================================================== */

stuHover = function() {
    var cssRule;
    var newSelector;
    for (var i = 0; i < document.styleSheets.length; i++)

        for (var x = 0; x < document.styleSheets[i].rules.length; x++) {
        cssRule = document.styleSheets[i].rules[x];
        if (cssRule.selectorText.indexOf("LI:hover") != -1) {
            newSelector = cssRule.selectorText.replace(/LI:hover/gi, "LI.iehover");
            document.styleSheets[i].addRule(newSelector, cssRule.style.cssText);
        }
    }
    
    var mnu = document.getElementById("navskl");

    if (mnu != undefined) {
        var getElm = mnu.getElementsByTagName("LI");

        for (var i = 0; i < getElm.length; i++) {
            getElm[i].onmouseover = function() {
                this.className += " iehover";
            }
            getElm[i].onmouseout = function() {
                this.className = this.className.replace(new RegExp(" iehover\\b"), "");
            }
        }
    }
}
if (window.attachEvent) window.attachEvent("onload", stuHover);

admin_accordian = function () {

    var idx = $('#accordion > A.accord').index($('.current')[0]);

    $("#accordion").tabs("#accordion div.pane",
    {
        tabs: 'A.accord',
        effect: 'fade',
        initialIndex: idx
    });

    //accordion removes the click link - so we need to redo it
    $('#accordion > A.accord').bind('click', function (event) {

        var href = this['href'];
        if (href != undefined) {
            if (href.toLowerCase().indexOf("publish") == -1 && href.toLowerCase() != location.pathname.toLowerCase()) {
                location.replace(href);
            }
        }

    });

}

//we need to do this because we can only have one event attached to onload
jQuery(function ($) {

    admin_accordian();

    //assign a web service event to the click of the publish button
    $('#publishMenuButton').click(function (clk) {

        var reply = confirm('Are you positive that you want to publish? Publishing within 30 minutes of a big onsale or during heavy traffic can be detrimental to how the store will function.');

        if (reply) {

            var params = new Array();

            exec_CallAdminService("PublishButton", params, publishSuccess, publishError);

            return true;
        }

        return false;

    });


});

function publishSuccess(result) {
    //examine xhr for result
    //if true then redirect to email page
    //    var toEml = result.d.toString().trim();
    //    if (toEml.length > 0) {
    //        try {
    //            dlg_resetError();
    //            var url = "/Components/Store/GiftCert_Email.aspx?eml=" + toEml;
    //            window.location = url;
    //        }
    //        catch (e) { }
    //    }
    alert('Publish complete!');
}

function publishError(xhr, ajaxOptions, thrownError) {
    // Boil the ASP.NET AJAX error down to JSON.
    //var err = eval("(" + xhr.responseText + ")");
    //    $('#dialogerror').append(err.Message);
    alert('Publish Error :(');
}



function exec_CallAdminService(fn, paramArray, successFn, errorFn) {
    var pagePath = window.location.pathname;
    //Create list of parameters in the form:   
    //{"paramName1":"paramValue1","paramName2":"paramValue2"}   
    var paramList = '';
    if (paramArray != undefined && paramArray.length > 0) {
        for (var i = 0; i < paramArray.length; i += 2) {
            if (paramList.length > 0) paramList += ',';
            paramList += '"' + paramArray[i] + '":"' + paramArray[i + 1] + '"';
        }
    }
    paramList = '{' + paramList + '}';
    //Call the page method   
    $.ajax({
        type: "POST",
        //url: pagePath + "/" + fn,
        url: "/Services/Admin/AdminServices.asmx/" + fn,
        contentType: "application/json; charset=utf-8",
        data: paramList,
        dataType: "json",
        success: successFn,
        error: errorFn
    });
}