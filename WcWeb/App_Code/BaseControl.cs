using System;
using System.Web.UI.WebControls;
using System.Threading;
using System.Diagnostics;

using Wcss;

namespace WillCallWeb
{
	/// <summary>
    ///		Summary description for WillCallWeb.BaseControl.
	/// </summary>
	public partial class BaseControl : System.Web.UI.UserControl
	{
        public bool IsPageAdminContext { get { return ((WillCallWeb.BasePage)base.Page).IsPageAdminContext; } }
        public bool IsAuthdAdminUser { get { return ((WillCallWeb.BasePage)base.Page).IsAuthdAdminUser; } }
        public string userWebInfo { get { return ((WillCallWeb.BasePage)base.Page).userWebInfo; } }
        /// <summary>
        /// must use this in the render event
        /// </summary>
        public string GetRenderedControl(System.Web.UI.Control control) 
        { return ((WillCallWeb.BasePage)base.Page).GetRenderedControl(control); }

        
        /// <summary>
        /// Indicates if there was an error to display - ShowException == true indicates there was an error
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        protected bool ShowException(Exception ex)
        {
            if (ex != null)
            {
                CustomValidator custom = (CustomValidator)this.FindControl("CustomValidation");
                if (custom != null)
                {
                    custom.IsValid = false;
                    custom.ErrorMessage = ex.Message;
                    return true;
                }
            }
            return false;
        }

		public void Redirect( string url )
		{
            ((WillCallWeb.BasePage)base.Page).Redirect(url);
		}
		public void RedirectControl( string controlPathAndQuery )
		{
            ((WillCallWeb.BasePage)base.Page).RedirectControl(controlPathAndQuery);
		}
		public void RedirectToDefault()
		{
            ((WillCallWeb.BasePage)base.Page).RedirectToDefault();
		}
        public bool OldUserMustUpdate(string userName)
        {
            return ((WillCallWeb.BasePage)base.Page).OldUserMustUpdate(userName);
        }
        
        public AdminContext Atx
        {
            get
            {
                return (AdminContext)((WillCallWeb.BasePage)this.Page).Atx;
            }
            set
            {
                ((WillCallWeb.BasePage)this.Page).Atx = value;
            }
        }

        public WebContext Ctx
		{
			get
			{
                return (WebContext)((WillCallWeb.BasePage)this.Page).Ctx;
			}
			set
			{
                ((WillCallWeb.BasePage)this.Page).Ctx = value;
			}
		}

		public virtual void PageLogic() { }
		public virtual void PageInit() { }
		private void InitializeComponent() { }

		protected void OnPageLoad(object sender, System.EventArgs e)
		{
			try
			{
				this.PageLogic();
			}
			catch(ThreadAbortException)
			{
				// do not log thread abort exceptions that occur on response.redirect
			}
			catch(Exception ex)
			{
				Wcss._Error.LogException(ex);

				Debug.WriteLine( ex.Message);
				Debug.Indent();
				Debug.WriteLine( ex.StackTrace);
				Debug.Unindent();

				Label err = (Label)FindControl("LabelError");
				if(err != null)
				{
					err.Visible = true;
					err.Text = "<br><ul><li>There was an error processing your request. Please try again later.</li></ul>";
				}
			}
		}

		protected void OnPageInit(object sender, System.EventArgs e)
		{
			try
			{
				this.PageInit();
			}
			catch(ThreadAbortException)
			{
				// do not log thread abort exceptions that occur on response.redirect
			}
			catch(Exception ex)
			{
				Wcss._Error.LogException(ex);
			}
		}

        
        protected void FillMerchStyleColorSizeLists(DropDownList style, DropDownList color, DropDownList size, Merch parentItem, Merch selectedItem, 
            bool useLookits, bool offerSelectOption, bool activeOnly)
        {
            if (parentItem != null)
            {
                if (style != null)
                {
                    if (style.Items.Count == 1)
                    {
                        System.Collections.Generic.List<string> _styles = new System.Collections.Generic.List<string>();
                        if (activeOnly)
                            _styles.AddRange(parentItem.ChildStyleList);
                        else
                            _styles.AddRange(parentItem.ChildStyleList_All);

                        if (_styles.Count > 1)
                            _styles.Sort();

                        style.AppendDataBoundItems = (offerSelectOption || _styles.Count > 1);
                        style.DataSource = _styles;
                        style.DataBind();
                    }

                    if (selectedItem != null && selectedItem.Style != null && selectedItem.Style.Trim().Length > 0)
                    {
                        ListItem li = style.Items.FindByText(selectedItem.Style);
                        if (li != null)
                            li.Selected = true;
                    }
                }
                if (color != null)
                {
                    if (color.Items.Count == 1)
                    {
                        MerchColorCollection list = new MerchColorCollection();

                        foreach (MerchColor entry in _Lookits.MerchColors)
                        {
                            //if (activeOnly && (useLookits || parentItem.ChildColorList.Contains(entry.Name)))
                                list.Add(entry);
                            //get from all children
                        }

                        if (list.Count > 1)
                            list.Sort("Name", true);

                        color.AppendDataBoundItems = (offerSelectOption || list.Count > 1);
                        color.DataSource = list;
                        color.DataTextField = "Name";
                        color.DataValueField = "Id";
                        color.DataBind();
                    }

                    if (selectedItem != null && selectedItem.Color != null && selectedItem.Color.Trim().Length > 0)
                    {
                        ListItem li = color.Items.FindByText(selectedItem.Color);
                        if (li != null)
                            li.Selected = true;
                    }
                }
                if (size != null)
                {
                    if (size.Items.Count == 1)
                    {
                        MerchSizeCollection list = new MerchSizeCollection();

                        foreach (MerchSize entry in _Lookits.MerchSizes)
                        {
                            //if (activeOnly && (useLookits || parentItem.ChildSizeList.Contains(entry.Code)))
                                list.Add(entry);
                        }

                        if (list.Count > 1)
                            list.Sort("IDisplayOrder", true);

                        size.AppendDataBoundItems = (offerSelectOption || list.Count > 1);
                        size.DataSource = list;
                        size.DataTextField = "Name";
                        size.DataValueField = "Id";
                        size.DataBind();
                    }

                    if (selectedItem != null && selectedItem.Size != null && selectedItem.Size.Trim().Length > 0)
                    {
                        ListItem li = size.Items.FindByText(selectedItem.Size);
                        if (li != null)
                            li.Selected = true;
                    }
                }
            }
        }

        
	}
}
