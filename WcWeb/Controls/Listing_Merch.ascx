<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Listing_Merch.ascx.cs" Inherits="WillCallWeb.Controls.Listing_Merch" %>

<div class="merch-division">  

    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="validationsummary" 
        ValidationGroup="Cart" HeaderText="" />
        
    <div id="itemlisting" class="<%=_merch.SeoName %>">
    
        <div class="legend"><%= _merch.DisplayName %></div>

        <div id="merch-wrapper" class="<%= _merch.DisplayTemplate.ToString().ToLower() %>">
            <asp:Literal ID="litHeaders" runat="server" EnableViewState="false" />

            <div id="merch-container">
                <asp:Table ID="tblMerch" CssClass="merch-table" runat="server" EnableViewState="false" 
                    CellPadding="0" CellSpacing="0" BorderStyle="None" BorderWidth="0" >                    
                    <asp:TableRow ID="rowmain" runat="server" >
                        <asp:TableCell ID="cellupdate" runat="server" class="cartaction">
                            <div class="action">
                                <div class="itemattrib">
                                    <asp:UpdatePanel ID="PanelShopItem" runat="server" UpdateMode="conditional">
                                        <ContentTemplate>
                                            <div>
                                                <span id="currentselection">&nbsp;</span>
                                                <span id="addtocartregion">
                                                    <asp:Literal ID="litNoneAvailable" runat="server" />
                                                    <span id="addtocartselector" style="display: none;">
                                                        <asp:LinkButton ID="btnAdd" CssClass="btntribe" runat="server" OnClick="btnAdd_Click"><span>Add To Cart</span></asp:LinkButton>
                                                    </span>
                                                </span>
                                            </div>                                        
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <asp:HiddenField ID="hdnDisplayComply18" runat="server" />
                                <asp:HiddenField ID="hdnIsComply18" runat="server" />
                                <asp:HiddenField ID="HiddenAttribs" runat="server" />  
                                <asp:CustomValidator ID="AddToCartValidator" Display="Static" ValidationGroup="Cart" runat="server" Text="*" CssClass="validator"></asp:CustomValidator>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>            
                <asp:Literal ID="litBundle" EnableViewState="false" runat="server" />
            </div>
        </div>
    </div>
    <div class="age-overlay" id="complianceoverlay" style="display:none;width:500px;">
	    <div class="contentWrap"></div>
    </div>
</div>  
  
<script type="text/javascript" src="/JQueryUI/ageoverlay.js"></script>                 
      
<script language="javascript" type="text/javascript">

    var selectionPanelId = '<%= this.PanelShopItem.UniqueID %>';    
    var style = getChildElement(selectionPanelId, "ddlStyle");
    var color = getChildElement(selectionPanelId, "ddlColor");
    var size = getChildElement(selectionPanelId, "ddlSize");
    var attribs = getChildElement(selectionPanelId, "HiddenAttribs");

    function pageLoad() {

       // Hook-up the change event of the dropdownlist.
       if(style) {
           $addHandlers(style, {change:onSelectionChosen}, this); 
       }
       if(color) {
           $addHandlers(color, {change:onSelectionChosen}, this); 
       }
       if(size) {
           $addHandlers(size, {change:onSelectionChosen}, this); 
       }
       
       //display add button if there are no choices
       var addToCart = $get("addtocartselector");
        
       //if there are no style, color or size choices - turn on the add button
       if(addToCart != undefined && (style == undefined && color == undefined && size == undefined)) { 
            addToCart.style.display = "inline";
       }
       else
           addToCart.style.display = "none";
           
    }
    
    function onSelectionChosen(evt) {
        var ddl = evt.target;
        var select = "";
        var cartAddable = false;
        var hideSelect = "";
        
        var selectionText = $get("currentselection");
        if(selectionText != undefined) selectionText.innerHTML = "";
        
        if(ddl) {
        
            if(style != undefined && ddl.id == style.id && ddl.selectedIndex > 0) {
                select = String.format("Style({0})", ddl.options[ddl.selectedIndex].value);
                
                if(size == undefined && color == undefined)
                    cartAddable = true;
            }
            else if(color != undefined && ddl.id == color.id && ddl.selectedIndex > 0) { 
                select = String.format("{0}Color({1})", 
                    (style != undefined && ddl.selectedIndex > 0) ? String.format("Style({0}) ", style.options[style.selectedIndex].value) : "", 
                    ddl.options[ddl.selectedIndex].value);
                    
                if(size == undefined)//style would have to be chosen
                    cartAddable = true;
            }
            else if(size != undefined && ddl.id == size.id && ddl.selectedIndex > 0) { 
                select = String.format("{0}{1}Size({2})", 
                    (style != undefined && style.selectedIndex > 0) ? String.format("Style({0}) ", style.options[style.selectedIndex].value) : "", 
                    (color != undefined && color.selectedIndex > 0) ? String.format("Color({0}) ", color.options[color.selectedIndex].value) : "", 
                    ddl.options[ddl.selectedIndex].value);
                
                cartAddable = true;
            }
            
            hideSelect = select.replace(/[(]/g,"=");
            hideSelect = hideSelect.replace(/[)]/g,";");
            
            attribs.value = hideSelect;
            
            selectionText.innerHTML = select;
        }
        
        var addToCart = $get("addtocartselector");
        
        if(addToCart != undefined && cartAddable) { 
            addToCart.style.display = "inline";
        }
        else
            addToCart.style.display = "none";
        
    }
    
</script>