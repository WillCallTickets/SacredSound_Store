
$(function () {


});

function initParams(includeSelected) {

    //update cart on server
    var pushParams = new Array();

//    pushParams.push("context");
//    var context = $("#hidContext").val();
//    pushParams.push(context);

//    pushParams.push("saleItem_ItemId");
//    var saleItemId = $("#hidSaleItem_ItemId").val();
//    pushParams.push(saleItemId);

//    pushParams.push("bundleId");
//    var bundleId = $("#hidBundleId").val();
//    pushParams.push(bundleId);

//    if (includeSelected != undefined) {
//        pushParams.push("selectedItemId");
//        pushParams.push(includeSelected);
//    }

    return pushParams;

}

function dlg_fnSuccess(result) {
    //examine xhr for result

    //update selectionStatus
    if (result.d != "false") {
        var data = result.d;

//        var status = $(".mblist-num-select");
//        status.text(data["SelectStatus"]);

//        $("#lstCollector").html(data["ListContent"]);

//        $(".priceline").html(data["Total"]);
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