
$(function () {

    InitSort();
});

var prmInstance = Sys.WebForms.PageRequestManager.getInstance();

prmInstance.add_endRequest(function () {
    //you need to re-bind your jquery events here
    InitSort();
});


function InitSort() {

    $("#fetord-wrapper UL").sortable({
        placeholder: "ui-state-highlight",
        update: function (event, ui) {
            //call to update the db

            //TODO: async is ok - 

            UpdateOrder_fet();
        }
    });

    $("#fetord-wrapper UL").disableSelection();

}

function initParams(str) {

    //update cart on server
    var pushParams = new Array();

    if (str != undefined) {
        pushParams.push("str");
        pushParams.push(str);
    }

    return pushParams;
}

function UpdateOrder_fet() {
    
    //get the list items and loop thru
    //construct a string with the order and update the db
    var s = '';
    var lis = $("#fetord-wrapper UL LI");

    if (lis.length > 0) {

        lis.each(function (index) {

            var ds = $(this).attr("id");
            var pieces = ds.split('_');
            var selectedItemId = pieces[1];

            s += selectedItemId + ',';
        });

        if(s.length > 0)
            s = s.slice(0, -1);
    }
    
    dlg_PageMethod("UpdateOrder_fet", initParams(s), dlg_fnSuccess, dlg_fnError);
}

function dlg_fnSuccess(result) {
    //examine xhr for result

    //update selectionStatus
    if (result.d != "false") {

        //reorder rows
        var lis = $("#fetord-wrapper UL LI");

        if (lis.length > 0) {

            var max = $("#hidMax").val();

            lis.each(function (index) {

                var idx = index + 1;

                //find the fetrow span element and update the text within to the index of the row
                $(this).find(".fetrow").text(idx);

                //if we are under the max then make sure we dont have the over-quota class
                if (max == "0" || idx <= max)
                    $(this).removeClass('over-quota');
                //if max != 0 and index > max then make sure we have the over-quota class
                else if (max != "0" && idx > max)
                    $(this).addClass('over-quota');

            });
        }
    }
}

function dlg_fnError(xhr, ajaxOptions, thrownError) {
    // Boil the ASP.NET AJAX error down to JSON.
    var err = eval("(" + xhr.responseText + ")");
    //$('#dialogerror').append(err.Message);
}

function dlg_PageMethod(fn, paramArray, successFn, errorFn) {

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
        url: pagePath + "/" + fn,
        contentType: "application/json; charset=utf-8",
        data: paramList,
        dataType: "json",
        success: successFn,
        error: errorFn
    });
}
