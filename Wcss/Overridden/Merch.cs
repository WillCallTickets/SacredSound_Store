using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Web.UI.WebControls;

namespace Wcss
{
    /// <summary>
    /// This is a simple class to add a quantity to merch items
    /// </summary>
    public partial class MerchWithQuantity
    {
        public int Qty { get; set; }
        public Merch MerchRecord { get; set; }

        public MerchWithQuantity(int qty, Merch merch)
        {
            Qty = qty;
            MerchRecord = merch;
        }
    }

    public partial class Merch
    {
        /// <summary>
        /// returns a link to the sales page for the item
        /// </summary>
        private string _salesUrl = null;
        public string SalesUrl
        {
            get
            {
                if (_salesUrl == null)
                    _salesUrl = string.Format("http://{0}/Store/ChooseMerch.aspx?mite={1}", 
                        _Config._DomainName, (this.IsParent) ? this.Id.ToString() : this.TParentListing.ToString());

                return _salesUrl;
            }
        }

        /// <summary>
        /// determines if this has merch bundle ties and adjust merchbundle status
        /// this item must be sold out to trigger the chain of events
        /// </summary>
        /// <param name="severity"></param>
        public void EvaluateMerchBundleStatus(string severity)
        {
            if(severity.ToLower() != "soldout")
                return;

            //see if this is a child of a bundle
            MerchBundleItemCollection coll = new MerchBundleItemCollection();
            coll.AddRange(this.MerchBundleItemRecords());
         
            if (coll.Count > 0)
            {
                bool requiresMerchRefresh = false;
                bool requiresTicketRefresh = false;

                //establish a list of bundles that may need to be deactivated
                List<MerchBundle> bundles = new List<MerchBundle>();

                foreach (MerchBundleItem mbi in coll)
                {
                    //add bundle to the list to evaluate later
                    MerchBundle bundle = mbi.MerchBundleRecord;
                    if (bundles.FindIndex(delegate(MerchBundle match) { return (match.Id == bundle.Id); }) == -1)
                        bundles.Add(bundle);

                    //deactivate the item 
                    mbi.IsActive = false;
                    mbi.Save();

                    //notify admin
                    EventQ.CreateInventoryNotification("System", severity, 
                        string.Format("The merch bundle item - {0} - connected to BUNDLE: {1} has been deactivated", mbi.MerchRecord.DisplayNameWithAttribs, mbi.MerchBundleRecord.Title), 
                        _Enums.InvoiceItemContext.bundle, mbi.Id, 0);
                }


                //now loop thru bundles and deactivate as necessary
                foreach (MerchBundle bundle in bundles)
                {
                    bool hasValid = true;
                    int sumOfAvailable = 0;

                    //determine if the bundle still has valid items
                    try
                    {   
                        using (System.Data.IDataReader reader = SPs.TxInventoryBundleGetChildItemCount(10011).GetReader())
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    sumOfAvailable = reader.GetInt32(reader.GetOrdinal("BundleSum"));
                                }
                            }
                        }

                        if (sumOfAvailable <= 0)
                            hasValid = false;
                    }
                    catch (Exception ex)
                    {
                        _Error.LogException(ex, true);
                    }


                    //if no valid items
                    if (!hasValid)
                    {
                        try
                        {
                            //deactivate bundle
                            bundle.BActive = false;
                            bundle.Save();

                            string parent = string.Empty;
                            _Enums.InvoiceItemContext ctx = _Enums.InvoiceItemContext.notassigned;
                            int idx = 0;

                            //deactivate parent
                            if (bundle.TMerchId.HasValue)
                            {
                                bundle.MerchRecord.BActive = false;
                                bundle.MerchRecord.Save();
                                requiresMerchRefresh = true;
                                parent = string.Format("MERCH ID:{0} {1}", bundle.TMerchId.ToString(), bundle.MerchRecord.DisplayNameWithAttribs);
                                ctx = _Enums.InvoiceItemContext.merch;
                                idx = bundle.TMerchId.Value;
                            }
                            else if (bundle.TShowTicketId.HasValue)
                            {
                                bundle.ShowTicketRecord.BActive = false;
                                bundle.ShowTicketRecord.Save();
                                requiresTicketRefresh = true;
                                parent = string.Format("SHOWTICKET ID:{0} {1}", bundle.TShowTicketId.ToString(), 
                                    Utils.ParseHelper.StripHtmlTags(bundle.ShowTicketRecord.DisplayNameWithAttribsAndDescription));
                                ctx = _Enums.InvoiceItemContext.ticket;
                                idx = bundle.TShowTicketId.Value;
                            }

                            string bundleValue = string.Format("MERCHBUNDLE ID:{0} {1}", bundle.Id, bundle.Title);

                            //notify admin
                            EventQ.CreateInventoryNotification("System", "deactivated",
                                string.Format("A BundledItem has run out of inventory which caused the Bundle to run out of inventory. As a consequence, {0} and {1} have also been deactivated",
                                    bundleValue, parent),
                                ctx, idx, 0);
                        }
                        catch (Exception ex)
                        {
                            _Error.LogException(ex, true);
                        }
                    }
                }

