using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.ComponentModel;
using WillCallWeb.StoreObjects;
using Wcss;
using Utils;
using System.Web.Services;

namespace WillCallWeb.Components.Cart
{   
    /// <summary>
    /// Displays a list of promotions available to the customer
    /// </summary>
    [ToolboxData("<{0}:Bundle_Listing ></{0}:Bundle_Listing>")]
    public partial class Bundle_Listing : WillCallWeb.BaseControl
    {
        #region Page Overhead

        protected bool ListSimply = false;
        private List<MerchBundle_Listing> _bundleCollection = null;
        private SaleItem_Base _saleItem = null;
        public SaleItem_Base SaleItem
        {
            get
            {
                return _saleItem;
            }
            set
            {
                _saleItem = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ListSimply = (this.Page.ToString().ToLower().IndexOf("cart_edit") == -1);
        }

        protected void updLoad(object sender, EventArgs e)
        {
            BindControl();
        }
        
        private void BindControl()
        {
            _bundleCollection = GetBundlesToList();

            if (_bundleCollection == null || _bundleCollection.Count == 0)
            {
                divbundle.Visible = false;
                return;
            }

            Repeater1.DataSource = _bundleCollection;
            Repeater1.DataBind();

            if (ListSimply)
                litSimply.Text = "<div class=\"bundle-section-title\">Bundled with this item:</div>";
            else litSimply.Text = string.Empty;
        }


        #endregion

        #region Get BundlesToList

        /// <summary>
        /// Returns a list of MerchBundle_Listing to display to user
        /// the binding control will handle binding the selection display
        /// </summary>
        /// <returns></returns>
        private List<MerchBundle_Listing> GetBundlesToList()
        {
            //do this by context
            bool isMerch = (SaleItem is SaleItem_Merchandise);
            bool isTicket = (SaleItem is SaleItem_Ticket);

            if (isMerch || isTicket)
            {
                List<MerchBundle_Listing> list = new List<MerchBundle_Listing>();
                MerchBundleCollection coll = new MerchBundleCollection();
                int cartQty = 0;

                if (isMerch)
                {
                    Merch itemToSearch = (((SaleItem_Merchandise)SaleItem).MerchItem.IsChild) ?
                        ((SaleItem_Merchandise)SaleItem).MerchItem.ParentMerchRecord : ((SaleItem_Merchandise)SaleItem).MerchItem;

                    List<SaleItem_Merchandise> likeMerchInCart = new List<SaleItem_Merchandise>();
                    //determine quantity of all merch items with the same parent
                    likeMerchInCart.AddRange(Ctx.Cart.MerchandiseItems.FindAll(delegate(SaleItem_Merchandise match)
                    { return (match.MerchItem.TParentListing != null && match.MerchItem.TParentListing == itemToSearch.Id); }));

                    foreach (SaleItem_Merchandise sim in likeMerchInCart)
                        cartQty += sim.Quantity;

                    coll.AddRange(itemToSearch.MerchBundleRecords().Get_MerchBundleRecords_RunningAndAvailable());
                }
                else if (isTicket)
                {
                    ShowTicket itmToSearch = ((SaleItem_Ticket)SaleItem).Ticket;
                    cartQty = SaleItem.Quantity;

                    coll.AddRange(itmToSearch.MerchBundleRecords().Get_MerchBundleRecords_RunningAndAvailable());
                }

                //loop thru the sale items bundles   
                if (coll.Count == 0)
                {
                    //TODO: set product to not active
                    //notify admin and user
                    //exit
                }

                if (coll.Count > 1)
                    coll.Sort("DisplayOrder", true);

                foreach (MerchBundle bun in coll)
                {
                    //determine qty of bundles to add to display
                    int reqParent = bun.RequiredParentQty;
                    int bundleQty = cartQty / reqParent;

                    //if we don't have enough in cart to qualify for even one of a bundle
                    //then display to user how many more they need to qualify
                    if (bundleQty <= 0)
                    {
                        //remove any and all selections for this bundle
                        SaleItem.MerchBundleSelections.RemoveAll(delegate(MerchBundle_Listing match)
                            { return (match.BundleId == bun.Id); });

                        //add an item to the list for display
                        //mark with a -1 to indicate that more qty is needed to activate the bundle
                        int needNMoreToQualify = reqParent - cartQty; 
                        list.Add(new MerchBundle_Listing(0, bun, -1, needNMoreToQualify));
                    }
                    //if the cart quantity has qualified for the bundle
                    else if (bundleQty > 0)
                    {
                        list.Add(new MerchBundle_Listing(0, bun, 0, bundleQty));
                    }
                }

                return list;
            }

            return null;

        }

        #endregion

        #region Repeater

        /// <summary>
        /// displays a status of the bundle
        /// displays if more selections are needed to activate
        /// displays any selections for the bundle
        /// displays a link to edit selections
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem ||
                e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.SelectedItem)
            {
                Repeater rpt = (Repeater)sender;
                MerchBundle_Listing listing = (MerchBundle_Listing)e.Item.DataItem;

                if (listing != null)
                {
                    //get the selections for this bundle
                    MerchBundle bun = listing.MerchBundleRecord;

                    int numSelected = 0;
                    bool isOnlyOneAvailableSelection = bun.HasOnlyOneAvailableSelection;
                    bool isOptOut = bun.OffersOptout;
                    bool isMultResult = bun.IsMultSelection;
                    bool isQualified = (listing.SelectedInventoryId != -1);


                    //if we have only one available - make sure that there is a selection for that item
                    //also ensure that the quantity matches
                    SaleItem.RemoveSelectionsOverQuota(bun);                    

                    List<MerchBundle_Listing> selections = new List<MerchBundle_Listing>();
                    if(SaleItem.MerchBundleSelections != null)
                        selections.AddRange(SaleItem.GetValidMerchBundleListings_Selected(bun.Id));

                    foreach (MerchBundle_Listing mlisting in selections)
                        numSelected += mlisting.Quantity;


                    Literal litTitle = (Literal)e.Item.FindControl("litTitle");
                    Literal litEdit = (Literal)e.Item.FindControl("litEdit");
                    Literal litBundleSelected = (Literal)e.Item.FindControl("litBundleSelected"); 
                    Literal litBundleItems = (Literal)e.Item.FindControl("litBundleItems");
                    Literal litPricing = (Literal)e.Item.FindControl("litPricing");

                    //<div class="mblist-title"><%# Eval("MerchBundleRecord.TitleEncoded") %></div>
                    if (litTitle != null)
                    {
                        litTitle.Text = "<div class=\"mblist-title\">";

                        //if(isOnlyOneAvailableSelection)
                        //    litTitle.Text += string.Format("{0} @ ", numSelected.ToString());

                        litTitle.Text += string.Format("{0}</div>", bun.TitleEncoded);
                    }

                    if (litEdit != null && (!isOnlyOneAvailableSelection) && isQualified)
                    {
                        //external page is given in the href
                        litEdit.Text = string.Format("<a href=\"/Store/Cart_MerchBundle.aspx?sim={0}&mbid={1}\" title=\"edit selections for this bundle\" rel=\"{2}\" class=\"btntribe ov-trigger{3}\" >", 
                            SaleItem.GUID.ToString(), listing.BundleId.ToString(), "#overlay-bundle",

                            numSelected < (listing.Quantity * bun.MaxSelections) ? " non-qual" : string.Empty);

                        //any element can be used inside the trigger
                        litEdit.Text += string.Format("edit bundle</a>");
                    }

                    //display number of selections for bundle
                    //only display num selections if qualified
                    if (litBundleSelected != null && (!isOnlyOneAvailableSelection))
                    {
                        litBundleSelected.Text = string.Format("<span class=\"mblist-num-select\">");

                        if(isQualified)
                        {                            
                            litBundleSelected.Text += string.Format("You have selected {0} of {1} selections for this bundle",
                                numSelected.ToString(), (listing.Quantity * bun.MaxSelections).ToString());
                        }
                        else
                            litBundleSelected.Text += string.Format("Add {0} more {1} to qualify for the {2} bundle.", 
                                listing.Quantity, bun.ParentDescription, bun.TitleEncoded);

                        litBundleSelected.Text += string.Format("</span>");
                    }

                    if (litBundleItems != null && isQualified)// && (!isOnlyOneAvailableSelection))
                    {
                        string selectionList = string.Empty;

                        //if there are selections for this bundle - display them here
                        foreach (MerchBundle_Listing selected in selections)
                            selectionList += string.Format("<div>{0} @ {1}</div>", selected.Quantity.ToString(), selected.SelectedInventory.DisplayNameWithAttribs);

                        if (selectionList.Trim().Length > 0)
                            litBundleItems.Text += string.Format("<div class=\"mblist-selection\">{0}</div>", 
                                selectionList);//.Trim(new char[] { ',', ' ' }));
                    }

                    if (litPricing != null && isOptOut)
                    {
                        litBundleItems.Text += string.Format("<div class=\"mblist-price\">bundle total: <span>{0}</span></div>", 
                            SaleItem.GetIndividualBundlePrice(bun).ToString("c"));
                    }
                }
            }
        }

        #endregion
    }
}