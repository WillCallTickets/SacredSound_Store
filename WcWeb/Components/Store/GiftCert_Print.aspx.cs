using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace WillCallWeb.Components.Store
{
    public partial class GiftCert_Print : BasePage
    {
        protected string _code = null;
        protected string GiftCode
        {
            get
            {
                return _code;
            }
            set
            {
                _code = value;
            }
        }
        protected string _to = null;
        protected string To
        {
            get
            {
                return _to;
            }
            set
            {
                _to = value;
            }
        }
        protected string _from = null;
        protected string From
        {
            get
            {
                return _from;
            }
            set
            {
                _from = value;
            }
        }
        protected string _amount = null;
        protected string Amount
        {
            get{ return _amount; }
            set{ _amount = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string reqCode = Request.QueryString["cd"];
            string reqTo = Request.QueryString["to"];
            string reqFrom = Request.QueryString["fm"];
            string reqAmount = Request.QueryString["am"];

            if(reqCode != null)
                GiftCode = reqCode.Trim();
            if(reqTo != null)
                To = reqTo.Trim();
            if(reqCode != null)
                From = reqFrom.Trim();
            if(reqAmount != null)
                Amount = reqAmount.Trim();
        }
}
}