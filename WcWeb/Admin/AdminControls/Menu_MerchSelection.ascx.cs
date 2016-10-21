using System;
using System.Web.UI;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Wcss;

/*
 * <asp:Button ID="btnCsv_NoImages" runat="server" CssClass="btnmed" Width="150px" Text="Shopify CSV - No Images" CommandName="csv" OnClick="CSV_Click" 
            OnClientClick="return confirm('This will create a CSV file for download. Includes parents and inventory items only - no images (other than the default image). Would you like to proceed?');" />
 */
namespace WillCallWeb.Admin.AdminControls
{
    [ToolboxData("<{0}:Menu_MerchSelection runat=\"Server\" EditorPage=\"ItemEdit\" ></{0}:Menu_MerchSelection>")]
    public partial class Menu_MerchSelection : BaseControl, System.Web.UI.IPostBackEventHandler
    {
        #region PostBack Handling for ticket links
        //RefreshCurrentMerchRecord
        public void RaisePostBackEvent(string eventArgument)
        {
            List<string> parts = new List<string>();
            parts.AddRange(eventArgument.ToLower().Split('~'));

            if (parts.Count > 0)
            {
                string cmd = parts[0].ToLower();
                switch (cmd)
                {
                    case "addnewitem":
                        Atx.SetCurrentMerchRecord(0);
                        base.Redirect(string.Format("/Admin/MerchEditor.aspx?p={0}", this.EditorPage));
                        break;
                    case "refresh":
                        Atx.RefreshCurrentMerchRecord();
                        //chkActive.DataBind();
                        litMenu.DataBind();
                        break;
                    case "toggleactive":
                        Atx.AdminMerchListingContext = (Atx.AdminMerchListingContext == 1) ? 0 : 1;
                        litMenu.DataBind();
                        break;
                }
            }
        }

        #endregion

        protected void CSV_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string fileAttachmentName = string.Empty;

            fileAttachmentName = string.Format("attachment; filename=ShopifyMerchExport_{0}.csv",
                DateTime.Now.Ticks.ToString());

            Wcss.QueryRow.ShopifyExport shopify;
            if(btn.ID == "btnCsv_Full")
                shopify = new Wcss.QueryRow.ShopifyExport(true);
            else 
                shopify = new Wcss.QueryRow.ShopifyExport(false);

            shopify.GetCSVReport_ShopifyMerchExport(fileAttachmentName, null);
        }

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        //Title and LinkToPage may not always match
        private string _title = "Merch";
        public string Title { get { return _title; } set { _title = value; } }
        private string _linkToPage = "Merch";
        public string LinkToPage { get { return _linkToPage; } set { _linkToPage = value; } }

        private string _editorPage = null;
        public string EditorPage
        {
            get
            {
                if (_editorPage == null)
                    _editorPage = "ItemEdit";//other option is "altedit"

                return _editorPage;
            }
            set
            {
                _editorPage = value;
            }
        }

        protected override object SaveControlState()
        {
            object[] ctlState = new object[4];
            ctlState[0] = base.SaveControlState();
            ctlState[1] = this._linkToPage;
            ctlState[2] = this._title;
            ctlState[3] = this._editorPage;
            return ctlState;
        }
        protected override void LoadControlState(object savedState)
        {
            if (savedState == null)
                return;
            object[] ctlState = (object[])savedState;
            base.LoadControlState(ctlState[0]);
            this._linkToPage = (string)ctlState[1];
            this._title = (string)ctlState[2];
            this._editorPage = (string)ctlState[3];
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Page.RegisterRequiresControlState(this);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.btnCsv_Full);
            //scriptManager.RegisterPostBackControl(this.btnCsv_NoImages);

