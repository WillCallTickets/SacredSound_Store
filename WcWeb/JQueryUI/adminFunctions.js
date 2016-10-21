

$(function () {

    //InitSort();

    InitTabOrdinalsAndSort();
});

var prmInstance = Sys.WebForms.PageRequestManager.getInstance();

prmInstance.add_endRequest(function () {
    //bind jquery events here
   InitTabOrdinalsAndSort();
});


//***************ORDERING
function InitTabOrdinalsAndSort() {

    var tabs = $("#ordinal-tabs");

    tabs.tabs({ cookie: { name: 'ordinaltab', path: "/"} });    

    /*use bbq for history
    tabs.tabs({ event: 'change' });
    var tab_a_selector = 'ul.ui-tabs-nav a';
    tabs.find(tab_a_selector).click(function () {
    var state = {};
    var id = $(this).closest('#ordinal-tabs').attr('id');
    var idx = $(this).parent().prevAll().length;
    state[id] = idx;
    $.bbq.pushState(state);
    });
    
    //TODO test with som ediff solutions as to where the last valid tab may come from

    $(window).bind('hashchange', function (e) {
    tabs.each(function () {

    //var incook = $.cookie("ordinaltab");

    var idx = $.bbq.getState(this.id, true) || 0;
    $(this).find(tab_a_selector).eq(idx).triggerHandler('change');
    });
    });

    $(window).trigger('hashchange');
    end-bbq*/

    $(".ordinal-wrapper UL LI.ordinal-header-row").disableSelection();

    $("#merchjoincat-ordinal .ordinal-wrapper UL").sortable({

        items: "li:not(.ordinal-header-row)", //ignore the header row for sorting
        placeholder: "ui-state-highlight",
        update: function (event, ui) {
            InitSortList(event, ui);
        }
    });
    $("#merchcategorie-ordinal .ordinal-wrapper UL").sortable({

        items: "li:not(.ordinal-header-row)", //ignore the header row for sorting
        placeholder: "ui-state-highlight",
        update: function (event, ui) {
            InitSortList(event, ui);
        }
    });
    $("#merchdivision-ordinal .ordinal-wrapper UL").sortable({

        items: "li:not(.ordinal-header-row)", //ignore the header row for sorting
        placeholder: "ui-state-highlight",
        update: function (event, ui) {
            InitSortList(event, ui);
        }
    });

    //fix for jqueryui dialog bug where btn text does not show
    if ($.attrFn) { $.attrFn.text = true; }

    $("#ordinal-modal").dialog({

        autoOpen: false,
        dialogClass: "ordinal-modal-container",
        closeOnEscape: true,
        height: 500,
        width: 400,
        modal: true,
        buttons: {
            "Ok": function () {

                var self = $(this);
                var tips = self.dialog("option", "validationDisplay");
                var errorList = $([]);

                acceptForm(self, errorList, tips);

                if (errorList.length > 0) {

                    displayErrors(errorList, tips);
                }
                else {

                    formOrdinalAjax(self, tips);
                }
            },
            "Cancel": function () {
                $(this).dialog("close");
            }
        },
        open: function (e) {

            initOrdinalForm($(this)); //setup allfields array
        },
        close: function () {

            var tips = $(this).dialog("option", "validationDisplay");
            var allFields = $(this).dialog("option", "formFields");

            tips.text("");
            for (i = 0; i < allFields.length; i++) {
                $(allFields[i]).val("").removeClass("ui-state-error");
            }
        }
    });

    function initOrdinalForm(sender) {

        var tips = $(".validateTips");
        var editMode = sender.dialog("option", "editMode");
        var ordinalContext = sender.dialog("option", "ordinalContext");
        var editValues = sender.dialog("option", "editValues");

        var name = $("#ordinal-entry-name"),
            description = $("#ordinal-entry-description"),
            internal = $("#ordinal-entry-internal"),            
            labelinternal = $('label[for="ordinal-entry-internal"]');

        //set visibility        
        (editMode == "edit") ? internal.show() : internal.hide();
        (editMode == "edit") ? labelinternal.show() : labelinternal.hide();

        //set values if edit
        if (editMode == "edit") {

            name.val(editValues.Name);
            description.val(editValues.Description);
            internal.attr("checked", editValues.Internal);
        }

        var formFields = { Name: name, Description: description, Internal: internal };

        sender.dialog("option", "formFields", formFields)
            .dialog("option", "validationDisplay", tips);
    }

    $(".ov-formtrigger").add(".ov-formtrigger-link").click(function (e) {

        e.preventDefault();

        var ordinalContext = "";
        var editMode = "";
        var currentDivId = 0;
        var currentSelectedIdx = 0;
        var currentValues = $([]);

        var wrap = $(this).closest(".merch-hire-container").get(0);
        if (wrap != undefined)
            ordinalContext = (wrap.id.indexOf("merchdivision") != -1) ? "MerchDivision" :
                (wrap.id.indexOf("merchcategorie") != -1) ? "MerchCategorie" : "";

        editMode = (this.id.indexOf("_btnAddNew") != -1) ? "insert" : (this.id.indexOf("lnkEdit") != -1) ? "edit" : "";

        //return if bad button
        if (editMode.length == 0 || ordinalContext.length == 0)
            return;

        if (editMode === "edit") {

            //get id out of the rel
            currentSelectedIdx = $(this).attr("rel");

            //get name, description and active/internal from currentValues
            var namer = $(this).closest("LI").find(".ordinal-name").get(0).innerHTML;
            
            var description = $(this).closest("LI").find(".ordinal-description A").get(0);
            var desc = "";
            if (description != undefined) {

                //get the crud out of alert
                desc = description.pathname.replace("alert('", "").replace("');", "").trim();
            }

            //active or internal
            var info = $(this).closest("LI").find(".ordinal-info").get(0).innerHTML;
            var infoVar = false;
            if (info.length > 0 && info.indexOf("Internal") != -1) {
                infoVar = true;
            }            

            currentValues = { Name: namer, Description: desc, Internal: infoVar};
        }

        if (ordinalContext == "MerchCategorie") {

            var catDivDdl = $(wrap).find('[id$="ddlDivision"]').get(0);
            currentDivId = catDivDdl.value;
        }

        var title = ((editMode == "insert") ? "Add New " : "Edit ") + ordinalContext;

        $("#ordinal-modal")
            .dialog("option", "title", title)
            .dialog("option", "editMode", editMode)
            .dialog("option", "ordinalContext", ordinalContext)
            .dialog("option", "divisionIdx", currentDivId)
            .dialog("option", "itemIdx", currentSelectedIdx)
            .dialog("option", "editValues", currentValues)
            .dialog("open");

    });
}

