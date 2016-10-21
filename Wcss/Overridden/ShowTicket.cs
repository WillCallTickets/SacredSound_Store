using System;
using System.Data;
using System.Xml.Serialization;
using System.Collections.Generic;
using SubSonic;

namespace Wcss
{
    public partial class ShowTicketCollection : Utils._Collection.IOrderable<ShowTicket>
    {
        /// <summary>
        /// Added to comply with interface - show tickets have their own methods in the editor page
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public ShowTicket AddToCollection(System.Collections.Generic.List<System.Web.UI.Pair> args)
        {
            return Utils._Collection.AddItemToOrderedCollection(this.GetList(), args);
        }

        /// <summary>
        /// Delete a ShowTicket from the collection by ID
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public bool DeleteFromCollection(int idx)
        {
            ShowTicket entity = (ShowTicket)this.Find(idx);

            if (entity != null)
            {
                //if we have sold any tickets for this - we cannot delete
                //suggest makinhg non active instead
                if (entity.InvoiceItemRecords().Count > 0)
                    throw new Exception("These tickets are linked to sales and cannot be deleted. As an alternative, you may mark the ticket as inactive.");

                if (this.Count > 1)
                    this.Sort("IDisplayOrder", true);

                CleanupPackages(entity);
                CleanupDosRelations(entity);
                CleanupPastPurchaseRequirements(idx);
                CleanupPostPurchaseTexts(idx);
            }

            return Utils._Collection.DeleteFromOrderedCollection(this.GetList(), idx);
        }
        /// <summary>
        /// get rid of any required show tickets and relations
        /// </summary>
        /// <param name="entity"></param>
        private void CleanupPostPurchaseTexts(int idx)
        {
            //find any required_showticket_pastPurchase where tshowticketid = idx
            //then remove matching required
            //then remove required_showticket_pastPurchase
            //string sql = "DELETE FROM [PostPurchaseText] WHERE [tShowTicketId] = @idx; ";

            //SubSonic.QueryCommand cmd = new QueryCommand(sql, SubSonic.DataService.Provider.Name);
            //cmd.Parameters.Add("@idx", idx, DbType.Int32);

            //try
            //{
            //    SubSonic.DataService.ExecuteQuery(cmd);
            //}
            //catch (Exception ex)
            //{
            //    _Error.LogException(ex);
            //    _Error.SendAdministrativeEmail(ex.Message);
            //}
        }
        /// <summary>
        /// get rid of any required show tickets and relations
        /// </summary>
        /// <param name="entity"></param>
        private void CleanupPastPurchaseRequirements(int idx)
        {
            //find any required_showticket_pastPurchase where tshowticketid = idx
            //then remove matching required
            //then remove required_showticket_pastPurchase
            string sql = "DECLARE @reqId int; SELECT @reqId = [TRequiredId] FROM [Required_ShowTicket_PastPurchase] WHERE [tShowTicketId] = @idx; ";
            sql += "IF (@reqId > 0) BEGIN DELETE FROM [Required] WHERE [Id] = @reqId END ";//this will cascade delete the required_showticket_pastPurchase

            SubSonic.QueryCommand cmd = new QueryCommand(sql, SubSonic.DataService.Provider.Name);
            cmd.Parameters.Add("@idx", idx, DbType.Int32);

            try
            {
                SubSonic.DataService.ExecuteQuery(cmd);
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
                _Error.SendAdministrativeEmail(ex.Message);
            }
        }
        /// <summary>
        /// provides logic for properly deleting related items
        /// </summary>
        private void CleanupDosRelations(ShowTicket entity)
        {
            int idx = 0;

            //parents need to clean up DOS tix
            if (!entity.IsDosTicket && entity.DosShowTicketRecord != null)//if it is a parent
            {
                if (entity.DosShowTicketRecord.InvoiceItemRecords().Count > 0)
                    throw new Exception("This ticket has a DOS ticket that is linked to sales and cannot be deleted. As an alternative, you may mark the DOS ticket as inactive.");

                //Parent joins with ShowTicketDosTicketRecords
                idx = entity.ShowTicketDosTicketRecords()[0].Id;

                ShowTicketDosTicket.Delete(idx);

                Utils._Collection.DeleteFromOrderedCollection(this.GetList(), entity.DosShowTicketRecord.Id);

                CleanupPastPurchaseRequirements(entity.DosShowTicketRecord.Id);
            }
            else if (entity.IsDosTicket && entity.ParentShowTicketRecord != null)//its a DOS ticket and we can ignore dealing with any other objects besides it's join records
            {
                //DOS joins with ShowTicketDosTicketRecordsFromShowTicket
                //cleans up the parent relation
                idx = entity.ShowTicketDosTicketRecordsFromShowTicket()[0].Id;
                ShowTicketDosTicket.Delete(idx);
            }
        }
        /// <summary>
        /// provides logic for properly deleting related items
        /// </summary>
        private void CleanupPackages(ShowTicket entity)
        {
            //for each linked ticket - delete the join record - then delete the liniked ticket
            ShowTicketPackageLinkCollection links = new ShowTicketPackageLinkCollection();
            ShowTicketCollection packages = new ShowTicketCollection();
            packages.AddRange(entity.LinkedShowTickets);

            links.AddRange(entity.ShowTicketPackageLinkRecords());

            foreach (ShowTicket st in packages)
                links.AddRange(st.ShowTicketPackageLinkRecords());

            try
            {
                foreach (ShowTicketPackageLink stpl in links)
                    ShowTicketPackageLink.Delete(stpl.Id);
            }
            catch (Exception e)
            {
                _Error.LogException(e);
                throw e;
            }

            try
            {
                foreach (ShowTicket st in packages)
                {
                    ShowTicket.Delete(st.Id);

                    //fix the display order in the linked show date's ticket list
                    foreach (ShowTicket ent in st.ShowDateRecord.ShowTicketRecords())
                    {
                        if (ent.DisplayOrder > st.DisplayOrder)
                        {
                            ent.DisplayOrder -= 1;
                            ent.Save_DisplayOrderOnly();
                        }
                    }

                    //remove the ticket from the show date
                    st.ShowDateRecord.ShowTicketRecords().Remove(st);
                }
            }
            catch (Exception e)
            {
                _Error.LogException(e);
                throw e;
            }
        }

