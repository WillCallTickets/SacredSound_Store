<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Contact.ascx.cs" Inherits="WillCallWeb.Controls.Contact" %>

<div id="contact">
    <div class="legend">Contact Us</div>
    <div id="contact-links">
        <div><%=Wcss._Config._Message_ContactPage %></div>
        <%if(Wcss._Config._Site_Entity_Name.ToLower().IndexOf("sts9") != -1){ %>
        <div class="eachandevery">WE REPLY TO EACH AND EVERY CUSTOMER SERVICE ISSUE!</div>
        <%} %>
        <div>Please note that confirmations & e-mail communications often go to spam or junk mail folders, please check those folders before contacting customer service. 
            Emails directed to anything other than customer service may not be replied to.</div>
        <div>Need to view  your <a href="/EditProfile.aspx">account details</a>
            <%if (Wcss._Config._SubscriptionsActive)
              {%>
              &nbsp;or manage your <a href="/EditProfile.aspx">email subscriptions</a>
            <%} %>?
        </div>
        <div>Didn't receive a confirmation email and need to <a href="/WebUser/">confirm an order</a> or view your <a href="/WebUser/">purchase history</a>?</div>
        <%if (Wcss._Config._Shipping_Merch_DefaultMethod.ToLower().IndexOf("ups ground") != -1 || Wcss._Config._Shipping_Tickets_DefaultMethod.ToLower().IndexOf("ups ground") != -1)
          { %>
            <div>Tracking numbers are sent directly from <a href="http://www.ups.com/">UPS</a>. Once again, be sure to check your junk email folders for correspondence.</div>
        <%} %>
        <div><%=Wcss._Config._Message_Contact_ReTicketShipping %></div>
        <div>If you are having issues adding items to your cart, make sure you have cookies and javascript enabled on your browser. Also ensure you have the most up-to-date version of your browser.</div>
        <div>If you are contacting us regarding an order, please be sure to include the date of your order as well as the invoice id for us to better serve you.</div>
    </div>
    <table class="contact" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <th>Your Name:<asp:RequiredFieldValidator runat="server" Display="static" ID="valRequireName" SetFocusOnError="true"
                  ControlToValidate="txtName" ErrorMessage="Your name is required">*</asp:RequiredFieldValidator></th>
            <td style="width: 400px;"><asp:TextBox runat="server" ID="txtName" Width="99%" /></td>
        </tr>
        <tr>
            <th>Your Email:
                <asp:RequiredFieldValidator runat="server" Display="static" ID="valRequireEmail" SetFocusOnError="true"
                  ControlToValidate="txtEmail" ErrorMessage="Your e-mail address is required">*</asp:RequiredFieldValidator>
               <asp:RegularExpressionValidator runat="server" Display="static" ID="valEmailPattern"  SetFocusOnError="true"
                  ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ErrorMessage="The e-mail address you specified is invalid">*</asp:RegularExpressionValidator>
            </th>
            <td><asp:TextBox runat="server" ID="txtEmail" Width="99%" /></td>
        </tr>
        <tr>
            <th>Subject:
                <asp:RequiredFieldValidator runat="server" Display="dynamic" ID="valRequireSubject" SetFocusOnError="true"
                  ControlToValidate="txtSubject" ErrorMessage="The subject is required">*</asp:RequiredFieldValidator>
            </th>
            <td><asp:TextBox runat="server" ID="txtSubject" Width="99%" /></td>
        </tr>
        <tr>
            <th style="vertical-align: top;">Message:
                <asp:RequiredFieldValidator runat="server" Display="dynamic" ID="valRequireBody" SetFocusOnError="true"
                  ControlToValidate="txtBody" ErrorMessage="The body is required">*</asp:RequiredFieldValidator>
            </th>
            <td><asp:TextBox runat="server" ID="txtBody" Width="99%" MaxLength="1200" TextMode="MultiLine" Rows="8" />
                (1200 chars max.)
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align: right;">
               <asp:Label runat="server" ID="lblFeedbackKO" Text="Sorry, there was a problem sending your message." ForeColor="Red" Visible="false" />
               <asp:LinkButton runat="server" ID="txtSubmit" CssClass="btntribe" OnClick="txtSubmit_Click" >Send</asp:LinkButton>
               <asp:ValidationSummary runat="server" ID="valSummary" ShowSummary="false" ShowMessageBox="true" />
            </td>
        </tr>
    </table>
</div>