<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Editor_MerchOrganization.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.Editor_MerchOrganization" %>
<%@ Register src="~/Components/Util/MerchHeierarchy.ascx" tagname="MerchHeierarchy" tagprefix="uc1" %>
<div id="srceditor">
    <div id="ordinal-tabs-container" class="rounded">
        <div id="ordinal-tabs">
            <ul>
                <li><a id="t1" href="#merchjoincat-ordinal">Merch Order</a></li>                
                <li><a id="t2" href="#merchcategorie-ordinal">Merch Categories</a></li>                
                <li><a id="t3" href="#merchdivision-ordinal">Merch Divisions</a></li>
            </ul>
            <input type="hidden" id="hidCurrentMerchOrdinalTab" value="" />
            <uc1:MerchHeierarchy ID="MerchHeierarchy_jc" runat="server" ContainerId="merchjoincat-ordinal" OrdinalContextString="MerchJoinCat" ValidationGroup="" DisplayTitle="false" TitleText="Merch Order" />
            <uc1:MerchHeierarchy ID="MerchHeierarchy_mc" runat="server" ContainerId="merchcategorie-ordinal" OrdinalContextString="MerchCategorie" ValidationGroup="" DisplayTitle="false" TitleText="Merch Categorie" />            
            <uc1:MerchHeierarchy ID="MerchHeierarchy_md" runat="server" ContainerId="merchdivision-ordinal" OrdinalContextString="MerchDivision" ValidationGroup="" DisplayTitle="false" TitleText="Merch Division" />
        </div>
     </div>
    <div id="ordinal-modal" title="Create new something">
        <p class="validateTips"></p> 
        <div id="ordinal-entry-form">
            <label class="check-label" for="ordinal-entry-internal">Internal</label>
            <input type="checkbox" name="ordinal-entry-internal" id="ordinal-entry-internal" style="display:none;" class="ui-widget-content" />

            <label for="ordinal-entry-name">Name<span class="required-input">(required)</span></label>
            <input type="text" name="ordinal-entry-name" id="ordinal-entry-name" class="text ui-widget-content ui-corner-all" />
            <label for="ordinal-entry-description">Description</label>
            <textarea name="ordinal-entry-description" id="ordinal-entry-description" rows="4" cols="21" class="text ui-widget-content ui-corner-all" ></textarea>
        </div>
    </div>    
</div>