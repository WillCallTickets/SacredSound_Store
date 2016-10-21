using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web.Services;

using Wcss;
using WillCallWeb;
using WillCallWeb.StoreObjects;

public partial class Store_Cart_MerchBundle : WillCallWeb.BasePage
{
    [WebMethod]
    public static object ClearAll(string context, int saleItem_ItemId, int bundleId)
    {
        return SaleItem_Services.ClearAll(new WebContext(), context, saleItem_ItemId, bundleId);
    }
    [WebMethod]
    public static object RemoveOne(string context, int saleItem_ItemId, int bundleId, int selectedItemId)
    {
        return SaleItem_Services.RemoveOne(new WebContext(), context, saleItem_ItemId, bundleId, selectedItemId);
    }
    [WebMethod]
    public static object AddChoice(string context, int saleItem_ItemId, int bundleId, int selectedItemId)
    {
        return SaleItem_Services.AddChoice(new WebContext(), context, saleItem_ItemId, bundleId, selectedItemId);
    }    

    /*
     * match the bundle in question
     * 
     * pull cart qtys
     * ["down
     * 
     * display select controls
     * 
     * opt outs
     * 
     * show selections for the bundle
     * 
     * 
     * */

    #region Properties

    private static System.Text.StringBuilder sb = new System.Text.StringBuilder();

    //A collection to hold the list of items selected for a bundle
    private List<MerchBundle_Listing> Selections = null;

    private int _saleItem_ItemId = 0;
    protected int SaleItem_ItemId
    {
        get
        {
            if (_saleItem_ItemId == 0 && SaleItem != null)
            {
                if (SaleItemContext == "m")
                    _saleItem_ItemId = ((SaleItem_Merchandise)SaleItem).tMerchId;
                else if (SaleItemContext == "t")
                    _saleItem_ItemId = ((SaleItem_Ticket)SaleItem).tShowTicketId;
            }

            return _saleItem_ItemId;
        }
    }

    private SaleItem_Base _saleItem = null;
    protected SaleItem_Base SaleItem
    {
        get
        {
            if (_saleItem == null)
            {
                string sim = this.Request["sim"];

                if (sim != null)
                {
                    //search tickets first
                    if(Ctx.Cart.HasTicketItems)
                        _saleItem = Ctx.Cart.TicketItems.Find(delegate(SaleItem_Ticket match)
                            { return (match.GUID.ToString() == sim); });

                    if(_saleItem == null)
                        _saleItem = Ctx.Cart.MerchandiseItems.Find(delegate(SaleItem_Merchandise match)
                            { return (match.GUID.ToString() == sim); });
                }
            }

            return _saleItem;
        }
    }
    private string _context = string.Empty;
    protected string SaleItemContext
    {
        get
        {
            if (_context == string.Empty && SaleItem != null)
            {
                if (SaleItem is SaleItem_Merchandise)
                    _context = "m";
                else if (SaleItem is SaleItem_Ticket)
                    _context = "t";
            }

            return _context;
        }
    }

    private MerchBundle _bundle = null;
    protected MerchBundle MerchBundleRecord
    {
        get
        {
            if (_bundle == null && SaleItem != null)
            {
                string mbid = this.Request["mbid"];

                if (mbid != null && Utils.Validation.IsInteger(mbid))
                {
                    List<MerchBundle> bundles = new List<MerchBundle>();

                    if (SaleItemContext == "m")
                        bundles.AddRange(((SaleItem_Merchandise)SaleItem).MerchItem.ParentMerchRecord.MerchBundleRecords());
                    else if (SaleItemContext == "t")
                        bundles.AddRange(((SaleItem_Ticket)SaleItem).Ticket.MerchBundleRecords());

                    foreach (MerchBundle bun in bundles)
                        if (bun.Id.ToString() == mbid)
                        {
                            _bundle = bun;
                            break;
                        }
                }
            }

            return _bundle;
        }
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DisplayInfoAndChoices();

            DisplaySelections();
        }
    }

    #region Selections

    //<ul id="lstCollector" runat="server" enableviewstate="false" ondatabinding="lstCollector_DataBinding">                
                //</ul>
    //protected void lstCollector_DataBinding(object sender, EventArgs e)
    //{
    //    System.Web.UI.HtmlControls.HtmlGenericControl ul = (System.Web.UI.HtmlControls.HtmlGenericControl)sender;
    //    ul.InnerHtml = SaleItem_Services.GetListInnerHtml(Selections);
    //}
    protected void litSelections_DataBinding(object sender, EventArgs e)
    {
        Literal lit = (Literal)sender;        
        lit.Text = SaleItem_Services.GetListInnerHtml(Selections);
    }

    #endregion

    #region Choices

    protected void DisplayInfoAndChoices()
    {
        if (SaleItem != null && MerchBundleRecord != null)
        {
            ShowStatus();

            DisplayChoices();

            DisplayTotal();
        }
    }

    protected void DisplayTotal()
    {
        litPrice.DataBind();            
    }
    protected void litPrice_DataBinding(object sender, EventArgs e)
    {
        Literal lit = (Literal)sender;
        lit.Text = SaleItem_Services.GetPriceLineInnerHtml(SaleItem, MerchBundleRecord);//, Selections, Ctx);
    }


    protected void DisplaySelections()
    {
        //lstCollector.DataBind();
        litSelections.DataBind();
    }

    protected void DisplayChoices()
    {
        //create draggable items
        sb.Length = 0;//reset

        //display a distinct list of items to choose from
        MerchCollection coll = new MerchCollection();
        coll.CopyFrom(MerchBundleRecord.ActiveInventory);

        if (coll.Count > 0)
        {
            //start element
            //sb.AppendLine("<ul>");

            foreach (Merch inventory in coll)
                sb.Append(SaleItem_Services.ConstructListElement(inventory));

            //sb.AppendLine("</ul>");
        }

        if(sb.Length > 0)
            litChoices.Text = sb.ToString();
    }    

    protected void ShowStatus()
    {
        //get qty of matching items in cart
        int allowedSelections = Ctx.Cart.GetMaxPossibleSelectionsAllowedForBundle(SaleItem, MerchBundleRecord.Id);
        int selectedQty = 0;

        //you have qualified for # selections
        string selectionList = string.Empty;

        Selections = new List<MerchBundle_Listing>();

        Selections.AddRange(SaleItem.MerchBundleSelections
            .FindAll(delegate(MerchBundle_Listing match) { return (match.BundleId == MerchBundleRecord.Id); }));

        if (Selections.Count > 0)
        {
            //if there are selctions for this bundle - display them here
            foreach (MerchBundle_Listing selected in Selections)
                selectedQty += selected.Quantity;
        }     

        //you have selected [selectedQty] out of [allowedSelections] selections
        litSelected.Text = string.Format("<span class=\"mblist-num-select\">You have selected {0} of {1} selections for this bundle.</span>",
            selectedQty.ToString(), allowedSelections.ToString());
    }

    #endregion

        
}
