

$(function () {

    $('#dlg_btnPrint').click(function () {

        dlgc_resetAllErrors();

        //clear the email inputs
        $("#email-panel .dialog-input").val('');

        var inputs = dlgc_gatherValidatedInput();

        //validate input
        if (inputs.length > 0) {

            //open print page
            try {
                var url = "/Components/Store/GiftCert_Print.aspx?am=" + inputs[0] + "&cd=" + inputs[1] + "&fm=" + inputs[2] + "&to=" + inputs[3];

                //reset inputs
                $("#print-panel .dialog-input").val('');

                //resetForm();
                doPagePopup(url);
            }
            catch (e) { }
        }

    });

});

function dlgc_emailClick() {

    dlgc_resetAllErrors();

    //clear the print inputs
    $("#print-panel .dialog-input").val('');
}

//a,c,f,t - alphbetical param list
function dlgc_gatherValidatedInput() {

    var ins = $([]);
    var msgs = new Array();    

    //find controls with prefix and ...
    var hidAmount = document.getElementById("hidAmount").value;
    var hidCode = document.getElementById("hidCode").value;

    var txtTo = $("#txtPrintTo").val().trim();
    var txtFrom = $("#txtPrintFrom").val().trim();
    
    //ignore all if there is no input
    if (txtTo.length == 0 && txtFrom.length == 0)
        return ins;

    if (txtTo.length == 0) {
        var to = $("#valPrintTo");
        msgs.push(to.attr('title'));
        to.show();
    }
    if (txtFrom.length == 0) {
        var from = $("#valPrintFrom");
        msgs.push(from.attr('title'));
        from.show();
    }

    if (msgs.length > 0) {

        var errors = "<ul>";
        for (i = 0; i < msgs.length; i++) {
            errors += "<li>" + msgs[i] + "</li>";
        }
        errors += "</ul>";

        $('#valsumprint').append(errors).show();

        return ins;

    }

    
    //push and observe order - alpha on param name
    ins.push(hidAmount);
    ins.push(hidCode);
    ins.push(txtFrom);
    ins.push(txtTo);
    return ins;
}


function dlgc_resetAllErrors() {
    $('.validationsummary').add('.val-indicator').val('');
    $('.validationsummary').add('.val-indicator').hide();
}


