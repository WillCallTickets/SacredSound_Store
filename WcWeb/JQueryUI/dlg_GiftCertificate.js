
$(function () {

   // $("#dialog:ui-dialog").dialog("destroy");

    // Dialog			
    $('#dialog').dialog({
        autoOpen: false,
        width: 400,
        modal: true,
        resizeable: false,
        draggable: false,
        buttons: {
            "Cancel": function () {
                dlg_resetError();
                $(this).dialog("close");
            }
        }
    });

    //hover states on the static widgets
    $('.dialog_link, ul#icons li').hover(
		function () { $(this).addClass('ui-state-hover'); },
		function () { $(this).removeClass('ui-state-hover'); }
	);

    // Dialog Link
    $('.dialog_link').click(function () {
        dlg_resetError();
        //get values from matching hidden fields and port to dialog
        //1 get id of dialoglink
        //note class versus id
        if (this.id.indexOf('dialoglink') == -1)
            return;

        //examine browser - if ie 9 then redirect - the modal does not work
        if ($.browser.msie && $.browser.version == '9.0') {
            //alert('suck it');
            $("#dialog:ui-dialog").dialog("open");
        }

        var idParts = this.id.split('_');
        var idx = idParts[1];

        var code = $('#hidCode_' + idx).val();
        $('#hidCode').val(code);

        var amt = $('#hidAmount_' + idx).val();
        $('#hidAmount').val(amt);

        $('#dlg_Amount').append(amt);




        $('#dialog').dialog("open");
        return false;
    });

    // Dialog Closer
    $('.dialogcloser').click(function () {
        $('#dialog').dialog('close');
        return false;
    });

    $('#dlg_btnEmail').click(function () {
        dlg_resetError();

        //var idParts = this.id.split('_');
        //var idx = idParts[2];

        var params = new Array();
        //params.push(idx);

        if (dlg_validateAndParameterizeInput(params)) {

            dlg_PageMethod("SendGcEml", params, dlg_fnEmailSuccess, dlg_fnEmailError);

            return true;
        }

        return false;
    });

    $('#dlg_btnPrint').click(function () {
        dlg_resetError();
        dlg_goPrint();
        return false;
    });

});

function dlg_goPrint() {

	var d = $('#hidCode');

	var hidAmount = document.getElementById("hidAmount");
	var hidCode = document.getElementById("hidCode");
	var txtTo = document.getElementById("toField");
	var txtFrom = document.getElementById("fromField");

	try {
		var url = "/Components/Store/GiftCert_Print.aspx?cd=" + hidCode.value + "&to=" + txtTo.value + "&fm=" + txtFrom.value + "&am=" + hidAmount.value;

		window.location = url;
	}
	catch (e) { }
}

function dlg_resetError() {
	$('#toField').removeClass("dlg_errorinput");
	$('#fromField').removeClass("dlg_errorinput");
	$('#emailField').removeClass("dlg_errorinput");
	$('#dialogerror').text("");
}

function dlg_fnEmailSuccess(result) {
	//examine xhr for result
	//if true then redirect to email page
	var toEml = result.d.toString().trim();
	if (toEml.length > 0) {
		try {
			dlg_resetError();
			var url = "/Components/Store/GiftCert_Email.aspx?eml=" + toEml;
			window.location = url;
		}
		catch (e) { }
	}
}

function dlg_fnEmailError(xhr, ajaxOptions, thrownError) {
	// Boil the ASP.NET AJAX error down to JSON.
	var err = eval("(" + xhr.responseText + ")");
	$('#dialogerror').append(err.Message);
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
	})
			;
}

function dlg_validateAndParameterizeInput(pushParams) {

	var msgs = new Array();
	var idx = pushParams[0];

	//to and from and email are required
	var inpTo = $('#toField').val().trim();
	if (inpTo.length == 0) {
		msgs.push('To is required');
		$('#toField').addClass("dlg_errorinput");
	}

	var inpFrom = $('#fromField').val().trim();
	if (inpFrom.length == 0) {
		msgs.push('From is required');
		$('#fromField').addClass("dlg_errorinput");
	}

	var inpEmail = $('#emailField').val().trim();
	if (inpEmail.length == 0) {
		msgs.push('Email is required');
		$('#emailField').addClass("dlg_errorinput");
	}

	//validate email
	var regexEmail = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
	if (inpEmail.length > 0 && !regexEmail.test(inpEmail)) {
		msgs.push('Email is invalid');
		$('#emailField').addClass("dlg_errorinput");
	}

	if (msgs.length > 0) {
		var errors = "<ul>";
		for (i = 0; i < msgs.length; i++) {
			errors += "<li>" + msgs[i] + "</li>";
		}
		errors += "</ul>";

		$('#dialogerror').append(errors);

		return false;
	}


	pushParams.push("to");
	pushParams.push(inpTo);
	pushParams.push("from");
	pushParams.push(inpFrom);
	pushParams.push("toEmail");
	pushParams.push(inpEmail);
	pushParams.push("code");
	var inpCode = $('#hidCode').val();
	pushParams.push(inpCode);
	pushParams.push("fromEmail");
	var inpFromEmail = $('#hidFromEmail').val();
	pushParams.push(inpFromEmail);

	return true;
}