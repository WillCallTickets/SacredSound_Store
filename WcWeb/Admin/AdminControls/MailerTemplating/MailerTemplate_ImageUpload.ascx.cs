using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.IO;

using Wcss;

namespace WillCallWeb.Admin.AdminControls.MailerTemplating
{
    public partial class MailerTemplate_ImageUpload : BaseControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            uplImage.Attributes.Add("size", "55");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager mgr = (ScriptManager)this.Page.Master.FindControl("ScriptManager1");
            if (mgr != null)
                mgr.RegisterPostBackControl(this.btnUpload);

            if (!IsPostBack)
            {
                lstImages.DataBind();
            }
        }

        #region Uploads

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            FileUpload upload = uplImage;
            CustomValidator validation = CustomValidation;

            if (upload != null && upload.HasFile)
            {
                string mappedFile = string.Empty;

                try
                {
                    //validate file name
                    string uploadExt = Path.GetExtension(upload.FileName).ToLower();

                    if (uploadExt.Trim().Length == 0 || (uploadExt != ".jpg" && uploadExt != ".jpeg" && uploadExt != ".gif" && uploadExt != ".png"))
                        throw new Exception("Valid file types are jpg, jpeg, gif and png only.");

                    string fileName = System.Text.RegularExpressions.Regex.Replace(Path.GetFileNameWithoutExtension(upload.FileName), @"\s+", string.Empty);
                    fileName = fileName.Replace("'", string.Empty).Replace("-", "_").Replace("&", "_");
                    //get the file name to save
                    fileName += uploadExt;

                    if (!Utils.Validation.IsValidImageFile(fileName))
                        throw new Exception("Please enter a valid file name. Valid filenames use letters, underscores and periods. Only jpg, jpeg, gif or png are valid");
                    //endvalidation

                    string pathFile = string.Format("{0}{1}", Wcss.SubscriptionEmail.Path_PostedImages, fileName);
                    mappedFile = Server.MapPath(pathFile);

                    if (System.IO.File.Exists(mappedFile))
                    {
                        System.IO.File.Delete(mappedFile);
                        //fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + uploadExt;
                        //mappedFile = Server.MapPath(string.Format("{0}{1}", Wcss.SubscriptionEmail.Path_PostedImages, fileName));
                    }

                    //this will overwrite existing if not checked
                    upload.SaveAs(mappedFile);

                    //then rebind the image/file listing
                    lstImages.DataBind();

                    //select the particular file                    
                    ListItem li = lstImages.Items.FindByText(Path.GetFileName(mappedFile));
                    if (li != null)
                    {
                        lstImages.SelectedIndex = -1;
                        li.Selected = true;
                    }

                    //bind the view image
                    litImage.DataBind();
                }
                catch (OutOfMemoryException)
                {
                    if (File.Exists(mappedFile))
                        File.Delete(mappedFile);

                    throw new System.ArgumentOutOfRangeException(string.Format("An Image file could not be created from the file specified - \"{0}\" ", mappedFile));
                }
                catch (Exception ex)
                {
                    if (File.Exists(mappedFile))
                        File.Delete(mappedFile);

                    Wcss._Error.LogException(ex);
                    validation.IsValid = false;
                    validation.ErrorMessage = ex.Message;
                }
            }
        }
        
        #endregion

        protected void lstImages_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox lst = (ListBox)sender;

            //bind image view
            litImage.DataBind();
        }
        protected void lstImages_DataBound(object sender, EventArgs e)
        {
            ListBox lst = (ListBox)sender;
            if(lst.Items.Count > 0 && lst.SelectedIndex == -1)
                lst.SelectedIndex = 0;

            //bind image view
            litImage.DataBind();
        }
        protected void litImage_DataBind(object sender, EventArgs e)
        {
            Literal lit = (Literal)sender;

            if (lstImages.SelectedIndex != -1)
            {
                string selection = lstImages.SelectedValue;
                string fileName = Path.GetFileName(selection);

                lit.Text = string.Format("<div>{0}</div>", fileName);
                lit.Text += string.Format("<img src=\"{0}{1}\" border=\"0\" />", SubscriptionEmail.Path_PostedImages, fileName);
            }
        }
        protected void lstImages_DataBinding(object sender, EventArgs e)
        {
            ListBox lst = (ListBox)sender;
            List<ListItem> list = new List<ListItem>();

            string virtualPath = SubscriptionEmail.Path_PostedImages;
            string mappedPath = Server.MapPath(virtualPath);
            string[] files = Directory.GetFiles(mappedPath);

            foreach (string s in files)
            {
                string fileName = Path.GetFileName(s);

                //only add appropriate file for list type
                if (Utils.Validation.IsValidImageFile(fileName))
                    list.Add(new ListItem(fileName, s));
            }

            lst.AppendDataBoundItems = true;
            lst.DataSource = list;
            lst.DataTextField = "Text";
            lst.DataValueField = "Value";
        }        
    }
}