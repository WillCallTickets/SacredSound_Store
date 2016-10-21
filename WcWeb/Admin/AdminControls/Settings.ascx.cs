using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.IO;
using System.Data;

using Wcss;

namespace WillCallWeb.Admin.AdminControls
{
    public partial class Settings : BaseControl
    {
        #region Collections and Page Objects

        protected string _context = "Default";

        protected void SetPageContext()
        {
            string req = Request.QueryString["p"];

            if (req == null)
                req = "Default";
            
            switch (req)
            {
                case "Default":
                    _context = "Default";
                    break;
                case "Images":
                    _context = "Image";
                    break;
                case "Downloads":
                    _context = "Download";
                    break;
                case "Ship":
                    _context = "Shipping";
                    break;
                case "Flow":
                    _context = "Order Flow";
                    break;
                case "PageMsg":
                    _context = "Page Message";
                    break;
                case "Admin":
                    _context = "Admin";
                    break;
                case "Email":
                    _context = "Email";
                    break;
                case "Service":
                    _context = "Service Charge";
                    break;
                case "fb_integration":
                    _context = "FaceBook Integration";
                    break;
                case "AddNew":
                    _context = "Add Setting";
                    break;
            }
        }

        protected void btnEdit_DataBind(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            //if the bound object is in the list of not allowed to edit
            //then we disable the button
            List<string> no_editlist = new List<string>();
            no_editlist.Add("bannerdimensiontext");

            try
            {
                if (no_editlist.Contains(((SiteConfig)((GridViewRow)btn.NamingContainer).DataItem).Name.ToLower()))
                {
                    btn.Enabled = false;
                    btn.OnClientClick = "";
                }
            }
            catch(Exception)
            {
            }
        }

        protected void ddlDataType_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            ddl.DataSource = Enum.GetNames(typeof(_Enums.ConfigDataTypes));
        }

        protected SiteConfigCollection OrderedCollection
        {
            get
            {
                SiteConfigCollection coll = new SiteConfigCollection();

                string req = Request.QueryString["p"];

                if (req == null)
                    req = "Default";

                coll.AddRange(_Lookits.SiteConfigs.GetList().FindAll(
                    delegate(SiteConfig match) { return (match.Context.ToLower() == req.ToLower()); }));

                if(coll.Count > 1)
                    coll.Sort("Name", true);

                return coll;
            }
        }
        
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageContext();
            lblResult.Text = string.Empty;

            if (!IsPostBack)
            {
                GridView1.DataBind();
            }

            //only use this when you need to test
            //_Config.ConfigTest();
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            System.Collections.Generic.List<string> _errors = new System.Collections.Generic.List<string>();

            //validate inputs
            string datatype = ddlDataType.SelectedValue.TrimStart('_');
            string maxlength = txtLength.Text;
            string context = ddlContext.SelectedValue;
            string description = txtDescription.Text.Trim();
            string name = txtName.Text.Trim();
            string value = txtValue.Text.Trim();

            Utils.Validation.ValidateRequiredField(_errors, "MaxLength", maxlength);
            Utils.Validation.ValidateIntegerField(_errors, "MaxLength", maxlength);
            Utils.Validation.ValidateRequiredField(_errors, "Name", name);

            //if there are errors....
            if (Utils.Validation.IncurredErrors(_errors, lblResult))
            {
                return;
            }

            //create setting for all applications
            string sql = "INSERT [SiteConfig] ([DataType], [MaxLength], [Context], [Description], [Name], [Value], [dtStamp], [ApplicationId]) ";
            sql += "SELECT @datatype, @maxlength, @context, @description, @name, @value, @now, [ApplicationId] FROM Aspnet_Applications ";
            sql += "SELECT * FROM [SiteConfig] WHERE [Name] = @name AND [ApplicationId] = @appId ";

            SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sql, SubSonic.DataService.Provider.Name);
            cmd.Parameters.Add("@datatype", datatype, DbType.String);
            cmd.Parameters.Add("@maxlength", int.Parse(maxlength), DbType.Int32);
            cmd.Parameters.Add("@context", context, DbType.String);
            cmd.Parameters.Add("@description", description, DbType.String);
            cmd.Parameters.Add("@name", name, DbType.String);
            cmd.Parameters.Add("@value", value, DbType.String);
            cmd.Parameters.Add("@now", DateTime.Now, DbType.DateTime);
            cmd.Parameters.Add("@appId", _Config.APPLICATION_ID, DbType.Guid);


            try
            {
                SiteConfigCollection configColl = new SiteConfigCollection();
                configColl.LoadAndCloseReader(SubSonic.DataService.GetReader(cmd));

                if (configColl.Count > 0)
                {
                    //add to collection
                    _Lookits.SiteConfigs.Add(configColl[0]);

                    //indicate what was added - with link to settings page?
                    string result = string.Format("the setting {0} was added to the {1} context", 
                        name,
                        string.Format("<a href=\"/Admin/Settings.aspx?p={0}\">{0}</a>", context)
                        );
                    lblResult.Text = result;
                }
            }
            catch(Exception ex)
            {
                _Error.LogException(ex);
            }
        }

        #region GridView

        protected void GridView1_DataBinding(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            if (_context == "Add Setting")
            {
                grid.Visible = false;
                pnlAdd.Visible = true;
                ddlDataType.DataBind();
                ddlContext.DataBind();
            }
            else
            {
                pnlAdd.Visible = false;
                grid.Visible = true;
                string[] keyNames = { "Id" };
                grid.DataKeyNames = keyNames;
                grid.DataSource = OrderedCollection;
            }
        }
        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView grid = (GridView)sender;

            grid.EditIndex = e.NewEditIndex;

            grid.DataBind();
        }
        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView grid = (GridView)sender;

            grid.EditIndex = -1;

            grid.DataBind();
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;
            GridViewRow row = (GridViewRow)e.Row;

            //only work on data rows and edit mode
            if (row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtInput = (TextBox)row.FindControl("txtInput");
                CheckBox chkInput = (CheckBox)row.FindControl("chkInput");

                if (txtInput != null && chkInput != null)
                {
                    txtInput.Visible = false;
                    chkInput.Visible = false;

                    SiteConfig config = (SiteConfig)row.DataItem;
                    if (config != null)
                    {
                        switch (config.DataType.ToLower())
                        {
                            case "int":
                            case "decimal":
                            case "string":
                                txtInput.Visible = true;
                                txtInput.MaxLength = config.MaxLength;
                                if (config.MaxLength > 256)
                                {
                                    txtInput.TextMode = TextBoxMode.MultiLine;
                                    txtInput.Width = Unit.Pixel(400);
                                    txtInput.Height = Unit.Pixel(100);
                                }
                                txtInput.Text = config.ValueX;//todo handle nulls?
                                break;
                            case "boolean":
                            case "bit":
                                chkInput.Visible = true;
                                chkInput.Checked = bool.Parse(config.ValueX);
                                break;
                        }
                    }
                }
            }
        }
        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridView grid = (GridView)sender;
            GridViewRow row = grid.Rows[e.RowIndex];   
            
            SiteConfig config = (SiteConfig)OrderedCollection.Find(int.Parse(grid.DataKeys[e.RowIndex].Value.ToString()));

            if (config != null)
            {   
                //Description
                TextBox txtDescription = (TextBox)row.FindControl("txtDescription");
                if (txtDescription != null && txtDescription.Text.Trim() != config.Description)
                    config.Description = txtDescription.Text.Trim();
               
                try
                {
                    //Value
                    switch (config.DataType.ToLower())
                    {
                        case "int":
                        case "decimal":
                        case "string":
                            TextBox txtInput = (TextBox)row.FindControl("txtInput");
                            if (txtInput != null)
                            {
                                string inp = txtInput.Text;
                                if (config.ValueX != inp)
                                {
                                    config.ValueX = inp;
                                    config.Save();

                                    RebuildResizedThumbnails(config, config.ValueX);
                                }
                            }
                            break;
                        case "boolean":
                            CheckBox chkInput = (CheckBox)row.FindControl("chkInput");
                            if (chkInput != null && chkInput.Checked != bool.Parse(config.ValueX))
                            {
                                config.ValueX = chkInput.Checked.ToString();
                                config.Save();
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    CustomValidation.IsValid = false;
                    CustomValidation.ErrorMessage = ex.Message;
                    return;
                }

                grid.EditIndex = -1;//get out of edit mode
                grid.DataBind();
            }
        }

        #endregion

        #region Rebuild Thumbnails

        protected void RebuildResizedThumbnails(SiteConfig config, string value)
        {
            string name = config.Name.ToLower();

            if (config.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() && 
                ((name.IndexOf("act_thumbnail_size_") != -1) || (name.IndexOf("venue_thumbnail_size_") != -1) || (name.IndexOf("show_thumbnail_size_") != -1))
                )
            {
                string mappedDir = string.Empty;

                if (name.IndexOf("act_") != -1)
                    mappedDir = _ImageManager._ActImageStorage_Local;
                else if (name.IndexOf("venue_") != -1)
                    mappedDir = _ImageManager._VenueImageStorage_Local;
                else if (name.IndexOf("show_") != -1)
                    mappedDir = _ImageManager._ShowImageStorage_Local;
                else
                    throw new Exception(string.Format("Config type not handled. {0}", config.Name));


                if (name.IndexOf("small") != -1)
                    mappedDir += _ImageManager.smallThumbPath;
                else if (name.IndexOf("large") != -1)
                    mappedDir += _ImageManager.largeThumbPath;
                else if (name.IndexOf("max") != -1)
                    mappedDir += _ImageManager.maxThumbPath;

                mappedDir = Server.MapPath(mappedDir);

                if (Directory.Exists(string.Format(mappedDir)))
                    foreach (string s in Directory.GetFiles(mappedDir))
                        System.IO.File.Delete(s);

            }
            else if (config.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() && name.ToLower().IndexOf("thumbnail_size_") != -1)
            {
                //rebuild all thumbnails for all images
                ItemImageCollection coll = new ItemImageCollection().Load();

                if (name.IndexOf("small") != -1)
                    foreach (ItemImage ii in coll)
                        ii.ImageManager.DeleteThumbnails_Small();
                else if (name.IndexOf("large") != -1)
                    foreach (ItemImage ii in coll)
                        ii.ImageManager.DeleteThumbnails_Large();
                else if (name.IndexOf("max") != -1)
                    foreach (ItemImage ii in coll)
                        ii.ImageManager.DeleteThumbnails_Max();
            }
        }

        #endregion
    }
}