<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Chooser_Merch.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.Choosers.Chooser_Merch" %>
<div id="srceditor">
    <div id="ChooserMerch-Container" class="Normal">
        <asp:UpdatePanel ID="UpdatePanelChooserMerch" runat="server">
            <ContentTemplate>
                <div class="selectors">
                    <div><span>Division</span><asp:DropDownList ID="ddlDivision" runat="server" AutoPostBack="true" DataSourceID="SqlDivision" DataTextField="Name" DataValueField="Idx" /></div>
                    <div><span>Category</span><asp:DropDownList ID="ddlCategory" runat="server" AutoPostBack="true" DataSourceID="SqlCategory" DataTextField="Name" DataValueField="Idx" /></div>
                    <div><span>Items</span><asp:DropDownList ID="ddlParent" runat="server" DataSourceID="SqlParent" DataTextField="Name" DataValueField="Idx" /></div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>

<asp:SqlDataSource ID="SqlDivision" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    
    SelectCommand="SELECT CASE WHEN e.[bInternalOnly] IS NULL OR e.[bInternalOnly] = 0 THEN e.[Name] ELSE '[internal] ' + e.[Name] END as [Name], e.[Id] as [Idx] 
    FROM [MerchDivision] e ORDER BY e.[iDIsplayOrder] ASC " 
    >
</asp:SqlDataSource>
<asp:SqlDataSource ID="SqlCategory" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    
    SelectCommand="SELECT CASE WHEN e.[bInternalOnly] IS NULL OR e.[bInternalOnly] = 0 THEN e.[Name] ELSE '[internal] ' + e.[Name] END as [Name], e.[Id] as [Idx] 
    FROM [MerchCategorie] e WHERE e.[tMerchDivisionId] = @divisionIdx ORDER BY e.[iDIsplayOrder] ASC " 
       >
    <SelectParameters>
        <asp:ControlParameter ControlID="ddlDivision" DbType="Int32" Name="divisionIdx" PropertyName="SelectedValue" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="SqlParent" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    
    SelectCommand="SELECT CASE WHEN e.[bActive] = 1 THEN '' ELSE '[not active] ' END + CASE WHEN e.[bInternalOnly] = 0 THEN '' ELSE '[internal] ' END + e.[Name] as [Name], 
    e.[Id] as [Idx] FROM [MerchJoinCat] j LEFT OUTER JOIN [Merch] e ON e.[Id] = j.[tMerchId] WHERE j.[tMerchCategorieId] = @categoryIdx ORDER BY e.[Name] ASC " 
    >
    <SelectParameters>
        <asp:ControlParameter ControlID="ddlDivision" DbType="Int32" Name="divisionIdx" PropertyName="SelectedValue" />
        <asp:ControlParameter ControlID="ddlCategory" DbType="Int32" Name="categoryIdx" PropertyName="SelectedValue" />
    </SelectParameters>
</asp:SqlDataSource>

<script language="javascript" type="text/javascript">
    // Get a reference to the PageRequestManager.
    var prm = Sys.WebForms.PageRequestManager.getInstance();

    var selectionPanelId = '<%= this.UpdatePanelChooserMerch.UniqueID %>';
    var division = getChildElement(selectionPanelId, "ddlDivision");
    var category = getChildElement(selectionPanelId, "ddlCategory");
    var parent = getChildElement(selectionPanelId, "ddlParent");

    // Using that prm reference, hook _initializeRequest
    // and _endRequest, to run our code at the begin and end
    // of any async postbacks that occur.
    prm.add_initializeRequest(InitializeRequest);
    prm.add_endRequest(EndRequest);

    // Executed anytime an async postback occurs.
    function InitializeRequest(sender, args) {
        // Change the Container div's CSS class to .Progress.
        $get('ChooserMerch-Container').className = 'Progress';

        // Get a reference to the element that raised the postback,
        //   and disables it.
        $get(args._postBackElement.id).disabled = true;

        //disable drop downs
        if (division)
            division.disabled = true;
        if (category)
            category.disabled = true;
        if (parent)
            parent.disabled = true;
    }

    // Executed when the async postback completes.
    function EndRequest(sender, args) {
        // Change the Container div's class back to .Normal.
        $get('ChooserMerch-Container').className = 'Normal';

        // Get a reference to the element that raised the postback
        //   which is completing, and enable it.
        $get(sender._postBackSettings.sourceElement.id).disabled = false;

        //enable drop downs
        if (division)
            division.disabled = false;
        if (category)
            category.disabled = false;
        if (parent)
            parent.disabled = false;

    }
</script>