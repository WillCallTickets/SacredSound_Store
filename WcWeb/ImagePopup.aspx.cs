using System;
using System.Web.UI.WebControls;

using Wcss;

public partial class ImagePopup : WillCallWeb.BasePage
{
    private ItemImageCollection _coll = new ItemImageCollection();
    protected Merch _item = null;

    protected override void OnPreInit(EventArgs e)
    {
        QualifySsl(false);
        base.OnPreInit(e);

        //set up entity
        string req = Request["img"];//this is the merch id
        if (req == null)
            base.Redirect("/Error.aspx");

        if (_coll.Count == 0 && req != null && Utils.Validation.IsInteger(req))
        {   
            _coll.AddRange(_Lookits.MerchImages.GetList().FindAll(
                 delegate(ItemImage match) { return (match.TMerchId == int.Parse(req) && match.IsDetailImage); }));
            if (_coll.Count > 1)
                _coll.Sort("IDisplayOrder", true);
        }

        //change page title
        if (_coll.Count > 0)
        {
            _item = _coll[0].MerchRecord;
            if (_item != null)
                this.Page.Title = string.Format("{0} - item images", _item.DisplayName);
        }

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            FormView1.DataBind();
    }

    protected void FormView1_DataBinding(object sender, EventArgs e)
    {
        FormView form = (FormView)sender;
        form.DataSource = _coll;
        string[] keyNames = { "Id" };
        form.DataKeyNames = keyNames;

        form.Width = Unit.Pixel(Wcss._Config._MerchThumbSizeMax);
    }

    protected void FormView1_DataBound(object sender, EventArgs e)
    {
        FormView form = (FormView)sender;

        ItemImage entity = (ItemImage)form.DataItem;
        Literal image = (Literal)form.FindControl("literalImage");

        if (entity != null && image != null)
        {
            string desc = entity.DetailDescription;

            if (desc != null && desc.Trim().Length > 0)
                image.Text = string.Format("<div class=\"detaildesc\">{0}</div>", desc.Trim());

            image.Text += string.Format("<div class=\"detailimage\"><img src=\"{0}\" class=\"detailimage\" {1} /></div>",
                (entity.OverrideThumbnail) ? entity.Url_Original : entity.Thumbnail_Max,
                (entity.OverrideThumbnail) ? (entity.IsPortrait) ? 
                   string.Format("height=\"{0}\"", _Config._MerchThumbSizeMax) : string.Format("width=\"{0}\"", _Config._MerchThumbSizeMax) : 
                   string.Empty);
        }
    }
    protected void FormView1_PageIndexChanging(object sender, FormViewPageEventArgs e)
    {
        FormView form = (FormView)sender;
        form.PageIndex = e.NewPageIndex;
    }
    protected void FormView1_PageIndexChanged(object sender, EventArgs e)
    {
        FormView form = (FormView)sender;
        form.DataBind();
    }
}
