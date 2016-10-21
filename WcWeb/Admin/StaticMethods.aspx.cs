using System;
using System.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Linq;

using Wcss;

namespace WillCallWeb.Admin
{
    public partial class StaticMethods : WillCallWeb.BasePage
    {
        protected override void OnPreInit(EventArgs e)
        {
            QualifySsl(false);
            base.OnPreInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {   
         
        }
        protected void btnMerchSelect_Click(object sender, EventArgs e)
        {
            int x = this.MerchSelector1.SelectedInventoryId;
            lblSelect.Text = x.ToString();
            //updStatic.Update();
            //this.MerchSelector1.
        }

        protected void btnSendTestMail_Click(object sender, EventArgs e)
        {
            MailQueue.SendEmail("rob@robkurtz.net", "rob", "rkurtz@willcalltickets.com", "", "", "testing email from solution", "some body stuff", "a text version",
                null, true, null);

            //_Error.LogException(new Exception("blah blah blah"));
        }

        protected void btnBundler_Click(object sender, EventArgs e)
        {
            string bnparentConst = "bundleparent=";
            

            string sql = "select top 250 tinvoiceid from invoiceitem where charindex('bundleparent=', notes) > 0 order by tinvoiceid";
            SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sql, SubSonic.DataService.Provider.Name);

            List<int> list = new List<int>();
            //list.Add(54414);

            if (list.Count == 0)
            {
                try
                {
                    using (System.Data.IDataReader dr = SubSonic.DataService.GetReader(cmd))
                    {
                        while (dr.Read())
                            list.Add((int)dr.GetValue(0));
                        dr.Close();
                    }
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                }
            }

            foreach (int idx in list)
            {

                Invoice i = new Invoice(idx);

                List<InvoiceItem> coll = new List<InvoiceItem>();
                coll.AddRange(i.InvoiceItemRecords().GetList()
                    .FindAll(delegate(InvoiceItem match)
                {
                    return (match.IsMerchandiseItem && match.Notes != null &&
                        match.Notes.IndexOf(bnparentConst) != -1);
                }));

                try
                {
                    //loop thru and get the appropriate nums - parents - etc
                    foreach (InvoiceItem ii in coll)
                    {
                        if (ii.PurchaseAction != _Enums.PurchaseActions.NotYetPurchased.ToString() && ii.Notes != null && ii.Notes.IndexOf(bnparentConst) != -1)
                        {
                            //extract the id from the notes
                            string[] parts = ii.Notes.Split('=');
                            int pid;
                            if (int.TryParse(parts[1], out pid))
                            {
                                if (pid > 0)
                                {
                                    /* 
                                     * now we need some numbers and objects
                                     * 
                                     * pid      the number of the merch item that sparked the addition of the bundle
                                     *          we need this number to get the id of the merch parent that the bundle is linked to
                                     *          as well as getting the the parentitem
                                     *          
                                     * parentItem   the item that is to be the parent of the bundle item. It is found by matching tmerchid
                                     *          to pid. We will make this the parent of the bundleItem
                                     *      
                                     * title    taken from the criteria of the item. we need to chop off the "BUNDLED ITEM: " and 
                                     *          then chop off the elipsis at the end - trim this value
                                     *          do a like search on this title to match up the appropriate bundle
                                     * 
                                     * see if we have an existing bundle entry for this!!!
                                     * 
                                     * bundle   find the matching bundle within the parent merchbundle records that matches (like) 
                                     *          title. Insert a bundle object into the invoiceitem table for this invoice
                                     *          purchasename = parentItem.PurchaseName
                                     *          mainactname is bundle.title
                                     *          description is bundle.comment
                                     *          criteria=BundleId=bundle.Id&ParentId=parentItem.Id
                                     * 
                                     *          Get the id of the new bundle item
                                     * bundleItem  retrieve it                
                                     *        
                                     * 
                                     * update ii    update the current invoiceitem (ii), the one that is the bundled selection and change 
                                     *          criteria to 
                                     *          BundleId=bundle.Id&ParentId=bundleItem.Id
                                     *          null out the notes
                                     *          examine the merchitem it is linked to - if it is a download, then add
                                     *          &DownloadCode=ii.CreateDeliveryCode()
                                     *          reset price to 0 - it has been recorded in the bundle row
                                     * 
                                     */

                                    //get the parentItem
                                    List<InvoiceItem> matchingItems = new List<InvoiceItem>();
                                    matchingItems.AddRange(i.InvoiceItemRecords().GetList().FindAll(delegate(InvoiceItem match) { return match.TMerchId == pid; }));

                                    if (matchingItems.Count > 1)
                                        throw new ArgumentOutOfRangeException();

                                    InvoiceItem parentItem = matchingItems[0];

                                    Merch parentMerch = parentItem.MerchRecord.ParentMerchRecord;

                                    Merch linkedMerch = ii.MerchRecord;

                                    //form the title
                                    string title = ii.Criteria ?? string.Empty;
                                    if (title.Trim().Length > 0)
                                        title = title.Replace("BUNDLED ITEM: ", string.Empty).TrimEnd('.').Trim();
                                    
                                    ////exceptions handled here
                                    //switch(title.ToLower())
                                    //{
                                    //    case "5 posters for $15!":
                                    //        title = "";
                                    //        break;
                                    //}


                                    //find the merchbundlerecord item in question
                                    List<MerchBundle> bundleList = new List<MerchBundle>();
                                    bundleList.AddRange(parentMerch.MerchBundleRecords().GetList()
                                        .FindAll(delegate(MerchBundle match) { return match.Title.ToLower().IndexOf(title.ToLower()) != -1; }));

                                    if (bundleList.Count == 1)
                                    {
                                        MerchBundle bundle = bundleList[0];

                                        //see if there is an existing bundle
                                        InvoiceItem bundleItem = i.InvoiceItemRecords().GetList()
                                            .Find(delegate(InvoiceItem match) { return (match.IsBundle && match.TMerchBundleId == bundle.Id); });

                                        if (bundleItem == null)
                                        {
                                            bundleItem = new InvoiceItem();
                                            bundleItem.TInvoiceId = i.Id;
                                            bundleItem.VcContext = _Enums.InvoiceItemContext.bundle.ToString();
                                            bundleItem.PurchaseName = parentItem.PurchaseName;
                                            bundleItem.MainActName = bundle.Title;
                                            // criteria=BundleId=bundle.Id&ParentId=parentItem.Id
                                            bundleItem.Criteria = string.Format("{0}{1}&{2}{3}",
                                                InvoiceItem.MerchBundleIdConstant, bundle.Id.ToString(),
                                                InvoiceItem.ParentItemIdConstant, parentItem.Id.ToString());
                                            if (bundle.Comment != null && bundle.Comment.Trim().Length > 0)
                                                bundleItem.Description = bundle.Comment;
                                            bundleItem.Quantity = ii.Quantity;
                                            bundleItem.Price = ii.Price;
                                            bundleItem.PurchaseAction = ii.PurchaseAction;
                                            bundleItem.TShipItemId = ii.TShipItemId;
                                            bundleItem.Guid = Guid.NewGuid();
                                            bundleItem.DtStamp = DateTime.Now;
                                            bundleItem.BRTS = false;

                                            bundleItem.Save();
                                            i.InvoiceItemRecords().Add(bundleItem);
                                        }

                                        if (!linkedMerch.IsDownloadDelivery && (bundleItem.ShippingMethod == null || bundleItem.DateShipped == DateTime.MaxValue))
                                        {
                                            bundleItem.DateShipped = ii.DateShipped;
                                            bundleItem.ShippingMethod = ii.ShippingMethod;
                                            bundleItem.ShippingNotes = ii.ShippingNotes;
                                            bundleItem.Save();
                                        }



                                        int newBundleId = bundleItem.Id;

                                        //   update ii    update the current invoiceitem (ii), the one that is the bundled selection and change 
                                        //*          criteria to 
                                        //*          BundleId=bundle.Id&ParentId=bundleItem.Id
                                        //*          null out the notes
                                        //*          examine the merchitem it is linked to - if it is a download, then add
                                        //*          &DownloadCode=ii.CreateDeliveryCode()
                                        //*          reset price to 0 - it has been recorded in the bundle row

                                        string code = string.Format("{0}{1}&{2}{3}",
                                            InvoiceItem.MerchBundleIdConstant, bundle.Id.ToString(),
                                            InvoiceItem.ParentItemIdConstant, bundleItem.Id.ToString());

                                        if (linkedMerch.IsDownloadDelivery)
                                            code += string.Format("&{0}", Inventory.CreateDeliveryCodeForInvoiceItem(ii, linkedMerch, true));

                                        ii.Criteria = code;
                                        ii.Notes = null;
                                        ii.Price = 0;

                                        ii.Save();

                                        txtInvoiceId.Text = string.Empty;

                                    }
                                    else
                                    {
                                        throw new ArgumentOutOfRangeException();
                                    }



                                }
                                else
                                    throw new ArgumentNullException();
                            }
                            else
                                throw new ArgumentOutOfRangeException();

                        }
                    }
                }
                catch (Exception ex)
                {
                    string d = ex.Message;
                    _Error.LogException(ex);
                }
            }
            //    }
            //}


        }

