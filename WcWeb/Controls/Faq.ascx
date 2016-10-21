<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Faq.ascx.cs" Inherits="WillCallWeb.Controls.Faq" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<div id="faq">

    <div class="legend">Frequently Asked Questions</div>
    
    <div id="faq-panel">
        <cc1:TabContainer ID="TabContainer1" runat="server" OnDataBinding="tab_Binding" CssClass="ajax__tab_ie-theme">
        </cc1:TabContainer>
    </div>
   
</div>    


    

