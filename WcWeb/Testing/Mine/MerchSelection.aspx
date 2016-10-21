<%@ Page Language="C#" MasterPageFile="~/Template.master" AutoEventWireup="true" CodeFile="MerchSelection.aspx.cs" Inherits="Testing_Mine_MerchSelection" Title="Untitled Page" %>




<asp:Content ID="Content1" ContentPlaceHolderID="SideContent" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div id="choosemerch">  
        <fieldset id="merch" runat="server">
            <div>
                Non-Update: <%= DateTime.Now.ToLongTimeString() %>
            </div>
            <div class="merchitem">
                <asp:DropDownList ID="ddlMerchSelector" runat="server" AutoPostBack="True" 
                    OnDataBinding="ddlMerchSelector_DataBinding" 
                    OnSelectedIndexChanged="ddlMerchSelector_SelectedIndexChanged">
                    <asp:ListItem Text="[ ... Select Merch Item ... ]" Value="" />
                </asp:DropDownList>
            </div>    
            
        </fieldset>    
    </div>
    

    <asp:UpdatePanel ID="UpdatePanelSelection" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div>
                <span id="currentSelection">current</span>
                <span id="addToCartOption" style="display:none;">
                    <a href="javascript:addSelectionToCart();">Add To Cart</a>
                </span>
            </div>
            <div>
                <asp:Button ID="Button1" runat="server" Text="Button" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <asp:UpdatePanel ID="UpdateMerchCart" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div>
                Async: <%= DateTime.Now.ToLongTimeString() %>
            </div>
            <div id="portingSelection">porting selection to shopping cart...</div>
            <div id="cartitems">
                <fieldset id="fsCart" runat="server">
                    <legend>Your cart contains:</legend>    
                    <asp:Repeater ID="rptCartItems" runat="server" OnItemCommand="rptCartItems_ItemCommand" OnItemDataBound="rptCartItems_ItemDataBound" OnDataBinding="rptCartItems_DataBinding">
                        <ItemTemplate>
                            <div class="cartitem">
                                <span class="quantity"><asp:DropDownList ID="ddlQty" runat="server"></asp:DropDownList></span>
                                <span class="selectedmerch"><%# Eval("MerchItem.DisplayName")%></span>
                                 <span class="edit">
                                    <asp:LinkButton ID="btnAdd" CssClass="btntribe" CommandName="updmrc" runat="server" ToolTip="Update Quantity" CausesValidation="False"
                                        CommandArgument='<%# Eval("tMerchId") %>'><span>Update Qty</span></asp:LinkButton>
                                    <asp:LinkButton ID="btnRemove" CssClass="btntribe" CommandName="remmrc" runat="server" ToolTip="Remove From Cart" CausesValidation="False"
                                        CommandArgument='<%# Eval("tMerchId") %>'><span>Remove</span></asp:LinkButton>
                                    <asp:CustomValidator ID="RowValidator" Display="Static" runat="server" Text="*" CssClass="validator"></asp:CustomValidator>
                                </span>
                            </div>
                        </ItemTemplate>
                        <SeparatorTemplate><hr /></SeparatorTemplate>
                    </asp:Repeater>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

<script type="text/javascript">
<!--
    var selectionPanelId = '<%= this.UpdatePanelSelection.UniqueID %>';
    var style = getChildElement(selectionPanelId, "ddlStyle");
    var color = getChildElement(selectionPanelId, "ddlColor");
    var size = getChildElement(selectionPanelId, "ddlSize");
    
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
    }
    
    function onStateChange(evt) {
        
        alert(evt.target);    
    }
    
    //this works on controls that have been named with an underscore
    function getChildElement(parentId, controlToFind) { 
    
        if(parentId == undefined)
            return "";
        
        var idParts = parentId.split(':');
        idParts.pop();//get rid of the last element - the parent id name
        idParts.push(controlToFind);
        
        return $get(idParts.join("_"));
    }
    
    function addSelectionToCart() {
        
        var selectionText = $get("currentSelection");
        
        if(selectionText != undefined && selectionText.outerText.trim().length > 0) {
        
            var porting = $get("portingSelection");
        
            if(porting != undefined)
                porting.innerHTML = selectionText.outerText.trim();
        }
    }
    
    function onSelectionChosen(evt) {
        
        var ddl = evt.target;
        var select = "";
        var cartAddable = false;
        
        var selectionText = $get("currentSelection");
        if(selectionText != undefined) selectionText.innerHTML = "";
        
        if(ddl) {
        
            if(ddl.id == style.id && ddl.selectedIndex > 0) {
                select = String.format("Style({0})", ddl.options[ddl.selectedIndex].value);
            }
            else if(ddl.id == color.id && ddl.selectedIndex > 0) { 
                select = String.format("Style({0}) Color({1})", 
                    style.options[style.selectedIndex].value, ddl.options[ddl.selectedIndex].value);
            }
            else if(ddl.id == size.id && ddl.selectedIndex > 0) { 
                select = String.format("Style({0}) Color({1}) Size({2})", style.options[style.selectedIndex].value, 
                    color.options[color.selectedIndex].value, ddl.options[ddl.selectedIndex].value);
                cartAddable = true;
            }
            
            selectionText.innerHTML = select;
        }
        
        var addToCart = $get("addToCartOption");
        
        if(addToCart != undefined && cartAddable) { 
            addToCart.style.display = "inline";
        }
        else
            addToCart.style.display = "none";
        
    }
    
    
//-->
</script>

</asp:Content>