        protected void btnTest_Click(object sender, EventArgs e)
        {
            _Error.LogException(new Exception("blah blah blah"));
        }

        protected void btnConfig_Click(object sender, EventArgs e)
        {
            _Config.ConfigTest();
        }

        protected void btnPast_Click(object sender, EventArgs e)
        {
            //TODO: make sure a version of the image is saved in the show directory!!!!!!!
            //TODO: change order by after testing

            //get a collection of past shows and loop thru
            ShowCollection shows = new ShowCollection();
            
            string sql = "SELECT TOP 1000 s.* FROM [Show] s WHERE s.[DisplayUrl] IS NULL OR LEN(RTRIM(LTRIM(s.[DisplayUrl]))) = 0 AND ";
            sql += "s.[Name] < ( CAST(DATEPART(yyyy,@date) as varchar) + '/' +  CAST(DATEPART(mm,@date) as varchar) + '/' + CAST(DATEPART(dd,@date) as varchar) ) ORDER BY ID DESC ";
            SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sql, SubSonic.DataService.Provider.Name);
            cmd.Parameters.Add("@date", DateTime.Now.Date.AddDays(-5), DbType.DateTime);

            shows.LoadAndCloseReader(SubSonic.DataService.GetReader(cmd));

            foreach (Show s in shows)
            {
                string imageVersion = s.ShowImageUrl;
                imageVersion = imageVersion.Replace("thumbsm", "thumblg");

                string mappedImage = System.Web.HttpContext.Current.Server.MapPath(imageVersion);
                string imageHtml = string.Empty;

                //if we are in venue mode, and we 
                //reverse logic at work here
                bool venuePass = true;

                if (venuePass)
                {
                    bool imagePass = true;

                    if (!System.IO.File.Exists(mappedImage))
                    {
                        //find the original and create a smaller version
                        //the original resides in the parent directory
                        string originalVersion = System.IO.Path.GetFileName(mappedImage);//.Replace("thumblg/", string.Empty);

                        //be sure the original exists
                        if (System.IO.File.Exists(originalVersion))
                        {
                            Utils.ImageTools.CreateAndSaveThumbnailImage(originalVersion, mappedImage, _Config._ShowThumbSizeSm);
                            //now mapped image should be valid
                        }
                        else
                        {
                            imagePass = false;
                            _Error.LogException(new Exception(string.Format("ShowId: {0} ShowId: {1} - {2} - originalImage version does not exist", 
                                s.Id.ToString(), "", s.Name)));
                        }
                    }

                    if(imagePass)
                    {
                        s.DisplayUrl = System.IO.Path.GetFileName(mappedImage);
                        //s.Save();
                    }



                }
            }
        }

