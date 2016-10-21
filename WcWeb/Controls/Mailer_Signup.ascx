<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Mailer_Signup.ascx.cs" Inherits="WillCallWeb.Controls.Mailer_Signup" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<div id="mailersignup">
    <div class="legend">
        <% if (Wcss._Config._Mailer_ControlTitle != null && Wcss._Config._Mailer_ControlTitle.Trim().Length > 0){ %>
            <%=Wcss._Config._Mailer_ControlTitle%><%} %>
    </div>
    <div id="signup-panel">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
            <ContentTemplate>                
                <div class="bd">
                    <% if (Wcss._Config._Mailer_ControlGreeting != null && Wcss._Config._Mailer_ControlGreeting.Trim().Length > 0){%>
                    <div class="greeting"><%=Wcss._Config._Mailer_ControlGreeting %></div><%} %>
                    <div style="white-space: nowrap;">
                        <asp:TextBox ID="txtUsername" runat="server" MaxLength="256" Width="300px" />
                        <asp:RegularExpressionValidator runat="server" ID="valEmailPattern" CssClass="validator" Display="Dynamic" 
                            SetFocusOnError="true" ControlToValidate="txtUsername" ErrorMessage="Please enter a valid e-mail address." 
                            ValidationGroup="mailersignup" onload="valEmailPattern_Load">*</asp:RegularExpressionValidator>
                        <cc1:TextBoxWatermarkExtender ID="WaterMark1" runat="server" TargetControlID="txtUserName" WatermarkCssClass="watermark" 
                            WatermarkText=" enter email address" />                                            
                    </div>
                    <asp:LinkButton ID="btnSub" runat="server" ToolTip="Subscribe to our mailing list" CssClass="btntribe" 
                        CausesValidation="true" ValidationGroup="mailersignup" onclick="btnProcess_Click"><span><b>Subscribe</b></span></asp:LinkButton>
                    <asp:LinkButton ID="btnUnsub" runat="server" ToolTip="Unsubscribe from our mailing list" CssClass="btntribe" 
                        CausesValidation="true" ValidationGroup="mailersignup" onclick="btnProcess_Click"><span><b>Un-Subscribe</b></span></asp:LinkButton>
                    <div class="results">
                        <asp:Label ID="lblOk" runat="server" class="feedback" SkinID="FeedbackOK" />
                        <asp:Label ID="lblKo" runat="server" class="feedback" SkinID="FeedbackKO" />
                    </div>
                </div>                     
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="cookiewarning">
        <div class="title">Problems managing your email?</div>
        <div class="warning">
            Please be sure to enter your email address in the box provided before choosing to unsubscribe or subscribe. Also be sure that javascript and cookies enabled on your browser. 
            <br /><br />
            Sometimes the email you receive is forwarded from another account. Be sure you are using the correct email address.
            <br /><br />
            If you continue to have problems please <a href="Contact.aspx">contact us</a>
        </div>
    </div>
</div>
