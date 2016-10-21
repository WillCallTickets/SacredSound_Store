using System;
using System.Web.UI;

using Wcss;

namespace WillCallWeb.Controls
{
    public partial class PurchaseDetails : WillCallWeb.BaseControl, System.Web.SessionState.IRequiresSessionState
    {
        protected AuthorizeNet _auth;
        protected string _billName;
        protected string _billAddress;
        protected string _shipName = string.Empty;
        protected string _shipAddress = string.Empty;

        protected bool _displayShippingSection
        {
            get
            {
                return (_auth != null && _auth.InvoiceRecord != null && _auth.InvoiceRecord.InvoiceBillShip != null && 
                    (!_auth.InvoiceRecord.InvoiceBillShip.SameAsBilling));
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Ctx == null)
                {
                    _Error.LogException(new Exception("Ctx is null in purchase details"));

                    base.Redirect("/Store/ChooseTicket.aspx");
                }

                _auth = Ctx.SessionAuthorizeNet;

                //cant have that can we? - take them to error page?
                if (_auth == null)
                {
                    //log to log
                    string err = string.Format("Auth is null at PurchaseDetails{0}{1}{0}{2}", Environment.NewLine, this.userWebInfo,
                        (Request == null || Request.UrlReferrer == null) ? "request or referrer, is null" : Request.UrlReferrer.ToString());

                    _Error.LogException(new Exception(err));

                    //redirect
                    base.Redirect("/");
                }

                if (_auth.InvoiceRecord == null)
                {
                    _Error.LogException(new Exception("_auth.InvoiceRedord is null in purchase details"));

                    base.Redirect("/Store/ChooseTicket.aspx");
                }

                //if we are loading this page more than 2 minutes after the _auth was made, then dont show email text
                LiteralSendEmail.Visible = (_auth.DtStamp.AddMinutes(2) > DateTime.Now);
                if (LiteralSendEmail.Visible)
                {
                    LiteralSendEmail.Text = "<div class=\"ordernotes\"><div>You will receive an email shortly with details of your order.</div>";
                    LiteralSendEmail.Text += string.Format("<div>*Please note: if you use a spamblocker, be sure to add {0} to your allow list.</div>",
                        _Config._Confirmation_Email);
                    LiteralSendEmail.Text += "</div>";
                }

                _billName = string.Format("{0} {1}", _auth.FirstName, _auth.LastName);
                _billAddress = string.Format("{0}<br>{1}, {2} {3}", _auth.BillingAddress, _auth.City, _auth.State, _auth.Zip);
                //do not show country if us or usa
                if (_auth.Country.ToLower() != "us" && _auth.Country.ToLower() != "usa")
                    _billAddress += string.Format("<br/>{0}", _auth.Country);


                shipinfo.Visible = _displayShippingSection;

                if (_displayShippingSection)
                {
                    //FUTURE
                    //tracking
                    _shipName = _auth.InvoiceRecord.InvoiceBillShip.FullName_Working;

                    string addie = string.Format("{0}{1}", _auth.InvoiceRecord.InvoiceBillShip.Address1_Working,
                        (_auth.InvoiceRecord.InvoiceBillShip.Address2_Working.Trim().Length > 0) ?
                        string.Format("<br />{0}", _auth.InvoiceRecord.InvoiceBillShip.Address2_Working) : string.Empty);

                    _shipAddress = string.Format("{0}<br>{1}, {2} {3}<br>{4}", addie, _auth.InvoiceRecord.InvoiceBillShip.City_Working,
                        _auth.InvoiceRecord.InvoiceBillShip.State_Working, _auth.InvoiceRecord.InvoiceBillShip.Zip_Working,
                        _auth.InvoiceRecord.InvoiceBillShip.Country_Working);
                }

                Ctx.BarCodeText = _auth.InvoiceRecord.UniqueId;

                //note - reversed format order
                LiteralInvoiceId.Text =
                    string.Format("<tr><td colspan=\"2\">{1}</td></tr><tr><th>Invoice Id:</th><td>{0}</td></tr>", Ctx.BarCodeText,
                    string.Format("<img style=\"display: block;\" src=\"{0}\" border=\"0\" alt=\"{1}\" />", string.Format("/Controls/JpegImage.aspx?bc={0}", Ctx.BarCodeText), Ctx.BarCodeText));
                
                LiteralPrint.Text = (this.Page.MasterPageFile.ToLower().IndexOf("templateprint") > -1) ? string.Empty :
                    string.Format("<a class=\"btntribe\" href=\"javascript:doPagePopup(&#39;/Store/PrintConfirm.aspx&#39;, &#39;true&#39);\">printer-friendly sales receipt</a>");
            }
            catch (Exception ex)
            {
                _Error.LogException(new Exception("PurchaseDetails.ascx Page Load Error. Details to follow"));
                _Error.LogException(ex);

                throw;
            }
        }
    }
}