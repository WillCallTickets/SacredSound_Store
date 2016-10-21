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
    [ToolboxData("<{0}:CheckSlider runat=\"Server\" TitleText=\"CheckSliderTitle\" MinQty =\"0\" MaxQty =\"1\" ValidationGroup=\"\" ></{0}:CheckSlider>")]
    [DefaultPropertyAttribute("TitleText")]
    public partial class CheckSlider : BaseControl
    {
        public string TitleText { get { EnsureChildControls(); return lblTitle.Text; } set { EnsureChildControls(); lblTitle.Text = value; } }
        public bool Selected { get { EnsureChildControls(); return chkSelect.Checked; } set { EnsureChildControls(); chkSelect.Checked = value; } }

        private int _minQty;
        private int _maxQty;
        private int _selectedQty;
        public int MinQty { get { return _minQty; } set { _minQty = value; } }
        public int MaxQty { get { return _maxQty; } set { _maxQty = value; } }
        public int SelectedQty { get { return _selectedQty; } set { _selectedQty = value; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                litRange.DataBind();
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "rangeinput", " $(\":range\").rangeinput(); ", true); 
        }
        protected virtual void litRange_DataBinding(object sender, EventArgs e)
        {
            if (MaxQty > 1)
            {
                Literal lit = (Literal)sender;
                lit.Text = string.Format("<input runat=\"server\" type=\"range\" name=\"{0}_{1}\" min=\"{2}\" max=\"{3}\" value=\"{4}\" />",
                    //this.UniqueID, "qtySlider", "100", "300", "150");
                        this.ClientID, "qtySlider", MinQty.ToString(), MaxQty.ToString(), SelectedQty.ToString());
            }
        }

        protected override void LoadControlState(object savedState)
        {
            object[] ctlState = (object[])savedState;
            base.LoadControlState(ctlState[0]);
            this._minQty = (int)ctlState[1];
            this._maxQty = (int)ctlState[2];
            this._selectedQty = (int)ctlState[3];
        }

        protected override object SaveControlState()
        {
            object[] ctlState = new object[4];
            ctlState[0] = base.SaveControlState();
            ctlState[1] = this._minQty;
            ctlState[2] = this._maxQty;
            ctlState[3] = this._selectedQty;
            return ctlState;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.Page.RegisterRequiresControlState(this);
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            chkSelect.Visible = (MaxQty == 1);

            ChildControlsCreated = true;
        }


        #region Events

        protected void chkSelect_CheckChanged(object sender, EventArgs e)
        {
            Fire_CheckSlider_Changed();
        }
        private void Fire_CheckSlider_Changed()
        {
            OnCheckSlider_Changed(new CheckSliderChanged_EventArgs(Selected, SelectedQty));
        }
        private static readonly object CheckSlider_ChangedEventKey = new object();
        public delegate void CheckSliderChanged_EventHandler(object sender, CheckSliderChanged_EventArgs e);
        public class CheckSliderChanged_EventArgs : EventArgs
        {
            public bool Selected { get; set; }
            public int SelectedQty { get; set; }

            public CheckSliderChanged_EventArgs(bool selected, int selectedQty)
            {
                Selected = selected;
                SelectedQty = selectedQty;
            }
        }
        public event CheckSliderChanged_EventHandler CheckBoxSlider_Changed
        {
            add { Events.AddHandler(CheckSlider_ChangedEventKey, value); }
            remove { Events.RemoveHandler(CheckSlider_ChangedEventKey, value); }
        }
        public virtual void OnCheckSlider_Changed(CheckSliderChanged_EventArgs e)
        {
            CheckSliderChanged_EventHandler handler = (CheckSliderChanged_EventHandler)Events[CheckSlider_ChangedEventKey];

            if (handler != null)
                handler(this, e);
        }

        #endregion
    }
}

