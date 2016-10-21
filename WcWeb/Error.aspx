<%@ Page Language="C#" MasterPageFile="~/Template.master" AutoEventWireup="true" CodeFile="Error.aspx.cs" Inherits="WillCallWeb.Error" Title="Error" %>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">
   <div class="legend">Unexpected error occurred!</div>
   <br />
   <p></p>
   <asp:Label Visible="false" Runat="server" SkinID="FeedbackKO" ID="lbl404" Text="The requested page or resource was not found." />
	<asp:Label Visible="false" Runat="server" SkinID="FeedbackKO" ID="lbl408" Text="The request timed out. This may be caused by a too high traffic. Please try again later." />
	<asp:Label Visible="false" Runat="server" SkinID="FeedbackKO" ID="lbl505" Text="The server encountered an unexpected condition which prevented it from fulfilling the request. Please try again later." />
	<asp:Label runat="server" SkinID="FeedbackKO" ID="lblError" Visible="false" 
	   Text="There was some problems processing your request. An e-mail with details about this error has been sent to the administrator." />
	   <br /><br />
	<p></p>
	If you would like to contact the webmaster to report the problem with more details, 
	please use the <asp:HyperLink runat="server" ID="lnkContact" class="btntribe" Text="Contact Us" NavigateUrl="~/Contact.aspx" /> page.
	<br /><br />
</asp:Content> 

