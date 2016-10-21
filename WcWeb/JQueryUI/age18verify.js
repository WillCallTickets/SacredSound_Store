
$(function () {

    var smt = $('[id$=btnSubmit]');
    if (smt != undefined)
        smt.click(function () { verifySubmission(this);  });
});

function verifySubmission(e) {

    //set cookie if good to go - ignore bogus data
    var month = $('[id$=ddlMonth]').get(0);
    var day = $('[id$=ddlDay]').get(0);
    var year = $('[id$=ddlYear]').get(0);

    var error = '';

    if (month != undefined && month.value !== "0" &&
        day != undefined && day.value !== "0" &&
        year != undefined && year.value !== "0") {
        //call an ajax page method - if we receive a success result than
        //  parent.$('#complianceoverlay a.close').trigger("click");
        $('#message').html('');
        dlg_PageMethod("SetComplianceDate18", initParams(month, day, year), dlg_fnSuccess, dlg_fnError);
    }
    
    return false;
}

function initParams(month, day, year) {

    var pushParams = new Array();

    //get hidden profile info
    pushParams.push("userName");
    var userName = $("#hidUserName").val();
    pushParams.push(userName);

    pushParams.push("profileDob");
    var profileDob = $("#hidProfileDob").val();
    pushParams.push(profileDob);

    pushParams.push("month");
    pushParams.push(month.value);

    pushParams.push("day");
    pushParams.push(day.value);

    pushParams.push("year");    
    pushParams.push(year.value);

    var paramList = '';
    for (var i = 0; i < pushParams.length; i += 2) {

        if (paramList.length > 0) paramList += ',';//add a comma if there is prior content
        paramList += '"' + pushParams[i] + '":"' + pushParams[i + 1] + '"';     
    }

    return '{' + paramList + '}';
}

function dlg_fnSuccess(result) {

    var data = result.d;
    var msg = data["Message"];

    if (msg == "SUCCESS") {
        parent.$('#complianceoverlay a.close').trigger("click");        
    }
    else {
        $('#message').fadeIn(800).html('<div class=\"msg-wrapper\">' + msg + '</div>').css({ 'color': 'red' });
    }
}

function dlg_fnError(xhr, ajaxOptions, thrownError) {
    // Boil the ASP.NET AJAX error down to JSON.
    var err = eval("(" + xhr.responseText + ")");
    $('#message').delay(300).html('<div class=\"msg-wrapper\">' + err + '</div>').css({ 'color': 'red' });
}

function dlg_PageMethod(fn, paramList, successFn, errorFn) {

    $.ajax({
        type: "POST",
        url: window.location.pathname + "/" + fn,
        contentType: "application/json; charset=utf-8",
        data: paramList,
        dataType: "json",
        success: successFn,
        error: errorFn
    });
}