function formOrdinalAjax(sender, tips) {

    var functionName = sender.dialog("option", "functionName");
    var paramList = sender.dialog("option", "paramList");
    
    admin_PageMethod("POST", "/Services/Admin/AdminServices.asmx/" + functionName,
        paramList,
        function (data) { successMerchOrdinalUpdate(data, sender) },
        function (xhr, ajaxOptions, thrownError) { errorRoutine_ForForm(xhr, ajaxOptions, thrownError, tips) });
}

function acceptForm(sender, errorList, tips) {

    var allFields = sender.dialog("option", "formFields");    

    if (allFields.length == 0) {

        errorList.push("no fields were found.");
        return;
    }

    var nameField = allFields.Name.removeClass("ui-state-error").get(0);
    var descriptionField = allFields.Description.removeClass("ui-state-error").get(0);
    var internalField = allFields.Internal.removeClass("ui-state-error").get(0);
    
    if (nameField != undefined)
     checkLength(errorList, nameField, "name", 1, 256);

    if(descriptionField != undefined)
        checkLength(errorList, descriptionField, "description", 0, 2000);

    //no need to proceed
    if (errorList.length > 0)
        return;

    var editMode = sender.dialog("option", "editMode");
    var ordinalContext = sender.dialog("option", "ordinalContext");
    var itemIdx = sender.dialog("option", "itemIdx");
    var divisionIdx = sender.dialog("option", "divisionIdx");
    var functionName = "", paramList = "";
    
    functionName = ((editMode == "edit") ? "Edit" : "AddNew") + ordinalContext;

    //setup ajax calls
    if (ordinalContext == "MerchDivision") {
        paramList = initParams_MerchTaxonomy(
            null,
            (editMode == "edit") ? itemIdx : null,
            nameField.value,
            descriptionField.value,
            ordinalContext,
            (editMode == "edit") ? internalField.checked : null);
    }
    else if (ordinalContext == "MerchCategorie") {
        paramList = initParams_MerchTaxonomy(
            divisionIdx,
            (editMode == "edit") ? itemIdx : null,
            nameField.value,
            descriptionField.value,
            ordinalContext,
            (editMode == "edit") ? internalField.checked : null);
    }

    sender.dialog("option", "functionName", functionName)
        .dialog("option", "paramList", paramList);
}
function errorRoutine_ForForm(xhr, ajaxOptions, thrownError, tips) {

    var err = eval("(" + xhr.responseText + ")");
    //alert(err.StackTrace);
    var errorList = $([]);
    errorList.push(err.Message);

    displayErrors(errorList, tips);
}
function successMerchOrdinalUpdate(result, sender) {

    if (result.d != "false") {
        sender.dialog("close");
        
        var clientContext = "#hidClientId-merchdivision-ordinal"//+ divid.replace("#", "");
        var clientid = $(clientContext).val();
        directedPostback(clientid, "merchordinalvaluechange");
    }
}
function directedPostback(clientid, postbackCommand) {

    //rebind the control        
    __doPostBack(clientid, postbackCommand);
}