        private class InvoiceBrief
        {
            //public int Id = 0;
            //public decimal Cost = 0;
            //public string Method = string.Empty;

            //public InvoiceBrief(IDataReader dr)
            //{
            //    Id = (int)dr["Id"];
            //    Cost = (decimal)dr["MerchandiseShipping"];
            //    Method = dr["MerchandiseShipMethod"].ToString();
            //}
        }
        
        protected void btnPwds_Click(object sender, EventArgs e)
        {
            int collSize = 0;
            int processedSize = 0;
            CashewCollection coll = new CashewCollection().Load();
            foreach (Cashew c in coll)
            {
                c.EMonth = "0";
                c.EYear = "2001";
                c.EName = "mrbiz";
                c.ENumber = "1234";
                c.Save();

                processedSize++;
            }
            collSize = coll.Count;
            /*
            AspnetUserCollection coll = new AspnetUserCollection().Where("UserName", SubSonic.Comparison.LessOrEquals, "z").Load();

            collSize = coll.Count;

            foreach(AspnetUser u in (coll))
            {
                MembershipProvider mp = Membership.Providers["AdminMembershipProvider"];

                MembershipUser user = mp.GetUser(u.UserName, false);
                
                ProfileCommon p = Profile.GetProfile(u.UserName);
                string[] roles = Roles.GetRolesForUser(u.UserName);
                if (roles.Length == 1)
                {
                    string newPass = Utils.ParseHelper.GenerateRandomPassword(7);
                    string pass = user.GetPassword();
                    user.ChangePassword(pass, newPass);
                    user.ChangePasswordQuestionAndAnswer(pass, "some question", Utils.ParseHelper.GenerateRandomPassword(7));
                    processedSize++;
                }
            }*/

            litPwds.Text = string.Format("{0} users processed out of {1}", processedSize.ToString(), collSize.ToString());
        }
        
        protected void btnCleanTune_Click(object sender, EventArgs e)
        {
            TuneCollection coll = new TuneCollection().Load();

            int cleansed = 0;

            foreach (Tune t in coll)
            {
                if (t.FileName.IndexOf("%") != -1)
                {
                    t.FileName = System.Web.HttpUtility.UrlDecode(t.FileName);
                    t.Save();
                    cleansed++;
                }
                //if (t.OriginalFileName.IndexOf("%") != -1)
                //{
                //    t.OriginalFileName = System.Web.HttpUtility.UrlDecode(t.OriginalFileName);
                //    t.Save();
                //    cleansed++;
                //}
            }

            cleantune.Text = string.Format("{0} filenames out of {1} cleansed", cleansed, coll.Count);
        }
        
        protected void btnMerchShipping_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            List<InvoiceBrief> coll = new List<InvoiceBrief>();

            ////see 2 ways - use objects
            //string sql = "SELECT Id, MerchandiseShipping, MerchandiseShipMethod FROM Invoice WHERE [InvoiceStatus] <> 'NotPaid' AND ";
            //sql += "[MerchandiseShipMethod] IS NOT NULL AND Len(Ltrim(RTrim([MerchandiseShipMethod]))) > 0 AND [MerchandiseShipMethod] <> 'In Store Pickup' ";

            //using (IDataReader dr = SubSonic.DataService.GetReader(new SubSonic.QueryCommand(sql)))
            //{
            //    while (dr.Read())
            //    {
            //        coll.Add(new InvoiceBrief(dr));
            //    }
            //}            

            //foreach (InvoiceBrief invoice in coll)
            //{
            //    sb.Length = 0;
            //    SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(string.Empty);

            //    //estimated ship date and description list
            // _Config._DisplayEstimatedShipDates && 
            //    DateTime shipDate = Wcss._Shipper.CalculateShipDate(invoice.InvoiceDate);
            //    string description = string.Empty;

            //    cmd.AddParameter("@invoiceId", invoice.Id, DbType.Int32);
            //    cmd.AddParameter("@shipDate", shipDate, DbType.DateTime);
            //    cmd.AddParameter("@method", invoice.Method);
            //    cmd.AddParameter("@price", invoice.Cost);


            //    sb.Append("DECLARE @idx int ");

            //    //if there is no shipping method
            //    sb.Append("IF NOT EXISTS (SELECT * FROM [InvoiceItem] WHERE [TInvoiceId] = @invoiceId AND [vcContext] = 'shippingmerch') BEGIN ");

            //    sb.Append("INSERT [InvoiceItem] ([TInvoiceId], [vcContext], [dtDateOfShow], [MainActName], [Description], [mPrice], [iQuantity], [PurchaseAction]) ");
            //    sb.AppendFormat("VALUES (@invoiceId, 'shippingmerch', @shipDate, @method, @desc, @price, 1, 'Purchased') ");

            //    sb.Append("SELECT @idx = SCOPE_IDENTITY() END ELSE BEGIN ");

