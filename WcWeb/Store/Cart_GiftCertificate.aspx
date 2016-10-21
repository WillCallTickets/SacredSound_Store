<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Cart_GiftCertificate.aspx.cs" Inherits="Store_Cart_GiftCertificate" Title="Gift Certificate Form" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd"> 
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="head1" runat="server">    
    <meta http-equiv="Content-Type" content="text/html; charset=windows-1252" />
    <title></title>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.6.1/jquery.min.js"></script>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.14/jquery-ui.min.js"></script>
    <script type="text/javascript" src="/Includes/GeneralJava.js"></script>
    <script type="text/javascript" src="/JQueryUI/giftscript.js"></script>
    <link href="/Styles/overlaybundlegc.css" type="text/css" rel="StyleSheet" />   
</head>
<body style="font: 70% Arial, Helvetica, Geneva, sans-serif;color: #000000;background-color:transparent;" class="gc-body">
<form id="Main" runat="server">
    
    <input type="hidden" id="hidCode" name="hidCode" value='<%=_code %>' />
	<input type="hidden" id="hidAmount" name="hidAmount" value='<%=_amount %>' />
	<input type="hidden" id="hidFromEmail" name="hidFromEmail" value='<%=_email %>' />

    <div id="gc-container">        
        <a class="bnd-back" href="/store/Confirmation.aspx">Back To Confirmation</a>
        <h3><%=SaleItem.MainActName%></h3>

        <div id="ask">
            <div id="print-panel" class="section-panel">
                <div id="valsumprint" class="validationsummary" style="display:none;" ></div>
                <h4>Print</h4>
                <h6>To print your gift certificate and send yourself, fill in the to and from fields and then click print gift</h6>
                <span class="input-label">TO</span>
                <input type="text" id="txtPrintTo" name="txtPrintTo" style="width:300px;" maxlength="256" class="dialog-input" />
                <span title="The TO field is required" id="valPrintTo" class="val-indicator" style="display:none;">*</span>
                <br />
                <span class="input-label">FROM</span>
                <input type="text" id="txtPrintFrom" name="txtPrintFrom" style="width:300px;" maxlength="256" class="dialog-input" />
                <span title="The FROM field is required" id="valPrintFrom" class="val-indicator" style="display:none;" >*</span>                
                <input type="button" name="dlg_btnPrint" id="dlg_btnPrint" class="btngc" value="Print Gift" />
            </div>

            <h4 class="extra-space">--or--</h4>

            <div id="email-panel" class="section-panel">
                <asp:ValidationSummary ID="valSummaryEmail" runat="server" ValidationGroup="eml" CssClass="validationsummary" />
                <h4>Email</h4>
                <asp:Panel ID="pnlEmailInput" runat="server">
                    <h6>To email your gift certificate, fill in the to and from fields and the recipient's email and click send email</h6>
                    <asp:Label ID="Label2" AssociatedControlID="txtEmailTo" runat="server" Text="TO" CssClass="input-label" />
                    <asp:TextBox ID="txtEmailTo" runat="server" Width="300px" MaxLength="256" CssClass="dialog-input" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtEmailTo" Display="Dynamic"
                        SetFocusOnError="true" ErrorMessage="The TO field is required" ValidationGroup="eml" CssClass="val-indicator">*</asp:RequiredFieldValidator>
                    <br />
                    <asp:Label ID="Label3" AssociatedControlID="txtEmailFrom" runat="server" Text="FROM" CssClass="input-label" />
                    <asp:TextBox ID="txtEmailFrom" runat="server" Width="300px" MaxLength="256" CssClass="dialog-input" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtEmailFrom" Display="Dynamic"
                        SetFocusOnError="true" ErrorMessage="The FROM field is required" ValidationGroup="eml" CssClass="val-indicator">*</asp:RequiredFieldValidator>
                    <br />
                    <h6>Enter the email to send the gift to below</h6>
                    <asp:Label ID="Label4" AssociatedControlID="txtEmailAddress" runat="server" Text="EMAIL" CssClass="input-label" />
                    <asp:TextBox ID="txtEmailAddress" runat="server" Width="300px" MaxLength="256" CssClass="dialog-input" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtEmailAddress" Display="Dynamic"
                        SetFocusOnError="true" ErrorMessage="The EMAIL field is required" ValidationGroup="eml" CssClass="val-indicator">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator runat="server" ID="valEmailPattern" Display="Dynamic" 
                        SetFocusOnError="true" ControlToValidate="txtEmailAddress" ErrorMessage="Please enter a valid e-mail address." 
                        ValidationGroup="eml" onload="valEmailPattern_Load" CssClass="val-indicator">*</asp:RegularExpressionValidator>
                    &nbsp;
                    <asp:Button ID="btnEmail" runat="server" OnClientClick="dlgc_emailClick()" OnClick="btnEmail_Click" Text="Send Email" ValidationGroup="eml" CssClass="btngc" />
                </asp:Panel>
                <asp:Panel ID="pnlEmailConfirm" runat="server" CssClass="eml-confirm" Visible="false">
                    Your email has been sent!
                </asp:Panel>
            </div>
        </div>        
        <div>
            <a class="bnd-back" href="/store/Confirmation.aspx">Back To Confirmation</a>
        </div>
    </div>
</form>
</body>
</html>