        /// <summary>
        /// Does not use theUtils._Collection method
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public ShowTicket ReorderItem(int idx, string direction)
        {
            //find item
            ShowTicket item = (ShowTicket)this.Find(idx);

            if (item != null)
            {
                if (direction.ToLower().Equals("up") && item.DisplayOrder > 0)
                {
                    //find the next one up
                    ShowTicket previous = (ShowTicket)this.GetList().Find(
                        delegate(ShowTicket match) { return (match.DisplayOrder == item.DisplayOrder - 1); });
                    if (previous != null)
                    {
                        previous.DisplayOrder = item.DisplayOrder;
                        int save = item.DisplayOrder - 1;
                        item.DisplayOrder = -1;
                        item.Save_DisplayOrderOnly();
                        previous.Save_DisplayOrderOnly();
                        item.DisplayOrder = save;
                        item.Save_DisplayOrderOnly();
                    }
                }
                else if (direction.ToLower().Equals("down") && item.DisplayOrder != (this.Count - 1))//count is 1 based
                {
                    ShowTicket next = (ShowTicket)this.GetList().Find(
                        delegate(ShowTicket match) { return (match.DisplayOrder == item.DisplayOrder + 1); });
                    if (next != null)
                    {
                        next.DisplayOrder = item.DisplayOrder;
                        int save = item.DisplayOrder + 1;
                        item.DisplayOrder = -1;
                        item.Save_DisplayOrderOnly();
                        next.Save_DisplayOrderOnly();
                        item.DisplayOrder = save;
                        item.Save_DisplayOrderOnly();
                    }
                }
            }

            return item;
        }
    }

    public partial class ShowTicket
    {
        #region Properties

        private string _isLottery = null;
        [XmlAttribute("IsLotteryTicket")]
        public bool IsLotteryTicket
        {
            get { if (this._isLottery == null) _isLottery = ((bool)(this.LotteryRecords().Count > 0)).ToString(); return bool.Parse(_isLottery); }
        }
        [XmlAttribute("IsActive")]
        public bool IsActive
        {
            get { return this.BActive; }
            set { this.BActive = value; }
        }
        [XmlAttribute("IsSoldOut")]
        public bool IsSoldOut
        {
            get { return this.BSoldOut; }
            set { this.BSoldOut = value; }
        }

        [XmlAttribute("IsUnlockActive")]
        public bool IsUnlockActive
        {
            get { return this.BUnlockActive; }
            set { this.BUnlockActive = value; }
        }
        [XmlAttribute("IsOverrideSellout")]
        public bool IsOverrideSellout
        {
            get { return this.BOverrideSellout; }
            set { this.BOverrideSellout = value; }
        }
        [XmlAttribute("IsFlatShip")]
        public bool IsFlatShip { get { return this.FlatShip > 0; } }
        [XmlAttribute("FlatShip")]
        public decimal FlatShip
        {
            get { return (!this.MFlatShip.HasValue) ? 0 : this.MFlatShip.Value; }
            set { this.MFlatShip = value; }
        }
        /// <summary>
        /// Flat Method also tracks to see if we have fully transferred DOS tickets
        /// </summary>
        [XmlAttribute("FlatMethod")]
        public string FlatMethod
        {
            get { return (this.VcFlatMethod == null) ? string.Empty : this.VcFlatMethod; }
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
            get { return (Wcss._Shipper.CalculateShipDate(this.BackorderDate) > Wcss._Shipper.CalculateShipDate(DateTime.Now)); }
        }
        [XmlAttribute("SpecialInstructions")]
        public string SpecialInstructions
        {
            get
            {
                if (this.IsBackordered && this.IsFlatShip)
                    return string.Format("This ticket ships separately at a flat rate of {0}. This ticket will not shipped until {1}.",
                        this.FlatShip.ToString("c"), Wcss._Shipper.CalculateShipDate(this.BackorderDate).ToString("MM/dd/yyyy"));
                else if (this.IsBackordered && this.IsShipSeparate)
                    return string.Format("This ticket must ship separately. This ticket will not be shipped until {0}.",
                        Wcss._Shipper.CalculateShipDate(this.BackorderDate).ToString("MM/dd/yyyy"));
                else if (this.IsBackordered)
                    return string.Format("This ticket will not be shipped until {0}.",
                        Wcss._Shipper.CalculateShipDate(this.BackorderDate).ToString("MM/dd/yyyy"));
                else if (this.IsFlatShip)
                    return string.Format("This ticket ships separately at a flat rate of {0}.", this.FlatShip.ToString("c"));
                else if (this.IsShipSeparate)
                    return string.Format("This ticket must ship separately.");

                return null;
            }
        }
        