function initParams_MerchTaxonomy(divIdx, idx, name, description, ordinalContext, isInternal) {

    //update cart on server
    var pushParams = new Array();

    if(divIdx != undefined) {
        pushParams.push("merchDivisionId");
        pushParams.push(divIdx);
    }
    if(idx != undefined) {
        pushParams.push("idx");
        pushParams.push(idx);
    }
    if (name != undefined) {
        pushParams.push("name");
        pushParams.push(name);
    }
    if (description != undefined) {
        pushParams.push("description");
        pushParams.push(description);
    }
    if (isInternal != undefined) {       
        pushParams.push("isInternal");
        pushParams.push(isInternal);        
    }
    
    return pushParams;
}

function displayErrors(errorList, errorElement) {

    var errorDisplayElement = $(errorElement).get(0);

    errorDisplayElement.innerHTML = "";

    if (errorList.length > 0) {

        errorDisplayElement.innerHTML += "<ul>";

        for (i = 0; i < errorList.length; i++) {
            errorDisplayElement.innerHTML += "<li>* " + errorList[i] + "</li>";
        }

        errorDisplayElement.innerHTML += "</ul>";

        errorElement.addClass("ui-state-highlight");

        setTimeout(function () {
            errorElement.removeClass("ui-state-highlight", 1500);
        }, 500);
    }
}


/*validate form inputs*/
function checkLength(errorList, o, n, min, max) {

    if (min > 0 && (o == undefined || (o != undefined && (o.value == undefined || o.value.length == 0)))) {
        $(o).addClass("ui-state-error");
        errorList.push(n + " is required.");
    }
    else if (o != undefined && o.value.length > max || o.value.length < min) {
        $(o).addClass("ui-state-error");
        errorList.push("Length of " + n + " must be between " +
                    min + " and " + max + ".");
    } 
}

function checkRegexp(errorList, o, regexp, n) {
    if (o != undefined && (!(regexp.test(o.value)))) {
        $(o).addClass("ui-state-error");
        errorList.push(n);
    }
}
/*examples
bValid = bValid && checkLength(name, "username", 3, 16);
bValid = bValid && checkLength(email, "email", 6, 80);
bValid = bValid && checkLength(password, "password", 5, 16);

bValid = bValid && checkRegexp(name, /^[a-z]([0-9a-z_])+$/i, "Username may consist of a-z, 0-9, underscores, begin with a letter.");
// From jquery.validate.js (by joern), contributed by Scott Gonzalez: http://projects.scottsplayground.com/email_address_validation/
bValid = bValid && checkRegexp(email, /^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i, "eg. ui@jquery.com");
bValid = bValid && checkRegexp(password, /^([0-9a-zA-Z])+$/, "Password field only allow : a-z 0-9");
*/
/*end of validate*/


