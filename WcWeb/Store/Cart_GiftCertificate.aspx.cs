using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web.Services;

using Wcss;
using WillCallWeb;
using WillCallWeb.StoreObjects;

public partial class Store_Cart_GiftCertificate : WillCallWeb.BasePage
{
    //[WebMethod]
    //public static object ClearAll(string context, int saleItem_ItemId, int bundleId)
    //{
    //    //return SaleItem_Services.ClearAll(new WebContext(), context, saleItem_ItemId, bundleId);
    //}
    //[WebMethod]
    //public static object RemoveOne(string context, int saleItem_ItemId, int bundleId, int selectedItemId)
    //{
    //    //return SaleItem_Services.RemoveOne(new WebContext(), context, saleItem_ItemId, bundleId, selectedItemId);
    //}
    //[WebMethod]
    //public static object AddChoice(string context, int saleItem_ItemId, int bundleId, int selectedItemId)
    //{
    //   // return SaleItem_Services.AddChoice(new WebContext(), context, saleItem_ItemId, bundleId, selectedItemId);
    //}

    /*
     * match the bundle in question
     * 
     * pull cart qtys
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

    private InvoiceItem _saleItem = null;
    protected InvoiceItem SaleItem
    {
        get
        {
            if (_saleItem == null && Ctx.SessionAuthorizeNet != null)
            {
                string sim = this.Request["sim"];

                if (sim != null)
                {
                    InvoiceItem ii = _invoice.InvoiceItemRecords().GetList().Find(delegate (InvoiceItem match) { return (match.Guid.ToString() == sim); } );
                    if (ii != null)
                    {
                        _saleItem = new InvoiceItem();
                        _saleItem.CopyFrom(ii);
                    }
                }
            }

            return _saleItem;
        }
    }

    #endregion

    protected Invoice _invoice = null;
    protected string _code;
    protected string _amount;
    protected string _email;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (_invoice == null && Ctx.SessionAuthorizeNet != null)
        {
            _invoice = Ctx.SessionAuthorizeNet.InvoiceRecord;

            if (_invoice == null)
            {
                Exception ex = new Exception("Session invoice could not be found at purchaseCart.ascx.");
                _Error.LogException(ex);
                throw ex;
            }

            _code = SaleItem.Guid.ToString();
            _amount = SaleItem.LineItemTotal.ToString("c"); 
            _email = Ctx.SessionAuthorizeNet.Email;            
        }
        else if (_invoice == null)
        {
            Exception ex = new Exception("Session authNet could not be found at purchaseCart.ascx.");
            _Error.LogException(ex);
            throw ex;
        }

        //if (!IsPostBack)
        //{
        //    //object o = this.SaleItem;
        //}
    }

    protected void btnEmail_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            string to = txtEmailTo.Text.Trim();
            string from = txtEmailFrom.Text.Trim();
            string email = txtEmailAddress.Text.Trim();
            if (to.Length > 0 && from.Length > 0 && email.Length > 0 && Utils.Validation.IsValidEmail(email))
            {
                //send email gc                
                MailQueue.SendGiftCertificate(email, this.Page.User.Identity.Name, to, from, SaleItem.LineItemTotal.ToString("n2"), SaleItem.DeliveryCode);

                //show confirm - hide panel
                txtEmailTo.Text = string.Empty;
                txtEmailFrom.Text = string.Empty;
                txtEmailAddress.Text = string.Empty;
                pnlEmailInput.Visible = false;
                pnlEmailConfirm.Visible = true;

            }
        }
    }

    protected void valEmailPattern_Load(object sender, EventArgs e)
    {
        RegularExpressionValidator regex = (RegularExpressionValidator)sender;

        regex.ValidationExpression = Utils.Validation.regexEmail.ToString();
    }
}