        [XmlAttribute("TicketDateList")]
        public List<DateTime> TicketDateList
        {
            get
            {
                List<DateTime> list = new List<DateTime>();

                if (this.IsPackage)
                {
                    ShowTicketPackageLinkCollection linkColl = new ShowTicketPackageLinkCollection();
                    linkColl.CopyFrom(this.ShowTicketPackageLinkRecords());

                    ShowTicketCollection coll = new ShowTicketCollection();
                    foreach (ShowTicketPackageLink link in linkColl)
                        coll.Add(link.ShowTicketToLinkedShowTicketIdRecord);

                    if (!coll.Contains(this))
                        coll.Add(this);

                    if (coll.Count > 1)
                        coll.Sort("DtDateOfShow", true);

                    foreach (ShowTicket st in coll)
                        list.Add(st.DateOfShow);
                }
                else
                    list.Add(this.DateOfShow);

                return list;
            }
        }
        private List<string> _ticketDateNameList = null;
        [XmlAttribute("TicketDateNameList")]
        public List<string> TicketDateNameList
        {
            get
            {
                if (_ticketDateNameList == null)
                {
                    _ticketDateNameList = new List<string>();

                    if (this.IsPackage)
                    {
                        ShowTicketPackageLinkCollection linkColl = new ShowTicketPackageLinkCollection();
                        linkColl.CopyFrom(this.ShowTicketPackageLinkRecords());

                        ShowTicketCollection coll = new ShowTicketCollection();
                        foreach (ShowTicketPackageLink link in linkColl)
                            coll.Add(link.ShowTicketToLinkedShowTicketIdRecord);

                        if (!coll.Contains(this))
                            coll.Add(this);

                        if (coll.Count > 1)
                            coll.SortBy_DateToOrderBy();//.Sort("DtDateOfShow", true);

                        foreach (ShowTicket st in coll)
                            _ticketDateNameList.Add(string.Format("{0} {1}", st.DateOfShow.ToString("MM/dd/yyyy hh:mmtt"), st.ShowDateRecord.ShowRecord.ShowNamePart));
                    }
                    else
                        _ticketDateNameList.Add(string.Format("{0} {1}", this.DateOfShow.ToString("MM/dd/yyyy hh:mmtt"), this.ShowDateRecord.ShowRecord.ShowNamePart));

                }

                return _ticketDateNameList;
            }
        }
        [XmlAttribute("ShowSpanDateStringCondensed")]
        public string ShowSpanDateStringCondensed
        {
            get
            {
                return Utils.ParseHelper.ConvertTildesToCommasAndAmpersands(TicketDateList, "M/d/yy h:mmtt");
            }
        }
        [XmlAttribute("ShowSpanDateString")]
        public string ShowSpanDateString
        {
            get
            {
                return Utils.ParseHelper.ConvertTildesToCommasAndAmpersands(TicketDateList, "MM/dd/yyyy hh:mmtt");
            }
        }

        [XmlAttribute("DescriptionAndDerived")]
        private string DescriptionAndDerived { get { return string.Format("{0} {1}", this.SalesDescription_Derived, this.CriteriaText_Derived).Trim(); } }

        /// <summary>
        /// sorry it's a hack - get over it
        /// </summary>
        /// <param name="desc"></param>
        /// <returns></returns>
        public static bool IsCampingPass(string desc)
        {
            return (desc.Length > 0 && desc.IndexOf("Camping pass", StringComparison.OrdinalIgnoreCase) != -1);
        }
        public bool IsCampingPass()
        {
            return ShowTicket.IsCampingPass(DescriptionAndDerived);
        }


        [XmlAttribute("DdlListing")]
        public string DdlListing
        {
            get
            {
                string desc = DescriptionAndDerived;
                
                //hack for camping passes - passes should be described in the description
                //we leave off the auto date for the camping
                if (IsCampingPass(desc))
                {
                    return Utils.ParseHelper.StripHtmlTags(System.Text.RegularExpressions.Regex.Replace(string.Format("{0} - {1} {2} {3}",
                        this.Id.ToString(), desc, 
                        this.AgeDescription, this.ShowDateRecord.ShowRecord.ShowNamePart.ToUpper()), @"\s+", " ").Trim());
                }

                return Utils.ParseHelper.StripHtmlTags(System.Text.RegularExpressions.Regex.Replace(string.Format("{0} - {1} {2} {3} {4}", 
                    this.Id.ToString(),
                    ShowSpanDateStringCondensed,
                    this.AgeDescription, this.ShowDateRecord.ShowRecord.ShowNamePart.ToUpper(), desc), @"\s+", " ").Trim());
            }
        }
        [XmlAttribute("DisplayNameWithAttribsAndDescription")]
        public string DisplayNameWithAttribsAndDescription
        {
            get
            {
                string desc = DescriptionAndDerived;

                //hack for camping passes - passes should be described in the description
                //we leave off the auto date for the camping
                if (IsCampingPass(desc))
                {
                    return System.Text.RegularExpressions.Regex.Replace(string.Format("{0} {1} {2}",                        
                        desc,
                        this.AgeDescription, this.ShowDateRecord.ShowRecord.ShowNamePart.ToUpper()), @"\s+", " ").Trim();
                }

                return System.Text.RegularExpressions.Regex.Replace(string.Format("{0} {1} {2} {3}",
                    this.DateOfShow.ToString("ddd MM/dd/yyyy hh:mmtt"),
                    this.AgeDescription, this.ShowDateRecord.ShowRecord.ShowNamePart.ToUpper(), desc), @"\s+", " ").Trim();
            }
        }
        [XmlAttribute("DisplayNameWithAttribsAndDesc_PkgDateList")]
        public string DisplayNameWithAttribsAndDesc_PkgDateList
        {
            get
            {
                string desc = DescriptionAndDerived;

                //hack for camping passes - passes should be described in the description
                //we leave off the auto date for the camping
                if (IsCampingPass(desc))
                {
                    return System.Text.RegularExpressions.Regex.Replace(string.Format("{0} {1} {2}",
                        desc,
                        this.AgeDescription, this.ShowDateRecord.ShowRecord.ShowNamePart.ToUpper()), @"\s+", " ").Trim();
                }

                return System.Text.RegularExpressions.Regex.Replace(string.Format("{0} {1} {2} {3}",
                    this.ShowSpanDateString,
                    this.AgeDescription, this.ShowDateRecord.ShowRecord.ShowNamePart.ToUpper(), desc), @"\s+", " ").Trim();
            }
        }
        

        [XmlAttribute("DisplayNamePart")]
        public string DisplayNamePart
        {
            get
            {
                return System.Text.RegularExpressions.Regex.Replace(string.Format("{0} {1}",
                    this.AgeDescription, this.ShowDateRecord.ShowRecord.ShowNamePart.ToUpper()), @"\s+", " ").Trim();
            }
        }

