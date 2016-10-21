using System;
using System.Drawing.Imaging;

using Wcss;
using Utils;

namespace WillCallWeb.Controls
{
	/// <summary>
	/// returns a captcha image
	/// </summary>
	public partial class JpegImage : WillCallWeb.BasePage
	{	
		protected bool IsBarCodeGenerator { get { return _barCode.Trim().Length > 0; } }
		private string _barCode = string.Empty;
		protected string BarCode { get { return _barCode; } set { _barCode = value; } }

        protected override void OnInit(EventArgs e)
        {
			string req = Request["bc"];
			if(req != null && req.Trim().Length > 0)
			{
				BarCode = req.Trim();
			}
		}

        protected override void OnLoad(EventArgs e)
        {   
			Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
			// Change the response headers to output a JPEG image.
			this.Response.Clear();
			this.Response.ContentType = "image/jpeg";

			if(IsBarCodeGenerator && Ctx.BarCodeText != null && Ctx.BarCodeText.Trim().Length > 0)
			{
				BarCode bci = new BarCode(_Config._BarCode39Path, 25, Ctx.BarCodeText, string.Empty, false);
				bci.Image.Save(this.Response.OutputStream, ImageFormat.Jpeg);

				// Dispose of the barCode image object.
				bci.Dispose();
			}
			else
			{
				// Create a CAPTCHA image using the text stored in the Session object.
				CaptchaImage ci = new CaptchaImage(Ctx.CurrentCaptcha, 200, 50, "Century Schoolbook");
			
				// Write the image to the response stream in JPEG format.
				ci.Image.Save(this.Response.OutputStream, ImageFormat.Jpeg);

				// Dispose of the CAPTCHA image object.
				ci.Dispose();
			}
		}
	}
}