            litMenu.DataBind();
        }
        protected void litMenu_DataBinding(object sender, EventArgs e)
        {
            //setup featured items
            MerchCollection promotions = new MerchCollection();
            MerchCollection bundles = new MerchCollection();
            MerchCollection featured = new MerchCollection();

            Literal lit = (Literal)sender;
            sb.Length = 0;
            int depth = 0;

            //loop thru divisions
            MerchDivisionCollection divs = new MerchDivisionCollection();
            divs.AddRange(_Lookits.MerchDivisions);
            if (divs.Count > 1)
                divs.Sort("IDisplayOrder", true);

            foreach (MerchDivision div in divs)
            {
                //begin DIV structure
                sb.AppendFormat("{0}{1}<li class=\"mnudiv\">{2}{3}", Utils.Constants.NewLine, Utils.Constants.Tabs(depth),
                    div.Name, (div.MerchCategorieRecords().Count > 0) ? " &#187;" : string.Empty);

                //loop thru categories
                MerchCategorieCollection cats = new MerchCategorieCollection();
                cats.AddRange(div.MerchCategorieRecords());
                if (cats.Count > 1)
                    cats.Sort("IDisplayOrder", true);

                if (cats.Count > 0)
                {
                    //start structure
                    sb.AppendFormat("{0}{1}<ul>", Utils.Constants.NewLine, Utils.Constants.Tabs(depth++));

                    foreach (MerchCategorie cat in cats)
                    {
                        sb.AppendFormat("{0}{1}<li><a href=\"#\">{2}{3}</a>", Utils.Constants.NewLine, Utils.Constants.Tabs(depth),
                            cat.Name, (cat.MerchJoinCatRecords().Count > 0) ? " &#187;" : string.Empty);

                        if (cat.MerchJoinCatRecords().Count > 0)
                        {
                            //start structure
                            sb.AppendFormat("{0}{1}<ul>", Utils.Constants.NewLine, Utils.Constants.Tabs(depth++));

                            MerchCollection parentMerch = new MerchCollection();
                            foreach (MerchJoinCat join in cat.MerchJoinCatRecords())
                            {
                                if(Atx.AdminMerchListingContext == 0 || join.MerchRecord.IsActive)
                                    parentMerch.Add(join.MerchRecord);
                            }

                            if (parentMerch.Count > 1)
                                parentMerch.Sort("Name", true);

                            foreach (Merch merchItem in parentMerch)
                            {
                                if (Atx.AdminMerchListingContext == 0 || (Atx.AdminMerchListingContext == 1 && merchItem.IsActive))
                                {
                                    if (merchItem.IsPromotionalItem)
                                    {
                                        int index = promotions.GetList().FindIndex(delegate(Merch match) { return (match.Id == merchItem.Id); });
                                        if(index == -1)
                                            promotions.Add(merchItem);
                                    }

                                    if (merchItem.IsFeaturedItem)
                                    {
                                        int index = featured.GetList().FindIndex(delegate(Merch match) { return (match.Id == merchItem.Id); });
                                        if (index == -1)
                                            featured.Add(merchItem);
                                    }

                                    if (merchItem.MerchBundleRecords().Count > 0)
                                    {
                                        int index = bundles.GetList().FindIndex(delegate(Merch match) { return (match.Id == merchItem.Id); });
                                        if (index == -1)
                                            bundles.Add(merchItem);
                                    }

                                    FillListItem(sb, depth, merchItem);
                                }
                            }

                            //end structure
                            sb.AppendFormat("{0}{1}</ul>", Utils.Constants.NewLine, Utils.Constants.Tabs(depth--));
                        }
                    }

                    //end structure
                    sb.AppendFormat("{0}{1}</ul>", Utils.Constants.NewLine, Utils.Constants.Tabs(depth--));
                }

                //end DIV structure
                sb.AppendFormat("{0}{1}</li>", Utils.Constants.NewLine, Utils.Constants.Tabs(depth));
            }

            //end framework
            sb.AppendFormat("{0}{1}</ul>", Utils.Constants.NewLine, Utils.Constants.Tabs(--depth));

            //add statics, featured and promos
            System.Text.StringBuilder statics = new System.Text.StringBuilder();

            //this is actually the start of the structure - we have moved it here to deal with 
            //the insert of the other collections easier
            statics.AppendFormat("{0}{1}<ul id=\"navskl\" style=\"border:none green 1px;display:inline-block;\">", Utils.Constants.NewLine, Utils.Constants.Tabs(depth++));

            statics.AppendFormat("{0}{1}<li class=\"mnudiv sectitle\">{2}{3}", Utils.Constants.NewLine, Utils.Constants.Tabs(depth),
                Title, " &#187;");

            statics.AppendFormat("{0}{1}<ul>", Utils.Constants.NewLine, Utils.Constants.Tabs(depth++));

            string addnew = Page.ClientScript.GetPostBackClientHyperlink(this, "addnewitem");
            string refresh = Page.ClientScript.GetPostBackClientHyperlink(this, "refreshitem");
            string toggleactive = Page.ClientScript.GetPostBackClientHyperlink(this, "toggleactive");

            statics.AppendFormat("{0}{1}<li><a href=\"{4}\">{2}{3}</a></li>", Utils.Constants.NewLine, Utils.Constants.Tabs(depth),
                "Add A New Item", "", addnew);

            //only use parent ids here
            string merchItemId = (Atx.CurrentMerchRecord != null) ? 
                (Atx.CurrentMerchRecord.IsParent) ?
                Atx.CurrentMerchRecord.Id.ToString() : Atx.CurrentMerchRecord.TParentListing.ToString() : 
                "0";

            if (Atx.CurrentMerchRecord != null)
            {
                statics.AppendFormat("{0}{1}<li><a href=\"/Admin/MerchEditor.aspx?p={2}&merchitem={3}\">{4}</a></li>",
                    Utils.Constants.NewLine, Utils.Constants.Tabs(depth),
                    this.EditorPage, 
                    merchItemId, "Edit Current Item");
                
                statics.AppendFormat("{0}{1}<li><a href=\"/Admin/MerchEditor.aspx?p=Images&merchitem={2}\">{3}</a></li>",
                    Utils.Constants.NewLine, Utils.Constants.Tabs(depth),
                    merchItemId, 
                    "Image Editor");

                statics.AppendFormat("{0}{1}<li><a href=\"/Admin/MerchEditor.aspx?p=Postp&merchitem={2}\">{3}</a></li>",
                    Utils.Constants.NewLine, Utils.Constants.Tabs(depth),
                    merchItemId,
                    "Post Purchase");

                statics.AppendFormat("{0}{1}<li><a href=\"/Admin/MerchEditor_NoUpdate.aspx?p=Bundle&merchitem={2}\">{3}</a></li>",
                    Utils.Constants.NewLine, Utils.Constants.Tabs(depth),
                    merchItemId,
                    "Bundle Editor");
                
                statics.AppendFormat("{0}{1}<li><a href=\"/Admin/MerchEditor.aspx?p=ItemCopy&merchitem={2}\">{3}</a></li>",
                    Utils.Constants.NewLine, Utils.Constants.Tabs(depth),
                    merchItemId, 
                    "Copy Item");

                statics.AppendFormat("{0}{1}<li><a href=\"/Admin/Reports.aspx?p=merch&merchitem={2}\">{3}</a></li>",
                    Utils.Constants.NewLine, Utils.Constants.Tabs(depth),
                    merchItemId, 
                    "View Item Sales");
                
                statics.AppendFormat("{0}{1}<li><a href=\"{4}\">{2}{3}</a></li>", 
                    Utils.Constants.NewLine, Utils.Constants.Tabs(depth),
                    "Refresh Current Item", "", refresh);

                statics.AppendFormat("{0}{1}<li><a href=\"{4}\">{2}{3}</a></li>",
                    Utils.Constants.NewLine, Utils.Constants.Tabs(depth),
                    string.Format("Toggle {0} menu items", (Atx.AdminMerchListingContext == 0) ? "ACTIVE" : "INACTIVE"), 
                    "", toggleactive);
            }

            statics.AppendFormat("{0}{1}</ul>", Utils.Constants.NewLine, Utils.Constants.Tabs(depth--));
            statics.AppendFormat("{0}{1}</li>", Utils.Constants.NewLine, Utils.Constants.Tabs(depth++));

            //collections
            statics.AppendFormat("{0}{1}<li class=\"mnudiv\">{2}{3}", Utils.Constants.NewLine, Utils.Constants.Tabs(depth),
                "Sets", " &#187;");
            statics.AppendFormat("{0}{1}<ul>", Utils.Constants.NewLine, Utils.Constants.Tabs(depth++));

            AddCollectionToList(statics, depth, "Featured Items", featured);
            AddCollectionToList(statics, depth, "Bundled Items", bundles);
            AddCollectionToList(statics, depth, "Promotion Items", promotions);

            //end the static structure
            statics.AppendFormat("{0}{1}</ul>", Utils.Constants.NewLine, Utils.Constants.Tabs(depth--));
            statics.AppendFormat("{0}{1}</li>", Utils.Constants.NewLine, Utils.Constants.Tabs(depth++));


            lit.Text = statics.ToString() + sb.ToString();
        }

        private void AddCollectionToList(System.Text.StringBuilder sb, int depth, string title, MerchCollection coll)
        {
            if (coll.Count > 0)
            {
                sb.AppendFormat("{0}{1}<li><a href=\"#\">{2}{3}</a>", Utils.Constants.NewLine, Utils.Constants.Tabs(depth),
                        title, " &#187;");

                sb.AppendFormat("{0}{1}<ul>", Utils.Constants.NewLine, Utils.Constants.Tabs(depth++));
                
                foreach (Merch m in coll)
                    FillListItem(sb, depth, m);

                sb.AppendFormat("{0}{1}</ul>", Utils.Constants.NewLine, Utils.Constants.Tabs(depth--));
            }
        }

        private void FillListItem(System.Text.StringBuilder builder, int depth, Merch merchRecord)
        {
            bool isInventoryLink = (this.Page.ToString() == "ASP.admin_orders_aspx");

            builder.AppendFormat("{0}{1}<li>", Utils.Constants.NewLine, Utils.Constants.Tabs(depth++));

            ItemImageCollection imgColl = new ItemImageCollection();
            imgColl.AddRange(merchRecord.ItemImageRecords().GetList().FindAll(delegate(ItemImage match) { return (match.IsItemImage); }));

            string img = string.Format("<img src=\"{0}\" alt=\"\" style=\"height:1px;\" />", _Config.SPACERIMAGEPATH);

            if (imgColl.Count > 0)
            {
                if (imgColl.Count > 1)
                    imgColl.Sort("IDisplayOrder", true);

                ItemImage itmimg = imgColl[0];

                img = string.Format("<img src=\"{0}\" alt=\"\" />", itmimg.Thumbnail_Small);
            }

            switch (LinkToPage.ToLower())
            {
                case "sales":
                    builder.AppendFormat("{0}{1}<a href=\"/Admin/Reports.aspx?p=merch&merchitem={2}\">{3}{4}</a>",
                        Utils.Constants.NewLine, Utils.Constants.Tabs(depth--),
                        merchRecord.Id, img, merchRecord.DisplayName);
                    break;
                case "merch":
                    builder.AppendFormat("{0}{1}<a href=\"/Admin/{2}{3}\">{4}{5}</a>",
                        Utils.Constants.NewLine, Utils.Constants.Tabs(depth--),
                        (isInventoryLink) ? string.Format("Order.aspx?p=merch&item=") :
                        string.Format("MerchEditor.aspx?p={0}&merchitem=", this.EditorPage),
                        merchRecord.Id, img, merchRecord.DisplayName);
                    break;
            }
            

            builder.AppendFormat("{0}{1}</li>", Utils.Constants.NewLine, Utils.Constants.Tabs(depth--));
        }
    }
}       