        [XmlAttribute("IsDosTicket")]
        public bool IsDosTicket
        {
            get { return this.BDosTicket; }
            set { this.BDosTicket = value; }
        }
        [XmlAttribute("HideShipMethod")]
        public bool HideShipMethod
        {
            get { return this.BHideShipMethod; }
            set { this.BHideShipMethod = value; }
        }
        [XmlAttribute("DisplayOrder")]
        public int DisplayOrder
        {
            get { return this.IDisplayOrder; }
            set { this.IDisplayOrder = value; }
        }
        [XmlAttribute("DateOfShow")]
        public DateTime DateOfShow
        {
            get { return this.DtDateOfShow; }
            set { this.DtDateOfShow = value; }
        }
        [XmlAttribute("Price")]
        public decimal Price
        {
            get { return (!this.MPrice.HasValue) ? 0 : decimal.Round(this.MPrice.Value, 2); }
            set
            {
                this.MPrice = value;
            }
        }
        /// <summary>
        /// The base price + service charge
        /// </summary>
        [XmlAttribute("PerItemPrice")]
        public decimal PerItemPrice { get { return Price + ServiceCharge; } }
        [XmlAttribute("DosPrice")]
        public decimal DosPrice
        {
            get { return (!this.MDosPrice.HasValue) ? 0 : decimal.Round(this.MDosPrice.Value, 2); }
            set { this.MDosPrice = value; }
        }
        [XmlAttribute("ServiceCharge")]
        public decimal ServiceCharge
        {
            get
            {
                if (!this.MServiceCharge.HasValue)
                    this.MServiceCharge = Wcss.ServiceCharge.ComputeTicketServiceFee(this.Price);

                return decimal.Round(this.MServiceCharge.Value, 2);
            }
            set { this.MServiceCharge = value; }
        }
        /// <summary>
        /// Most likely you should be using this only for allowing shipping. To see if an item is shippable - use "IsCurrentlyShippable"
        /// </summary>
        [XmlAttribute("IsAllowShipping")]
        public bool IsAllowShipping
        {
            get { return this.BAllowShipping; }
            set { this.BAllowShipping = value; }
        }
        [XmlAttribute("IsAllowWillCall")]
        public bool IsAllowWillCall
        {
            get { return this.BAllowWillCall; }
            set { this.BAllowWillCall = value; }
        }
        /// <summary>
        /// To be TRUE, ticket shipping must be active, the ticket must allow shipping and also be within the cutoff date. 
        /// Check for usa eligibility later in the process
        /// For packages. They all must be shippable
        /// </summary>
        [XmlAttribute("IsCurrentlyShippable")]
        public bool IsCurrentlyShippable
        {
            get { return _Config._Shipping_Tickets_Active && this.IsAllowShipping && DateTime.Now < this.ShipCutoffDate && (!this.IsDosTicket); }
        }
        [XmlAttribute("MaxQuantityPerOrder")]
        public int MaxQuantityPerOrder
        {
            get { return (!this.IMaxQtyPerOrder.HasValue) ? _Config._MaxTicketPurchaseQuantity : this.IMaxQtyPerOrder.Value; }
            set { this.IMaxQtyPerOrder = value; }
        }
        [XmlAttribute("Allotment")]//do not inherit from parent
        public int Allotment
        {
            get
            {
                #region check DOS
                //if we are dealing with a DOS ticket...
                if (this.IsDosTicket && _Config.SHOWOFFSETDATE == this.DateOfShow.Date && this.IsActive && 
                    (this.FlatMethod == null || this.FlatMethod != "all transferred"))
                {
                    //bool keepActive = false;
                    int transferred = 0;
                    int pending = 0;
                    int allotment = 0;

                    if (this.ShowTicketDosTicketRecordsFromShowTicket().Count > 0)//make sure it actually exists
                    {
                        ShowTicket parent = this.ShowTicketDosTicketRecordsFromShowTicket()[0].ParentShowTicketRecord;

                        if (parent != null && parent.IsActive && (!parent.IsSoldOut) && parent.LinkedShowTickets.Count == 0)//do not do this for pkg tix
                        {
                            //run a proc to see if any tickets transferred or if there are any pending tickets
                            //also returns the current allotment
                            //note that the "add to cart" method will recheck for availability
                            try
                            {
                                using (System.Data.IDataReader dr = SPs.TxInventoryTransferTicket(parent.Id, this.Id).GetReader())
                                {
                                    while (dr.Read())
                                    {
                                        //note that transferred can be negative if there are pending
                                        transferred = (int)dr.GetValue(dr.GetOrdinal("Transferred"));
                                        pending = (int)dr.GetValue(dr.GetOrdinal("Pending"));
                                        allotment = (int)dr.GetValue(dr.GetOrdinal("Allotment"));
                                    }

                                    dr.Close();
                                }

                                if (transferred == 0 && pending == 0)
                                    this.FlatMethod = "all transferred";
                                else if (transferred > 0)//log and notify of transfer
                                {
                                    //we have transferred and will be updating the current ticket
                                    //todo - we should update the parent ticket as well

                                    string result = string.Format("DOS msg: {0} tickets transferred from {1} to {2}. ",
                                        transferred.ToString(), parent.Id.ToString(), this.Id.ToString());

                                    EventQ.LogEvent(DateTime.Now, DateTime.Now, _Enums.EventQStatus.Success, null, Guid.Empty, null,
                                       _Enums.EventQContext.ShowDate, _Enums.EventQVerb.InventoryTransferred, _Enums.InvoiceItemContext.ticket.ToString(),
                                       transferred.ToString(), result);

                                    EventQ.CreateInventoryNotification(null, "DosTransfer", 
                                        Utils.ParseHelper.StripHtmlTags(this.DisplayNameWithAttribsAndDescription), _Enums.InvoiceItemContext.ticket, this.Id,
                                        transferred, pending);
                                }
                                
                                this.IAllotment = allotment;

                            }
                            catch (Exception ex)
                            {
                                _Error.LogException(ex);
                            }
                        }
                    }
                }

                #endregion

                return this.IAllotment;
            }
            set
            {
                this.IAllotment = value;
            }
        }
        //#w
        [XmlAttribute("Pending")]//do not inherit from parent
        public int Pending
        {
            get { return this.IPending; }
            set { this.IPending = value; }
        }
        [XmlAttribute("Sold")]//do not inherit from parent
        public int Sold
        {
            get { return this.ISold; }
            set { this.ISold = value; }
        }
        [XmlAttribute("Available")]//do not inherit from parent
        public int Available
        {
            //#w
            //get { return Allotment - Pending - Sold; }
            get { return Allotment - Pending - Sold; }
        }
        
