using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;

using Wcss;

namespace WillCallWeb.Components.Navigation
{
    [ToolboxData("<{0}:gglPager runat=\"Server\" PagerTitle=\"\" PageSize=\"\" PageIndex=\"0\" PageButtonClass=\"\" ></{0}:gglPager>")]
    public partial class gglPager : WillCallWeb.BaseControl, IPostBackEventHandler
    {
        public void RaisePostBackEvent(string eventArgument)
        {
            List<string> parts = new List<string>();
            parts.AddRange(eventArgument.ToLower().Split('~'));

            if (parts.Count > 0)
            {
                string cmd = parts[0].ToLower();

                switch (cmd)
                {
                    case "pagelink":
                        int arg = int.Parse(parts[1]);
                        OnGooglePagerChanged(arg - 1);
                        break;
                }
            }
        }

        #region PageEvents && Enums

        public enum CssClasses
        {
            selectedpage,
            disabled
        }

        //next, prev, first, last, page
        public class GooglePagerEventArgs : EventArgs
        {
            protected int _pageSize;
            protected int _pageIndex;

            //Alt Constructor
            public GooglePagerEventArgs(int newPageSize, int newPageIndex)
            {
                _pageSize = newPageSize;
                _pageIndex = newPageIndex;
            }

            public int NewPageSize { get { return _pageSize; } }
            public int NewPageIndex { get { return _pageIndex; } }
        }

        public delegate void GooglePagerChangedEvent(object sender, GooglePagerEventArgs e);
        public event GooglePagerChangedEvent GooglePagerChanged;
        /// <summary>
        /// called for index changes - Caller should rebind control
        /// </summary>
        public void OnGooglePagerChanged(int newIndex)
        {
            if (newIndex < 0)
                newIndex = 0;

            //when we change page size - reset the page index as well
            PageIndex = newIndex;

            if (GooglePagerChanged != null) { GooglePagerChanged(this, new GooglePagerEventArgs(PageSize, PageIndex)); }
        }
        /// <summary>
        /// called when page size is changed
        /// </summary>
        public void OnGooglePageSizeChanged(int newPageSize)
        {
            if (newPageSize < 0)
                newPageSize = 0;

            //when we change page size - reset the page index as well
            PageSize = newPageSize;
            PageIndex = 0;

            if (GooglePagerChanged != null) { GooglePagerChanged(this, new GooglePagerEventArgs(PageSize, PageIndex)); }
        }

        #endregion

        #region Page Overhead

        private ITemplate _template;
        [PersistenceMode(PersistenceMode.InnerProperty),
        TemplateContainer(typeof(TemplateControl))]
        public ITemplate Template
        {
            get { return _template; }
            set { _template = value; }
        }

        private string _pagerTitle;
        public string PagerTitle
        {
            get { return _pagerTitle; }
            set { _pagerTitle = value; }
        }
        private int _pgDataSetSize = 10000;
        private int _pgPageIndex = 0;
        private int _pgPageSize = 20;
        private string _pageButtonClass;
        /// <summary>
        /// if we are to use the jquery blockUI
        /// </summary>
        public int DataSetSize { get { return _pgDataSetSize; } set { _pgDataSetSize = value; } }
        /// <summary>
        /// zero-based - internally converted to 1 based for link text
        /// </summary>
        public int PageIndex { get { return _pgPageIndex; } set { _pgPageIndex = value; } }
        public int PageSize { get { return _pgPageSize; } set { _pgPageSize = value; } }
        protected int DataPages { get { if (DataSetSize == 0) return 0; return (int)Math.Ceiling((double)DataSetSize / PageSize); } }
        public string PageButtonClass 
        {
            get { if (_pageButtonClass == null) _pageButtonClass = string.Empty; return _pageButtonClass; }
            set { _pageButtonClass = value; }
        }
        public int StartRowIndex { get { return ((PageIndex * PageSize) + 1); } }

        protected override void LoadControlState(object savedState)
        {
            object[] ctlState = (object[])savedState;
            base.LoadControlState(ctlState[0]);
            this._pgDataSetSize = (int)ctlState[1];
            this._pgPageIndex = (int)ctlState[2];
            this._pgPageSize = (int)ctlState[3];
            this._pageButtonClass = ctlState[4].ToString();
            this._pagerTitle = ctlState[5] as string;
        }
        protected override object SaveControlState()
        {
            object[] ctlState = new object[6];
            ctlState[0] = base.SaveControlState();
            ctlState[1] = DataSetSize;
            ctlState[2] = PageIndex;
            ctlState[3] = PageSize;
            ctlState[4] = PageButtonClass;
            ctlState[5] = PagerTitle;
            return ctlState;
        }
        protected override void OnInit(EventArgs e)
        {
            if (_template != null)
                _template.InstantiateIn(placeValidation);

            base.OnInit(e);
            this.Page.RegisterRequiresControlState(this);
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
                ddlPageSize.DataBind();
        }