function InitSortList(event, ui) {

    var itm = ui.item.get(0);

    var wrap = $(itm).closest(".merch-hire-container").get(0);
    if (wrap != undefined)
        context = (wrap.id.indexOf("merchdivision") != -1) ? "merchdivision" : 
            (wrap.id.indexOf("merchcategorie") != -1) ? "merchcategorie" :
            (wrap.id.indexOf("merchjoincat") != -1) ? "merchjoincat" : "";

    if (context.length > 0 && itm != undefined && itm.className != "ordinal-header-row") {

        var divId = "#" + context + "-ordinal";//eg "#merchjoincat-ordinal"

        var lis = $(divId + " .ordinal-wrapper UL LI");

        //determine if a ordinal-header-row exists in the list
        var headerOffset = lis.index("#ordinalheader-" + context); 
        headerOffset = (headerOffset == -1) ? 0 : 1;

        //this is the items new index
        var itemIndex = lis.index(itm);

        if (itemIndex != -1) {

            admin_PageMethod("POST", "/Services/Admin/AdminServices.asmx/UpdateOrdinal_Merch",
                initParams_Reorder(context, itm.id, itemIndex - headerOffset), //allow for header item
                function (data) { success_UpdateOrdinal_Merch(data, context, divId) },
                errorRoutine);

            return true;
        }
    }
}

function initParams_Reorder(context, paramId, newOrdinal) {

    //update cart on server
    var pushParams = new Array();

    pushParams.push("ordinalContext");
    pushParams.push(context);

    pushParams.push("itemIdx");
    pieces = paramId.split('_');
    pushParams.push(pieces[1]);

    pushParams.push("newOrdinal");
    pushParams.push(newOrdinal);

    return pushParams;
}

function success_UpdateOrdinal_Merch(dt, ctx, divid) {

    if (dt.d != "false") {

        var clientContext = "#hidClientId-" + divid.replace("#", ""); // "#hidClientId_" + ctx;
        var clientId = $(clientContext).val();

        __doPostBack(clientId, 'ordinal_changed');
    }
}
//***************END ORDERING


//***************LINK REMOVAL - allows sale promotion trigger items to be removed from the list and object
function addRemoveLinks() {

    //http://stackoverflow.com/questions/5152845/double-click-event-on-option-of-listbox-not-firing-in-ie
    //select vctriggerlist_merch, * from salepromotion where vctriggerlist_merch is not null

    $("select.req-merch-list").dblclick(function () {

        $("select.req-merch-list option:selected").each(function () {

            proceed = confirm('Are you sure you want to remove this item from the list?');

            //if no - return
            if (proceed == 'false' || proceed == 0)
                return;

            //if yes - run service to remove element            
            val = this.value;

            //must use page method to handle session and postback properly
            admin_PageMethod("POST", window.location.pathname + "/RemoveMerchChoiceFromSalePromotion", 
                initParamsForMerchListRemoval(val),
                successMerchListRemoval, 
                errorRoutine);

        });
    });
}

function initParamsForMerchListRemoval(selectedItemId) {

    //update cart on server
    var pushParams = new Array();

    pushParams.push("salePromotionId");
    var salePromotionId = $("#hidSalePromotionId").val();
    pushParams.push(salePromotionId);

    pushParams.push("removeId");
    var removeId = selectedItemId;
    pushParams.push(removeId);

    return pushParams;
}

function successMerchListRemoval(result) {

    //update selectionStatus
    if (result.d != "false") {
        var data = result.d;

        var clientId = $("#hidClientControlRemoverId").val();

        //rebind the control        
        __doPostBack(clientId, 'merchlist_changed');
    }
}
//***********************END OF LINK REMOVAL