        [XmlAttribute("Refunded")]//do not inherit from parent
        public int Refunded
        {
            get { return this.IRefunded; }
            set { this.IRefunded = value; }
        }
        [XmlAttribute("ShipCutoffDate")]
        public DateTime ShipCutoffDate
        {
            get { return this.DtShipCutoff; }
            set { this.DtShipCutoff = value; }
        }

        [XmlAttribute("UnlockDate")]
        public DateTime UnlockDate
        {
            get { return (!this.DtUnlockDate.HasValue) ? Utils.Constants._MinDate : this.DtUnlockDate.Value; }
            set { this.DtUnlockDate = value; }
        }
        [XmlAttribute("UnlockEndDate")]
        public DateTime UnlockEndDate
        {
            get { return (!this.DtUnlockEndDate.HasValue) ? DateTime.MaxValue : this.DtUnlockEndDate.Value; }
            set { this.DtUnlockEndDate = value; }
        }
        [XmlAttribute("PublicOnsaleDate")]
        public DateTime PublicOnsaleDate
        {
            get { if(this.IsDosTicket)
                    return this.DateOfShow.Date.AddHours(_Config.DayTurnoverTime);

                return (!this.DtPublicOnsale.HasValue) ? Utils.Constants._MinDate : this.DtPublicOnsale.Value; 
            }
            set { this.DtPublicOnsale = value; }
        }
        [XmlAttribute("EndDate")]
        public DateTime EndDate
        {
            get { return (!this.DtEndDate.HasValue) ? DateTime.MaxValue : this.DtEndDate.Value; }
            set { this.DtEndDate = value; }
        }
        [XmlAttribute("CriteriaText_Derived")]
        public string CriteriaText_Derived
        {
            get { return (this.CriteriaText == null) ? string.Empty : this.CriteriaText; }
            set { this.CriteriaText = value.Trim(); }
        }
        /// <summary>
        /// returns the name of the age record
        /// </summary>
        [XmlAttribute("AgeDescription")]
        public string AgeDescription
        {
            get { return ((Age)_Lookits.Ages.Find(this.TAgeId)).Name; }
        }
        [XmlAttribute("SalesDescription_Derived")]
        public string SalesDescription_Derived
        {
            get { return (this.SalesDescription == null) ? string.Empty : this.SalesDescription; }
            set { this.SalesDescription = value.Trim(); }
        }

        #endregion

        #region Packages and Linked Tix

        public bool IsPackage { get { return (this.ShowTicketPackageLinkRecords().Count > 0); } }
        public ShowTicket PackageBase
        {
            get
            {
                //if it is not a package
                if (! IsPackage)
                    return null;

                try
                {
                    ShowTicketPackageLinkCollection linkColl = new ShowTicketPackageLinkCollection();
                    linkColl.AddRange(this.ShowTicketPackageLinkRecords());

                    ShowTicketCollection coll = new ShowTicketCollection();
                    foreach (ShowTicketPackageLink link in linkColl)
                        coll.Add(link.ShowTicketToLinkedShowTicketIdRecord);

		            if (!coll.Contains(this))
                        coll.Add(this);
                        
                    if (coll.Count > 1)
                        coll.Sort("DtDateOfShow", true);

                    return coll[0];
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                }

                return null;
            }
        }
        public bool IsBaseOfPackage
        {
            get
            {
                if (!IsPackage)
                    return false;

                return this.Id == this.PackageBase.Id;
            }
        }

        /// <summary>
        /// this method will not find itself - only the others it is linked to
        /// </summary>
        public bool IsLinkedToTicket(int idxLinkTo)
        {
            if (this.IsPackage)
            {
                ShowTicket st = (ShowTicket)this.LinkedShowTickets.Find(idxLinkTo);

                if (st != null && st.Id > 0)
                    return true;
            }

            return false;
        }

        private ShowTicketCollection _linkedShowTickets = new ShowTicketCollection();
        /// <summary>
        /// This is an easier to use object than the link relations. Use this to deal with Package tickets
        /// </summary>
        public ShowTicketCollection LinkedShowTickets
        {
            get
            {
                if ((_linkedShowTickets == null && this.ShowTicketPackageLinkRecords().Count > 0) || (_linkedShowTickets.Count != this.ShowTicketPackageLinkRecords().Count))
                {
                    foreach (ShowTicketPackageLink link in this.ShowTicketPackageLinkRecords())
                        _linkedShowTickets.Add(link.ShowTicketToLinkedShowTicketIdRecord);
                }

                try
                {
                    if (_linkedShowTickets.Count > 1)
                        _linkedShowTickets.Sort("DtDateOfShow", true);
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                }

                return _linkedShowTickets;
            }
        }

        #endregion

        #region CRUD