                try
                {
                    if (requiresMerchRefresh)
                    {
                        Utils.StateManager.ResetWebsiteCache("merch");
                        Utils.StateManager.ResetWebsiteCache("lookups");
                    }
                    else if (requiresTicketRefresh)
                    {
                        Utils.StateManager.ResetWebsiteCache("shows");
                    }
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex, true);
                }
            }
        }

        public bool IsPromotionalItem
        {
            get
            {
                //this.SalePromotionRecords - this refers to when the merch item is required to turn on the promotion
                return ((this.IsParent) && (this.SalePromotionAwardRecords().Count > 0));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Save_AvoidRealTimeVars()
        {
            //negative one is ignored
            Save_AvoidRealTimeVars(-1);
        }
        public void Save_AvoidRealTimeVars(int damagedQty)
        {
            SPs.TxMerchUpdateAvoidRealTimeVars(this.Id, this.Name, this.Style, this.Color, this.Size, this.IsActive, //this.IsListed, 
                this.IsInternalOnly, this.IsSoldOut, this.IsTaxable,
                this.IsFeaturedItem, this.ShortText, this.DisplayTemplate.ToString(), this.Description, this.IsUnlockActive, this.UnlockCode, this.UnlockDate, this.UnlockEndDate,
                this.PublicOnsaleDate, this.EndDate, this.Price, this.UseSalePrice, this.SalePrice, this.DeliveryType.ToString(), this.IsLowRateQualified, 
                this.Weight, this.FlatShip, this.FlatMethod, 
                this.DtBackorder, this.BShipSeparate, this.MaxQuantityPerOrder, this.Allotment, damagedQty).Execute();
        }

        [XmlAttribute("DisplayTemplate")]
        public _Enums.MerchDisplayTemplate DisplayTemplate
        {
            get 
            {
                if (this.IsChild)
                    return this.ParentMerchRecord.DisplayTemplate;

                if (this.VcDisplayTemplate == null)
                    return _Enums.MerchDisplayTemplate.Legacy;

                return (_Enums.MerchDisplayTemplate)Enum.Parse(typeof(_Enums.MerchDisplayTemplate), this.VcDisplayTemplate, true); 
            }
            set { this.VcDisplayTemplate = value.ToString(); }
        }
        [XmlAttribute("IsDisplayRichText")]
        public bool IsDisplayRichText
        {
            get
            {
                return (this.DisplayTemplate != _Enums.MerchDisplayTemplate.Legacy);
            }
        }

        [XmlAttribute("IsLowRateQualified")]
        public bool IsLowRateQualified
        {
            get 
            {
                if (IsParent)
                    return (this.BLowRateQualified.HasValue) ? this.BLowRateQualified.Value : false;
                else
                    return (this.BLowRateQualified.HasValue) ? this.BLowRateQualified.Value : this.ParentMerchRecord.IsLowRateQualified;
            }
            set { this.BLowRateQualified = value; }
        }

        /// <summary>
        /// The number of allowed downloads per item
        /// </summary>
        [XmlAttribute("DownloadMax")]
        public int DownloadMax { get { return _Config._DownloadMax; } }
        
        [XmlAttribute("IsFlatShip")]
        public bool IsFlatShip { get { return this.FlatMethod != null && this.FlatMethod.Trim().Length > 0; } }
        [XmlAttribute("FlatShip")]
        public decimal FlatShip
        {
            get { return (!this.MFlatShip.HasValue) ? (IsChild) ? this.ParentMerchRecord.FlatShip : 0 : this.MFlatShip.Value; }
            set { this.MFlatShip = value; }
        }
        [XmlAttribute("FlatMethod")]
        public string FlatMethod
        {
            get { return (this.VcFlatMethod == null) ? (IsChild) ? this.ParentMerchRecord.FlatMethod : string.Empty : this.VcFlatMethod; }
            set { this.VcFlatMethod = value; }
        }
        [XmlAttribute("IsShipSeparate")]
        public bool IsShipSeparate
        {
            get { return (this.BShipSeparate.HasValue) ? this.BShipSeparate.Value : false; }
            set { this.BShipSeparate = value; }
        }
        /// <summary>
        /// also could be called pre-order date. The date when the item is available for shipping. Does not inherit from parent
        /// </summary>
        [XmlAttribute("BackorderDate")]
        public DateTime BackorderDate
        {
            get { return (!this.DtBackorder.HasValue) ? Utils.Constants._MinDate : this.DtBackorder.Value; }
            set 
            {
                if (value <= Utils.Constants._MinDate)
                    this.DtBackorder = null;
                else
                    this.DtBackorder = value;
            }
        }
        [XmlAttribute("IsBackordered")]
        public bool IsBackordered
        {
            get { 
                DateTime backDate = Wcss._Shipper.CalculateShipDate(this.BackorderDate);
                DateTime nowDate = Wcss._Shipper.NowShip;

                return backDate > nowDate; }
        }
        [XmlAttribute("SpecialInstructions")]
        public string SpecialInstructions
        {
            get
            {
                if (this.IsBackordered && this.IsFlatShip)
                    return string.Format("This item ships separately at a flat rate of {0}. This item is backordered and will not ship before {1}.",
                        this.FlatShip.ToString("c"), Wcss._Shipper.CalculateShipDate(this.BackorderDate).ToString("MM/dd/yyyy"));
                else if (this.IsBackordered && this.IsShipSeparate)
                    return string.Format("This item must ship separately. This item is backordered and will not ship before {0}.",
                        Wcss._Shipper.CalculateShipDate(this.BackorderDate).ToString("MM/dd/yyyy"));
                else if (this.IsBackordered)
                    return string.Format("This item is backordered and will not ship before {0}.",
                        Wcss._Shipper.CalculateShipDate(this.BackorderDate).ToString("MM/dd/yyyy"));
                else if (this.IsFlatShip)
                    return string.Format("This item ships separately at a flat rate of {0}.", this.FlatShip.ToString("c"));
                else if (this.IsShipSeparate)
                    return string.Format("This item must ship separately.");

                return null;
            }
        }



        #region ProductName - keep in sync - keep price info out of it

        public string PromotionDdlListing
        {
            get
            {
                return string.Format("{0}({1}){2}", (!this.IsActive) ? "(na)" : string.Empty, this.Available, this.DisplayNameWithAttribs);
            }
        }
        public string DisplayNameWithAttribsLeadingId
        {
            get
            {
                return string.Format("{0} - {1}", this.Id.ToString(), this.DisplayNameWithAttribs);
            }
        }
        public string DisplayNameWithAttribs
        {
            get
            {
                if (this.IsGiftCertificateDelivery)
                    return string.Format("{0}{1}", (AttribChoice.Trim().Length > 0) ? string.Format("{0} ", this.AttribChoice) : string.Empty, this.DisplayName).Trim();
                else
                    return string.Format("{0} {1}", this.DisplayName, (AttribChoice.Trim().Length > 0) ? string.Format("- {0}", this.AttribChoice) : string.Empty).Trim();
            }
        }

        public string DisplayName
        {
            get
            {
                string display = string.Empty;

                //children append parent name - parents are required to have name
                if (IsChild && this.Name != null && this.Name.Trim().Length > 0)
                    display = string.Format("{0} ", this.Name.Trim());
                if (IsChild && this.ParentMerchRecord.DisplayName.Length > 0)//is already trimmed
                    display += string.Format("{0} ", this.ParentMerchRecord.DisplayName);
                else if (IsParent && Name != null && Name.Trim().Length > 0)
                    display += string.Format("{0} ", this.Name.Trim());

                return display.Trim();

            }
        }
        private string _seoName = null;
        public string SeoName
        {
            get
            {
                if (_seoName == null)
                {
                    string divCatPart = string .Empty;

                    MerchJoinCatCollection coll1 = new MerchJoinCatCollection();
                    coll1.AddRange(this.MerchJoinCatRecords());
                    foreach (MerchJoinCat mjc in coll1)
                    {
                        string divPart = Utils.ParseHelper.SeoFormat(mjc.MerchCategorieRecord.MerchDivisionRecord.Name);
                        string catPart = Utils.ParseHelper.SeoFormat(mjc.MerchCategorieName);

                        string sandwich = string.Format("{0}{1}{2}", 
                            (divPart.Trim().Length > 0) ? divPart.Trim() : string.Empty,
                            (divPart.Trim().Length > 0 && catPart.Trim().Length > 0) ? "-" : string.Empty,
                            (catPart.Trim().Length > 0) ? catPart.Trim() : string.Empty
                            );

                        if (sandwich.Trim().Length > 0)
                            divCatPart += string.Format("merchcat-{0} ", sandwich);
                    }

                    string namePart = Utils.ParseHelper.SeoFormat(this.DisplayName);

                    if (divCatPart.Trim().Length > 0)
                        _seoName = divCatPart.Trim();

                    _seoName += string.Format("{0}{1}", 
                        (divCatPart.Trim().Length > 0 && namePart.Trim().Length > 0) ? " " : string.Empty,
                        namePart.Trim()
                        );

                    if (_seoName.Trim().Length == 0)
                        _seoName = string.Empty;
                }

                return _seoName;
            }
        }

        public string AttribChoice
        {
            get
            {
                //only return a price for the inventory
                if (this.IsGiftCertificateDelivery)
                    return (IsChild) ? Price.ToString("c") : string.Empty;

                string attrib = string.Empty;

                if (this.Style != null && Style.Trim().Length > 0)
                    attrib = string.Format("Style({0}) ", this.Style.Trim());
                if (this.Color != null && Color.Trim().Length > 0)
                    attrib += string.Format("Color({0}) ", this.Color.Trim());
                if (this.Size != null && Size.Trim().Length > 0)
                    attrib += string.Format("Size({0}) ", this.Size.Trim());

                return attrib;
            }
        }


        /// <summary>
        /// indicates if the item is a child/inventory item or if it is a parent item with one active child - so a parent acting as an inventory item
        /// </summary>
        //public bool IsEndAllItem { get { return this.IsChild || (this.IsParent && this.ChildMerchRecords_Active.Count == 1); } }
       
        private List<string> _childSizeList = new List<string>();
        private List<string> _childColorList = new List<string>();

        public MerchCollection ChildMerchRecords_Active
        {
            get
            {
                MerchCollection coll = new MerchCollection();
                if(this.ChildMerchRecords().Count > 0)
                    coll.AddRange(this.ChildMerchRecords().GetList().FindAll(delegate(Merch match) { return (match.IsActive); }));
                return coll;
            }
        }

        public void ResetChildStyles() { _childStyleList = new List<string>(); }
        public void ResetChildColors() { _childColorList = new List<string>(); }
        public void ResetChildSizes() { _childSizeList = new List<string>(); }

        public bool HasChildStyles { get { return ChildStyleList.Count > 0; } }
        public bool HasChildSizes { get { return ChildSizeList.Count > 0; } }
        public bool HasChildColors { get { return ChildColorList.Count > 0; } }

        private string Style_Derived { get { if (this.Style == null) { return string.Empty; } else { return this.Style.Trim(); } } }
        private string Color_Derived { get { if (this.Color == null) { return string.Empty; } else { return this.Color.Trim(); } } }
        private string Size_Derived { get { if (this.Size == null) { return string.Empty; } else { return this.Size.Trim(); } } }

        public Merch FindChildItem(string style, string color, string size)
        {
            //do not check for having properties - keep this method generic
            if (IsParent)
            {
                if(style == null) style = string.Empty;
                if(color == null) color = string.Empty;
                if(size == null) size = string.Empty;

                //two ways to do this
                //foreach (Merch chil in this.ChildMerchRecords())
                //{
                //    if (chil.Style_Derived.ToLower() == style.ToLower() && 
                //        chil.Color_Derived == color.ToLower() && 
                //        chil.Size_Derived.ToLower() == size.ToLower())
                //        return chil;
                //}

                Merch child = (Merch)this.ChildMerchRecords().GetList().Find(
                    delegate (Merch match) 
                    { return (
                        _Config.APPLICATION_ID == match.ApplicationId && 
                        style.ToLower() == match.Style_Derived.ToLower() &&
                        color.ToLower() == match.Color_Derived.ToLower() &&
                        size.ToLower() == match.Size_Derived.ToLower()); } );

                if (child != null && child.Id > 0) return child;
            }

            return null;
        }

        public bool HasChildStyle(string style)
        {
            if (IsParent && HasChildStyles)
            {
                string s = ChildStyleList.Find(delegate(string match) { return (match.Trim().ToLower() == style.Trim().ToLower()); });
                if (s != null && s.Length > 0)
                    return true;
            }
            return false;
        }
        public bool HasChildColor(string color)
        {
            if (IsParent && HasChildColors)
            {
                string s = ChildColorList.Find(delegate(string match) { return (match.Trim().ToLower() == color.Trim().ToLower()); });
                if (s != null && s.Length > 0)
                    return true;
            }
            return false;
        }
        public bool HasChildSize(string size)
        {
            if (IsParent && HasChildSizes)
            {
                string s = ChildSizeList.Find(delegate(string match) { return (match.Trim().ToLower() == size.Trim().ToLower()); });
                if (s != null && s.Length > 0)
                    return true;
            }
            return false;
        }
        
        public bool IsOfStyle(string style)
        {
            if(!IsParent)
                if(this.Style.Trim().ToLower() == style.Trim().ToLower())
                    return true;

            return false;
        }
        public bool IsOfColor(string color)
        {
            if (!IsParent)
                if (this.Color.Trim().ToLower() == color.Trim().ToLower())
                    return true;

            return false;
        }
        public bool IsOfSize(string size)
        {
            if (!IsParent)
                if (this.Size.Trim().ToLower() == size.Trim().ToLower())
                    return true;

            return false;
        }
        public bool IsGiftCertificateDelivery { get { return this.DeliveryType == _Enums.DeliveryType.giftcertificate; } }
        public bool IsParcelDelivery { get { return this.DeliveryType == _Enums.DeliveryType.parcel; } }
        /// <summary>
        /// this indicates if we need to break down multiple quantities into a single line item. Because a code is provided for delivery, each item must list separate.
        /// Gift certificates are not included here as different rules apply when added to the cart.
        /// </summary>
        public bool IsRequiresSeparateListing { get { return (this.IsActivationCodeDelivery || this.IsDownloadDelivery); } }
        public bool IsDownloadDelivery { get { return this.DeliveryType == _Enums.DeliveryType.download; } }
        public bool IsActivationCodeDelivery { get { return this.DeliveryType == _Enums.DeliveryType.activationcode; } }

        
        private List<string> _childStyleList = new List<string>();
        public List<string> ChildStyleList
        {
            get
            {
                if (IsParent)
                {
                    foreach (Merch child in this.ChildMerchRecords_Active)
                    {
                        string style = child.Style;
                        if (style != null && style.Trim().Length > 0 && (!_childStyleList.Contains(style)))
                            _childStyleList.Add(style);
                    }
                }

                if (_childStyleList.Count > 1)
                {
                    if (this.IsGiftCertificateDelivery)
                    {
                        //we need to transform the values into decimals and return that sort

                        //or compare as though they were decimal values
                        _childStyleList.Sort(delegate(string left, string right)
                        {
                            decimal dl = decimal.Parse(left.Replace("$", string.Empty));
                            decimal dr = decimal.Parse(right.Replace("$", string.Empty));
                            return dl.CompareTo(dr);
                        });
                   
                    }
                    else
                        _childStyleList.Sort();
                }
                return _childStyleList;
            }
        }
        private List<string> _childStyleList_All = new List<string>();
        public List<string> ChildStyleList_All
        {
            get
            {
                if (IsParent)
                {
                    foreach (Merch child in this.ChildMerchRecords())
                    {
                        string style = child.Style;
                        if (style != null && style.Trim().Length > 0 && (!_childStyleList_All.Contains(style)))
                            _childStyleList_All.Add(style);
                    }
                }

                if (_childStyleList_All.Count > 1)
                {
                    if (this.IsGiftCertificateDelivery)
                    {
                        //we need to transform the values into decimals and return that sort

                        //or compare as though they were decimal values
                        _childStyleList_All.Sort(delegate(string left, string right)
                        {
                            decimal dl = decimal.Parse(left.Replace("$", string.Empty));
                            decimal dr = decimal.Parse(right.Replace("$", string.Empty));
                            return dl.CompareTo(dr);
                        });

                    }
                    else
                        _childStyleList_All.Sort();
                }
                return _childStyleList_All;
            }
        }
        
        public List<string> ChildColorList
        {
            get
            {
                if (IsParent)
                {   
                    foreach (Merch child in this.ChildMerchRecords_Active)
                    {
                        string color = child.Color;
                        if (color != null && color.Trim().Length > 0 && (!_childColorList.Contains(color)))
                            _childColorList.Add(color);
                    }
                }

                if (_childColorList.Count > 1)
                    _childColorList.Sort();
                return _childColorList;
            }
        }
        public List<string> ChildColorList_All
        {
            get
            {
                if (IsParent)
                {
                    foreach (Merch child in this.ChildMerchRecords())
                    {
                        string color = child.Color;
                        if (color != null && color.Trim().Length > 0 && (!_childColorList.Contains(color)))
                            _childColorList.Add(color);
                    }
                }

                if (_childColorList.Count > 1)
                    _childColorList.Sort();
                return _childColorList;
            }
        }
        
        public List<string> ChildSizeList
        {
            get
            {
                List<ListItem> list = new List<ListItem>();

                if (IsParent)
                {
                    foreach (Merch child in this.ChildMerchRecords_Active)
                    {
                        if (child.Size != null && child.Size.Trim().Length > 0)
                        {
                            string size = _childSizeList.Find(
                               delegate(string match) { return (match.ToLower() == child.Size.ToLower()); });

                            if (size == null || size.Trim().Length == 0)
                            {
                                size = child.Size.Trim().ToLower();

                                MerchSize mSize = (MerchSize)_Lookits.MerchSizes.GetList().Find(
                                    delegate(MerchSize match) { return (match.Name.Trim().ToLower() == size || 
                                        match.Code.Trim().ToLower() == size); });

                                int ordinal = (mSize != null) ? mSize.DisplayOrder : 0;

                                list.Add(new ListItem(child.Size, ordinal.ToString()));
                            }   
                        }
                    }
                }

                if (list.Count > 1)
                    list.Sort(new Utils.Reflector.CompareEntities<ListItem>(Utils.Reflector.Direction.Ascending, "Value"));

                foreach(ListItem li in list)
                    _childSizeList.Add(li.Text);

                return _childSizeList;
            }
        }
        public List<string> ChildSizeList_All
        {
            get
            {
                List<ListItem> list = new List<ListItem>();

                if (IsParent)
                {
                    foreach (Merch child in this.ChildMerchRecords())
                    {
                        if (child.Size != null && child.Size.Trim().Length > 0)
                        {
                            string size = _childSizeList.Find(
                               delegate(string match) { return (match.ToLower() == child.Size.ToLower()); });

                            if (size == null || size.Trim().Length == 0)
                            {
                                size = child.Size.Trim().ToLower();

                                MerchSize mSize = (MerchSize)_Lookits.MerchSizes.GetList().Find(
                                    delegate(MerchSize match)
                                    {
                                        return (match.Name.Trim().ToLower() == size ||
                                            match.Code.Trim().ToLower() == size);
                                    });

                                int ordinal = (mSize != null) ? mSize.DisplayOrder : 0;

                                list.Add(new ListItem(child.Size, ordinal.ToString()));
                            }
                        }
                    }
                }

                if (list.Count > 1)
                    list.Sort(new Utils.Reflector.CompareEntities<ListItem>(Utils.Reflector.Direction.Ascending, "Value"));

                foreach (ListItem li in list)
                    _childSizeList.Add(li.Text);

                return _childSizeList;
            }
        }
        #endregion

        #region Properties
        public bool IsParent 
        { 
            get 
            { 
                return (!this.IsChild); 
            } 
        }
        public bool IsChild { get { return (this.TParentListing.HasValue); } }
        public bool IsActive 
        {
            get { return this.BActive; }
            set { this.BActive = value;  }
        }
        /// <summary>
        /// ensures merch is announced and active - see other IsDisplayable properties (web and ticketing)
        /// </summary>
        public bool IsDisplayable
        {
            get { return this.IsActive && this.PublicOnsaleDate < DateTime.Now && this.EndDate > DateTime.Now; }
        }
        public bool IsInternalOnly
        {
            get { return this.BInternalOnly; }
            set { this.BInternalOnly = value; }
        }
        public bool IsTaxable
        {
            get { return this.BTaxable; }
            set { this.BTaxable = value; }
        }
        ///
        ///Do not inherit from parent
        ///
        public bool IsFeaturedItem
        {
            get { return this.BFeaturedItem; }
            set { this.BFeaturedItem = value; }
        }
        public bool IsSoldOut
        {
            get { if (!this.BSoldOut.HasValue) return (IsChild) ? this.ParentMerchRecord.IsSoldOut : false; return this.BSoldOut.Value; }
            set { this.BSoldOut = value; }
        }
        ///do not inherit from parent
        public bool IsUnlockActive
        {
            get { return this.BUnlockActive; }
            set { this.BUnlockActive = value; }
        }
        ///do not inherit from parent
        public DateTime UnlockDate
        {
            get { return (!this.DtUnlockDate.HasValue) ? Utils.Constants._MinDate : this.DtUnlockDate.Value; }
            set { this.DtUnlockDate = value; }
        }
        ///do not inherit from parent
        public DateTime UnlockEndDate
        {
            get { return (!this.DtUnlockEndDate.HasValue) ? DateTime.MaxValue : this.DtUnlockEndDate.Value; }
            set { this.DtUnlockEndDate = value; }
        }
               
        public DateTime PublicOnsaleDate
        {
            get { return (!this.DtStartDate.HasValue) ? (IsChild) ? this.ParentMerchRecord.PublicOnsaleDate : Utils.Constants._MinDate : this.DtStartDate.Value; }
            set { this.DtStartDate = value; }
        }
        public DateTime EndDate
        {
            get { return (!this.DtEndDate.HasValue) ? (IsChild) ? this.ParentMerchRecord.EndDate : DateTime.MaxValue : this.DtEndDate.Value; }
            set { this.DtEndDate = value; }
        }
        public decimal Price
        {
            get { return (!this.MPrice.HasValue) ? (IsChild) ? this.ParentMerchRecord.Price : 0 : decimal.Round(this.MPrice.Value, 2); }
            set
            {
                this.MPrice = value;
            }
        }

        public string PriceListing
        {
            get
            {
                string _priceListing = string.Empty;

                if (this.IsChild)
                    _priceListing = string.Format("${0}", this.Price.ToString("n2"));
                else
                {
                    List<decimal> prices = new List<decimal>();
                    foreach (Merch child in this.ChildMerchRecords_Active)
                    {
                        decimal childPrice = child.Price;
                        if (!prices.Contains(childPrice))
                            prices.Add(childPrice);
                    }

                    if (prices.Count > 1)
                    {
                        prices.Sort();
                        _priceListing = string.Format("${0} - {1}", prices[0].ToString("n2"), prices[prices.Count - 1].ToString("n2"));
                    }
                    else if (prices.Count == 1)
                        _priceListing = string.Format("${0}", prices[0].ToString("n2"));
                }

                return _priceListing;
            }
        }

        public bool PriceHasMultipleLevelsInInventory
        {
            get
            {
                return PriceListing.IndexOf("- ") != -1;
            }
        }

        /// <summary>
        /// determines what price is if a sale price exists etc etc
        /// </summary>
        public decimal Price_Effective
        {
            get
            {
                if (UseSalePrice)
                    return this.SalePrice;
                else
                    return (!this.MPrice.HasValue) ? (IsChild) ? this.ParentMerchRecord.Price : 0 : decimal.Round(this.MPrice.Value, 2);
            }
        }
        public bool UseSalePrice
        {
            get { return (!this.BUseSalePrice.HasValue) ? (IsChild) ? this.ParentMerchRecord.UseSalePrice : false : this.BUseSalePrice.Value; }
            set
            {
                this.BUseSalePrice = value;
            }
        }
        public decimal SalePrice
        {
            get { return (!this.MSalePrice.HasValue) ? ((IsChild) ? this.ParentMerchRecord.SalePrice : 0) : decimal.Round(this.MSalePrice.Value, 2); }
            set
            {
                this.MSalePrice = value;
            }
        }
        public decimal SalePriceSavings
        {
            get { return this.Price - this.Price_Effective; }
        }
        public _Enums.DeliveryType DeliveryType
        {
            get
            {
                if (this.VcDeliveryType == null)
                {
                    if(this.IsParent)
                        return _Enums.DeliveryType.parcel;
                    else
                        return this.ParentMerchRecord.DeliveryType;
                }
                return (_Enums.DeliveryType)Enum.Parse(typeof(_Enums.DeliveryType), this.VcDeliveryType, true);
            }
            set { this.VcDeliveryType = value.ToString(); }
        }
        public decimal Weight
        {
            get { return (!this.MWeight.HasValue) ? (IsChild) ? this.ParentMerchRecord.Weight : 0 : decimal.Round(this.MWeight.Value, 2); }
            set { this.MWeight = value; }
        }
        public int MaxQuantityPerOrder
        {
            get { return this.IMaxQtyPerOrder; }
            set { this.IMaxQtyPerOrder = value; }
        }
        ///do not inherit from parent
        public int Allotment
        {
            get {
                if (IsParent)
                    return this.AllotedChildren;
    
                return this.IAllotment; }
            set 
            {
                this.IAllotment = value;
            }
        }
        ///do not inherit from parent
        public int Damaged
        {
            get
            {
                if (IsParent)
                    return this.DamagedChildren;

                return this.IDamaged;
            }
            set { this.IDamaged = value; }
        }
        //do not inherit from parent
        public int Pending
        {
            get
            {
                //if (IsParent)
                //    return this.PendingChildren;

                return this.IPending;
            }
            set { this.IPending = value; }
        }
        ///do not inherit from parent
        public int Sold
        {
            get {
                if (IsParent)
                    return this.SoldChildren; 
                
                return this.ISold;
            }
            set { this.ISold = value; }
        }
        ///do not inherit from parent
        public int Available
        {
            get {
                if (IsParent)
                    return this.AvailableChildren;
                
                return Allotment-Damaged-Sold; 
            }
        }
        ///do not inherit from parent
        public int Refunded
        {
            get {
                if (IsParent)
                    return this.RefundedChildren; 
                
                return this.IRefunded;
            }
            set { this.IRefunded = value; }
        }
        #endregion

        #region Children & Total Inventory

        private int AllotedChildren
        {
            get
            {
                int total = 0;
                foreach (Merch child in ChildMerchRecords_Active)
                    total += child.Allotment;
                return total;
            }
        }
        private int DamagedChildren
        {
            get
            {
                int total = 0;
                foreach (Merch child in ChildMerchRecords_Active)
                    total += child.Damaged;
                return total;
            }
        }
        //private int PendingChildren
        //{
        //    get
        //    {
        //        int total = 0;
        //        foreach (Merch child in ChildMerchRecords_Active)
        //            total += child.Pending;
        //        return total;
        //    }
        //}
        public int SoldChildren
        {
            get
            {
                int total = 0;
                foreach (Merch child in ChildMerchRecords_Active)
                    total += child.Sold;
                return total;
            }
        }
        public int AvailableChildren
        {
            get
            {
                int total = 0;
                
                foreach (Merch child in ChildMerchRecords_Active)
                    total += child.Available;

                return total;
            }
        }

        /// <summary>
        /// Check the items inventory based on criteria: 
        /// 1) item is active and running, 2) is not backordered, 3) is not sold out, 4) has available inventory.
        /// This DOES NOT check for unlock codes!!!!
        /// </summary>
        public MerchCollection AvailableInventory
        {
            get
            {
                MerchCollection coll = new MerchCollection();

                if (this.IsParent)
                {
                    coll.AddRange(this.ChildMerchRecords().GetList()
                        .FindAll(delegate(Merch match) { return ((match.IsDisplayable) && (!match.IsBackordered) && (!this.IsSoldOut) && match.Available > 0); }));
                }
                else if (this.IsDisplayable && (!this.IsBackordered) && (!this.IsSoldOut) && this.Available > 0)
                    coll.Add(this);


                return coll;
            }
        }

        #region Merch Bundles
        
        ///We do not want to do this as we have no way of caching it reliably in the order flow
        ///this is done at the saleitem_merchandise
        //public MerchBundleCollection GetAvailableBundles(string unlock, List<string> coupons)
        //{

        #endregion

        private int RefundedChildren
        {
            get
            {
                int total = 0;
                foreach (Merch child in ChildMerchRecords_Active)
                    total += child.Refunded;
                return total;
            }
        }
        #endregion

        #region Categories And Divisions
        public bool IsInCategorie(MerchCategorie cat)
        {
            foreach (MerchJoinCat merchCat in this.MerchJoinCatRecords())
            {
                if (merchCat.MerchCategorieRecord == cat)
                    return true;
            }
            return false;
        }
        public bool IsInCategorie(int merchCategorieId)
        {
            foreach (MerchJoinCat merchCat in this.MerchJoinCatRecords())
            {
                if (merchCat.MerchCategorieRecord.Id == merchCategorieId)
                    return true;
            }
            return false;
        }
        public MerchJoinCatCollection MerchJoinCats
        {
            get
            {
                if (this.MerchJoinCatRecords().Count == 0 && IsChild)
                    return this.ParentMerchRecord.MerchJoinCatRecords();

                return this.MerchJoinCatRecords();
            }
        }
        //returns a distinct list of the 
        public MerchDivisionCollection MerchDivisionCollection
        {
            get
            {
                MerchDivisionCollection coll = new MerchDivisionCollection();

                if (this.MerchJoinCats.Count == 0)
                    return coll;

                foreach (MerchJoinCat cat in this.MerchJoinCats)
                {
                    //find a merch division in the collection with the matching division
                    MerchDivision entry = coll.GetList().Find(
                        delegate(MerchDivision match) { return (match.Id == cat.MerchCategorieRecord.TMerchDivisionId); });

                    if (entry == null)
                        coll.Add((MerchDivision)_Lookits.MerchDivisions.Find(cat.MerchCategorieRecord.TMerchDivisionId));
                }

                return coll;
            }
        }
        #endregion

        #region Misc
        public String Description_Derived
        {
            get
            {
                if (this.Description == null)
                    return (IsChild) ? this.ParentMerchRecord.Description_Derived : null;
                return (this.Description != null) ? this.Description.Trim() : null;
            }
        }

        #endregion

        public static Merch _wcFindById(MerchCollection coll, int idx)
        {
            return (Merch)coll.Find(idx);
        }
    }
}
