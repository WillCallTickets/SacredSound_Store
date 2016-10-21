using System;
using System.Data;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.ComponentModel;

namespace Wcss
{
    public partial class ActivationWindow
    {
        public static ActivationWindow GetActivationWindowRecord(_Enums.ActivationWindowContext context, int parentIdx)
        {
            string sql = "SELECT aw.* FROM [ActivationWindow] aw WHERE aw.[vcContext] = @tableName AND aw.[TParentId] = @parentIdx; ";

            SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sql, SubSonic.DataService.Provider.Name);

            cmd.Parameters.Add("@appId", _Config.APPLICATION_ID, DbType.Guid);
            cmd.Parameters.Add("@tableName", context.ToString());
            cmd.Parameters.Add("@parentIdx", parentIdx, DbType.Int32);

            ActivationWindow aw = new ActivationWindow();
            aw.LoadAndCloseReader(SubSonic.DataService.GetReader(cmd));

            return (aw.Id > 0) ? aw : null;
        }

        [XmlAttribute("ActivationWindowContext")]
        public _Enums.ActivationWindowContext ActivationWindowContext
        {
            get
            {
                return (_Enums.ActivationWindowContext)Enum.Parse(typeof(_Enums.ActivationWindowContext), this.VcContext, true);
            }
            set
            {
                this.VcContext = value.ToString();
            }
        }
        [XmlAttribute("UseCode")]
        public bool UseCode
        {
            get { return this.BUseCode; }
            set { this.BUseCode = value; }
        }
        [XmlAttribute("DateCodeStart")]
        public DateTime DateCodeStart
        {
            get { return (this.DtCodeStart.HasValue) ? this.DtCodeStart.Value : Utils.Constants._MinDate; }
            set 
            {
                if (value == Utils.Constants._MinDate)
                    this.DtCodeStart = null;
                else
                    this.DtCodeStart = value;
            }
        }
        [XmlAttribute("DateCodeEnd")]
        public DateTime DateCodeEnd
        {
            get { return (this.DtCodeEnd.HasValue) ? this.DtCodeEnd.Value : DateTime.MaxValue; }
            set
            {
                if (value == DateTime.MaxValue)
                    this.DtCodeEnd = null;
                else
                    this.DtCodeEnd = value;
            }
        }
        [XmlAttribute("DatePublicStart")]
        public DateTime DatePublicStart
        {
            get { return (this.DtPublicStart.HasValue) ? this.DtPublicStart.Value : Utils.Constants._MinDate; }
            set
            {
                if (value == Utils.Constants._MinDate)
                    this.DtPublicStart = null;
                else
                    this.DtPublicStart = value;
            }
        }
        [XmlAttribute("DatePublicEnd")]
        public DateTime DatePublicEnd
        {
            get { return (this.DtPublicEnd.HasValue) ? this.DtPublicEnd.Value : DateTime.MaxValue; }
            set
            {
                if (value == DateTime.MaxValue)
                    this.DtPublicEnd = null;
                else
                    this.DtPublicEnd = value;
            }
        }

    }
    public partial class ProductAccess
    {
        public bool HasTicketAccess(int ticketId)
        {
            return (this.ProductAccessProductRecords().GetList().FindIndex(delegate(ProductAccessProduct match) { 
                return (match.ProductContext == _Enums.ProductAccessProductContext.ticket && match.TParentId == ticketId); }) != -1);
        }
        public bool AppliesToCurrentMerch(Merch m)
        {
            //easiest to most intensive search
            //match user name first
            //match a ticket to this showdate
            foreach (ProductAccessProduct pap in this.ProductAccessProductRecords())
            {
                if (pap.ProductContext == _Enums.ProductAccessProductContext.merch && pap.ParentMerchRecord != null &&
                    pap.ParentMerchRecord.Id == m.Id)
                {
                    return true;
                }
            }

            return false;
        }
        public bool IsWithinActivationPeriod(string marketingProgramKey)
        {
            //is this PA within the window of activation?
            if (this.ActivationWindowRecord != null)
            {
                ActivationWindow aw = this.ActivationWindowRecord;
                DateTime nowDate = DateTime.Now;

                //if it does not pass activation then break
                if ((!aw.UseCode) || (aw.UseCode && aw.Code != null && aw.Code.Trim().Length > 0 && marketingProgramKey.ToLower() == aw.Code.ToLower()))
                {
                    //if use code - check dates
                    if (aw.UseCode && (nowDate < aw.DateCodeStart || nowDate > aw.DateCodeEnd))
                        return false;

                    //check public dates
                    if (nowDate < aw.DatePublicStart || nowDate > aw.DatePublicEnd)
                        return false;
                }
                else
                    return false;
            }

            return true;
        }

        public static int User_HasPurchasedPastAccess(string userName, int tShowTicketId)
        {
            //TODO: this still allows a loophole where someone who has changed their username in thepast can get access to more than quota allowed
            string sql = "SET NOCOUNT ON; SELECT ISNULL(SUM(ii.[iQuantity]), 0) ";
            sql += "FROM [InvoiceItem] ii LEFT OUTER JOIN [Invoice] i ON i.[Id] = ii.[tInvoiceId] LEFT OUTER JOIN [aspnet_Users] au ON au.[UserId] = i.[UserId] ";
            sql += "WHERE au.[UserName] = @username AND ii.[PurchaseAction] = 'Purchased' AND ii.[tShowTicketId] IS NOT NULL AND ii.[tShowTicketId] = @tShowTicketId ";
            SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sql, SubSonic.DataService.Provider.Name);
            cmd.Parameters.Add("@username", userName);
            cmd.Parameters.Add("@tShowTicketId", tShowTicketId, DbType.Int32);

            int pastQty = (int)SubSonic.DataService.ExecuteScalar(cmd);

            return pastQty;
        }

        public static ProductAccessCollection Populate_ProductAccess_Lookup(Guid appId, int hourStartOffset)
        {
            ProductAccessCollection collAccess = new ProductAccessCollection();
            ActivationWindowCollection collActivation = new ActivationWindowCollection();
            ProductAccessProductCollection collProduct = new ProductAccessProductCollection();
            List<System.Web.UI.WebControls.ListItem> listUsers = new List<System.Web.UI.WebControls.ListItem>();

            using (IDataReader rdr = SPs.TxProductAccessLookupData(appId, hourStartOffset).GetReader())
            {
                //productaccess - activationwindow is auto loaded by productaccess load method
                //TODO: get activation windows from the above SP - save some db hits
                while (rdr.Read())
                {
                    ProductAccess row = new ProductAccess();
                    row.Load(rdr);
                    collAccess.Add(row);
                }

                //rdr.NextResult();

                ////activation window
                //while (rdr.Read())
                //{
                //    ActivationWindow row = new ActivationWindow();
                //    row.Load(rdr);
                //    collActivation.Add(row);
                //}

                rdr.NextResult();

                //productacessproduct
                while (rdr.Read())
                {
                    ProductAccessProduct row = new ProductAccessProduct();
                    row.Load(rdr);
                    collProduct.Add(row);
                }

                rdr.NextResult();

                //users
                while (rdr.Read())
                {
                    listUsers.Add(new System.Web.UI.WebControls.ListItem(rdr.GetString(rdr.GetOrdinal("UserName")), 
                        rdr.GetInt32(rdr.GetOrdinal("TProductAccessId")).ToString()));
                }
            }


            //now we can construct our lookup
            foreach (ProductAccess p in collAccess)
            {
                if (p.IsActive)
                {
                    ////we know that the context has been determined - so just match id
                    //ActivationWindow aw = collActivation.GetList().Find(delegate(ActivationWindow match) { return (match.TParentId == p.Id); });
                    //if (aw != null && aw.Id > 0)
                    //    p.ActivationWindowRecord = aw;

                    //add in products
                    ProductAccessProductCollection collProd = new ProductAccessProductCollection();
                    collProd.AddRange(collProduct.GetList().FindAll(delegate(ProductAccessProduct match) { return (match.TProductAccessId == p.Id); }));
                    if (collProd.Count > 0)
                    {
                        p.colProductAccessProductRecords = collProd;
                        p.colProductAccessProductRecords.ListChanged += new ListChangedEventHandler(p.colProductAccessProductRecords_ListChanged);
                    }

                    //add in users
                    //List<System.Web.UI.WebControls.ListItem> users = new List<System.Web.UI.WebControls.ListItem>();
                    List<string> users = new List<string>();
                    users.AddRange(listUsers.FindAll(delegate(System.Web.UI.WebControls.ListItem match) { return match.Value == p.Id.ToString(); })
                        .ConvertAll(li => li.Text.ToLower()));

                    if (users.Count > 0)
                    {
                        if (p.OrderFlowUserList == null)
                            p.OrderFlowUserList = new List<string>();

                        p.OrderFlowUserList.AddRange(users);
                    }
                }
            }

            //if there are no products or users than why bother? remove those without those collections
            collAccess.GetList().RemoveAll(delegate(ProductAccess match) { return (match.ProductAccessProductRecords().Count <= 0 || match.OrderFlowUserList == null || match.OrderFlowUserList.Count == 0); });

            //populate objects
            foreach (ProductAccess pa in collAccess)
            {
                foreach (ProductAccessProduct pap in pa.ProductAccessProductRecords())
                {
                    if (pap.ProductContext == _Enums.ProductAccessProductContext.ticket)
                    {
                        //traverse the heierarchy
                        Show s = pap.ShowTicketRecord.ShowDateRecord.ShowRecord;

                        //TODO: populate any packages
                    }
                    else if (pap.ProductContext == _Enums.ProductAccessProductContext.merch)
                    {
                        //traverse - we know this is a parent
                        MerchCollection inventory = pap.ParentMerchRecord.ChildMerchRecords();
                    }
                }
            }

            return collAccess;
        }

        /// <summary>
        /// use this list to more easily track users in the order flow that are subscribed to this productaccess. If the 
        /// user is in this list, then the actual row can be retrieved to gather data
        /// </summary>
        public List<string> OrderFlowUserList { get; set; }

        [XmlAttribute("IsActive")]
        public bool IsActive
        {
            get { return this.BActive; }
            set { this.BActive = value; }
        }

        //CampaignName - vc 512
        //CampaignCode - vc 50

        [XmlAttribute("DisplayOrder")]
        public int DisplayOrder
        {
            get { return this.IDisplayOrder; }
            set { this.IDisplayOrder = value; }
        }
        protected override void Loaded()
        {
            base.Loaded();
            DetectExistingActivationWindowRecord();
        }
        public override void Initialize()
        {
            base.Initialize();
        }

        private ActivationWindow _activationWindowRecord = null;
        public void DetectExistingActivationWindowRecord()
        {
            _activationWindowRecord = ActivationWindow.GetActivationWindowRecord(_Enums.ActivationWindowContext.ProductAccess, this.Id);
        }
        public ActivationWindow ActivationWindowRecord
        {
            get
            {
                return _activationWindowRecord;
            }
            set
            {
                _activationWindowRecord = value;
            }
        }
        public void DeleteActivationWindowRecord()
        {
            if (this.ActivationWindowRecord != null)
            {
                string sql = "DELETE FROM [ActivationWindow] WHERE [Id] = @idx; ";
                SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sql, SubSonic.DataService.Provider.Name);
                cmd.Parameters.Add("@idx", this.ActivationWindowRecord.Id, DbType.Int32);

                SubSonic.DataService.ExecuteQuery(cmd);

                this._activationWindowRecord = null;
            }
        }

    }

    public partial class ProductAccessCollection : Utils._Collection.IOrderable<ProductAccess>
    {
        /// <summary>
        /// Adds a Tune to the collection
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="originalFileName"></param>
        /// <param name="fileNameOnly"></param>
        /// <param name="displayText"></param>
        /// <returns></returns>
        public ProductAccess AddToCollection(string campaignName)
        {
            System.Collections.Generic.List<System.Web.UI.Pair> args = new System.Collections.Generic.List<System.Web.UI.Pair>();
            args.Add(new System.Web.UI.Pair("DtStamp", DateTime.Now));
            args.Add(new System.Web.UI.Pair("ApplicationId", _Config.APPLICATION_ID));
            args.Add(new System.Web.UI.Pair("CampaignName", campaignName));
            args.Add(new System.Web.UI.Pair("CampaignCode", Utils.ParseHelper.GenerateRandomPassword(10)));
            args.Add(new System.Web.UI.Pair("IsActive", false));

            return AddToCollection(args);
        }

        public ProductAccess AddToCollection(System.Collections.Generic.List<System.Web.UI.Pair> args)
        {
            return Utils._Collection.AddItemToOrderedCollection(this.GetList(), args, true);
        }

        /// <summary>
        /// Delete a ProductAccess campaign from the collection by ID
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public bool DeleteFromCollection(int idx)
        {
            return Utils._Collection.DeleteFromOrderedCollection(this.GetList(), idx);
        }

        /// <summary>
        /// Reorder a Tune by ID and direction
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public ProductAccess ReorderItem(int idx, string direction)
        {
            return Utils._Collection.ReorderOrderedCollection(this.GetList(), idx, direction);
        }
    }

    public partial class ProductAccessProduct
    {
        //ProductAccessRecord

        [XmlAttribute("ProductContext")]
        public _Enums.ProductAccessProductContext ProductContext
        {
            get
            {
                return (_Enums.ProductAccessProductContext)Enum.Parse(typeof(_Enums.ProductAccessProductContext), this.VcContext, true);
            }
            set
            {
                this.VcContext = value.ToString();
            }
        }

        private Merch _parentMerchRecord = null;
        [XmlAttribute("ParentMerchRecord")]
        public Merch ParentMerchRecord
        {
            get
            {
                if(_parentMerchRecord == null && this.ProductContext == _Enums.ProductAccessProductContext.merch)
                {
                    _parentMerchRecord = Merch.FetchByID(this.TParentId);
                }

                return _parentMerchRecord;
            }
        }

        private ShowTicket _showTicketRecord = null;
        [XmlAttribute("ShowTicketRecord")]
        public ShowTicket ShowTicketRecord
        {
            get
            {
                if (_showTicketRecord == null && this.ProductContext == _Enums.ProductAccessProductContext.ticket)
                {
                    _showTicketRecord = ShowTicket.FetchByID(this.TParentId);
                }

                return _showTicketRecord;
            }
        }

    }

    public partial class ProductAccessUser
    {
        //ProductAccessRecord
        //UserName
        //Referral
        //Instructions
        [XmlAttribute("QuantityAllowed")]
        public int QuantityAllowed
        {
            get { return this.IQuantityAllowed; }
            set { this.IQuantityAllowed = value; }
        }
    }
}