        public ShowTicket CopyShowTicketComplete(ShowDate dateToCopyTo, string userName, Guid providerUserKey)
        {
            //todo - ensure that this does all the same stuff as date copy
            ShowTicket copiedTicket = ShowTicket.CreateSingleTicket(this.VendorRecord, dateToCopyTo, 
                this.PriceText, this.Price, this.IsDosTicket, this.DosText, this.DosPrice, 
                this.SalesDescription, this.CriteriaText, this.TAgeId, this.HideShipMethod, 
                userName, providerUserKey);

            //copy any past purchase texts
            if (this.PostPurchaseTextRecords().Count > 0)
            {
                copiedTicket.PostPurchaseTextRecords().CopyFrom(this.PostPurchaseTextRecords());
                copiedTicket.PostPurchaseTextRecords().SaveAll();
            }

            ////copy any requirements
            //if (this.RequiredShowTicketPastPurchaseRecords().Count > 0)
            //{
            //    foreach (RequiredShowTicketPastPurchase past in this.RequiredShowTicketPastPurchaseRecords())
            //    {
            //        Required req = new Required();
            //        req.CopyFrom(past.RequiredRecord);
            //        req.Save();

            //        RequiredShowTicketPastPurchase pp = new RequiredShowTicketPastPurchase();
            //        pp.DtStamp = DateTime.Now;
            //        pp.TShowTicketId = copiedTicket.Id;
            //        pp.TRequiredId = req.Id;

            //        copiedTicket.RequiredShowTicketPastPurchaseRecords().Add(pp);
            //    }

            //    copiedTicket.RequiredShowTicketPastPurchaseRecords().SaveAll();
            //}

            //if this is a parent ticket - create its date of show counterpart
            foreach (ShowTicketDosTicket stdt in this.ShowTicketDosTicketRecords())
            {
                ShowTicket oldDosTicket = stdt.DosShowTicketRecord;

                ShowTicket newDosTicket = ShowTicket.CreateSingleTicket(oldDosTicket.VendorRecord, dateToCopyTo,
                    oldDosTicket.PriceText, oldDosTicket.Price, oldDosTicket.IsDosTicket, oldDosTicket.DosText,
                    oldDosTicket.DosPrice, oldDosTicket.SalesDescription, oldDosTicket.CriteriaText, 
                    oldDosTicket.TAgeId, oldDosTicket.HideShipMethod, 
                    userName, providerUserKey);

                
                //copy any POST purchase texts
                if (oldDosTicket.PostPurchaseTextRecords().Count > 0)
                {
                    newDosTicket.PostPurchaseTextRecords().CopyFrom(oldDosTicket.PostPurchaseTextRecords());
                    newDosTicket.PostPurchaseTextRecords().SaveAll();
                }

                ////copy any requirements
                //if (oldDosTicket.RequiredShowTicketPastPurchaseRecords().Count > 0)
                //{
                //    foreach (RequiredShowTicketPastPurchase past in oldDosTicket.RequiredShowTicketPastPurchaseRecords())
                //    {
                //        Required req = new Required();
                //        req.CopyFrom(past.RequiredRecord);
                //        req.Save();

                //        RequiredShowTicketPastPurchase pp = new RequiredShowTicketPastPurchase();
                //        pp.DtStamp = DateTime.Now;
                //        pp.TShowTicketId = newDosTicket.Id;
                //        pp.TRequiredId = req.Id;

                //        newDosTicket.RequiredShowTicketPastPurchaseRecords().Add(pp);
                //    }

                //    newDosTicket.RequiredShowTicketPastPurchaseRecords().SaveAll();
                //}

                ShowTicketDosTicket newJoin = new ShowTicketDosTicket();
                newJoin.DtStamp = DateTime.Now;
                newJoin.ParentId = copiedTicket.Id;
                newJoin.DosId = newDosTicket.Id;

                newJoin.Save();
            }

            return copiedTicket;
        }


        public void Save_DisplayOrderOnly()
        {
            SPs.TxShowTicketUpdateDisplayOrder(this.Id, this.DisplayOrder).ExecuteScalar();
        }
        public void Save_AvoidRealTimeVars()
        {
            SPs.TxShowTicketUpdateAvoidRealTimeVars(this.Id, this.DateOfShow, this.CriteriaText, this.SalesDescription, this.TAgeId, 
                this.IsActive, this.IsSoldOut, this.Status, this.IsDosTicket, this.PriceText, this.Price, this.DosText, this.DosPrice,
                this.ServiceCharge, this.IsAllowShipping, this.IsAllowWillCall, this.HideShipMethod, this.ShipCutoffDate, 
                this.BOverrideSellout, 
                this.IsUnlockActive, this.UnlockCode, this.UnlockDate,
                this.UnlockEndDate, this.PublicOnsaleDate, this.EndDate, this.MaxQuantityPerOrder, this.Allotment, this.FlatShip, this.FlatMethod, 
                this.DtBackorder, this.BShipSeparate).ExecuteScalar();
        }