        #endregion

        public override void DataBind()
        {
            SetPagerControls();
        }

        #region PageSize

        protected void ddlPageSize_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            if (ddl.Items.Count == 0)
            {
                for (int i = 5; i <= 100; i += 5)
                {
                    ListItem li = new ListItem(i.ToString());

                    if (i == this.PageSize)
                        li.Selected = true;

                    ddl.Items.Add(li);
                }
            }
        }
        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            int current = PageSize;
            int newSize = int.Parse(ddl.SelectedValue);

            //workaround a blank display page that occurs when page data is out of new page bounds
            if (current != newSize)
               OnGooglePageSizeChanged(newSize);//event handles informing the caller - which will rebind control
        }

        #endregion

        #region List of Links

        private int _itmCount = 10;//max 10 links
        private int _halfMax = 5;
        protected void rptPageLink_DataBinding(object sender, EventArgs e)
        {
            Repeater rpt = (Repeater)sender;
            List<ListItem> list = new List<ListItem>(10);

            //set the range
            int startPageNum = ((PageIndex - _halfMax) < 0) ? 1 : PageIndex - _halfMax + 1;
            int endPageNum = (startPageNum + _itmCount - 1);

            //when 1
            //if we have over ten possible pages
                //if the page is > 1 then '...'
            //else
                //1

            string last = "...";
            string first = (DataPages > 10 && startPageNum > 1) ? last : "1";

            for (int i = startPageNum; i <= endPageNum; i++)
                list.Add(new ListItem(
                    (i == startPageNum) ? first : (i == endPageNum) ? last : i.ToString(),
                    i.ToString()//value is equal to the count + index - 1-based
                    ));

            rpt.DataSource = list;
        }
        protected void rptPageLink_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rpt = (Repeater)sender;

            //if it is the current selection - disable the nav
            //highlight the selection
            ListItem li = (ListItem)e.Item.DataItem;
            Button btnPage = (Button)e.Item.FindControl("btnPage");

            if (li != null && btnPage != null)
            {
                if (li.Value == (PageIndex + 1).ToString())
                {
                    btnPage.Enabled = false;
                    btnPage.CssClass = gglPager.CssClasses.selectedpage.ToString();
                }
                else if(int.Parse(li.Value) > DataPages)
                {
                    btnPage.Visible = false;
                }
            }
        }

        #endregion

        protected void nav_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string comm = btn.CommandName.ToLower();

            switch (comm)
            {
                case "firstpage":
                    OnGooglePagerChanged(0);
                    break;
                case "prevpage":
                    OnGooglePagerChanged(PageIndex - 1);
                    break;
                case "nextpage":
                    OnGooglePagerChanged(PageIndex + 1);
                    break;
                case "lastpage":
                    OnGooglePagerChanged(DataPages - 1);
                    break;
                case "page":
                    int newPage = int.Parse(btn.CommandArgument) - 1;
                    OnGooglePagerChanged(newPage);
                    break;
            }
        }
        private void SetPagerControls()
        {
            ddlPageSize.DataBind();
            rptPageLink.DataBind();

            string disabledClass = string.Format("{0} {1}", this.PageButtonClass, gglPager.CssClasses.disabled.ToString()).Trim();

            //first - 
            btnFirst.Enabled = DataPages > 1 && PageIndex > 0;
            btnFirst.CommandName = (btnFirst.Enabled) ? "firstpage" : string.Empty;
            btnFirst.CssClass = (btnFirst.Enabled) ? this.PageButtonClass : disabledClass;

            //prev - must be on a page other than first page and must have data
            btnPrev.Enabled = btnFirst.Enabled;
            btnPrev.CommandName = (btnPrev.Enabled) ? "prevpage" : string.Empty;
            btnPrev.CssClass = (btnPrev.Enabled) ? this.PageButtonClass : disabledClass;

            //next - determined by having pages and not being on thelast page
            btnNext.Enabled = DataPages > 1 && PageIndex != (DataPages - 1);
            btnNext.CommandName = (btnNext.Enabled) ? "nextpage" : string.Empty;
            btnNext.CssClass = (btnNext.Enabled) ? this.PageButtonClass : disabledClass;

            //last - 
            btnLast.Enabled = btnNext.Enabled;//offset for zero index
            btnLast.CommandName = (btnLast.Enabled) ? "lastpage" : string.Empty;
            btnLast.CssClass = (btnLast.Enabled) ? this.PageButtonClass : disabledClass;

            //viewing x out of y
            int lastView = (PageIndex * PageSize) + PageSize;
            if (lastView > DataSetSize)
                lastView = DataSetSize;

            int firstView = (lastView == 0) ? 0 : ((PageIndex * PageSize) + 1);


            litViewing.Text = string.Format("{0} to {1} of {2}", firstView.ToString(), lastView.ToString(), DataSetSize.ToString());
        }
    }
}