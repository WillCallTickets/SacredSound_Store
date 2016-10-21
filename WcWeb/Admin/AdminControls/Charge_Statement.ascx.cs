using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Wcss;

namespace WillCallWeb.Admin.AdminControls
{
    public partial class Charge_Statement : BaseControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlYear.DataBind();
            }
        }

        #region Year Selection

        protected void ddlYear_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            List<int> yrs = new List<int>();
            for (int i = DateTime.Now.Year; i >= _Config._ApplicationStartDate.Year; i--)
                yrs.Add(i);

            ddl.DataSource = yrs;
        }
        protected void ddlYear_DataBound(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            if (ddl.SelectedIndex == -1 && ddl.Items.Count > 0)
                ddl.SelectedIndex = 0;
        }
        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            GridView1.SelectedIndex = -1;
            GridView1.DataBind();
        }

        #endregion

        #region Total Vars

        protected int _salesQty = 0;
        protected decimal _salesQtyPortion = 0;
        protected int _refundQty = 0;
        protected decimal _refundQtyPortion = 0;
        protected decimal _gross = 0;
        protected decimal _grossPct = 0;
        protected decimal _grossPortion = 0;
        protected decimal _ticketPortion = 0;
        protected decimal _merchPortion = 0;
        protected decimal _shipPortion = 0;
        protected decimal _mailerPortion = 0;
        protected decimal _hourlyPortion = 0;
        protected decimal _discount = 0;
        protected decimal _lineTotal = 0;
        protected decimal _amountPaid = 0;

        #endregion

        #region Month Grid

        protected void SqlMonths_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            //if we are looking at current year - redo current month data
            if (ddlYear.SelectedValue == DateTime.Now.Year.ToString())
            {
                SPs.TxChargeStatementEdit(
                    _Config.APPLICATION_NAME, DateTime.Now.Month, DateTime.Now.Year,
                    ChargeStatement._Rate_PerSale, ChargeStatement._Rate_PerRefund,
                    ChargeStatement._Rate_PctGrossSalesThreshhold, ChargeStatement._Rate_PctGrossSales,
                    ChargeStatement._Rate_PerTicketInvoice, ChargeStatement._Rate_PerTicketUnit,
                    ChargeStatement._Rate_PctTicketSales, ChargeStatement._Rate_PerMerchInvoice,
                    ChargeStatement._Rate_PerMerchUnit, ChargeStatement._Rate_PctMerchSales,
                    ChargeStatement._Rate_PerTktShip, ChargeStatement._Rate_PctTktShipSales, 
                    ChargeStatement._Rate_PerSubscription, ChargeStatement._Rate_PerMailSent, 
                    true).Execute();
            }

            e.Command.Parameters["@appName"].Value = _Config.APPLICATION_NAME;
        }

        protected void GridView1_DataBinding(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            _salesQty = 0;
            _salesQtyPortion = 0;
            _refundQty = 0;
            _refundQtyPortion = 0;
            _gross = 0;
            _grossPct = 0;
            _grossPortion = 0;
            _ticketPortion = 0;
            _merchPortion = 0;
            _shipPortion = 0;
            _mailerPortion = 0;
            _hourlyPortion = 0;
            _discount = 0;
            _lineTotal = 0;
            _amountPaid = 0;
        }
        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            if (grid.SelectedIndex == -1 && grid.Rows.Count > 0)
                grid.SelectedIndex = grid.Rows.Count - 1;
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;
            GridViewRow rowView = e.Row;
            DataRowView dataRow = (DataRowView)rowView.DataItem;
            
            if (rowView.RowType == DataControlRowType.DataRow)
            {
                DataRow row = dataRow.Row;

                _salesQty += (int)Utils.DataHelper.GetColumnValue(row, "SalesQty", DbType.Int32);
                _salesQtyPortion += (decimal)Utils.DataHelper.GetColumnValue(row, "SalesQtyPortion", DbType.Decimal);
                _refundQty += (int)Utils.DataHelper.GetColumnValue(row, "RefundQty", DbType.Int32);
                _refundQtyPortion += (decimal)Utils.DataHelper.GetColumnValue(row, "RefundQtyPortion", DbType.Decimal);
                _gross += (decimal)Utils.DataHelper.GetColumnValue(row, "Gross", DbType.Decimal);
                _grossPct += (decimal)Utils.DataHelper.GetColumnValue(row, "GrossPct", DbType.Decimal);
                _grossPortion += (decimal)Utils.DataHelper.GetColumnValue(row, "GrossPortion", DbType.Decimal);
                _ticketPortion += (decimal)Utils.DataHelper.GetColumnValue(row, "TicketPortion", DbType.Decimal);
                _merchPortion += (decimal)Utils.DataHelper.GetColumnValue(row, "MerchPortion", DbType.Decimal);
                _shipPortion += (decimal)Utils.DataHelper.GetColumnValue(row, "ShipPortion", DbType.Decimal);
                _mailerPortion += (decimal)Utils.DataHelper.GetColumnValue(row, "MailerPortion", DbType.Decimal);
                _hourlyPortion += (decimal)Utils.DataHelper.GetColumnValue(row, "HourlyPortion", DbType.Decimal);
                _discount += (decimal)Utils.DataHelper.GetColumnValue(row, "Discount", DbType.Decimal);
                _lineTotal += (decimal)Utils.DataHelper.GetColumnValue(row, "LineTotal", DbType.Decimal);
                _amountPaid += (decimal)Utils.DataHelper.GetColumnValue(row, "AmountPaid", DbType.Decimal);
            }
            else if (rowView.RowType == DataControlRowType.Footer)
            {
                rowView.Cells[1].Text = _salesQty.ToString();
                //skip sales qty rate
                rowView.Cells[3].Text = _salesQtyPortion.ToString("n2");
                rowView.Cells[4].Text = _refundQty.ToString();
                //skip refund qty rate
                rowView.Cells[6].Text = _refundQtyPortion.ToString("n2");
                rowView.Cells[7].Text = _gross.ToString("n2");
                //skip gross rate
                rowView.Cells[9].Text = _grossPortion.ToString("n2");
                rowView.Cells[10].Text = _ticketPortion.ToString("n2");
                rowView.Cells[11].Text = _merchPortion.ToString("n2");
                rowView.Cells[12].Text = _shipPortion.ToString("n2");
                rowView.Cells[13].Text = _mailerPortion.ToString("n2");
                rowView.Cells[14].Text = _hourlyPortion.ToString("n2");
                rowView.Cells[15].Text = _discount.ToString("n2");
                rowView.Cells[16].Text = _lineTotal.ToString("n2");
                rowView.Cells[17].Text = _amountPaid.ToString("n2");
            }
        }
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;
        }

        #endregion

        #region Form and Child grids, etc

        protected void FormView1_DataBound(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;

            if (form.DataItem != null)
            {
                DataRowView drv = (DataRowView)form.DataItem;
                DataRow row = drv.Row;
                Literal title = (Literal)form.FindControl("litTitle");
                Button save = (Button)form.FindControl("btnSave");
                Literal sub = (Literal)form.FindControl("litSubtotal");
                
                if(title != null)
                    title.Text = DateTime.Parse(Utils.DataHelper.GetColumnValue(row, "MonthYear", DbType.String).ToString())
                        .ToString("MMMM yyyy");

                if(save != null)
                {
                    int year = int.Parse(ddlYear.SelectedValue);
                    int mo = (int)Utils.DataHelper.GetColumnValue(row, "iMonth", DbType.Int32);
                    DateTime now = DateTime.Now;

                    save.Visible = (!(now.Year == year && now.Month == mo));
                }

                if(sub != null)
                {
                    decimal subtotal = (decimal)Utils.DataHelper.GetColumnValue(row, "SalesQtyPortion", DbType.Decimal) +
                        (decimal)Utils.DataHelper.GetColumnValue(row, "RefundQtyPortion", DbType.Decimal) +
                        (decimal)Utils.DataHelper.GetColumnValue(row, "GrossPortion", DbType.Decimal) +
                        (decimal)Utils.DataHelper.GetColumnValue(row, "TicketPortion", DbType.Decimal) +
                        (decimal)Utils.DataHelper.GetColumnValue(row, "MerchPortion", DbType.Decimal) +
                        (decimal)Utils.DataHelper.GetColumnValue(row, "ShipPortion", DbType.Decimal) +
                        (decimal)Utils.DataHelper.GetColumnValue(row, "MailerPortion", DbType.Decimal) +
                        (decimal)Utils.DataHelper.GetColumnValue(row, "HourlyPortion", DbType.Decimal);
                    sub.Text = subtotal.ToString("n2");
                }
            }

        }
        protected void FormView1_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            string cmd = e.CommandName.ToLower();

            switch (cmd)
            {
                case "create":
                    int year = int.Parse(ddlYear.SelectedValue);
                    int mo = int.Parse(GridView1.DataKeys[GridView1.SelectedIndex]["iMonth"].ToString());

                    SPs.TxChargeStatementEdit(
                        _Config.APPLICATION_NAME, mo, year,
                        ChargeStatement._Rate_PerSale, ChargeStatement._Rate_PerRefund,
                        ChargeStatement._Rate_PctGrossSalesThreshhold, ChargeStatement._Rate_PctGrossSales,
                        ChargeStatement._Rate_PerTicketInvoice, ChargeStatement._Rate_PerTicketUnit,
                        ChargeStatement._Rate_PctTicketSales, ChargeStatement._Rate_PerMerchInvoice,
                        ChargeStatement._Rate_PerMerchUnit, ChargeStatement._Rate_PctMerchSales,
                        ChargeStatement._Rate_PerTktShip, ChargeStatement._Rate_PctTktShipSales,
                        ChargeStatement._Rate_PerSubscription, ChargeStatement._Rate_PerMailSent, 
                        true).Execute();

                    GridView1.DataBind();

                    break;
            }
        }
        protected void GridHours_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            if (grid.SelectedIndex == -1 && grid.Rows.Count > 0)
                grid.SelectedIndex = grid.Rows.Count - 1;
        }
        protected void FormHours_DataBound(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;
            TextBox rate = (TextBox)form.FindControl("txtRate");
            
            if (form.CurrentMode == FormViewMode.Insert)
            {
                if (rate != null)
                    rate.Text = ChargeStatement._Rate_Hourly.ToString("n2");
            }
        }
        protected void SqlHourlyUnit_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            GridView hours = (GridView)FormView1.FindControl("GridHours");
            if(hours != null && hours.SelectedIndex != -1)
                e.Command.Parameters["@HourlyIdx"].Value = hours.SelectedValue;
        }
        protected void FormHours_ItemDeleting(object sender, FormViewDeleteEventArgs e)
        {
        }
        protected void SqlHourlyUnit_Deleting(object sender, SqlDataSourceCommandEventArgs e)
        {
            //ensure that input is valid
            e.Command.Parameters["@TChargeStatementId"].Value = (int)FormView1.SelectedValue;
        }
        protected void SqlHourlyUnit_Deleted(object sender, SqlDataSourceStatusEventArgs e)
        {
            GridView grid = (GridView)FormView1.FindControl("GridHours");
            grid.SelectedIndex = -1;

            GridView1.DataBind();
            FormView1.DataBind();
        }
        protected void SqlHourlyUnit_Inserting(object sender, SqlDataSourceCommandEventArgs e)
        {
            //ensure that input is valid
            e.Command.Parameters["@TChargeStatementId"].Value = (int)FormView1.SelectedValue;
        }
        protected void FormHours_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            //ensure that input is valid
            FormView form = (FormView)sender;
            CustomValidator val = (CustomValidator)form.FindControl("rowValidator");

            if(e.Values["dtPerformed"] == null || e.Values["dtPerformed"].ToString() == string.Empty)
                e.Values["dtPerformed"] = DateTime.Now.ToString();

            if ((e.Values["ServiceDescription"] == null || e.Values["ServiceDescription"].ToString() == string.Empty) && val != null)
            {
                val.IsValid = false;
                val.ErrorMessage = "Service description is required.";
                e.Cancel = true;
                return;
            }

            string hr = (string)e.Values["Hours"];
            string tr = (string)e.Values["Rate"];
            string fr = (string)e.Values["FlatRate"];

            decimal hd = 0;
            decimal rd = 0;
            decimal fd = 0;

            if (Utils.Validation.IsDecimal(hr))
                hd = Decimal.Parse(hr);
            if (Utils.Validation.IsDecimal(tr))
                rd = Decimal.Parse(tr);
            if (Utils.Validation.IsDecimal(fr))
                fd = Decimal.Parse(fr);

            if ((hd * rd) + fd == 0)
            {
                val.IsValid = false;
                val.ErrorMessage = "Please specify an amount to charge.";
                e.Cancel = true;
                return;
            }

            e.Values["Hours"] = hd;
            e.Values["Rate"] = rd;
            e.Values["FlatRate"] = fd;

        }
        protected void SqlHourlyUnit_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {
            GridView grid = (GridView)FormView1.FindControl("GridHours");
            grid.SelectedIndex = grid.Rows.Count + 1;
            grid.EditIndex = -1;
            GridView1.DataBind();
            FormView1.DataBind();
        }
        protected void SqlHourlyUnit_Updating(object sender, SqlDataSourceCommandEventArgs e)
        {
            e.Command.Parameters["@TChargeStatementId"].Value = (int)FormView1.SelectedValue;
        }
        protected void SqlHourlyUnit_Updated(object sender, SqlDataSourceStatusEventArgs e)
        {
            GridView1.DataBind();
            FormView1.DataBind();
        }

        #endregion
    }
}