        public static ShowTicket CreateTicketPackage(Vendor vendor, ShowDate showDate, 
            System.Web.UI.WebControls.ListItemCollection dates,
            string priceText, decimal price, bool isDosTicket, string description, string criteria, 
            int ageIdx, bool hideShipMethod, string userName, Guid userId)
        {
            //showDate is automatically added
            ShowTicket contextTicket = CreateSingleTicket(vendor, showDate, priceText, price, isDosTicket, 
                string.Empty, 0, description, criteria, ageIdx, hideShipMethod, userName, userId);
            showDate.ShowTicketRecords().SaveAll();//ok to call save all here - as the insert is the only dirty row

            DateTime earliestCutoff = contextTicket.ShipCutoffDate;

            ShowTicketCollection ticketsInPackage = new ShowTicketCollection();
            ticketsInPackage.Add(contextTicket);

            foreach (System.Web.UI.WebControls.ListItem li in dates)
            {
                ShowDate sd = ShowDate.FetchByID(li.Value);

                ShowTicket st = CreateSingleTicket(vendor, sd, priceText, price, isDosTicket, 
                    string.Empty, 0, description, criteria, ageIdx, hideShipMethod, userName, userId);

                if (st.ShipCutoffDate < earliestCutoff)
                    earliestCutoff = st.ShipCutoffDate;

                sd.ShowTicketRecords().SaveAll();//ok to call save all here - as the insert is the only dirty row

                ticketsInPackage.Add(st);
            }

            //use this to track groups of linked tix
            System.Guid guid = System.Guid.NewGuid();

            ShowTicketPackageLinkCollection packaged = new ShowTicketPackageLinkCollection();
            foreach (ShowTicket ticket in ticketsInPackage)
            {
                if(ticket.ShipCutoffDate < earliestCutoff)
                {
                    ticket.ShipCutoffDate = earliestCutoff;
                    ticket.Save();
                }

                foreach (ShowTicket linked in ticketsInPackage)
                {
                    if (ticket.Id != linked.Id)
                    {
                        ShowTicketPackageLink link = new ShowTicketPackageLink();
                        link.GroupIdentifier = guid;
                        link.ParentShowTicketId = ticket.Id;
                        link.LinkedShowTicketId = linked.Id;
                        link.DtStamp = DateTime.Now;

                        packaged.Add(link);
                    }
                }
            }

            packaged.SaveAll();

            return contextTicket;
        }
        public static ShowTicket CreateSingleTicket(Vendor vendor, ShowDate showDate, string priceText, decimal price, 
            bool isDosTicket, string dosText, decimal dosPrice, string description, string criteria, 
            int ageIdx, bool hideShipMethod, string userName, Guid userId)
        {
            ShowTicket st = new ShowTicket();
            
            st.DtStamp = DateTime.Now;
            st.TVendorId = vendor.Id;
            st.DateOfShow = showDate.DateOfShow;
            st.TShowDateId = showDate.Id;
            st.TShowId = showDate.TShowId;
            st.IsActive = true;
            st.IsSoldOut = false;

            //display order configured automatically
            st.DisplayOrder = showDate.ShowTicketRecords().Count;
            st.IsAllowShipping = false;
            st.IsAllowWillCall = true;
            st.HideShipMethod = hideShipMethod;
            st.IsUnlockActive = false;
            //st.UnlockCode = unlockCode;

            st.IsDosTicket = isDosTicket;
            //st.PriceText = priceText;//not used
            st.Price = price;
            //st.DosText = dosText;//not used
            st.DosPrice = dosPrice;
            st.ServiceCharge = Wcss.ServiceCharge.ComputeTicketServiceFee(price);
            st.SalesDescription = description;
            st.CriteriaText = criteria;
            st.TAgeId = ageIdx;

            st.MaxQuantityPerOrder = _Config._MaxTicketPurchaseQuantity;
            st.DosText = string.Empty;
            st.DosPrice = 0;
            st.Status = string.Empty;
            st.ShipCutoffDate = (isDosTicket) ? Utils.Constants._MinDate : showDate.DateOfShow.Date.AddDays(-1*_Config._Shipping_Ticket_CutoffDays);
            st.UnlockDate = Utils.Constants._MinDate;
            st.UnlockEndDate = DateTime.MaxValue;
            st.PublicOnsaleDate = (isDosTicket) ? showDate.DateOfShow.Date.AddHours(_Config.DayTurnoverTime) : showDate.ShowRecord.DateOnSale;

            st.EndDate = (isDosTicket) ? showDate.DtDateOfShow.Date.AddMinutes((double)(_Config._DosTicket_SalesCutoff.TotalMinutes)) : DateTime.MaxValue;

            //code info
            st.UnlockCode = Utils.ParseHelper.GenerateRandomPassword(7);

            showDate.ShowTicketRecords().Add(st);

            showDate.ShowTicketRecords().SaveAll();//ok to call save all here - as the insert is the only dirty row
            
            
            string val = string.Format("{0} + {1} - {2}: {3}", st.Price.ToString("c"), st.ServiceCharge.ToString("n2"), st.AgeDescription, st.CriteriaText_Derived);

            EventQ.LogEvent(DateTime.Now, DateTime.Now, _Enums.EventQStatus.Success, userName, userId, null, _Enums.EventQContext.ShowDate, _Enums.EventQVerb._TicketAdded, 
                    val, string.Format("ShowDate: {0} - {1}", showDate.DateOfShow.ToString(), st.TShowDateId.ToString()), showDate.ShowRecord.Name);

            return st;
        }

        #endregion

        #region Ticket availability

