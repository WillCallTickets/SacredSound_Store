using System;
using System.Collections.Generic;

using Wcss;
using Wcss.QueryRow;

namespace WillCallWeb.Admin
{
    public partial class PrintTicketList : WillCallWeb.BasePage
    {
        #region Props

        List<TicketSalesRow> list = new List<TicketSalesRow>();
        List<_Enums.PrintTicketItemType> types = new List<_Enums.PrintTicketItemType>();
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        protected ShowDate ShowDateRecord = null;
        protected string ShowTicketIds = string.Empty;
        protected string ShipContext = "NA";
        protected string PurchaseContext = "PURCHASES";
        protected string VendorContext = "online";

        int tikCount = 0;
        
        #endregion

        #region Page Overhead

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            string script = "$('#" + chkPhone.ClientID + "').on('click', function() { $('.phoner').toggle(); })";
            Atx.RegisterJQueryScript(this, script, false);
        }

        protected override void OnPreInit(EventArgs e)
        {
            QualifySsl(false);
            base.OnPreInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetInputs();

                ConstructPageListing();
            }
        }

        #endregion

        #region Set Inputs

        private void SetInputs()
        {
            //dates have precedence
            string date = Request.QueryString["date"];

            if (date != null && Utils.Validation.IsInteger(date))
            {
                ShowDateRecord = new ShowDate(int.Parse(date));
                
                if (ShowDateRecord != null && ShowDateRecord.ShowRecord.ApplicationId != _Config.APPLICATION_ID)
                    ShowDateRecord = null;
            }
            else
            {
                CustomValidation.IsValid = false;
                CustomValidation.ErrorMessage = "The event date was not specified.";
                return;
            }

            if (ShowDateRecord == null)
            {
                string errorMsg = string.Format("The event specified ({0}) does not match the application ({1}).", date, _Config.APPLICATION_ID);

                _Error.LogException(new Exception(errorMsg));

                CustomValidation.IsValid = false;
                CustomValidation.ErrorMessage = errorMsg;
                return;
            }

            string tik = Request.QueryString["tik"];

            if (tik != null && tik.Trim().Length > 0 && tik.Trim() != "0")
                ShowTicketIds = tik;
            else
                ShowTicketIds = string.Empty;

            string ctx = Request.QueryString["ctx"];
            if (ctx != null)
                ShipContext = ctx;

            string purch = Request.QueryString["purch"];
            if (purch != null)
                PurchaseContext = purch;
            
            types.AddRange(_Config._PrintTicketTypeList);
            types.Add(_Enums.PrintTicketItemType.PhoneBilling);
            types.Add(_Enums.PrintTicketItemType.PhoneShipping);
            types.Add(_Enums.PrintTicketItemType.PhoneProfile);

            list = TicketSalesRow.GetTicketIdSales(ShowDateRecord.Id, ShowTicketIds, ShipContext, PurchaseContext,
                Ctx.Search_TicketManifestSort.ToString(), 1, 100000);
        }

        #endregion

        private void ConstructPageListing()
        {
            sb.Length = 0;
            LiteralSales.Text = SalesTable(true);

            //this needs to follow - it needs the list
            sb.Length = 0;
            LiteralInventory.Text = ShowInventory();
        }

        private string ShowInventory()
        {
            sb.AppendFormat("{1}<div class=\"header\">", Utils.Constants.NewLine, Utils.Constants.Tab);

            int sold = 0;
            if (ShowTicketIds != null && ShowTicketIds.Trim().Length > 0 && ShowTicketIds != "0")
            {
                foreach (TicketSalesRow tsr in list)
                    sold += tsr.Qty;
            }
            else
                sold = ShowDateRecord.SoldChildren;

            sb.AppendFormat("Ship Context: {0} - &nbsp;", ShipContext);

            sb.AppendFormat("Sold: {0} - &nbsp;", sold);

            if (ShowTicketIds != null && ShowTicketIds.Trim().Length > 0 && ShowTicketIds != "0")
            {
                ShowTicketCollection coll = new ShowTicketCollection();
                string sql = "SELECT * FROM [ShowTicket] st WHERE st.[Id] IN (SELECT DISTINCT [ListItem] FROM fn_ListToTable(@idxs)) ";
                Wcss._DatabaseCommandHelper cmd = new Wcss._DatabaseCommandHelper(sql);
                cmd.AddCmdParameter("idxs", ShowTicketIds.Replace('~', ','), System.Data.DbType.String);

                try
                {
                    coll.LoadAndCloseReader(cmd.GetReader());
                }
                catch (System.Data.SqlClient.SqlException sex)
                {
                    _Error.LogException(sex);
                    throw new Exception(string.Format("{0} Sql Error.\r\n{1}\r\n{2}", "Print - get sold count", sex.Message, sex.StackTrace));
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                    throw new Exception(string.Format("{0} Error.\r\n{1}\r\n{2}", "Print - get sold count", ex.Message, ex.StackTrace));
                }

                sb.Append("Ticket Listing");
                foreach (ShowTicket st in coll)
                {
                    sb.AppendFormat("<div>{0}</div>", Utils.ParseHelper.StripHtmlTags(string.Format("Id: {0} - Ages: {1} - Price: {2} {3} {4}",
                        st.Id, st.AgeDescription, st.Price.ToString("c"),
                        (st.CriteriaText_Derived.Trim().Length > 0) ? st.CriteriaText_Derived.Trim() : string.Empty,
                        (st.SalesDescription_Derived.Trim().Length > 0) ? st.SalesDescription_Derived.Trim() : string.Empty)));
                }
            }
            else
            {
                sb.AppendFormat("{1}All Tickets For: {2}{0}", Utils.Constants.NewLine, Utils.Constants.Tab,
                    ShowDateRecord.DateOfShow.ToString("MM/dd/yyyy hh:mmtt"));
            }
            sb.AppendFormat("{1}</div>{0}", Utils.Constants.NewLine, Utils.Constants.Tab);

            return sb.ToString();
        }     

        private string SalesTable(bool isNotForDownload)
        {
            if (isNotForDownload)
                sb.AppendFormat("{1}<table class=\"sales\" cellspacing=\"1\" cellpadding=\"0\" border=\"1\">{0}", 
                    Utils.Constants.NewLine, Utils.Constants.Tab);

            bool includePhone = (!isNotForDownload && chkPhone.Checked);

            //do header row
            CreateHeaderRow(isNotForDownload, includePhone);
            
            //reset count
            tikCount = 0;

            CreateListingRows(isNotForDownload, includePhone);

            if (isNotForDownload)
                CreateFooterRow();

            if (isNotForDownload)
                sb.AppendFormat("{1}</table><br/><br/>{0}", Utils.Constants.NewLine, Utils.Constants.Tab);

            return sb.ToString();
        }

        private void CreateHeaderRow(bool isNotForDownload, bool includePhone)
        {
            //sales header row
            if (isNotForDownload)
            {
                sb.AppendFormat("{1}<tr class=\"saleheader\">{0}", Utils.Constants.NewLine, Utils.Constants.Tab);
                sb.AppendFormat("{1}<th align=\"center\" class=\"qty\">Tkt Id</th>{0}", Utils.Constants.NewLine, Utils.Constants.Tab);
            }
            else
                //sb.Append("HasShipped,TicketId,");
                sb.Append("TicketId,");
            

            foreach (Wcss._Enums.PrintTicketItemType type in types)
            {
                switch (type)
                {
                    case _Enums.PrintTicketItemType.PickupName:
                        if (isNotForDownload)
                            sb.AppendFormat("{1}<th align=\"left\" class=\"pickup\">Pickup Name</th>{0}", Utils.Constants.NewLine, Utils.Constants.Tab);
                        else
                            sb.Append("Pickup Name,");
                        break;
                    case _Enums.PrintTicketItemType.PurchaseName:
                        if (isNotForDownload)
                            sb.AppendFormat("{1}<th align=\"left\" class=\"pickup\">Purchase Name</th>{0}", Utils.Constants.NewLine, Utils.Constants.Tab);
                        else
                            sb.Append("Purchase Name,");
                        break;
                    case _Enums.PrintTicketItemType.NameOnCard:
                        if (isNotForDownload)
                            sb.AppendFormat("{1}<th align=\"left\" class=\"pickup\">Name On Card</th>{0}", Utils.Constants.NewLine, Utils.Constants.Tab);
                        else
                            sb.Append("Name On Card,");
                        break;
                    case _Enums.PrintTicketItemType.LastFourDigits:
                        if (isNotForDownload)
                            sb.AppendFormat("{1}<th align=\"left\" class=\"pickup\">Last Four</th>{0}", Utils.Constants.NewLine, Utils.Constants.Tab);
                        else
                            sb.Append("Last Four,");
                        break;
                    case _Enums.PrintTicketItemType.Email:
                        if (isNotForDownload)
                            sb.AppendFormat("{1}<th align=\"left\" class=\"email\">Email</th>{0}", Utils.Constants.NewLine, Utils.Constants.Tab);
                        else
                            sb.Append("Email,");
                        break;
                    case _Enums.PrintTicketItemType.InvoiceId:
                        if (isNotForDownload)
                            sb.AppendFormat("{1}<th align=\"left\" class=\"pickup\">InvoiceId</th>{0}", Utils.Constants.NewLine, Utils.Constants.Tab);
                        else
                            sb.Append("InvoiceId,");
                        break;
                    case _Enums.PrintTicketItemType.Quantity:
                        if (isNotForDownload)
                            sb.AppendFormat("{1}<th width=\"30px\" class=\"qty\">Qty</th>{0}", Utils.Constants.NewLine, Utils.Constants.Tab);
                        else
                            sb.Append("Qty,");
                        break;
                    case _Enums.PrintTicketItemType.Age:
                        if (isNotForDownload)
                            sb.AppendFormat("{1}<th class=\"age\">Age</th>{0}", Utils.Constants.NewLine, Utils.Constants.Tab);
                        else
                            sb.Append("Age,");
                        break;
                    case _Enums.PrintTicketItemType.Notes:
                        if (isNotForDownload)
                            sb.AppendFormat("{1}<th align=\"left\" class=\"notes\">Notes</th>{0}", Utils.Constants.NewLine, Utils.Constants.Tab);
                        else
                            sb.Append("Notes,");
                        break;
                    case _Enums.PrintTicketItemType.ReturnedToSenderRTS:
                        if (isNotForDownload)
                            sb.AppendFormat("{1}<th class=\"notes\">RTS</th>{0}", Utils.Constants.NewLine, Utils.Constants.Tab);
                        else
                            sb.Append("RTS,");
                        break;
                    case _Enums.PrintTicketItemType.ShipDate:
                        if (isNotForDownload)
                            sb.AppendFormat("{1}<th nowrap class=\"dtship\">Shipped On</th>{0}", Utils.Constants.NewLine, Utils.Constants.Tab);
                        else
                            sb.Append("Shipped On,");
                        break;
                    case _Enums.PrintTicketItemType.ShipMethod:
                        if (isNotForDownload)
                            sb.AppendFormat("{1}<th nowrap class=\"shipmeth\">Ship Meth</th>{0}", Utils.Constants.NewLine, Utils.Constants.Tab);
                        else
                            sb.Append("Ship Meth,");
                        break;
                    case _Enums.PrintTicketItemType.PhoneBilling:
                    case _Enums.PrintTicketItemType.PhoneShipping:
                    case _Enums.PrintTicketItemType.PhoneProfile:
                        if(!isNotForDownload)
                            sb.AppendFormat("{0},", type.ToString());
                        break;
                }
            }

            if (isNotForDownload)
                sb.AppendFormat("{1}</tr>{0}", Utils.Constants.NewLine, Utils.Constants.Tab);
            else
            {
                sb.Length = sb.Length - 1;//trim the trailing comma

                sb.AppendLine();
            }
        }

        private void CreateListingRows(bool isNotForDownload, bool includePhone)
        {
            foreach (TicketSalesRow sale in list)
            {
                string rowstyle = string.Empty;
                string shipped = sale.DateShipped;

                //indicate if shipped
                if (isNotForDownload)
                {
                    if ((!sale.IsReturned) && shipped != null && shipped.Trim().Length > 0 && Utils.Validation.IsDate(shipped))
                        rowstyle = string.Format(" style=\"background-color: #e1e1e1; text-decoration: line-through;\" ");

                    sb.AppendFormat("{1}<tr class=\"salerow\"{2}>{0}", Utils.Constants.NewLine, Utils.Constants.Tab,
                        (rowstyle.Trim().Length > 0) ? rowstyle : string.Empty);
                }

                //id is always first
                if (isNotForDownload)
                    sb.AppendFormat("{1}<td align=\"center\" class=\"qty\">{2}</td>{0}", Utils.Constants.NewLine, Utils.Constants.Tab,
                        sale.ShowTicketId.ToString());                
                else
                    sb.AppendFormat("{0},", sale.ShowTicketId.ToString());
                    
                foreach (Wcss._Enums.PrintTicketItemType type in types)
                {
                    switch (type)
                    {
                        case _Enums.PrintTicketItemType.PickupName:
                            if (isNotForDownload)
                                sb.AppendFormat("{1}<td class=\"pickup\"><div class=\"pickname\">{2}</div><div class=\"phoner\"><label>(ship)</label>{3}</div></td>{0}",
                                    Utils.Constants.NewLine, Utils.Constants.Tab, sale.PickupName, sale.PhoneShipping);
                            else
                                sb.AppendFormat("{0},", sale.PickupName.Replace(',','_'));
                            break;
                        case _Enums.PrintTicketItemType.PurchaseName:
                            if (isNotForDownload)
                                sb.AppendFormat("{1}<td class=\"pickup\">{2}<div class=\"phoner\"><label>(bill)</label>{3}</div></td>{0}",
                                    Utils.Constants.NewLine, Utils.Constants.Tab, sale.PurchaseName, sale.PhoneBilling);
                            else
                                sb.AppendFormat("{0},", sale.PurchaseName.Replace(',', '_'));
                            break;
                        case _Enums.PrintTicketItemType.NameOnCard:
                            if (isNotForDownload)
                                sb.AppendFormat("{1}<td class=\"pickup\">{2}</td>{0}",
                                    Utils.Constants.NewLine, Utils.Constants.Tab, sale.NameOnCard);
                            else
                                sb.AppendFormat("{0},", sale.NameOnCard.Replace(',', '_'));
                            break;
                        case _Enums.PrintTicketItemType.LastFourDigits:
                            if (isNotForDownload)
                                sb.AppendFormat("{1}<td class=\"pickup\">{2}</td>{0}",
                                    Utils.Constants.NewLine, Utils.Constants.Tab, sale.LastFour);
                            else
                                sb.AppendFormat("{0},", sale.LastFour.Replace(',', '_'));
                            break;
                        case _Enums.PrintTicketItemType.Email:
                            if (isNotForDownload)
                                sb.AppendFormat("{1}<td class=\"email\">{2}<div class=\"phoner\"><label>(prof)</label>{3}</div></td>{0}",
                                    Utils.Constants.NewLine, Utils.Constants.Tab, sale.Email, sale.PhoneProfile);
                            else
                                sb.AppendFormat("{0},", sale.Email);
                            break;
                        case _Enums.PrintTicketItemType.InvoiceId:
                            if (isNotForDownload)
                                sb.AppendFormat("{1}<td class=\"pickup\">{2}</td>{0}",
                                    Utils.Constants.NewLine, Utils.Constants.Tab, sale.UniqueInvoiceId);
                            else
                                sb.AppendFormat("{0},", sale.UniqueInvoiceId.Replace(',', '_'));
                            break;
                        case _Enums.PrintTicketItemType.Quantity:
                            if (isNotForDownload)
                                sb.AppendFormat("{1}<td align=\"center\" class=\"qty\">{2}</td>{0}",
                                    Utils.Constants.NewLine, Utils.Constants.Tab, sale.Qty.ToString());
                            else
                                sb.AppendFormat("{0},", sale.Qty.ToString());

                            tikCount += sale.Qty;
                            break;
                        case _Enums.PrintTicketItemType.Age:
                            if (isNotForDownload)
                                sb.AppendFormat("{1}<td align=\"center\" class=\"age\">{2}</td>{0}",
                                    Utils.Constants.NewLine, Utils.Constants.Tab, sale.Age);
                            else
                                sb.AppendFormat("{0},", sale.Age.Replace(',', '_'));
                            break;
                        case _Enums.PrintTicketItemType.Notes:
                            if (isNotForDownload)
                                sb.AppendFormat("{1}<td class=\"notes\">{2}&nbsp;</td>{0}",
                                    Utils.Constants.NewLine, Utils.Constants.Tab, sale.Notes);
                            else
                                sb.AppendFormat("{0},", sale.Notes.Replace(',', '_'));
                            break;
                        case _Enums.PrintTicketItemType.ReturnedToSenderRTS:
                            if (isNotForDownload)
                                sb.AppendFormat("{1}<td class=\"notes\"><input type=\"checkbox\" name=\"chkRTS\" disabled=\"disabled\" {2}/></td>{0}",
                                    Utils.Constants.NewLine, Utils.Constants.Tab, (sale.IsReturned) ? "checked" : string.Empty);
                            else
                                sb.AppendFormat("{0},", (sale.IsReturned) ? "returned" : string.Empty);
                            break;
                        case _Enums.PrintTicketItemType.ShipDate:
                            if (isNotForDownload)
                                sb.AppendFormat("{1}<td align=\"center\" class=\"dtship\">{2}&nbsp;</td>{0}",
                                    Utils.Constants.NewLine, Utils.Constants.Tab, sale.DateShipped);
                            else
                                sb.AppendFormat("{0},", sale.DateShipped);
                            break;
                        case _Enums.PrintTicketItemType.ShipMethod:
                            if (isNotForDownload)
                                sb.AppendFormat("{1}<td width=\"100px\" align=\"center\" class=\"shipmeth\">{2}&nbsp;</td>{0}",
                                    Utils.Constants.NewLine, Utils.Constants.Tab, sale.ShippingMethod);
                            else
                                sb.AppendFormat("{0},", sale.ShippingMethod.Replace(',', '_'));
                            break;
                        case _Enums.PrintTicketItemType.PhoneBilling:
                            if (!isNotForDownload)
                                sb.AppendFormat("{0},", sale.PhoneBilling.Replace(',', '_'));
                            break;                        
                        case _Enums.PrintTicketItemType.PhoneShipping:
                            if (!isNotForDownload)
                                sb.AppendFormat("{0},", sale.PhoneShipping.Replace(',', '_'));
                            break;                        
                        case _Enums.PrintTicketItemType.PhoneProfile:
                            if (!isNotForDownload)
                                sb.AppendFormat("{0},", sale.PhoneProfile.Replace(',', '_'));
                            break;
                    }
                }

                if (isNotForDownload)
                    sb.AppendFormat("{1}</tr>{0}", Utils.Constants.NewLine, Utils.Constants.Tab);
                else
                {
                    sb.Length = sb.Length - 1;//trim the trailing comma

                    sb.AppendLine();
                }
            }
        }

        private void CreateFooterRow()
        {
            //footer - mostly placeholders
            sb.AppendFormat("{1}<tr>{0}", Utils.Constants.NewLine, Utils.Constants.Tab);

            //add one for the id row
            sb.AppendFormat("{1}<td colspan=\"2\">&nbsp;</td>{0}", Utils.Constants.NewLine, Utils.Constants.Tab);

            foreach (Wcss._Enums.PrintTicketItemType type in types)
            {
                switch (type)
                {
                    case _Enums.PrintTicketItemType.Quantity:
                        sb.AppendFormat("{1}<td style=\"text-align: center;\" class=\"qty\">{2}</td>{0}",
                            Utils.Constants.NewLine, Utils.Constants.Tab, tikCount.ToString());
                        break;
                    //default:
                    //    sb.AppendFormat("{1}<td>&nbsp;</td>{0}",
                    //        Utils.Constants.NewLine, Utils.Constants.Tab);
                    //    break;
                }
            }

            sb.AppendFormat("{1}<td colspan=\"99\">&nbsp;</td></tr>{0}", Utils.Constants.NewLine, Utils.Constants.Tab);
        }

        #region CSV export

        protected void btnExport_Click(object sender, EventArgs e)
        {   
            ExportCSV();
        }
        public void ExportCSV()
        {
            SetInputs();

            if (ShowDateRecord != null)
            {
                string filename = string.Format("{0}_{1}_{2}_{3}", 
                    ShowDateRecord.DateOfShow.ToString("MMddyyyyhhmmtt"),
                    string.Format("tktids{0}", ShowTicketIds),
                    string.Format("shpctx{0}", ShipContext.ToUpper()),
                    string.Format("{0}", PurchaseContext.ToUpper())
                    );

                string attachment = string.Format("attachment; filename=CsvList_{0}.csv", filename);

                System.Web.HttpContext context = System.Web.HttpContext.Current;
                context.Response.Clear();
                context.Response.ContentType = "application/x-download";//"text/csv";
                context.Response.AddHeader("Content-Disposition", attachment);

                sb.Length = 0;

                context.Response.Write(SalesTable(false));

                context.Response.End();
            }
        }


        #endregion
    }
}