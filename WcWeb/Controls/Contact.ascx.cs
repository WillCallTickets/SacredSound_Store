using System;
using System.Net.Mail;

using Wcss;

namespace WillCallWeb.Controls
{
   public partial class Contact : BaseControl
   {
       protected void Page_Load(object sender, EventArgs e)
       {
           //testing only
           //throw new Exception("lolly");

           if(!IsPostBack)
            txtSubmit.OnClientClick = string.Format("return confirm('Are you sure you want to send email to {0} customer service?');", _Config._Site_Entity_Name);
       }

       protected void txtSubmit_Click(object sender, EventArgs e)
       {
           MailMessage msg = new MailMessage();

           try
           {
               // send the mail
               msg.IsBodyHtml = false;
               msg.From = new MailAddress(txtEmail.Text.Trim(), txtName.Text.Trim());

               msg.To.Add(new MailAddress(_Config._CustomerService_Email, _Config._CustomerService_FromName));
               
               //if(_Config._CCDev.Length > 0)
               //    msg.CC.Add(new MailAddress(_Config._CCDev));

               msg.Subject = string.Format("Store Contact - {0}", txtSubject.Text.Trim());

               string body = txtBody.Text.Trim();
               
               body = body.Insert(body.Length, string.Format("\r\n\r\n\r\n====================\r\n\r\n{0}", userWebInfo));
               msg.Body = body.Insert(0, string.Format("===================={2}From: {0} <{1}>{2}Subject: {3}{2}===================={2}{2}", 
                   txtName.Text.Trim(), txtEmail.Text.Trim(), Environment.NewLine, txtSubject.Text.Trim()));
               
               
               SmtpClient client = new SmtpClient();
               client.Send(msg);

               //body is ok here because it is less than max length of description column
               UserEvent.NewUserEvent(txtEmail.Text.Trim(), DateTime.Now, DateTime.Now, _Enums.EventQStatus.Success, txtEmail.Text.Trim(),
                   _Enums.EventQContext.User, _Enums.EventQVerb.UserSentContactMessage, null, null, body, true);

               // show a confirmation message, and reset the fields
               lblFeedbackKO.Visible = false;
               txtName.Text = "";
               txtEmail.Text = "";
               txtSubject.Text = "";
               txtBody.Text = "";

               base.Redirect("/ContactProcessing.aspx");
           }
           catch (System.Threading.ThreadAbortException) { }
           catch (Exception ex)
           {
               _Error.LogException(ex);
               lblFeedbackKO.Visible = true;
           }
       }
   }
}
