using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web.Services;
using System.Text.RegularExpressions;

using Wcss;
using WillCallWeb;
using WillCallWeb.StoreObjects;

namespace WillCallWeb.Admin.AdminControls.Wysiwyg
{
    public partial class Wysiwyg : WillCallWeb.BasePage
    {
        #region Properties

        private string _itemContext = null;
        protected string ItemContext
        {
            get
            {
                if (_itemContext == null)
                {
                    string context = this.Request["context"];
                    if (context != null)
                    {
                        if (context.ToLower() == "m")
                            _itemContext = "merch";
                        else if (context.ToLower() == "s")
                            _itemContext = "show";
                        else if (context.ToLower() == "sb")
                            _itemContext = "showbilling";
                        else if (context.ToLower() == "ppt")
                            _itemContext = "postpurchase_tix";
                        else if (context.ToLower() == "ppm")
                            _itemContext = "postpurchase_merch";
                        else if (context.ToLower() == "co")
                            _itemContext = "charitableorg";
                        else if (context.ToLower() == "faq")
                            _itemContext = "faqitem";
                        else if (context.ToLower() == "med")
                            _itemContext = "mailercontent";
                    }
                }

                return _itemContext;
            }
        }

        private Merch _merchRecord = null;
        protected Merch MerchRecord
        {
            get
            {
                if (_merchRecord == null && ItemContext == "merch")
                    _merchRecord = Atx.CurrentMerchRecord;

                return _merchRecord;
            }
        }

        private Show _showRecord = null;
        protected Show ShowRecord
        {
            get
            {
                if (_showRecord == null && (ItemContext == "show" || ItemContext == "showbilling"))
                    _showRecord = Atx.CurrentShowRecord;

                return _showRecord;
            }
        }

        private FaqItem _faqItemRecord = null;
        protected FaqItem FaqItemRecord
        {
            get
            {
                if (_faqItemRecord == null && ItemContext == "faqitem")
                {
                    string pid = Request["faqid"];
                    if (pid != null && Utils.Validation.IsInteger(pid))
                    {
                        _faqItemRecord = FaqItem.FetchByID(int.Parse(pid));
                    }
                }

                return _faqItemRecord;
            }
        }

        private MailerContent _mailerContentRecord = null;
        protected MailerContent MailerContentRecord
        {
            get
            {
                if (_mailerContentRecord == null && ItemContext == "mailercontent")
                {
                    string pid = Request["medid"];
                    if (pid != null && Utils.Validation.IsInteger(pid))
                    {
                        _mailerContentRecord = Atx.CurrentMailer.MailerContentRecords().GetList()
                            .Find(delegate(MailerContent match) { return (match.TMailerTemplateContentId == int.Parse(pid)); });
                    }
                }

                return _mailerContentRecord;
            }
        }

        private CharitableOrg _charitableOrgRecord = null;
        protected CharitableOrg CharitableOrgRecord
        {
            get
            {
                if (_charitableOrgRecord == null && ItemContext == "charitableorg")
                    _charitableOrgRecord = CharitableOrg.FetchByID(Atx.CurrentCharitableOrgId);

                return _charitableOrgRecord;
            }
        }

        private PostPurchaseText _postPurchase_Tix = null;
        protected PostPurchaseText PostPurchase_Tix
        {
            get
            {
                if (_postPurchase_Tix == null && ItemContext == "postpurchase_tix")
                {
                    string pid = Request["ppid"];
                    if (pid != null && Utils.Validation.IsInteger(pid))
                    {
                        _postPurchase_Tix = PostPurchaseText.FetchByID(int.Parse(pid));
                    }
                }

                return _postPurchase_Tix;
            }
        }

        private PostPurchaseText _postPurchase_Merch = null;
        protected PostPurchaseText PostPurchase_Merch
        {
            get
            {
                if (_postPurchase_Merch == null && ItemContext == "postpurchase_merch")
                {
                    string pid = Request["ppid"];
                    if (pid != null && Utils.Validation.IsInteger(pid))
                    {
                        _postPurchase_Merch = PostPurchaseText.FetchByID(int.Parse(pid));
                    }
                }

                return _postPurchase_Merch;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindPageControls();
            }
        }

        protected void BindPageControls()
        {
            litTitle.DataBind();
            Ck_Edit.DataBind();
        }
    