        // <summary>
        /// indicates if the individual ticket can be show to the user
        //dont display tickets with an onsale greater than today unless they are keyed - and not day of show. the turnover for display
        //  is 3AM, ie. we will sell tix until 3AM the day of the show. Note that tickets with unlock codes will still need to be checked
        //</summary>
        public bool IsAvailableForListing(_Enums.VendorTypes vendorType, string unlockCode, bool includeLotterySignups, bool includeLotteryFulfillments, ShowTicketCollection existingTix)
        {
            return IsAvailableForListing(vendorType, unlockCode, includeLotterySignups, includeLotteryFulfillments, existingTix, null, false);
        }
        public bool IsAvailableForListing(_Enums.VendorTypes vendorType, string unlockCode, bool includeLotterySignups, bool includeLotteryFulfillments, ShowTicketCollection existingTix, 
            ProductAccess productAccess, bool productAccessActivationValid)
        {
            //TODO: real time allotment?
            int alot = this.Allotment;

            if (vendorType != _Enums.VendorTypes.all)
            {
                string vendorName = this.VendorRecord.Name.ToLower().Trim();

                //vendor name is boxoffice or online
                if (vendorName != vendorType.ToString().ToLower().Trim())
                    return false;
            }


            //End date must be checked prior to allotment test
            if (this.IsActive && this.DateOfShow.Date >= _Config.SHOWOFFSETDATE &&
                (!this.IsSoldOut) && 
                this.EndDate > DateTime.Now && alot > 0 && (alot - this.Sold > 0))
            
                //keep if clause below for testing after 6 PM!
                //if (this.IsActive && this.DateOfShow.Date >= _Config.SHOWOFFSETDATE &&
                //    (!this.IsSoldOut) && this.EndDate.Date.AddHours(20) > DateTime.Now && alot > 0 && (alot - this.Sold > 0))
            {
                //CRITERIA FOR DOS TIX AND NON_DOS TICKETS
                //DateTime.Now.AddHours(-_Config.DayTurnoverTime).Date;
                if(this.IsDosTicket && _Config.SHOWOFFSETDATE.Date != this.DateOfShow.Date)
                    return false;
                else if ((!this.IsDosTicket) && _Config.SHOWOFFSETDATE.Date >= this.DateOfShow.Date)
                    return false;

                //LOTTERY IS ALWAYS PUBLIC
                //use signup here - client page will not allow the ticket to be sold
                //we shouldnt' have to worry about package lotteries - because they will display much before the cutoff dates
                if ((includeLotterySignups || includeLotteryFulfillments) && this.IsLotteryTicket)
                {
                    Lottery lott = this.LotteryRecords()[0];

                    if (includeLotterySignups && (!(lott.IsActive_Signup && lott.SignupStartDate < DateTime.Now && lott.SignupEndDate > DateTime.Now)))
                        return false;
                    else if (includeLotteryFulfillments && (!(lott.IsActive_Fulfillment && lott.FulfillStartDate < DateTime.Now && lott.FulfillEndDate > DateTime.Now)))
                        return false;
                }

                //simplify comparison
                if (unlockCode == null) unlockCode = string.Empty;


                //if productAccess then compare to productAccess activation window
                //otherwise compare to ticket activation
                bool meetsActivationWindow = (productAccessActivationValid) ? true :
                    
                    (productAccess != null) ? productAccess.IsWithinActivationPeriod(UnlockCode) :

                    ((this.ShowDateRecord.ShowRecord.DateOnSale < DateTime.Now && this.PublicOnsaleDate < DateTime.Now) ||
                    (this.IsUnlockActive && this.UnlockEndDate > DateTime.Now && this.UnlockCode == unlockCode));

                //if public onsale is on and this ticket is past public onsale (end date was checked above)
                //or if it requires an unlock code, we have a matching code and the start and end dates are within range
                //if((this.ShowDateRecord.ShowRecord.DateOnSale < DateTime.Now && this.PublicOnsaleDate < DateTime.Now) ||
                //   (this.IsUnlockActive && this.UnlockDate < DateTime.Now && this.UnlockEndDate > DateTime.Now && this.UnlockCode == unlockCode))
                //    return true;
                if (meetsActivationWindow)
                {
                    //CHECK PACKAGES
                    //don't show packages if the base is out of display date
                    //we use less than or equals here because we dont want to sell pkg tix DOS
                    if (this.IsPackage)
                    {
                        if (this.PackageBase.DateOfShow.Date <= _Config.SHOWOFFSETDATE)
                            return false;

                        if (existingTix != null)//if its null - it can't already be in the collection!
                        {
                            //see if the package is already in the collection
                            ShowTicketCollection linked = new ShowTicketCollection();
                            linked.AddRange(this.LinkedShowTickets);

                            foreach (ShowTicket st in linked)
                            {
                                //if we have one of these tickets in the package - any linked ticket - does not have to be base
                                int idx = existingTix.GetList().FindIndex(delegate(ShowTicket match) { return (match.Id == st.Id); });
                                if (idx != -1)
                                    return false;
                            }
                        }
                    }
                    //END PACKAGES

                    return true;
                }
            }

            return false;
        }

        public bool IsUnlocked(string key, DateTime dateNow)
        {
            if (this.IsActive)
            {
                if (
                    ((this.UnlockCode == null) || (this.UnlockCode != null && this.UnlockCode.Length == 0)) && 
                    this.PublicOnsaleDate <= dateNow && dateNow <= this.EndDate)
                    return true;
                else
                {
                    if (key == null)
                        key = string.Empty;

                    //if key matches code and dates are valid
                    if (this.UnlockCode.Equals(key.ToLower()) && this.UnlockDate <= dateNow && dateNow <= this.EndDate)
                        return true;
                    //if no key match and public onsale is past and end date is in future
                    else if ((!this.UnlockCode.Equals(key.ToLower())) && this.UnlockDate <= dateNow && dateNow <= this.EndDate && this.PublicOnsaleDate <= dateNow)
                        return true;
                }
            }

            return false;
        }

        #endregion

        #region Displays

        /// <summary>
        /// references ShowDate_Tickets.ascx.cs(417):            ddl.DataTextField = "CopyListing";
        /// </summary>
        public string CopyListing
        {
            get
            {
                return string.Format("{0} ({1}) => {2} - {3}", this.Price.ToString("n"), this.ServiceCharge.ToString("n2"),
                    this.PerItemPrice.ToString("c"), Utils.ParseHelper.StripHtmlTags(this.DisplayNameWithAttribsAndDescription));
            }
        }

        public string TicketInfo_Short
        {
            get
            {
                string desc = string.Format("{0} {1}", SalesDescription_Derived, CriteriaText_Derived).Trim();

                return string.Format("{0} {1} + {2} - {3} {4}",

                    ShowTicket.IsCampingPass(desc) ? "CAMPING" : this.DateOfShow.ToString("MM/dd/yy hh:mmtt"),
                    this.Price.ToString("c"),
                    this.ServiceCharge.ToString("n2"), 
                    this.ShowDateRecord.ShowRecord.ShowNamePart,
                    desc

                    ).Trim();
            }
        }

        #endregion

    }

    public partial class ShowTicketCollection
    {
        public void SortBy_DateToOrderBy()
        {
            if (this != null && this.Count > 1)
                this.GetList().Sort(delegate(ShowTicket x, ShowTicket y) { return (x.ShowDateRecord.DateOfShow_ToSortBy.CompareTo(y.ShowDateRecord.DateOfShow_ToSortBy)); });
        }
    }
}
