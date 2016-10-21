using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.IO;
using System.Xml;
using System.Linq;

using Wcss;

/*
 * <asp:Button ID="btnEditPreview" runat="server" Text="Edit Mailer"
                            onclick="btnEditPreview_Click" CausesValidation="false" EnableViewState="false" />*/
namespace WillCallWeb.Admin.AdminControls
{
    public partial class Mailer_ListGenerator : BaseControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlShowList.DataBind();
                ddlEmailLetter.DataBind();
                rblShipContext.DataBind();
            }

            litPreview.DataBind();
        }

        #region Calendars

        protected void clock_Init(object sender, EventArgs e)
        {
            WillCallWeb.Components.Util.CalendarClock cal = (WillCallWeb.Components.Util.CalendarClock)sender;

            if (cal.ID.ToLower().IndexOf("clockstart") != -1)
            {
                cal.SelectedDate = DateTime.Parse(string.Format("{0}/1/{1}",
                    DateTime.Now.AddMonths(-1).Month, DateTime.Now.AddMonths(-1).Year));
                cal.UseTime = false;
            }
            else if (cal.ID.ToLower().IndexOf("clockend") != -1)
            {
                cal.SelectedDate = DateTime.Parse(string.Format("{0}/1/{1}",
                    DateTime.Now.AddMonths(3).Month, DateTime.Now.AddMonths(3).Year)).AddMonths(1).AddSeconds(-1);
                cal.UseTime = false;
            }
            else
                cal.SelectedDate = DateTime.Now;
        }
        protected void clock_SelectedDateChanged(object sender, WillCallWeb.Components.Util.CalendarClock.CalendarClockChangedEventArgs e)
        {
            WillCallWeb.Components.Util.CalendarClock cal = (WillCallWeb.Components.Util.CalendarClock)sender;

            if (cal.ID.ToLower().IndexOf("start") != -1)
                clockStart.SelectedDate = e.ChosenDate;
            else
                clockEnd.SelectedDate = e.ChosenDate;

            EnsureValidCalendarSelection();

            //GooglePager1.PageIndex = 0;
            //GridView1.PageIndex = GooglePager1.PageIndex;
            //ddlShowList.DataBind();
        }
        private void EnsureValidCalendarSelection()
        {
            DateTime startClock = clockStart.SelectedDate;
            DateTime endClock = clockEnd.SelectedDate;

            if (endClock < startClock)
                clockEnd.SelectedDate = startClock.AddDays(1);
        }

        #endregion

        protected void btnDupes_Click(object sender, EventArgs e)
        {
            //go thru list and remove dupes
            //also remove invoiceid - make blank
            //also remove purchasename - make blank
            //string.Format("{0}~{1}~{2}", invoiceId, userId, purName)));
            List<ListItem> nodupes = new List<ListItem>();
            foreach (ListItem li in lstEmails.Items)
            {
                if(nodupes.FindIndex(delegate(ListItem match) { return (match.Text == li.Text); } ) == -1)
                {
                    string[] parts = li.Value.Split('~');

                    nodupes.Add(new ListItem(li.Text, string.Format("~{0}~", parts[1])));
                }
            }
            
            lstEmails.Items.Clear();
            lstEmails.SelectedIndex = -1;
            lstEmails.Items.AddRange(nodupes.ToArray());
        }

        protected void btnClearList_Click(object sender, EventArgs e)
        {
            lstEmails.Items.Clear();
        }
        protected void btnEditPreview_Click(object sender, EventArgs e)
        {
            base.Redirect("/Admin/Mailers.aspx?p=customer");
        }
        protected void btnDisplayList_Click(object sender, EventArgs e)
        {
            //get & set the data
            Atx.CurrentDisplayList = null;
            Atx.CurrentDisplayList = new List<string>();

            foreach (ListItem li in lstEmails.Items)
                Atx.CurrentDisplayList.Add(li.Text);

            if (Atx.CurrentDisplayList != null && Atx.CurrentDisplayList.Count > 0)
            {
                //do the popup
                string script = "doPagePopup('/Admin/DisplayList.aspx', 'true');";

                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(),
                    Guid.NewGuid().ToString(), " ;" + script, true);
            }
        }
        protected void btnSendTest_Click(object sender, EventArgs e)
        {
            if (Atx.CurrentEmailLetter == null)
            {
                CustomSendTest.IsValid = false;
                CustomSendTest.ErrorMessage = "Please select a message to send";
                return;
            }

            string email = txtTestEmail.Text.Trim();
            string emailListing = string.Empty;

            List<string> emailList = new List<string>();
            emailList.AddRange(email.Split(','));

            List<string> invalids = Utils.Validation.InvalidArrayOfEmails(emailList);

            if(invalids.Count > 0)
            {
                foreach (string s in invalids)
                    emailListing += string.Format("{0}~", s);

                emailListing = Utils.ParseHelper.ConvertTildesToCommasAndAmpersands(emailListing.Trim());

                CustomSendTest.IsValid = false;
                CustomSendTest.ErrorMessage = string.Format("{0} {1}", 
                    emailListing, (invalids.Count == 1) ? "is not a valid email address" : 
                    "are not valid email addresses");

                return;
            }

            string mappedFile = Server.MapPath(Atx.CurrentEmailLetter.HtmlVersion);
            string file = Utils.FileLoader.FileToString(mappedFile);
            List<string> valids = Utils.Validation.ValidArrayOfEmails(emailList);

            foreach (string s in valids)
            {
                emailListing += string.Format("{0}~", s);

                //create a dictionary with dummy values
                System.Collections.Specialized.ListDictionary dict = new System.Collections.Specialized.ListDictionary();
                dict.Add("<PARAM>FIRSTNAME</PARAM>", "sample_FirstName");
                dict.Add("<PARAM>LASTNAME</PARAM>", "sample_LastName");
                dict.Add("<PARAM>EMAILADDRESS</PARAM>", s);
                dict.Add("<PARAM>INVOICEID</PARAM>", "sample_InvoiceId");

                MailQueue.SendEmail(_Config._CustomerService_Email, _Config._CustomerService_FromName, s, null, null,
                    Atx.CurrentEmailLetter.Subject, file, null, dict, true, null);
            }

            txtTestEmail.Text = string.Empty;

            emailListing = Utils.ParseHelper.ConvertTildesToCommasAndAmpersands(emailListing.Trim());
            lblStatus.Text = string.Format("Test email has been sent to {0}", emailListing.ToLower());

            lblStatus.ForeColor = System.Drawing.Color.Green;            
        }
        protected void btnSend_Click(object sender, EventArgs e)
        {
            if (lstEmails.Items.Count == 0)
            {
                CustomSend.IsValid = false;
                CustomSend.ErrorMessage = "There are no emails selected to send";
                return;
            }
            if (Atx.CurrentEmailLetter == null)
            {
                CustomSend.IsValid = false;
                CustomSend.ErrorMessage = "Please select a message to send";
                return;
            }

            int count = 0;
            DateTime dateToProcess = DateTime.Now;

            //get info from selected email
            string mappedFile = Server.MapPath(Atx.CurrentEmailLetter.HtmlVersion);
            string file = Utils.FileLoader.FileToString(mappedFile);

            if (file.Trim().Length > 0)
            {
                //validate listed emails? - or just assume
                foreach (ListItem li in this.lstEmails.Items)
                {
                    //get the user info - profile info?
                    string[] vals = li.Value.Split('~');
                    string invoiceId = vals[0];
                    Guid userId = new Guid(vals[1]);
                    string userName = vals[2];
                    string firstName = userName.Substring(userName.IndexOf(',')).TrimStart(',').Trim();
                    string lastName = userName.Substring(0, userName.IndexOf(','));
                    string emailAddress = (_Config._RefundTestMode) ? _Config._Admin_EmailAddress : li.Text;

                    //create replacement dictionary
                    System.Collections.Specialized.ListDictionary dict = new System.Collections.Specialized.ListDictionary();
                    dict.Add("<PARAM>FIRSTNAME</PARAM>", (firstName != null && firstName.Trim().Length > 0) ? firstName : string.Empty);
                    dict.Add("<PARAM>LASTNAME</PARAM>", (lastName != null && lastName.Trim().Length > 0) ? lastName : string.Empty);
                    dict.Add("<PARAM>EMAILADDRESS</PARAM>", (emailAddress != null && emailAddress.Trim().Length > 0) ? emailAddress : string.Empty);
                    dict.Add("<PARAM>INVOICEID</PARAM>", (invoiceId != null && invoiceId.Trim().Length > 0) ? invoiceId : string.Empty);

                    MailQueue.SendEmail(_Config._CustomerService_Email, _Config._CustomerService_FromName, emailAddress, null, null,
                        Atx.CurrentEmailLetter.Subject, file, null, dict, true, null);

                    count++;
                }

                
                lblStatus.Text = string.Format("{0} Email{1} have been sent", count, (count > 1) ? "s" : string.Empty);
                lblStatus.ForeColor = System.Drawing.Color.Green;
                lblStatus.Visible = true;
            }                
        }
        protected void btnLoad_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string id = btn.ID.ToLower();
            List<string> selections = new List<string>();

            switch (id)
            {
                case "btnloaddate":
                    if(chkShowDates.SelectedIndex != -1)
                    {
                        //if all dates is selected - pass all date ids
                        if (chkShowDates.SelectedValue != null && chkShowDates.SelectedValue == "0")
                            FillListBy("show", ddlShowList.SelectedValue);
                        else
                        {
                            foreach (ListItem li in chkShowDates.Items)
                                if(li.Selected)
                                    selections.Add(li.Value);

                            FillListBy("showdate", selections);
                        }
                    }
                    break;
                case "btnloadticket":
                    if (chkShowTickets.SelectedIndex != -1)
                    {
                        //if all dates is selected - pass all date ids
                        if (chkShowTickets.SelectedValue != null && chkShowTickets.SelectedValue == "0")
                        {
                            foreach (ListItem li in chkShowTickets.Items)
                                if (li.Value != "0")
                                    selections.Add(li.Value);
                        }
                        else
                        {
                            foreach (ListItem li in chkShowTickets.Items)
                                if (li.Selected)
                                    selections.Add(li.Value);
                        }
                        FillListBy("ticket", selections);
                    }
                    break;
                case "btnloadmerch":
                    //if all dates is selected - pass all date ids
                    if (chkChildren.SelectedValue != null && chkChildren.SelectedValue == "0")
                        FillListBy("parent", ddlParentList.SelectedValue);
                    else
                    {
                        foreach (ListItem li in chkChildren.Items)
                            if (li.Selected)
                                selections.Add(li.Value);

                        FillListBy("children", selections);
                    }
                    break;
            }
        }
        private void FillListBy(string context, List<string> selections)
        {
            string selectList = string.Empty;

            foreach (string s in selections)
                selectList += string.Format("{0},",s);
            
            selectList = selectList.TrimEnd(',');

            FillListBy(context, selectList);
        }
        private void FillListBy(string context, string input)
        {
            SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(string.Empty, SubSonic.DataService.Provider.Name);

            lstEmails.Items.Clear();
            lstEmails.SelectedIndex = -1;

            string purchOrRefund = rdoPurchase.SelectedValue;

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append("SELECT DISTINCT u.[UserName], ii.[PurchaseName], i.[UniqueId] as InvoiceId, u.[UserId] ");
            sb.Append("FROM InvoiceItem ii, Invoice i, Aspnet_Users u ");

            bool useShipContext = false;

            switch (context)
            {
                //sb.AppendFormat("ii.[PurchaseAction] = 'PurchasedThenRemoved' AND ISNULL(CHARINDEX(ii.[Notes], 'EXCHANGED'), -1) = -1 ");
                //changed to 
                //sb.AppendFormat("ii.[PurchaseAction] = 'PurchasedThenRemoved' AND (ii.[Notes] IS NULL OR CHARINDEX(ii.[Notes], 'EXCHANGED') < 1 ");

                case "show":
                    useShipContext = true;
                    sb.Append(", ShowTicket st ");
                    sb.AppendFormat("WHERE ii.[tShowTicketId] IS NOT NULL AND ii.[tShowTicketId] = st.[Id] AND st.[tShowId] = {0} AND ", input);
                    if(purchOrRefund == "purchases")
                        sb.AppendFormat("ii.[PurchaseAction] = 'Purchased' ");
                    else
                        sb.AppendFormat("ii.[PurchaseAction] = 'PurchasedThenRemoved' AND (ii.[Notes] IS NULL OR CHARINDEX(ii.[Notes], 'EXCHANGED') < 1 ");
                    break;
                case "showdate":
                    useShipContext = true;
                    sb.Append(", ShowTicket st ");
                    sb.AppendFormat("WHERE ii.[tShowTicketId] IS NOT NULL AND ii.[tShowTicketId] = st.[Id] AND st.[tShowDateId] IN ({0}) AND ", input);
                    if (purchOrRefund == "purchases")
                        sb.AppendFormat("ii.[PurchaseAction] = 'Purchased' ");
                    else
                        sb.AppendFormat("ii.[PurchaseAction] = 'PurchasedThenRemoved' AND (ii.[Notes] IS NULL OR CHARINDEX(ii.[Notes], 'EXCHANGED') < 1 ");
                    break;
                case "ticket":
                    useShipContext = true;
                    sb.AppendFormat("WHERE ii.[tShowTicketId] IS NOT NULL AND ii.[tShowTicketId] IN ({0}) AND ", input);
                    if (purchOrRefund == "purchases")
                        sb.AppendFormat("ii.[PurchaseAction] = 'Purchased' ");
                    else
                        sb.AppendFormat("ii.[PurchaseAction] = 'PurchasedThenRemoved' AND (ii.[Notes] IS NULL OR CHARINDEX(ii.[Notes], 'EXCHANGED') < 1 ");
                    break;
                case "parent":
                case "children":
                    sb.AppendFormat("WHERE ii.[tMerchId] IS NOT NULL AND ii.[tMerchId] IN ({0}) AND ii.[PurchaseAction] = 'Purchased' ", input);
                    break;
            }

            if(useShipContext && ShipContext.ToLower() != "all")
            {
                sb.Append("AND CASE @shipContext WHEN @willCallText THEN ");
                sb.Append("CASE WHEN (ii.[ShippingMethod] IS NULL OR (ii.[ShippingMethod] IS NOT NULL AND LEN(LTRIM(RTRIM(ii.[ShippingMethod]))) = 0)) OR ");
				sb.Append("(ii.[ShippingMethod] IS NOT NULL AND LEN(LTRIM(RTRIM(ii.[ShippingMethod]))) > 0 AND ");
                sb.Append("ii.[ShippingMethod] = @willCallText) THEN 1 ELSE 0 END ");
                sb.Append("WHEN 'Shipped' THEN ");
				sb.Append("CASE WHEN ");
				sb.Append("ii.[PurchaseAction] = 'Purchased' AND ");
				sb.Append("ii.[ShippingMethod] IS NOT NULL AND LEN(LTRIM(RTRIM(ii.[ShippingMethod]))) > 0 AND ");
				sb.Append("ii.[ShippingMethod] <> @willCallText THEN 1 ELSE 0 END ");
                sb.Append("ELSE 1 END = 1 ");

                cmd.Parameters.Add("@shipContext", ShipContext);
                cmd.Parameters.Add("@willCallText", ShipMethod.WillCall);
            }

            sb.Append("AND ii.[tInvoiceId] = i.[Id] AND i.[InvoiceStatus] <> 'NotPaid' AND i.[UserId] = u.[UserId] ");
            sb.Append(" ORDER BY u.[UserName] ");


            cmd.CommandSql = sb.ToString();

            using (IDataReader dr = SubSonic.DataService.GetReader(cmd))
            {
                while (dr.Read())
                {
                    string invoiceId = dr.GetValue(dr.GetOrdinal("InvoiceId")).ToString();
                    string userId = dr.GetValue(dr.GetOrdinal("UserId")).ToString();
                    string eml = dr.GetValue(dr.GetOrdinal("UserName")).ToString();
                    string purName = dr.GetValue(dr.GetOrdinal("PurchaseName")).ToString();

                    //add into list
                    lstEmails.Items.Add(new ListItem(eml, string.Format("{0}~{1}~{2}", invoiceId, userId, purName)));
                }

                dr.Close();
            }
        }
        protected string ShipContext { get { return rblShipContext.SelectedValue; } }
        protected void rblShipContext_DataBinding(object sender, EventArgs e)
        {
            RadioButtonList rbl = (RadioButtonList)sender;

            if (rbl.Items.Count == 0)
            {
                rbl.Items.Add(new ListItem("All", "all"));
                rbl.Items.Add(new ListItem("Shipped", "shipped"));
                rbl.Items.Add(new ListItem("Will_Call_Only", ShipMethod.WillCall));
            }
        }
        protected void rblShipContext_DataBound(object sender, EventArgs e)
        {
            RadioButtonList rbl = (RadioButtonList)sender;
            if (rbl.Items.Count > 0 && rbl.SelectedIndex == -1)
                rbl.SelectedIndex = 0;
        }
        protected void chkShowDates_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckBoxList list = (CheckBoxList)sender;
            //chkShowTickets.DataBind();
        }
        protected void ddlEmailLetter_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            EmailLetterCollection coll = new EmailLetterCollection();

            //get a list of the file names in the cust service sent directory
            string mappedDirectory = Server.MapPath(string.Format("/{0}/MailTemplates/CustomerServiceSent/", 
                _Config._VirtualResourceDir));
            List<string> files = new List<string>();
            files.AddRange(Directory.GetFiles(mappedDirectory));

            if (files.Count > 0)
            {
                //get a list of emailletters that correspond to the file list
                string fileList = string.Empty;
                foreach (string file in files)
                    fileList += string.Format("'{0}',", Path.GetFileName(file));
                fileList = fileList.TrimEnd(',');

                string sql = string.Format("SELECT * FROM [EmailLetter] el WHERE el.[Name] IN ({0})", fileList);
                SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sql, SubSonic.DataService.Provider.Name);

                try
                {
                    coll.LoadAndCloseReader(SubSonic.DataService.GetReader(cmd));
                }
                catch (Exception ex)
                {
                    CustomSend.IsValid = false;
                    CustomSend.ErrorMessage = ex.Message;
                    return;
                }
            }

            ddl.DataSource = coll;
        }
        protected void ddlEmailLetter_DataBound(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            if (Atx.CurrentEmailLetter != null && ddl.SelectedValue != Atx.CurrentEmailLetter.Id.ToString())
            {
                ddl.SelectedIndex = -1;
                ListItem li = ddl.Items.FindByValue(Atx.CurrentEmailLetter.Id.ToString());
                if (li != null)
                    li.Selected = true;
            }

            if (ddl.Items.Count > 0 && (ddl.SelectedIndex == -1 || ddl.SelectedIndex == 0) && Atx.CurrentEmailLetter == null)
            {
                ddl.SelectedIndex = 0;
                Atx.CurrentEmailLetter = EmailLetter.FetchByID(int.Parse(ddl.SelectedValue));
            }

            
        }
        protected void litPreview_DataBinding(object sender, EventArgs e)
        {
            Literal lit = (Literal)sender;
            lit.Text = "<iframe src=\"/Admin/AdminControls/Mailer_EmailLetterPreview.aspx\" width=\"100%\" height=\"400px\" align=\"left\"></iframe>";
        }
        protected void ddlEmailLetter_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            Atx.CurrentEmailLetter = EmailLetter.FetchByID(int.Parse(ddl.SelectedValue));

            //refresh the iframe
            litPreview.DataBind();
        }
        protected void SqlParentList_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@appId"].Value = Wcss._Config.APPLICATION_ID;
        }
        protected void SqlShowList_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@appId"].Value = Wcss._Config.APPLICATION_ID;
            e.Command.Parameters["@startDate"].Value = clockStart.SelectedDate.Date;
            e.Command.Parameters["@endDate"].Value = clockEnd.SelectedDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
        }
        protected void SqlTicketList_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            //we must have a show declared
            if(ddlShowList.SelectedIndex == -1 || ddlShowList.SelectedValue == "0" || chkShowDates.SelectedIndex == -1)
            {
                e.Cancel = true;
                return;
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            //if we have 0 as showdateid - then show all tickets for that show
            List<string> selections = new List<string>();
            foreach (ListItem li in chkShowDates.Items)
                if(li.Selected)
                    selections.Add(li.Value);

            sb.Append("IF EXISTS (SELECT * FROM [ShowTicket] WHERE [tShowId] = @showId ) BEGIN ");

            sb.Append("SELECT ' [..All Tickets..]' as TicketName, 0 as TicketId UNION ");

            if(selections.Contains("0"))
            {
                sb.Append("SELECT '(' + CAST(st.[iSold] as varchar) + ') ' + ");
                sb.Append("(CASE WHEN st.[bDosTicket] IS NOT NULL AND st.[bDosTicket] = 1 THEN 'DOS ' ELSE '' END) + ");
                sb.Append("'$' + CAST(st.[mPrice] as varchar) + ' + ' + CAST(st.[mServiceCharge] as varchar) + ' svc ' + ");
                sb.Append(" st.[CriteriaText] + ' ' + st.[SalesDescription] as TicketName, ");
                sb.Append("st.[Id] as TicketId FROM [ShowTicket] st WHERE [tShowId] = @showId ");
                sb.Append("ORDER BY TicketName ASC END ");
            }
            else
            {
                sb.Append("SELECT '(' + CAST(st.[iSold] as varchar) + ') ' + ");
                sb.Append("(CASE WHEN st.[bDosTicket] IS NOT NULL AND st.[bDosTicket] = 1 THEN 'DOS ' ELSE '' END) + ");
                sb.Append("'$' + CAST(st.[mPrice] as varchar) + ' + ' + CAST(st.[mServiceCharge] as varchar) + ' svc ' + ");
                sb.Append(" st.[CriteriaText] + ' ' + st.[SalesDescription] as TicketName, ");
                sb.AppendFormat("st.[Id] as TicketId FROM [ShowTicket] st WHERE [tShowDateId] IN (");
                foreach (string s in selections)
                    sb.AppendFormat("{0},", s);
                sb.Length = sb.Length -1;
                sb.Append(") ORDER BY TicketName ASC END ");
            }

            e.Command.Parameters["@showId"].Value = int.Parse(ddlShowList.SelectedValue);
            e.Command.CommandText = sb.ToString();
        }
}
}