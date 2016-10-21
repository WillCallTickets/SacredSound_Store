using System;
using System.Security.Cryptography;
using System.Threading;

namespace WillCallWeb
{
   public partial class Error : BasePage
   {
       protected override void OnPreInit(EventArgs e)
       {
           QualifySsl(false);
           base.OnPreInit(e);
       }

      protected void Page_Load(object sender, EventArgs e)
      {
          byte[] delay = new byte[1];
          RandomNumberGenerator prng = new RNGCryptoServiceProvider();

          prng.GetBytes(delay);
          Thread.Sleep((int)delay[0]);

          IDisposable disposable = prng as IDisposable;
          if (disposable != null) { disposable.Dispose(); }


         //lbl404.Visible = (this.Request.QueryString["code"] != null && this.Request.QueryString["code"] == "404");
         //lbl408.Visible = (this.Request.QueryString["code"] != null && this.Request.QueryString["code"] == "408");
         //lbl505.Visible = (this.Request.QueryString["code"] != null && this.Request.QueryString["code"] == "505");
         lblError.Visible = true;// (string.IsNullOrEmpty(this.Request.QueryString["code"]));
      }
   }
}
