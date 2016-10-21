using System;

using Wcss;

namespace WillCallWeb.Admin
{
    public partial class PrintPackList : WillCallWeb.BasePage
    {
        protected Invoice _invoice = null;
        protected InvoiceShipment _shipment = null;
        protected override void OnPreInit(EventArgs e)
        {
            QualifySsl(false);
            base.OnPreInit(e);
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            string req = Request["ship"];

            if (req != null && Utils.Validation.IsInteger(req))
                _shipment = InvoiceShipment.FetchByID(int.Parse(req));

            if(_shipment != null)
                _invoice = _shipment.InvoiceRecord;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (_invoice.ApplicationId == _Config.APPLICATION_ID)
            {
                if (!IsPostBack)
                {
                    DisplayPackingList();
                }

                litMessage.Text = string.Format("<div style=\"width: 75%;\">{0}</div>",
                    (_shipment.ShipMessage != null) ? _shipment.ShipMessage.Trim() : string.Empty);
                litTracking.Text = string.Format("<div style=\"width: 75%;\">{0}</div>",
                    (_shipment.TrackingInformation != null) ? _shipment.TrackingInformation.Trim() : string.Empty);
            }
            else
                this.Controls.Clear();
        }

        protected void DisplayPackingList()
        {
            int rowCount = 1;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            string[] parts = _shipment.PackingList.TrimEnd('~').Split('~');

            foreach (string s in parts)
            {
                string mottled = string.Empty;

                if (s.IndexOf("@") != -1)
                {
                    int idx = s.IndexOf('@');
                    string start = s.Substring(0, idx);
                    mottled = string.Format("<span class=\"ship-qty\">{0}</span> {1}", start.Trim(), s.Substring(idx));
                }
                else 
                    mottled = s;

                //do not allow bundle titles to increase the count
                //and do not list the bundle as an item - list it plainly
                if(mottled.ToLower().IndexOf("bundle:") != -1)
                    sb.AppendFormat("<div class=\"ship-item\">{0}</div>", mottled);
                else
                    sb.AppendFormat("<div class=\"ship-item\">(item #{0}) {1}</div>", rowCount++, mottled);
            }

            if (_shipment.PackingAdditional != null && _shipment.PackingAdditional.Trim().Length > 0)
                sb.AppendFormat("<div class=\"ship-item\">({0}) {1}</div>", rowCount++, _shipment.PackingAdditional.Trim());

            LiteralPackingList.Text = sb.ToString();

            if (_shipment.InvoiceRecord.HasCharitableItems)
            {
                System.Text.StringBuilder don = new System.Text.StringBuilder();

                foreach(InvoiceItem ii in _shipment.InvoiceRecord.CharitableItems)
                    if(ii.PurchaseAction == _Enums.PurchaseActions.Purchased.ToString())
                        don.AppendFormat("<div>{0} {1}</div>", ii.LineItemTotal.ToString("c"), ii.MainActName);

                if (don.Length > 0)
                {
                    don.Insert(0, "<fieldset><legend class=\"controlheader\"><span class=\"title\">Thank you for your donation. We appreciate your support.</span></legend>");
                    don.Append("</fieldset>");

                    litDonations.Text = don.ToString();
                }
            }
        }
}
}