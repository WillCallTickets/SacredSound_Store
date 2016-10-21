<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="false" CodeFile="MerchSelector.ascx.cs" Inherits="WillCallWeb.Components.Util.MerchSelector" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<div class="merchselector" >
    <div class="ui-widget">
        <input type="text" id="txtParentSearch" class="autosuggest" style="width:350px" />
        <select id="selInventory" name="selInventory" runat="server" style="width:200px" ></select>        
    </div>
</div>

<input type="hidden" id="hidClientId" value='<%=this._uniqueId %>' />
<input type="hidden" id="hidSelectedParentId" runat="server" />
<input type="hidden" id="hidShowInventory" value='<%=ShowInventory.ToString() %>' />