        protected void litTitle_DataBinding(object sender, EventArgs e)
        {
            Literal lit = (Literal)sender;

            string title = (ItemContext == "merch") ? ((MerchRecord != null) ? MerchRecord.Name : string.Empty) :
                (ItemContext == "show") ? ((ShowRecord != null) ? ShowRecord.Name : string.Empty) :
                (ItemContext == "showbilling") ? ((ShowRecord != null) ? string.Format("Billing - {0}", ShowRecord.Name) : string.Empty) :
                (ItemContext == "postpurchase_tix") ? ((PostPurchase_Tix != null) ? string.Format("Post Text - {0}", 
                    Utils.ParseHelper.StripHtmlTags(PostPurchase_Tix.ShowTicketRecord.DisplayNameWithAttribsAndDescription)) : string.Empty) :
                (ItemContext == "postpurchase_merch") ? ((PostPurchase_Merch != null) ? string.Format("Post Text - {0}", PostPurchase_Merch.MerchRecord.DisplayNameWithAttribs) : string.Empty) :
                (ItemContext == "charitableorg") ? ((CharitableOrgRecord != null) ? CharitableOrgRecord.Name : string.Empty) :
                (ItemContext == "mailercontent") ? ((MailerContentRecord != null) ? MailerContentRecord.Title : string.Empty) :
                (ItemContext == "faqitem") ? ((FaqItemRecord != null) ? FaqItemRecord.Question : string.Empty) :
                string.Empty;

            lit.Text = title;
        }

        protected void CKEditor_DataBinding(object sender, EventArgs e)
        {
            CKEditor.NET.CKEditorControl editor = (CKEditor.NET.CKEditorControl)sender;

            string description = (ItemContext == "merch") ? ((MerchRecord != null) ? MerchRecord.Description : string.Empty) :
                (ItemContext == "show") ? ((ShowRecord != null) ? ShowRecord.BotText : string.Empty) :
                (ItemContext == "showbilling") ? ((ShowRecord != null) ? ShowRecord.ActBilling : string.Empty) :
                (ItemContext == "postpurchase_tix") ? ((PostPurchase_Tix != null) ? PostPurchase_Tix.PostText : string.Empty) :
                (ItemContext == "postpurchase_merch") ? ((PostPurchase_Merch != null) ? PostPurchase_Merch.PostText : string.Empty) :
                (ItemContext == "charitableorg") ? ((CharitableOrgRecord != null) ? CharitableOrgRecord.Description : string.Empty) :
                (ItemContext == "mailercontent") ? ((MailerContentRecord != null) ? MailerContentRecord.VcContent : string.Empty) :
                (ItemContext == "faqitem") ? ((FaqItemRecord != null) ? FaqItemRecord.Answer : string.Empty) :
                string.Empty;

            editor.Text = description;

            CKFinder.FileBrowser _FileBrowser = new CKFinder.FileBrowser();
            
            _FileBrowser.BasePath = "/ckfinder/";
            _FileBrowser.RememberLastFolder = false;
            _FileBrowser.SetupCKEditor(editor);
        }

        private readonly int faqMaxSize = 20000;

        protected void btn_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string cmd = btn.CommandName.ToLower().Trim();

            switch (cmd)
            {
                case "save":
                    CKEditor.NET.CKEditorControl editor = (CKEditor.NET.CKEditorControl)Ck_Edit;
                    string editVal = editor.Text.Trim();

                    //brs get an added space - remove it
                    editVal = Regex.Replace(editVal, "<br/> ", "<br/>", RegexOptions.IgnoreCase);
                    editVal = Regex.Replace(editVal, "<br /> ", "<br />", RegexOptions.IgnoreCase);

                    switch (ItemContext)
                    { 
                        case "merch":
                            MerchRecord.Description = editVal;
                            MerchRecord.Save_AvoidRealTimeVars();
                            break;
                        case "show":
                            ShowRecord.BotText = editVal;
                            ShowRecord.Save();
                            break;
                        case "showbilling":
                            ShowRecord.ActBilling = editVal;
                            ShowRecord.Save();
                            break;
                        case "postpurchase_tix":
                            PostPurchase_Tix.PostText = editVal;
                            PostPurchase_Tix.Save();
                            break;
                        case "postpurchase_merch":
                            PostPurchase_Merch.PostText = editVal;
                            PostPurchase_Merch.Save();
                            break;
                        case "charitableorg":
                            CharitableOrgRecord.Description = editVal;
                            CharitableOrgRecord.Save();
                            break;
                        case "mailercontent":
                            MailerContentRecord.VcContent = editVal;
                            MailerContentRecord.Save();
                            break;
                        case "faqitem":
                            if (editVal.Length > faqMaxSize)
                                throw new ArgumentOutOfRangeException(string.Format("Please limit your entry to {0} characters.", faqMaxSize.ToString()));
                            FaqItemRecord.Answer = editVal;
                            FaqItemRecord.Save();
                            _Lookits.RefreshLookup("FaqCategories");
                            break;
                    }
                    break;

                case "cancel":
                    Ck_Edit.DataBind();
                    break;
            }
        }
    
    }
}

