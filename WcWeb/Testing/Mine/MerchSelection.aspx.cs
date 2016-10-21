using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Wcss;
using WillCallWeb;
using WillCallWeb.StoreObjects;
using System.Collections.Generic;

public partial class Testing_Mine_MerchSelection : BasePage
{
    protected Merch _merch;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if(!IsAsync)
                ddlMerchSelector.DataBind();
        }
    }

    protected void BindSelectionDisplay()
    {
    }

    #region Cart Item List Repeater

    protected void rptCartItems_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        string cmd = e.CommandName.ToLower();
        int idx = int.Parse(e.CommandArgument.ToString());
        DropDownList ddl = (DropDownList)e.Item.FindControl("ddlQty");
        string result = string.Empty;

        if (ddl != null)
        {
            int qty = int.Parse(ddl.SelectedValue);

            switch (cmd)
            {
                case "updmrc":
                    //Ctx.Cart.Update(idx, qty, "merch");
                    result = Ctx.Cart.SaleItem_AddUpdate(_Enums.InvoiceItemContext.merch, idx, qty, this.Profile);
                    break;
                case "remmrc":
                    //Ctx.Cart.RemoveItem(idx, "merch");
                    result = Ctx.Cart.SaleItem_Remove(_Enums.InvoiceItemContext.merch, idx);
                    break;
            }

            BindSelectionDisplay();
        }
    }
    protected void rptCartItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        //fill the qty lists - items are saleitem merch
        ListItemType lit = (ListItemType)e.Item.ItemType;

        if (lit != ListItemType.Footer && lit != ListItemType.Header && lit != ListItemType.Pager && lit != ListItemType.Separator)
        {
            SaleItem_Merchandise saleItem = (SaleItem_Merchandise)e.Item.DataItem;
            DropDownList ddl = (DropDownList)e.Item.FindControl("ddlQty");

            if (saleItem != null && ddl != null)
            {
                int currentQty = saleItem.Quantity;
                int max = _merch.MaxQuantityPerOrder;

                for (int i = 1; i <= max; i++)
                    ddl.Items.Add(new ListItem(i.ToString()));

                ddl.SelectedIndex = currentQty - 1;//zero-based
            }
        }
    }

    protected void rptCartItems_DataBinding(object sender, EventArgs e)
    {
        //get list of cart items that correspond to the parent - dont show all  items
        List<SaleItem_Merchandise> cartItems = new List<SaleItem_Merchandise>();

        cartItems.AddRange(
            Ctx.Cart.MerchandiseItems.FindAll(delegate(SaleItem_Merchandise match)
            { return (match.MerchItem.Id == _merch.Id || (match.MerchItem.TParentListing.HasValue && match.MerchItem.TParentListing == _merch.Id)); }));

        if (cartItems.Count > 0)
        {
            fsCart.Visible = true;
            rptCartItems.Visible = true;

            rptCartItems.DataSource = cartItems;
        }
        else
        {
            fsCart.Visible = false;
            rptCartItems.Visible = false;
        }

    }

    #endregion

    protected void ddlMerchSelector_DataBinding(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;

        if (ddl.Items.Count <= 1)//allow for select option
        {
            ddl.AppendDataBoundItems = true;
            ddl.DataSource = Ctx.SaleMerch.GetList().FindAll(delegate(Merch match) { return (match.IsParent); });
            ddl.DataTextField = "Name";
            ddl.DataValueField = "Id";
        }
    }
    protected void ddlMerchSelector_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;

        if (ddl.SelectedIndex > 0)//allow for select option
        {
            int idx = int.Parse(ddl.SelectedValue);

            if (idx > 0)
                _merch = (Merch)Ctx.SaleMerch.Find(idx);

            if (_merch != null)
                SetPanelAttribs();

        }
    }
    protected void AttributeSelector_Load(object sender, EventArgs e)
    {
        //SetPanelAttribs();
    }
    private void SetPanelAttribs()
    {
        if (_merch != null)
        {
            UpdatePanel panel = UpdatePanelSelection;

            int listWidth = 200;

            //STYLES
            DropDownList oldStyle = (DropDownList)panel.FindControl("ddlStyle");

            DropDownList styleList = null;
            AjaxControlToolkit.CascadingDropDown cddStyle = null;
            DropDownList colorList = null;
            AjaxControlToolkit.CascadingDropDown cddColor = null;
            DropDownList sizeList = null;
            AjaxControlToolkit.CascadingDropDown cddSize = null;

            if (_merch.HasChildSizes)
            {
                sizeList = new DropDownList();
                cddSize = new AjaxControlToolkit.CascadingDropDown();

                sizeList.ID = "ddlSize";
                sizeList.Width = Unit.Pixel(listWidth);

                cddSize.ID = "CascadingSize";
                cddSize.Category = "Size";
                cddSize.UseContextKey = true;
                cddSize.ContextKey = _merch.Id.ToString();
                cddSize.LoadingText = "[ ... Loading Sizes ... ]";
                cddSize.PromptText = "Please Select A Size";
                cddSize.TargetControlID = sizeList.ID;

                if (colorList != null)
                    cddSize.ParentControlID = colorList.ID;
                else if (styleList != null)
                    cddSize.ParentControlID = styleList.ID;
                else
                    cddSize.ParentControlID = string.Empty;

                cddSize.ServicePath = "/Services/MerchAttribute_Svc.asmx";
                cddSize.ServiceMethod = "GetSizes";
                cddSize.BehaviorID = "theSize";

                //be sure to add cdd after the ddl
                panel.ContentTemplateContainer.Controls.AddAt(0,sizeList);
                panel.ContentTemplateContainer.Controls.AddAt(0, cddSize);
            }

            if (_merch.HasChildColors)
            {
                colorList = new DropDownList();
                cddColor = new AjaxControlToolkit.CascadingDropDown();

                colorList.ID = "ddlColor";
                colorList.Width = Unit.Pixel(listWidth);

                cddColor.ID = "CascadingColor";
                cddColor.Category = "Color";
                cddColor.UseContextKey = true;
                cddColor.ContextKey = _merch.Id.ToString();
                cddColor.LoadingText = "[ ... Loading Colors ... ]";
                cddColor.PromptText = "Please Select A Color";
                cddColor.TargetControlID = colorList.ID;

                if (styleList != null)
                    cddColor.ParentControlID = styleList.ID;

                cddColor.ServicePath = "/Services/MerchAttribute_Svc.asmx";
                cddColor.ServiceMethod = "GetColors";
                cddColor.BehaviorID = "theColor";

                //be sure to add cdd after the ddl
                panel.ContentTemplateContainer.Controls.AddAt(0,colorList);
                panel.ContentTemplateContainer.Controls.AddAt(0, cddColor);
            }

            if (_merch.HasChildStyles)
            {
                styleList = new DropDownList();
                cddStyle = new AjaxControlToolkit.CascadingDropDown();

                styleList.ID = "ddlStyle";
                styleList.Width = Unit.Pixel(listWidth);

                cddStyle.ID = "CascadingStyle";
                cddStyle.Category = "Style";
                cddStyle.UseContextKey = true;
                cddStyle.ContextKey = _merch.Id.ToString();
                cddStyle.LoadingText = "[ ... Loading Styles ... ]";
                cddStyle.PromptText = "Please Select A Style";
                cddStyle.TargetControlID = styleList.ID;
                cddStyle.ServicePath = "/Services/MerchAttribute_Svc.asmx";
                cddStyle.ServiceMethod = "GetStyles";
                cddStyle.BehaviorID = "theStyle";

                //be sure to add cdd after the ddl
                panel.ContentTemplateContainer.Controls.AddAt(0,styleList);
                panel.ContentTemplateContainer.Controls.AddAt(0, cddStyle);
            }
        }
    }
    protected void btnAddToCart_Click(object sender, EventArgs e)
    {
        //rebind data for merch & selections
        //ddlMerchSelector.DataBind();
    }
}