            //    sb.Append("DECLARE @existingId int ");
            //    sb.Append("SELECT @existingId = [Id] FROM [InvoiceItem] WHERE [TInvoiceId] = @invoiceId AND [vcContext] = 'shippingmerch'");

            //    sb.Append("UPDATE [InvoiceItem] SET [dtDateOfShow] = @shipDate, [Description] = @desc WHERE [Id] = @existingId ");
            //    sb.Append("SET @idx = @existingId END ");


            //    foreach (InvoiceItem ii in invoice.MerchItems)
            //    {
            //        string dn = ii.MerchRecord.DisplayNameWithAttribs;
            //        description += string.Format("{0},", (dn.Length > 45) ? string.Format("{0}..", dn.Substring(0, 45).Trim()) : dn);

            //        sb.AppendFormat("UPDATE [InvoiceItem] SET [TShipItemId] = @idx, [ShippingMethod] = @method WHERE [Id] = {0} ", ii.Id);
            //    }

            //    //must be set after loop
            //    cmd.AddParameter("@desc", description);

            //    cmd.CommandSql = sb.ToString();

            //    try
            //    {
            //        //SubSonic.DataService.ExecuteScalar(cmd);
            //    }
            //    catch (Exception ex)
            //    {
            //        _Error.LogException(ex);
            //        throw new Exception(string.Format("Confirmation Sql Error.\r\n{0}\r\n{1}", ex.Message, ex.StackTrace));
            //    }               

