using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.ComponentModel;

using WillCallWeb.StoreObjects;

namespace WillCallWeb.Components.Util
{
    /// <summary>
    /// Note that viewstate may need to be reinstated to have the time portion function properly
    /// </summary>
    [ToolboxData("<{0}:CalendarClock runat=\"Server\" DateQualifierText=\"\" IsRequired=\"false\" TabIndex=\"0\" ValidationGroup=\"\" UseReset=\"true\" SelectedValue=\"\" UseDate=\"true\" Width=\"100%\" UseTime=\"true\"></{0}:CalendarClock>")]
    [DefaultPropertyAttribute("SelectedValue")]
    [ValidationProperty("SelectedValue")]
    public partial class CalendarClock : BaseControl
    {
        #region Page Overhead

        protected string _width = "300px";
        public string Width { get 
        {
            if (_width == "300px" && UseDate && (!UseTime) && (!UseReset))
                _width = "120px";
            return _width; 
        } set { _width = value; } }
        public int TabIndex { get { return txtDate.TabIndex; } set { txtDate.TabIndex = (short)value; } }
        protected string _valGroup = string.Empty;
        public string ValidationGroup { get { return _valGroup; } set { _valGroup = value; } }
        public bool UseDate { get { return txtDate.Visible; } set { txtDate.Visible = value; ImageButton2.Visible = value; } }
        public bool UseTime { get { return ddlHour.Visible; } set { ddlHour.Visible = value; ddlMinute.Visible = value; ddlAmpm.Visible = value; } }
        public bool UseReset { get { return btnReset.Visible; } set { btnReset.Visible = value; } }
        public bool HasSelection { get { return (SelectedDate.Date != DefaultValue.Date); } }


        public DropDownList AMPM { get { return this.ddlAmpm; } }
        
        public DateTime _defaultValue = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
        public DateTime DefaultValue
        {
            get
            {
                if (_defaultValue == null || _defaultValue <= System.Data.SqlTypes.SqlDateTime.MinValue.Value)
                    _defaultValue = System.Data.SqlTypes.SqlDateTime.MinValue.Value;

                return _defaultValue;
            }
            set
            {
                if (value == null || value <= System.Data.SqlTypes.SqlDateTime.MinValue.Value)
                    value = System.Data.SqlTypes.SqlDateTime.MinValue.Value;

                _defaultValue = value;
            }
        }
        public string SelectedValue 
        { 
            get 
            {
                return SelectedDate.ToString();
            }
            set
            {
                if (value == null || value.Trim().Length == 0 || DateTime.Parse(value) <= System.Data.SqlTypes.SqlDateTime.MinValue || DateTime.Parse(value).Date == DateTime.MaxValue.Date)
                    value = DefaultValue.ToString();

                SelectedDate = DateTime.Parse(value);
            }
        }
        public DateTime _selectedDate = DateTime.MinValue;
        public DateTime SelectedDate
        {
            get
            {
                if (this.UseDate && txtDate.Text.Trim().Length == 0)
                    return DefaultValue;

                try
                {
                    return DateTime.Parse(string.Format("{0} {1}:{2}{3}", (txtDate.Text.Trim().Length > 0) ? txtDate.Text : "1/1/1980",
                         (UseTime) ? ddlHour.SelectedValue : "12", (UseTime) ? ddlMinute.SelectedValue : "00", (UseTime) ? ddlAmpm.SelectedValue : "AM"));
                }
                catch(Exception)
                {
                    return DefaultValue;
                }
            }
            set
            {
                _selectedDate = value;
                SetControlsToSelectedDate(_selectedDate);
            }
        }

        private void SetControlsToSelectedDate(DateTime selected)
        {
            //DateTime min = ;
            if (selected > System.Data.SqlTypes.SqlDateTime.MinValue.Value && selected.Date != DateTime.MaxValue.Date)
            {
                txtDate.Text = this._selectedDate.ToString("MM/dd/yyyy");

                if (UseTime)
                {
                    string h = this._selectedDate.ToString("hh");

                    ListItem hour = ddlHour.Items.FindByText(h.TrimStart('0'));
                    if (hour != null)
                    {
                        ddlHour.SelectedIndex = -1;
                        hour.Selected = true;
                    }
                    ListItem minute = ddlMinute.Items.FindByText(this._selectedDate.ToString("mm"));
                    if (minute != null)
                    {
                        ddlMinute.SelectedIndex = -1;
                        minute.Selected = true;
                    }
                    ListItem ampm = ddlAmpm.Items.FindByText(this._selectedDate.ToString("tt"));
                    if (ampm != null)
                    {
                        ddlAmpm.SelectedIndex = -1;
                        ampm.Selected = true;
                    }
                }
            }
            else
            {
                txtDate.Text = string.Empty;
                ddlHour.SelectedIndex = -1;
                ddlMinute.SelectedIndex = -1;
                ddlAmpm.SelectedIndex = -1;
            }
        }

        #endregion

        public void Reset()
        {
            SelectedDate = DefaultValue;
            FireDateChange();
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        public void SelectedDateChange (object sender, EventArgs e)
        {
            FireDateChange();
        }
        private void FireDateChange()
        {
            OnSelectedDateChanged(new CalendarClockChangedEventArgs(SelectedDate));
        }

        public class CalendarClockChangedEventArgs : EventArgs
        {
            protected DateTime _dt;

            //Alt Constructor
            public CalendarClockChangedEventArgs(DateTime datetime)
            {
                _dt = datetime;
            }

            public DateTime ChosenDate { get { return _dt; } }
        }
        private static readonly object SelectedDateChangedEventKey = new object();

        public delegate void SelectedDateChangedEventHandler(object sender, CalendarClockChangedEventArgs e);

        public event SelectedDateChangedEventHandler SelectedDateChanged
        {
            add { Events.AddHandler(SelectedDateChangedEventKey, value); }
            remove { Events.RemoveHandler(SelectedDateChangedEventKey, value); }
        }
        public virtual void OnSelectedDateChanged(CalendarClockChangedEventArgs e)
        {
            SelectedDateChangedEventHandler handler = (SelectedDateChangedEventHandler)Events[SelectedDateChangedEventKey];

            if (handler != null)
                handler(this, e);
        }
}
}

