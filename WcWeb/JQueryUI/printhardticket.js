
$(function () {

    InitPrintTickets();
});

var prmInstance = Sys.WebForms.PageRequestManager.getInstance();

prmInstance.add_endRequest(function () {
    //you need to re-bind your jquery events here
    InitPrintTickets();
});


function InitPrintTickets() {

    var lblError = '.formview-printtix #ctl00_MainContent_ctl01_FormView1_lblError';
    var chkTesteree = '.formview-printtix #ctl00_MainContent_ctl01_FormView1_chkTestPrint';
    var txtQtyor = '.formview-printtix #ctl00_MainContent_ctl01_FormView1_txtQty';
    var txtStartNumbb = '.formview-printtix #ctl00_MainContent_ctl01_FormView1_txtStartNum';
    var btnPrintButton = '.formview-printtix #ctl00_MainContent_ctl01_FormView1_btnPrint';
    var hdnSelIdTkt = '#ctl00_MainContent_ctl01_hdnTicketPrintId';

    //alert(btnPrintButton);

    $(btnPrintButton).bind('click', function () {

        //to hold the querystring
        var qs = '';


        var tktid = $(hdnSelIdTkt).val();
        var isTest = $(chkTesteree).attr('checked') == 'checked';
        var start = $(txtStartNumbb).val();
        var qty = $(txtQtyor).val();


        //validate inputs
        if (isTest == true) {
            qs = "?tkt=" + tktid + "&test=true";
        }
        else if (start != '' && qty != '') {

            //let the c# page handle the validation
            //start should be an integer between zero and 20000
            //qty should be an integer between zero and 2000
            qs = "?tkt=" + tktid + "&start=" + start + "&qty=" + qty;
        }
        else {
            return;
        }

        doPagePopup("/Admin/PrintHardTickets.aspx" + qs, "false");

    });
}