            //}
        }

        protected void btnProduct_Click(object sender, EventArgs e)
        {
            //retrieve UserId, UserName, eNumber, eMonth, eYear, eName from cashew and re-encrypt
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            //create a backup of the cashew

            //if the table does not exist - create it - this table will track our progress
            sb.Append("IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Reproduct]') AND type in (N'U'))	");
            sb.Append("BEGIN CREATE TABLE Reproduct ( [Id] [int], Processed bit default 0 ) END ");
            //sb.Append("IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Cashew_Backup]') AND type in (N'U')) ");
            //sb.AppendFormat("BEGIN SELECT * INTO Cashew_Backup FROM Cashew END END ", DateTime.Now.ToString("yyMMdd"));
            sb.Append("INSERT Reproduct(id) SELECT TOP 25000 [Id] FROM Invoice WHERE Invoice.[vcProducts] is null and Invoice.[id] NOT IN (SELECT Id FROM Reproduct) ");
            sb.Append("SELECT top 25000 i.* ");
            sb.Append("FROM Invoice i, Reproduct r ");
            sb.Append("WHERE i.[Id] = r.[Id] AND r.[Processed] = 0 ORDER BY i.[id] ");

            InvoiceCollection coll = new InvoiceCollection();
            coll.LoadAndCloseReader(SubSonic.DataService.GetReader(new SubSonic.QueryCommand(sb.ToString(), SubSonic.DataService.Provider.Name)));

            foreach(Invoice i in coll)
            {
                System.Text.StringBuilder pr = new System.Text.StringBuilder();

                foreach(InvoiceItem ii in i.InvoiceItemRecords())
                {
                    pr.Append(InvoiceItem.FormatItemProductListing(ii));
                }

                SubSonic.QueryCommand cmd = new SubSonic.QueryCommand("UPDATE Invoice SET vcProducts = @product WHERE ID = @idx; UPDATE reproduct SET processed = 1 WHERE ID = @idx",
                    SubSonic.DataService.Provider.Name);
                cmd.Parameters.Add("@product", pr.ToString());
                cmd.Parameters.Add("@idx", i.Id, DbType.Int32);

                //i.VcProducts = pr.ToString();

                try
                {
                    SubSonic.DataService.ExecuteQuery(cmd);
                    //i.Save();

                    //SubSonic.QueryCommand cmd1 = new SubSonic.QueryCommand(string.Format("UPDATE reproduct SET processed = 1 WHERE ID = {0}", i.Id));
                    //SubSonic.DataService.ExecuteQuery(cmd1);
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                }
            }
        }

        protected void btnRename_Click(object sender, EventArgs e)
        {
            ShowCollection coll = new ShowCollection();

            coll.LoadAndCloseReader(Show.FetchAll());

            foreach (Show s in coll)
            {
                JShowActCollection acts = new JShowActCollection();
                //int dates = s.ShowDateRecords().Count;
                //int sellouts = 0;

                foreach (ShowDate sd in s.ShowDateRecords())
                {
                    //if (sd.ShowStatus.Name.ToLower() == _Enums.ShowDateStatus.SoldOut.ToString().ToLower())
                    //    sellouts += 1;

                    foreach (JShowAct j in sd.JShowActRecords())
                        if (j.TopBilling_Effective)
                        {
                            JShowActCollection jColl = new JShowActCollection();
                            jColl.AddRange(acts.GetList().FindAll(delegate(JShowAct match) { return (match.TActId == j.TActId); }));

                            if (jColl.Count == 0)
                                acts.Add(j);
                        }
                }

                if (acts.Count > 1)
                    acts.Sort("IDisplayOrder", true);
                ActCollection sortedHeads = new ActCollection();
                foreach (JShowAct jsa in acts)
                    sortedHeads.Add(jsa.ActRecord);

                string rename = Show.CalculatedShowName(s.FirstDate, s.VenueRecord, sortedHeads);
                s.ResetCalculatedActName();
                s.Name = rename;

                //if(sellouts == dates)
                //    s.BSoldOut

                try
                {
                    s.Save();
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                }
            }
        }
        
        protected void btnPass_Click(object sender, EventArgs e)
        {
            AspnetUsersOldCollection coll = new AspnetUsersOldCollection();
            string username = "knudsenm@colorado.edu";
            coll.LoadAndCloseReader(SubSonic.DataService.GetReader(new SubSonic.QueryCommand(string.Format("Select * from aspnet_users_old where username = '{0}'", username), 
                SubSonic.DataService.Provider.Name)));

            pass.Text = string.Format("count: {0} - userid: {1} - pass: {2}", coll.Count, coll[0].UserId.ToString(), coll[0].Password);
        }
        
        protected void btnRecrypt_Click(object sender, EventArgs e)
        {
            //retrieve UserId, UserName, eNumber, eMonth, eYear, eName from cashew and re-encrypt
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            //create a backup of the cashew

            //if the table does not exist - create it - this table will track our progress
            sb.Append("IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Recash]') AND type in (N'U'))	");
            sb.Append("BEGIN CREATE TABLE Recash ( [Id] [int], Processed bit default 0 ) END ");
            //sb.Append("IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Cashew_Backup]') AND type in (N'U')) ");
            //sb.AppendFormat("BEGIN SELECT * INTO Cashew_Backup FROM Cashew END END ", DateTime.Now.ToString("yyMMdd"));
            sb.Append("INSERT Recash(id) SELECT TOP 3000 [Id] FROM Cashew WHERE Cashew.[id] NOT IN (SELECT Id FROM Recash) ");
            sb.Append("SELECT c.[Id], u.[LoweredUserName] as 'UserName', c.[UserId], c.[eNumber], c.[eMonth], c.[eYear], c.[eName] ");
            sb.Append("FROM Aspnet_Users u, Cashew c, Recash r ");
            sb.Append("WHERE c.[Id] = r.[Id] AND r.[Processed] = 0 AND c.[UserId] = u.[UserId] ORDER BY c.[dtStamp] ");

            using (IDataReader dr = SubSonic.DataService.GetReader(new SubSonic.QueryCommand(sb.ToString(), SubSonic.DataService.Provider.Name)))
            {
                while (dr.Read())
                {
                    string id = dr["Id"].ToString();

                    try
                    {   
                        string userName = dr["UserName"].ToString();
                        string userId = dr["UserId"].ToString();
                        string eNumber = dr["eNumber"].ToString();
                        string eMonth = dr["eMonth"].ToString();
                        string eYear = dr["eYear"].ToString();
                        string eName = dr["eName"].ToString();


                        /*this changed the encryption key from the old way - username to the new way (when combinbed with method below
                        string oldNumber = Utils.Crypt.Decrypt(eNumber, userName);
                        if (oldNumber.Length > 4)
                            oldNumber = oldNumber.Substring((oldNumber.Length - 4), 4);
                        string oldMonth = Utils.Crypt.Decrypt(eMonth, userName);
                        string oldYear = Utils.Crypt.Decrypt(eYear, userName);
                        string oldName = Utils.Crypt.Decrypt(eName, userName);
                        */

                        /*now we want to decrypt from the old fox way and use the new encryptor*/
                        string oldNumber = Utils.ObsCrypt.Decrypt(eNumber, userName.ToLower());
                        if (oldNumber.Length > 4)
                            oldNumber = oldNumber.Substring((oldNumber.Length - 4), 4);
                        string oldMonth = Utils.ObsCrypt.Decrypt(eMonth, userName.ToLower());
                        string oldYear = Utils.ObsCrypt.Decrypt(eYear, userName.ToLower());
                        string oldName = eName;// Utils.ObsCrypt.Decrypt(eName, userName.ToLower());//TODO now we have to get the name from?


                        string encryptor = Cashew.FormatEncryptor(userId);

                        string newNum = Utils.Crypt.Encrypt(oldNumber, encryptor);
                        string newMonth = Utils.Crypt.Encrypt(oldMonth, encryptor);
                        string newYear = Utils.Crypt.Encrypt(oldYear, encryptor);
                        string newName = Utils.Crypt.Encrypt(oldName, encryptor);

                        //update the row
                        sb.Length = 0;
                        sb.AppendFormat("UPDATE Cashew SET [eNumber] = '{0}', [eMonth] = '{1}', [eYear] = '{2}', [eName] = '{3}' WHERE [Id] = {4} ",
                            newNum, newMonth, newYear, newName, id);
                        SubSonic.DataService.ExecuteQuery(new SubSonic.QueryCommand(sb.ToString(), SubSonic.DataService.Provider.Name));


                        sb.Length = 0;
                        sb.AppendFormat("UPDATE Recash SET Processed = 1 WHERE Id = {0} ", id);
                        SubSonic.DataService.ExecuteQuery(new SubSonic.QueryCommand(sb.ToString(), SubSonic.DataService.Provider.Name));

                    }
                    catch (Exception ex)
                    {
                        _Error.LogException(ex);
                    }
                }
            }
        }

        /// <summary>
        /// basically, this finds the list of matching invoiceitems , then refunds the item on its parent invoice.
        /// The query should really check the appId
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnMaskEarly_Click(object sender, EventArgs e)
        {
            InvoiceItemCollection coll = new InvoiceItemCollection();
         
            System.Text.StringBuilder sb = new System.Text.StringBuilder();            
            //sb.AppendFormat("Select * from invoiceitem where tshowticketid = 10104 and purchaseaction = 'purchased' and shippingmethod = 'will call' ");
            sb.AppendFormat("{0}", hidEarly.Value);
            SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sb.ToString(), SubSonic.DataService.Provider.Name);
            coll.LoadAndCloseReader(SubSonic.DataService.GetReader(cmd));

            MassRefund(hidEarlyDescriptor.Value, coll);            
        }

        protected void btnMaskLate_Click(object sender, EventArgs e)
        {
            InvoiceItemCollection coll = new InvoiceItemCollection();

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //sb.AppendFormat("Select * from invoiceitem where tshowticketid = 10119 and purchaseaction = 'purchased' and shippingmethod = 'will call' ");
            sb.AppendFormat("{0}", hidLate.Value);
            SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sb.ToString(), SubSonic.DataService.Provider.Name);
            coll.LoadAndCloseReader(SubSonic.DataService.GetReader(cmd));

            MassRefund(hidLateDescriptor.Value, coll);
        }
     
        protected void btnWinter_Click(object sender, EventArgs e)
        {
            InvoiceCollection coll = new InvoiceCollection();

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            //select top 1 tinvoiceid from murphund.dbo.murphinv where lock = 0
            //sb.AppendFormat("update murphund.dbo.murphinv set lock = 0; ");
            sb.AppendFormat("select top {0} tinvoiceid into #tmpids from murphund.dbo.murphinv where lock = 0 and tinvoiceid <= {1} order by tinvoiceid; select tinvoiceid from #tmpids; ",
                hidWinterTopNumberForQuery.Value, hidWinterMaxInvoiceId.Value);
            sb.AppendFormat("update murphund.dbo.murphinv set lock = 1 where tinvoiceid in (select tinvoiceid from #tmpids); ");
            sb.AppendFormat("drop table #tmpids; ");

            SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sb.ToString(), SubSonic.DataService.Provider.Name);
            //Debug.WriteLine(DateTime.Now.ToLongTimeString());
            using (IDataReader dr = SubSonic.DataService.GetReader(cmd))
            {
                while (dr.Read())
                {
                    int idx = (int)dr.GetValue(dr.GetOrdinal("tinvoiceid"));

                    int exists = coll.GetList().FindIndex(delegate(Invoice match) { return (match != null && match.Id == idx); });

                    if (exists == -1)
                    {
                        Invoice inv = Invoice.FetchByID(idx);
                        coll.Add(inv);
                    }
                    //else
                    //    Debug.WriteLine(string.Format("{0} Already exists!", idx.ToString()));
                }
            }

            //Debug.WriteLine(DateTime.Now.ToLongTimeString());

            ///*test*/
            //Invoice inv = Invoice.FetchByID(55531);
            //coll.Add(inv);

            //return;
            ExecuteRefunds(hidWinterDescriptor.Value, coll);
            
        }

        public void ExecuteRefunds(string refundDescription, InvoiceCollection coll)
        {
            foreach (Invoice i in coll)
            {
                //reset grid
                GridView1.DataSource = null;
                GridView1.DataBind();

                //load the user's profile 
                //ProfileCommon userProfile = Profile.GetProfile(i.AspnetUserRecord.UserName);

                //do the refund                
                string proc = "AuthNet";

                //construct list of items to refund in the invoice
                List<RefundListItem> list = new List<RefundListItem>();
                InvoiceItemCollection items = new InvoiceItemCollection();
                items.AddRange(i.InvoiceItemRecords());

                DateTime dtStart = DateTime.Parse(hidWinterTixDateStart.Value);
                DateTime dtEnd = DateTime.Parse(hidWinterTixDateEnd.Value);

                foreach (InvoiceItem ii in items)
                {
                    bool refund = false;

                    if (ii.PurchaseAction == _Enums.PurchaseActions.Purchased.ToString() && ii.LineItemTotal > 0)
                    {
                        //we are refunding tickets
                        if (ii.IsTicketItem && ii.DateOfShow >= dtStart && ii.DateOfShow <= dtEnd)//date set in a hidden field
                            refund = true;
                        //merchdownloads within the period specified
                        //else if (ii.IsMerchandiseItem && ii.MerchRecord.IsDownloadDelivery && ii.MerchRecord.Id >= 10926 && ii.MerchRecord.Id <= 10951)
                        //    refund = true;
                        //shipping except for these items that have already shipped
                        else if (ii.IsShippingItem_Ticket)// && ii.DateShipped < DateTime.MaxValue)
                            refund = true;

                        //downloads
                        else if (ii.IsMerchandiseItem && ii.IsDownloadDelivery)
                            refund = true;


                        //processing if no merch(except downloads)
                        else if (ii.IsProcessingFee)
                        {
                            //examine items collection for merch that is not downloads
                            InvoiceItemCollection merches = new InvoiceItemCollection();
                            merches.AddRange(items.GetList().FindAll(delegate(InvoiceItem match)
                            {
                                return (match.LineItemTotal > 0 &&
                                    match.IsMerchandiseItem && (match.MerchRecord.IsParcelDelivery || match.TMerchId == 11577));//disqualify past download (tabby) - kind of hacky, but it will do
                            }));

                            refund = (merches.Count == 0);

                            //mainactname <> 2013.12.27 :: THE TABERNACLE - tmerchid = 11577
                        }
                        
                        //no discounts are affected
                        //no note items
                    }

                    //if we are refunding this item - add it to the grid of refundables
                    if (refund)
                    {
                        RefundListItem rli = new RefundListItem(ii);
                        list.Add(rli);
                    }

                }



                GridView1.DataSource = list;
                GridView1.DataBind();


                //setup logging - will log into the Logs/WillCallErrorLogs folder
                string result = string.Empty;
                string logit = string.Format("Customer: {0} \r\n", i.AspnetUserRecord.UserName);
                logit += string.Format("Invoice: {0} - {1}\r\n", i.Id, i.UniqueId);
                logit += string.Format("InvoiceDate: {0}\r\n", i.InvoiceDate.ToString("MM/dd/yyyy hh:mmtt"));


                try
                {
                    //do not process prepaid or gift cards
                    AuthorizeNetCollection auths = new AuthorizeNetCollection();
                    auths.AddRange(i.AuthorizeNetRecords());

                    int prepaid = -1;
                    prepaid = auths.GetList().FindIndex(delegate(AuthorizeNet match) { return 
                        (
                        match.IsAuthorized == true &&
                        match.TransactionType.ToLower() == "auth_capture" && 

                        (match.NameOnCard.ToLower() == "a gift for you" ||
                        match.NameOnCard.ToLower() == "gift recipient" || 
                        match.NameOnCard.ToLower() == "visa" ||
                        match.NameOnCard.ToLower() == "american express" || 
                        match.NameOnCard.ToLower() == "wachovia" || 
                        match.NameOnCard.ToLower() == "gift card" ||
                        match.NameOnCard.ToLower() == "5524860000517188"
                        )
                        ); 
                    });

                    if (prepaid != -1)
                        result = "This transaction was made with a gift card or prepaid card and will need to be refunded manually.";
                    else
                    {
                        //string emailLink = "For questions regarding refunds please visit<br/><a href=\"http://sts9.com/?p=6557\">http://sts9.com/?p=6557</a><br/>for more information";

                        string emailLink = hidWinterLink.Value;

                        result = OrderRefund.DoRefund(proc, i, GridView1,
                            refundDescription, this.Profile.UserName, this.Request.UserHostAddress, emailLink);
                    }
                    
                    //result = string.Empty;
                }
                catch (Exception ex)
                {
                    result = ex.Message;
                }

                if (result == "SUCCESS")
                {
                    //log success into refund log
                    _Error.LogToFile(logit, refundDescription);

                    if (result.IndexOf("SYNCUSER") != -1)
                        WillCallWeb.StoreObjects.SaleItem_StoreCredit.Profile_StoreCredit_Sync(Atx.CurrentInvoiceRecord.AspnetUserRecord.UserName, this.Profile.UserName, true);
                }
                else
                {
                    logit += string.Format("Reason: {0}\r\n", result);
                    //log error
                    _Error.LogToFile(logit, string.Format("{0}_FAILURES", refundDescription));
                }

                GridView1.DataSource = null;
                GridView1.DataBind();

            }//end of loop thru invoices
        }


        /// <summary>
        /// July 19 2011
        /// I have not tested this yet - but should be pretty close to going live
        /// </summary>
        /// <param name="tShowTicketId"></param>
        /// <param name="amountDiscountPerItem"></param>
        /// <param name="reasonForDiscount"></param>
        private void PostSaleReduceTicketPriceRefund(int tShowTicketId, decimal amountDiscountPerItem, string reasonForDiscount)
        {
            List<string> failedInvoices = new List<string>();
            InvoiceCollection _coll = new InvoiceCollection();

            //from that list - get the invoices - with the identified items
            string sql = "SELECT i.* FROM [InvoiceItem] ii LEFT OUTER JOIN [Invoice] i ON ii.[tInvoiceId] = i.[Id] WHERE ii.[PurchaseAction] = 'Purchased' AND ii.[tShowTicketId] = @showticketid ";
            SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sql, SubSonic.DataService.Provider.Name);
            cmd.Parameters.Add("@showticketid", tShowTicketId, DbType.Int32);

            try
            {
                _coll.LoadAndCloseReader(SubSonic.DataService.GetReader(cmd));
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
            }

            if (_coll.Count > 0)
            {
                //loop thru invoices and extract items in question
                foreach (Invoice i in _coll)
                {
                    //load the user's profile 
                    ProfileCommon userProfile = Profile.GetProfile(i.AspnetUserRecord.UserName);

                    //GridView1.DataSource = null;
                    //List<RefundListItem> list = new List<RefundListItem>();
                    ////GridView1.DataBind();
                    List<InvoiceItem> items = new List<InvoiceItem>();
                    items.AddRange(i.InvoiceItemRecords().GetList()
                        .FindAll(delegate(InvoiceItem match) { return (match.TShowTicketId == tShowTicketId && match.PurchaseAction == _Enums.PurchaseActions.Purchased.ToString()); } ));

                    foreach (InvoiceItem ii in items)
                    {
                        int qty = ii.IQuantity;
                        decimal amountToCredit = amountDiscountPerItem * qty;

                        //attempt the refund
                        try
                        {
                            AuthorizeNet auth = OrderRefund.AuthorizeNetRefund(i, User.Identity.Name, amountToCredit, reasonForDiscount, this.Request.UserHostAddress);

                            if(auth.IsAuthorized)
                            {
                                //if good....
                                //update item - no leave original purchase alone
                                //update invoice
                                i.InvoiceStatus = _Enums.InvoiceStatii.PartiallyRefunded.ToString();
                                i.TotalRefunds += amountToCredit;
                                i.Save();
                                //add note item - discount
                                InvoiceItem discountItem = new InvoiceItem();
                                discountItem.Context = _Enums.InvoiceItemContext.discount;
                                discountItem.Guid = Guid.NewGuid();
                                discountItem.TInvoiceId = i.Id;
                                discountItem.MainActName = reasonForDiscount;
                                discountItem.Price = amountToCredit;
                                discountItem.Quantity = 1;
                                discountItem.PurchaseAction = _Enums.PurchaseActions.Purchased.ToString();
                                discountItem.BRTS = false;
                                discountItem.DtStamp = DateTime.Now;
                                discountItem.Save();

                                //add invoice event
                                EventQ.CreateTicketRefundEvent(DateTime.Now, DateTime.Now, User.Identity.Name, i.PurchaseEmail, i, tShowTicketId, false, false, false, reasonForDiscount);
                            }
                            else
                                failedInvoices.Add(string.Format("InvoiceId: {0} - failed refund process", i.Id.ToString()));
                            
                        }
                        //else save a list of failures to some list
                        catch(Exception ex)
                        {
                            failedInvoices.Add(string.Format("InvoiceId: {0} - failed refund process: EX: {1}", i.Id.ToString(), ex.Message));
                        }
                    }
                }
            }

            //if there are errors than log somewhere
            if (failedInvoices.Count > 0)
            {
                System.Text.StringBuilder builder = new System.Text.StringBuilder();
                foreach(string s in failedInvoices)
                    builder.AppendLine(s);

                _Error.LogToFile(builder.ToString(), string.Format("{0}_PostSaleReduceTicketPriceRefund_Failures", DateTime.Now.ToString("yyMMddhhmmtt")));
            }
        }


        private void MassRefund(string showIdentifier, InvoiceItemCollection coll)
        {
            foreach (InvoiceItem ii in coll)
            {
                GridView1.DataSource = null;
                GridView1.DataBind();

                Invoice _invoice = ii.InvoiceRecord;

                //load the user's profile 
                ProfileCommon userProfile = Profile.GetProfile(_invoice.AspnetUserRecord.UserName);

                //do the refund
                //_Enums.FundsProcessor processor = _Enums.FundsProcessor.AuthorizeNet;
                //string proc = "AuthNet";
                string description = showIdentifier;

                RefundListItem rli = new RefundListItem(ii);
                List<RefundListItem> list = new List<RefundListItem>();
                list.Add(rli);

                GridView1.DataSource = list;
                GridView1.DataBind();

                string result = string.Empty;
                string logit = string.Format("Customer: {0} \r\n", userProfile.UserName);
                logit += string.Format("Invoice: {0} - {1}\r\n", _invoice.Id, _invoice.UniqueId);
                logit += string.Format("InvoiceDate: {0}\r\n", _invoice.InvoiceDate.ToString("MM/dd/yyyy hh:mmtt"));

                try
                {
                    //result = OrderRefund.DoRefund(userProfile, proc, _invoice, GridView1,
                    //    description, this.Profile.UserName, this.Request.UserHostAddress);
                    result = string.Empty;
                }
                catch(Exception ex)
                {
                    result = ex.Message;
                }

                if (result == "SUCCESS")
                {
                    //log success into refund log
                    _Error.LogToFile(logit, showIdentifier);

                    if (result.IndexOf("SYNCUSER") != -1)
                        WillCallWeb.StoreObjects.SaleItem_StoreCredit.Profile_StoreCredit_Sync(Atx.CurrentInvoiceRecord.AspnetUserRecord.UserName, this.Profile.UserName, true);
                }
                else
                {
                    logit += string.Format("Reason: {0}\r\n", result);
                    //log error
                    _Error.LogToFile(logit, string.Format("{0}_FAILURES", showIdentifier));
                }

                GridView1.DataSource = null;
                GridView1.DataBind();
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;

            RefundListItem rli = (RefundListItem)e.Row.DataItem;

            if (rli != null)
            {
                CheckBox chkSelect = (CheckBox)e.Row.FindControl("chkSelect");
                if (chkSelect != null)
                    chkSelect.Checked = true;

                DropDownList qty = (DropDownList)e.Row.FindControl("ddlQty");
                int max = rli.Quantity;
                for (int i = 1; i <= max; i++)
                    qty.Items.Add(new ListItem(i.ToString()));
                qty.SelectedIndex = qty.Items.Count - 1;
                qty.Enabled = (qty.Items.Count > 1);

                //tickets have service charges
                CheckBox chkService = (CheckBox)e.Row.FindControl("chkService");
                Literal litService = (Literal)e.Row.FindControl("litService");
                if (rli.Context == _Enums.InvoiceItemContext.ticket)
                {
                    chkService.Visible = true;
                    chkService.Checked = bool.Parse(hidRefundServiceFees.Value);
                    litService.Text = string.Format("{0}", rli.Service.ToString("n"));
                }
                else
                {
                    chkService.Visible = false;
                    litService.Text = string.Empty;
                }

                Literal description = (Literal)e.Row.FindControl("litDescription");
                if (description != null)
                {
                    InvoiceItem item = InvoiceItem.FetchByID(rli.ItemId);

                    //if an item has already gone thru its process or poses a risk if it were to be refunded - highlight in red
                    //ex: items that have shipped
                    if (item != null && item.DateShipped < DateTime.Now)
                        description.Text = string.Format("<div style=\"color: Red;\">{0}</div>", rli.Description);
                    else
                        description.Text = rli.Description;
                }
            }
        }

        protected void btnCleanAct_Click(object sender, EventArgs e)
        {
            string actName = txtAct.Text.Trim();
            if (actName.Length > 0)
            {
                SubSonic.QueryCommand cmd = new SubSonic.QueryCommand("UPDATE Act SET PictureUrl = null, iPicWidth = 0, ipicwidth = 0 WHERE NameRoot = @actName ",
                    SubSonic.DataService.Provider.Name);
                cmd.Parameters.Add("@actName", actName);

                try
                {
                    SubSonic.DataService.ExecuteQuery(cmd);
                    litCleanAct.Text = "Success";
                    txtAct.Text = string.Empty;
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                    litCleanAct.Text = ex.Message;                    
                }
            }
        }
}
}