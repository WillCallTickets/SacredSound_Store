

var pagemgrInstance = Sys.WebForms.PageRequestManager.getInstance();

//do this because we can only have one event attached to onload
jQuery(function ($) {

    registerManifestPhoneToggle();
    registerAdminDisplayModal();

});

//Normal page load
// $()
// add_pageLoaded

//AJAX postback
// add_beginRequest
// add_pageLoaded
// add_endRequest

//Page.add_beginRequest(OnBeginRequest);
//Page.add_pageLoaded(OnPageLoaded);
//Page.add_endRequest(OnEndRequest);

pagemgrInstance.add_beginRequest(function () {

});

pagemgrInstance.add_pageLoaded(function () {

});

pagemgrInstance.add_endRequest(function () {

    registerAdminDisplayModal();
});


/*****************************************************************/
/****************************POSTBACK*****************************/
/*****************************************************************/
//numRemoveControls 1 for page, 1+ when the control is nested in another control within the page
function getPostbackContainerIdHelper(controlName, numRemoveControls) {
    var pieces = controlName.split('$');
    pieces.splice(-(numRemoveControls), numRemoveControls)
    return pieces.join('$');
}
function postBack_ToControl_WithArgs(controlName, argumentArray) {
    __doPostBack(controlName, JSON.stringify(argumentArray));
}

/* Usage
.on('changeDate', function (ev) {
    var container = getPostbackContainerIdHelper(ev.currentTarget.name, 1);
    postBack_ToControl_WithArgs(container, { "commandName": 'dtpickerchange', "newDate": ev.date.valueOf().toString() });
*/
/*****************************************************************/



function exec_CallAdminService(fn, paramArray, successFn, errorFn) {
    var pagePath = window.location.pathname;
    //Create list of parameters in the form:   
    //{"paramName1":"paramValue1","paramName2":"paramValue2"}   
    //Call the page method   
    $.ajax({
        type: "POST",
        url: "/Svc/Admin/AdminServices.asmx/" + fn,
        contentType: "application/json; charset=utf-8",
        data: parseParams(paramArray),
        dataType: "json",
        success: successFn,
        error: errorFn
    });
}


registerManifestPhoneToggle = function () {

    //$('#" + chkPhone.ClientID + "').on('click', function () { $('.phoner').toggle(); });
}


registerAdminDisplayModal = function () {

    launchPopupModal = function (args) {

        var s = args;

        $('#popupmodal').modal('show');
    }

    loadPopupInfo = function () {

        var s = 'l';
        //_renderContent(null, "adminshow", "", "#admindisplaymodal .modal-body", "#admindisplaymodal #admindisplaymodallabel");
    }

    //manifest - if (Session["OrderEvent"] == null)

    //$('.btn-popupmodal-launch').on('click', function () {


        
    //    loadPopupInfo();
    //});

    $('#popupmodal .modal-body')
        .html('<button type="button" class="hide-from-view" data-toggle="button" data-loading-text="<span class=\'wct-modal-loader-spinner\'></span>Loading...">Loading info...</button>');

    $('#popupmodal')
        .on('show.bs.modal', function () {

            var content = $('#popupmodal .modal-body').html();
            if (content == "") {
                loadPopupInfo();
            }
        })
        .on('shown.bs.modal', function () {
        })
        .on('hidden.bs.modal', function () {
            $('#popupmodal .modal-body').html('');
        })
    ;
}
//registerShowCopyModal = function () {

//    var copyModal = $('#showcopymodal');

//    if (copyModal.length > 0) {

//        $('.showcopy-modal-launcher').on('click', function () {
//            $('#showcopymodal').modal('show');
//        });

//        $('#showcopymodal').wctModal(
//            'showCopy',
//            //define success
//            wct_showCopySuccess,
//            //define inputs
//            ['#copyshowdate', '#hdnUserName', '#hdnCurrentShowId'],
//            wct_showCopyParamBuilder
//            ).on('show.bs.modal', function () {

//                //help ie with init - also need to perform "update"
//                $('#copyshowdate').val($('#startdateinput').val());
//            });
//    }
//}