//***********************MERCH SEARCH CONTROL
function SearchText() {

    searchBoxId = "txtParentSearch";
    merchsearch = "Search Merch";


    //tend to server controls
    clientId = $("#hidClientId").get(0).value;
    parentControl = $("#" + clientId + "_hidSelectedParentId").get(0);
    inventoryControl = $("#" + clientId + "_selInventory").get(0);
    showInventoryControl = $("#hidShowInventory").get(0);
    selectedParent = parentControl.value;
    selectedInventory = (inventoryControl != undefined) ? inventoryControl.value : selectedParent;
    displayInventory = showInventoryControl.value.toLowerCase();


    //very simple watermark
    $("#" + searchBoxId).val(merchsearch).css("color", "#ccc")
        .bind('focus', function (item, ui) {
            if (this.value == merchsearch)
                $(this).css("color", "#000").val('');
    });
        
    //display inventory chooser    
    if (displayInventory == 'false')
        $("#" + clientId + "_selInventory").hide();

    $("#txtParentSearch").autocomplete({
        
        source: function (request, response) {

            admin_PageMethod("POST", "/Services/Admin/SuggestionService.asmx/AutocompleteMerchSelection",
                '{"partialName":"' + document.getElementById(searchBoxId).value + '"}',

                function (data, textStatus, jqXHR) {
                    var list = $.parseJSON(data.d);

                    response($.map(list, function (item) {
                        return {

                            label: item.Text,
                            value: item.Value
                        }
                    }))
                },

                errorRoutine);
        },
        minLength: 1,
        select: function (event, ui) {

            //todo $("#" + btnId).enabled = true;

            $("#" + searchBoxId).val(ui.item.label);
            parentId = ui.item.value;
            parentControl.value = parentId;

            if (displayInventory == 'false') {
                $("#" + clientId + "_selInventory").hide();
            }
            else {
                $("#" + clientId + "_selInventory").show();
                inventoryControl.innerHTML = "";

                FillInventory(parentId, inventoryControl);
            }

            return false;
        }
    });
}

function FillInventory(parentId, inventoryControl) {

    if(parentId == undefined || parentId == "0")
        return;

    admin_PageMethod("POST", "/Services/Admin/SuggestionService.asmx/AutocompleteMerchInventory",
        "{'parentId':'" + parentId + "'}",
        success_FillInventory, 
        errorRoutine);
}

function success_Autocomplete(data, textStatus, jqXHR) {
    
    var list = $.parseJSON(data.d);

    $.map(list, function (item) {
        return {

            label: item.Text,
            value: item.Value
        }
    })
}

function success_FillInventory(data) {

    var list = $.parseJSON(data.d);

    $.map(list, function (item) {

        var option = $('<option />');
        option.attr('value', item.Value).text(item.Text);

        $("#" + inventoryControl.id).append(option);
    });
}

//***********************END OF MERCH SEARCH CONTROL


//***********************SHARED FUNCTIONS**********************************
//*************************************************************************

//***********************ERROR HANDLER
function errorRoutine(xhr, ajaxOptions, thrownError) {

    var err = eval("(" + xhr.responseText + ")");
    alert(err.StackTrace);
}
//***********************END OF ERROR HANDLER


//***********************AJAX CALL WRAPPER
function admin_PageMethod(requestType, functionPath, paramArray, successFn, errorFn) {

    var pagePath = window.location.pathname;
    //Create list of parameters in the form:   
    //{"paramName1":"paramValue1","paramName2":"paramValue2"}   
    var paramList = '';
    if (paramArray != undefined && paramArray.length > 0) {

        if (paramArray.indexOf('{') == -1) {

            for (var i = 0; i < paramArray.length; i += 2) {
                if (paramList.length > 0) 
                    paramList += ',';
                paramList += '"' + paramArray[i] + '":"' + paramArray[i + 1] + '"';        
            }

            paramList = '{' + paramList + '}';
        }
        else {
            paramList = paramArray;
        }
    }

    if (paramList.length == 0)
        paramList = '{}';
    

    //Call the page method   
    $.ajax({
        type: requestType,
        url: functionPath,
        contentType: "application/json; charset=utf-8",
        data: paramList,
        dataType: "json",
        success: successFn,
        error: errorFn
    });
}
//***********************END OF AJAX CALL WRAPPER
