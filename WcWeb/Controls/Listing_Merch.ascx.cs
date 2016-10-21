using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using Wcss;

namespace WillCallWeb.Controls
{
    public partial class Listing_Merch : WillCallWeb.BaseControl, IPostBackEventHandler
    {
        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            string[] args = eventArgument.Split('~');
            string command = args[0];
            string result = string.Empty;

            switch (command.ToLower())
            {
                case "complianceclose":

                    //todo: check for compliance - if we are now compliant - redirect to product page
                    if (Ctx.UserIs18OrOlder)
                    {
                        base.Redirect(string.Format("/Store/ChooseMerch.aspx?mite={0}", Globals.MerchId.ToString()));
                    }
                    else
                    {
                        Globals.MerchId = 0;
                        Globals.MerchItem = null;
                        base.Redirect("/Store/ChooseMerch.aspx");
                    }
                    break;
            }
        }

        #region Page Overhead

        protected Merch _merch;
        protected bool _display18Compliance = false;
        protected bool _is18Compliant = false;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (base.IsAuthdAdminUser && (this.Page.ToString().ToLower() == "asp.admin_display_merch_aspx" || this.Page.ToString().ToLower() == "asp.admin_showmerch_aspx"))
                _merch = Atx.CurrentMerchRecord;
            else
                _merch = Globals.MerchItem;
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (_merch != null)
            {
                bool isAdminDisplay = (base.IsAuthdAdminUser && this.Page.ToString().ToLower() == "asp.admin_display_merch_aspx");                

                if (!isAdminDisplay)
                {
                    if (!_merch.IsDisplayable)
                        base.Redirect("/Store/ChooseMerch.aspx");

                    string userName = (this.Profile != null && (!this.Profile.IsAnonymous)) ? this.Profile.UserName : null;
                    bool accessGranted = false;

                    if(userName != null)
                    {
                        //active PAs only have been filtered

                        ProductAccessCollection pacoll = new ProductAccessCollection();
                        pacoll.AddRange(_Lookits.ProductAccessors.GetList().FindAll(delegate(ProductAccess match) 
                        { 
                            return (match.IsActive && match.IsWithinActivationPeriod(Ctx.MarketingProgramKey) &&   
                                match.AppliesToCurrentMerch(_merch));
                        }));

                        //if we have a product accessor in play
                        //then see if current user applies
                        if (pacoll.Count > 0)
                        {
                            foreach (ProductAccess pa in pacoll)
                            {
                                if (pa.OrderFlowUserList.Contains(userName.ToLower()))
                                {
                                    accessGranted = true;
                                    break;
                                }
                            }

                            if (!accessGranted)
                                base.Redirect("/Store/ChooseMerch.aspx");
                        }
                    }


                    //perform other tests to see if we can display
                    //execute exit on non-conformance
                    if (!accessGranted)
                    {
                        RequiredMerchCollection reqs = new RequiredMerchCollection();
                        reqs.AddRange(_merch.RequiredMerchRecords());

                        if (reqs != null && reqs.Count > 0)
                        {
                            if (userName == null)
                                base.Redirect("/Store/ChooseMerch.aspx");

                            if (RequiredMerch.ScanPurchaseRequirements(userName, _merch).First != true)
                                base.Redirect("/Store/ChooseMerch.aspx");

                            //pass through with reqs that are allowed

                            //ignore internal only if reqs are met

                        }
                        else if (_merch.IsInternalOnly)//these items only display in admin editor
                            base.Redirect("/Store/ChooseMerch.aspx");
                    }


                    //set display based on over 18
                    int idx = _merch.Id;
                    if (Ctx.Merch_Requires_18Over_Acknowledge_List.Contains(idx))
                    {
                        _display18Compliance = true;
                        _is18Compliant = Ctx.UserIs18OrOlder;
                    }
                }

                if (!IsPostBack)
                {
                    SetPanelAttribs(PanelShopItem, _merch, _listWidth);
                }

                if (Ctx.CurrentCartException != null && Ctx.CurrentCartException.Trim().Length > 0)
                {
                    AddToCartValidator.IsValid = false;
                    AddToCartValidator.ErrorMessage = string.Format("{0}", Ctx.CurrentCartException);

                    Ctx.CurrentCartException = null;
                }

                DisplayMerchItem();

                hdnDisplayComply18.Value = _display18Compliance.ToString();
                hdnIsComply18.Value = _is18Compliant.ToString();
            }
        }

        protected string ConstructHeaderInfo()
        {
            sb.Length = 0;

            //Inventory 
            //if we have a solo inventory child
            if (_merch.IsSoldOut)
            {
                btnAdd.Visible = false;

                sb.AppendLine("<div class=\"no-inventory\">This item is Sold Out!</div>");

                return sb.ToString();//dont show promotions, etc
            }
            else if (_merch.Available <= 0 || ((!_merch.HasChildStyles) && (!_merch.HasChildColors) && (!_merch.HasChildSizes)))
            {
                MerchCollection coll = new MerchCollection();
                coll.AddRange(_merch.ChildMerchRecords().GetList().FindAll(delegate(Merch match)
                { return (match.IsActive && match.Available > 0); }));

                btnAdd.Visible = coll.Count > 0;

                if (!btnAdd.Visible)
                    sb.AppendLine("<div class=\"no-inventory\">This item is currently out of stock.</div>");
            }

            //display short text

            if (_merch.ShortText != null && _merch.ShortText.Trim().Length > 0)
            {
                //string stripped = Utils.ParseHelper.StripHtmlTags(_merch.ShortText);
                string strip = _merch.ShortText.Trim();

                if (strip.ToLower() != "<br />" && strip.ToLower() != "<br/>")
                {
                    sb.AppendLine("<div class=\"merch-header\">");
                    sb.AppendFormat("<span class=\"merch-header-text\">{0}</span>", strip);
                    sb.AppendLine("</div>");
                }
            }

            //set possible promotion - below header
            SalePromotionCollection collect = new SalePromotionCollection();
            collect.AddRange(Ctx.Cart.SalePromotions_RunningAndAvailable.GetList().FindAll(delegate(SalePromotion match)
            {
                if (match.Requires_MerchItem)
                {
                    if (match.RequiredMerchListing.Contains(_merch.Id))
                        return true;

                    foreach (Merch m in match.RequiredMerchItems(Ctx))
                    {
                        if ((m.TParentListing.HasValue && m.TParentListing == _merch.Id) || m.Id == _merch.Id)
                            return true;
                    }
                }

                return false;
            }));

            foreach (SalePromotion promo in collect)
            {
                string childRequirements = string.Empty;

                if (!promo.RequiredMerchListing.Contains(_merch.Id))
                {
                    //leave a space to separate
                    childRequirements = string.Format(" (see promotion for details)");
                }

                sb.AppendFormat("<div class=\"merch-promo-wrapper\"><span class=\"merch-promo\">{0}{1}</span></div>", promo.DisplayText, childRequirements);
                sb.AppendLine();
            }

            //this will incorporate more options as time goes on
            if (_merch.SpecialInstructions != null && _merch.SpecialInstructions.Trim().Length > 0)
            {
                sb.AppendFormat("<div class=\"special-instructions\">{0}</div>", _merch.SpecialInstructions);
                sb.AppendLine();
            }

            //POST PURCHASE TEXTS
            PostPurchaseTextCollection postColl = new PostPurchaseTextCollection();
            postColl.AddRange(_merch.PostPurchaseTextRecords().GetList()
                .FindAll(delegate(PostPurchaseText match) { return (match.InProcessDescription != null && match.InProcessDescription.Trim().Length > 0); }));
            if (postColl.Count > 0)
            {
                postColl.Sort("IDisplayOrder", true);
                sb.AppendLine("<div class=\"postpurchase\">");

                foreach (PostPurchaseText pp in postColl)
                {
                    sb.AppendFormat("<div class=\"pptext\">{0}</div>", System.Web.HttpUtility.HtmlEncode(pp.InProcessDescription.Trim()));
                    sb.AppendLine();
                }

                sb.AppendLine("</div>");

                sb.AppendLine();
            }

            return sb.ToString();
        }

        protected void DisplayMerchItem()
        {
            string head = ConstructHeaderInfo();
            if (head.Trim().Length > 0)
                litHeaders.Text = string.Format("<div class=\"headers\">{0}{1}{0}</div>{0}", Environment.NewLine, head);

            ConstructMerchDisplay();

            /*display merch bundles*/
            litBundle.Text = MerchBundle.DisplayBundle_Listing(_merch, true, true, true, this.Page.ToString());
        }

        private StringBuilder sb = new StringBuilder();
        private string _detailImage = string.Empty;

        private string ConstructPrice()
        {
            sb.Length = 0;
            sb.AppendLine();
            sb.AppendLine("<span class=\"pricepanel\">");

            //the main act name speaks for the price - no reason to list twice
            if (_merch.IsGiftCertificateDelivery)
            {   
                //sb.AppendLine("<span class=\"saleprice\">");
                //sb.AppendFormat("<span class=\"label\">{0}</span>", _merch.ShortText);
                //sb.AppendLine("</span>");
                //sb.AppendLine();
            }
            else if(_merch.UseSalePrice)
            {
                sb.AppendFormat("<span class=\"originalprice\">originally {0}</span> ", _merch.Price.ToString("c"));
                sb.AppendLine();
                sb.AppendFormat("<span class=\"saveprice\">you save {0}</span>", _merch.SalePriceSavings.ToString("c"));
                sb.AppendLine();
                sb.AppendLine("<span class=\"saleprice\">");
                sb.AppendLine("<span class=\"label\">sale price:</span> ");
                sb.AppendFormat("<span class=\"money\">{0}</span>", _merch.Price_Effective.ToString("c"));
                sb.AppendLine();
                sb.AppendLine("</span>");
            }
            else
            {
                sb.AppendLine("<span class=\"saleprice\">");
                sb.AppendLine("<span class=\"label\">price each:</span> ");
                sb.AppendFormat("<span class=\"money\">{0}</span>", _merch.PriceListing);//.Price.ToString("c"));
                sb.AppendLine("</span>");
                sb.AppendLine();
            }

            sb.AppendLine("</span>");
            sb.AppendLine();

            return sb.ToString();
        }

        private string ConstructDescription()
        {
            sb.Length = 0;
            if (_merch.Description != null && _merch.Description.Trim().Length > 0)
            {
                string strip = _merch.Description.Trim();

                if (strip.ToLower() != "<br />" && strip.ToLower() != "<br/>")
                {
                    sb.AppendLine();
                    sb.AppendLine("<span class=\"description-wrapper\">");
                    sb.AppendFormat("<span class=\"item-desc\">{0}</span>", strip);
                    sb.AppendLine();
                    sb.AppendLine("<div class=\"clear\"></div>");
                    sb.AppendLine("</span>");
                    sb.AppendLine();
                }
            }
            return sb.ToString();
        }

        protected string ConstructPictureDisplay()
        {
            sb.Length = 0;

            //add a link to detail view if we have a detail - we just need to have one - collection comes later
            ItemImageCollection details = new ItemImageCollection();
            details.AddRange(_Lookits.MerchImages.GetList().FindAll(
                delegate(ItemImage match) { return match.TMerchId == _merch.Id && match.IsDetailImage; }));

            if (details.Count > 0)
            {
                string conText = "View Image(s)";
                string status = string.Format("onMouseOver=\" window.status=&#39;{0}&#39;; return true\" onMouseOut=\"window.status=&#39; &#39;; return true\"", conText);
                sb.AppendLine(); 
                sb.AppendFormat("<a href=\"javascript:imagePopup(&#39;{0}&#39;,&#39;{1}&#39;);\" {2} class=\"btntribe view-detail\" title=\"{3}\" alt=\"{3}\" border=\"0\" >{3}</a>",
                    _merch.Id, _Config._MerchThumbSizeMax, status, conText);
                sb.AppendLine();
                sb.AppendLine("<div class=\"clear\"></div>");

                _detailImage = sb.ToString();
                sb.Length = 0;
            }

            ItemImageCollection coll = new ItemImageCollection();
            coll.AddRange(_merch.ItemImageRecords().GetList().FindAll(delegate(ItemImage match) { return match.IsItemImage; }));
            if (coll.Count > 1)
                coll.Sort("IDisplayOrder", true);

            if (coll.Count > 0)
            {
                sb.AppendLine("<div class=\"merch-pics\">");
                foreach (ItemImage image in coll)
                {
                    string img = (_Config._AlwaysUseLargeThumbsForDetail) ? image.Thumbnail_Large : image.Thumbnail_Small;
                    sb.AppendFormat("<img src=\"{0}\" border=\"0\" class=\"{1}\" {2} />", img, image.ThumbClass,
                        (image.IsPortrait) ? string.Format("height=\"{0}\"", _Config._MerchThumbSizeLg) : string.Format("width=\"{0}\"", _Config._MerchThumbSizeLg));
                    sb.AppendLine();
                }
                sb.AppendLine("</div>");
            }

            return sb.ToString();
        }

        protected Literal CreateLiteralFromText(string txt)
        {
            Literal lit = new Literal();
            lit.Text = txt;
            return lit;
        }

        protected void ConstructMerchDisplay()
        {
            bool drt = _merch.IsDisplayRichText;
            TableRow descRow = new TableRow();
            int cellIdx = 0;

            switch (_merch.DisplayTemplate)
            {
                case _Enums.MerchDisplayTemplate.Legacy:    
                case _Enums.MerchDisplayTemplate.ThreeColumn:
                    if (_merch.DisplayTemplate == _Enums.MerchDisplayTemplate.Legacy)
                    {
                        //add description to update cell
                        cellupdate.Controls.AddAt(0, CreateLiteralFromText(ConstructDescription()));
                    }
                    else
                    {
                        //leave update cell asis

                        //add a new cell and fill with description
                        rowmain.Cells.AddAt(0, new TableCell());
                        rowmain.Cells[0].CssClass = "itemdesc";
                        rowmain.Cells[0].Controls.Add(CreateLiteralFromText(ConstructDescription()));
                    }

                    //add a new cell and fill with price, detaillink and pics
                    rowmain.Cells.AddAt(0, new TableCell());
                    rowmain.Cells[0].CssClass = "iteminfo";
                    //must try to construct pictures before link - note order of adds!
                    rowmain.Cells[0].Controls.Add(CreateLiteralFromText(ConstructPictureDisplay()));
                    rowmain.Cells[0].Controls.AddAt(0, CreateLiteralFromText(_detailImage));
                    rowmain.Cells[0].Controls.AddAt(0, CreateLiteralFromText(ConstructPrice()));

                    break;
                case _Enums.MerchDisplayTemplate.ControlsAboveRichText:
                case _Enums.MerchDisplayTemplate.ControlsBelowRichText:
                    cellIdx = (_merch.DisplayTemplate == _Enums.MerchDisplayTemplate.ControlsBelowRichText) ? 0 : 1;
                    //leave update cell asis

                    //add a new cell and add price and detaillink
                    rowmain.Cells.AddAt(0, new TableCell());
                    rowmain.Cells[0].CssClass = "iteminfo";
                    //must try to construct pictures before link - note order of adds!
                    rowmain.Cells[0].Controls.Add(CreateLiteralFromText(ConstructPrice()));
                    ConstructPictureDisplay();
                    rowmain.Cells[0].Controls.Add(CreateLiteralFromText(_detailImage));                    

                    //add a new cell in a new row and fill with description                                      
                    descRow.Cells.Add(new TableCell());
                    descRow.Cells[0].CssClass = "itemdesc";
                    descRow.Cells[0].ColumnSpan = 2;
                    descRow.Cells[0].Controls.Add(CreateLiteralFromText(ConstructDescription()));
                    tblMerch.Rows.AddAt(cellIdx, descRow);

                    break;
                case _Enums.MerchDisplayTemplate.ControlsToLeftOfDescription:
                case _Enums.MerchDisplayTemplate.ControlsToRightOfDescription:
                    cellIdx = (_merch.DisplayTemplate == _Enums.MerchDisplayTemplate.ControlsToLeftOfDescription) ? 1 : 0;
                    //add price and link above update                    
                    cellupdate.Controls.AddAt(0, CreateLiteralFromText(ConstructPrice()));
                    ConstructPictureDisplay();
                    cellupdate.Controls.Add(CreateLiteralFromText(_detailImage));

                    //add a new cell and fill with description
                    rowmain.Cells.AddAt(cellIdx, new TableCell());
                    rowmain.Cells[cellIdx].CssClass = "itemdesc";                    
                    rowmain.Cells[cellIdx].Controls.Add(CreateLiteralFromText(ConstructDescription()));

                    break;
            }

        }

        #endregion

        #region Ajax controls

        //this matches the default.css defintion for 
        private const int _listWidth = 200;

        public static void CreateDropDown(Merch merchandise, ref DropDownList listToCreate, 
            ref AjaxControlToolkit.CascadingDropDown cascadingCounterpart, string categoryName, int listWidth)
        {
            listToCreate = new DropDownList();
            cascadingCounterpart = new AjaxControlToolkit.CascadingDropDown();

            listToCreate.ID = string.Format("ddl{0}", categoryName);
            listToCreate.Width = Unit.Pixel(listWidth);
            listToCreate.CssClass = "itemattrib";
            listToCreate.EnableViewState = false;

            cascadingCounterpart.ID = string.Format("Cascading{0}", categoryName);
            cascadingCounterpart.Category = categoryName;
            cascadingCounterpart.UseContextKey = true;
            cascadingCounterpart.ContextKey = merchandise.Id.ToString();
            cascadingCounterpart.LoadingText = string.Format("[ ... Loading {0} ... ]", categoryName);

            if(categoryName.ToLower() != "style" || (!merchandise.IsGiftCertificateDelivery))
                cascadingCounterpart.PromptText = string.Format("[ Please Select A {0} ]", categoryName);
            else
                cascadingCounterpart.PromptText = "[ Please Select An Amount ]";

            cascadingCounterpart.TargetControlID = listToCreate.ID;
            cascadingCounterpart.ServicePath = "/Services/MerchAttribute_Svc.asmx";
            cascadingCounterpart.ServiceMethod = string.Format("Get{0}s", categoryName);
            cascadingCounterpart.BehaviorID = string.Format("the{0}", categoryName);
        }

        public static void SetPanelAttribs(UpdatePanel panelToHoldControls, Merch merchandise, int listWidth)
        {
            if (merchandise != null && merchandise.IsParent)
            {
                //STYLES
                DropDownList styleList = null;
                AjaxControlToolkit.CascadingDropDown cddStyle = null;
                DropDownList colorList = null;
                AjaxControlToolkit.CascadingDropDown cddColor = null;
                DropDownList sizeList = null;
                AjaxControlToolkit.CascadingDropDown cddSize = null;

                if (merchandise.HasChildStyles)
                    CreateDropDown(merchandise, ref styleList, ref cddStyle, "Style", listWidth);

                if (merchandise.HasChildColors)
                {
                    CreateDropDown(merchandise, ref colorList, ref cddColor, "Color", listWidth);

                    if (styleList != null)
                        cddColor.ParentControlID = styleList.ID;
                }

                if (merchandise.HasChildSizes)
                {
                    CreateDropDown(merchandise, ref sizeList, ref cddSize, "Size", listWidth);

                    if (colorList != null)
                        cddSize.ParentControlID = colorList.ID;
                    else if (styleList != null)
                        cddSize.ParentControlID = styleList.ID;
                    else
                        cddSize.ParentControlID = string.Empty;
                }

                //add to panel in correct order
                if (sizeList != null && cddSize != null)
                {
                    panelToHoldControls.ContentTemplateContainer.Controls.AddAt(0, sizeList);
                    panelToHoldControls.ContentTemplateContainer.Controls.Add(cddSize);
                }
                if (colorList != null && cddColor != null)
                {
                    panelToHoldControls.ContentTemplateContainer.Controls.AddAt(0, colorList);
                    panelToHoldControls.ContentTemplateContainer.Controls.Add(cddColor);
                }
                if (styleList != null && cddStyle != null)
                {
                    panelToHoldControls.ContentTemplateContainer.Controls.AddAt(0, styleList);
                    panelToHoldControls.ContentTemplateContainer.Controls.Add(cddStyle);
                }
            }
        }

        #endregion

        #region Add From Selected style, color, size (if exists)

        /// <summary>
        /// 
        /// </summary>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            bool isAdminDisplay = (base.IsAuthdAdminUser && this.Page.ToString().ToLower() == "asp.admin_display_merch_aspx");

            if (!isAdminDisplay)
            {
                int idx = _merch.Id;
                if (Ctx.Merch_Requires_18Over_Acknowledge_List.Contains(idx))
                {
                    if (!Ctx.UserIs18OrOlder)
                    {
                        Ctx.CurrentCartException = "You must be 18 years or older to add this item to your cart.";
                        //redirect to clear selection - need to do this to get around ajax limitations
                        base.Redirect(string.Format("{0}", Request.RawUrl));
                    }
                }
            }

            string style = string.Empty;
            string color = string.Empty;
            string size = string.Empty;

            //this will hold the values from any dropdowns on the page - style; size; color
            string selection = HiddenAttribs.Value;

            if (selection != null && selection.Trim().Length > 0)
            {
                string[] attribs = selection.Trim().TrimEnd(';').Split(';');

                if (attribs.Length > 0)
                {
                    foreach (string s in attribs)
                    {
                        string[] parts = s.Split('=');
                        if (parts[0].ToLower().Trim() == "style")
                            style = parts[1].Trim();
                        else if (parts[0].ToLower().Trim() == "color")
                            color = parts[1].Trim();
                        else if (parts[0].ToLower().Trim() == "size")
                            size = parts[1].Trim();
                    }
                }
            }


            //if the item is already in the cart - this method will not change the existing item
            string result = Ctx.Cart.AddMerchFromAttribs(_merch.Id, style, color, size, 1, this.Profile);
            
            //if all is good then move onto the cart where they can add more items, etc
            if (result.Trim().Length == 0)
            {
                base.Redirect("/Store/Cart_Edit.aspx");
            }

            Ctx.CurrentCartException = result;

            //redirect to clear selection - need to do this to get around ajax limitations
            base.Redirect(string.Format("{0}", Request.RawUrl));
        }

        #endregion

    }
}
