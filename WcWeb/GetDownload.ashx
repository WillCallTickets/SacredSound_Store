<%@ WebHandler Language="C#" Class="GetDownload" %>
using System;
using System.IO;
using System.Web;

using Wcss;


// /getdownload.ashx?downloadid=83C1C3F6-C539-41D7-815D-143FBD40E41F
public class GetDownload : System.Web.IHttpHandler
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public void ProcessRequest(HttpContext context)
    {
        if(!_Config._DownloadsActive)
        {
            string req = context.Request.QueryString["downloadid"];
            
            if(req != null)
            {
                Guid g;
                try
                {
                    g = new Guid(req);
                    ProcessDownload(context, g);
                }
                catch(Exception ex)
                {
                    _Error.LogException(ex);
                }
            }
        }
        
        //int orderProductVariantID = CommonHelper.QueryStringInt("OrderProductVariantID");
        //int sampleDownloadProductVariantID = CommonHelper.QueryStringInt("SampleDownloadProductVariantID");

        //if (orderProductVariantID > 0)
        //    processOrderProductVariantDownload(context, orderProductVariantID);
        //else if (sampleDownloadProductVariantID > 0)
        //    processSampleDownloadProductVariant(context, sampleDownloadProductVariantID);
    }

    private void returnError(HttpContext context, string Message)
    {
        context.Response.Clear();
        context.Response.Write(Message);
        context.Response.Flush(); ;
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
    
    private void ProcessDownload(HttpContext context, Guid g)
    {
        //TODO OPTION: make SETTING for user must be logeed in to download
        //if (NopContext.Current.User == null)
        //    context.Response.Redirect("~/Login.aspx");
        
        InvoiceItem purchase = new InvoiceItem("Guid", g);
        
        if(purchase == null || purchase.Context != _Enums.InvoiceItemContext.merch)
            throw new System.Data.ObjectNotFoundException("purchase item could not be found");

        if (purchase.PurchaseAction != _Enums.PurchaseActions.Purchased.ToString())
            throw new Exception(string.Format("purchase item is not valid for download - items is {0}", purchase.PurchaseAction));
        
        //purchase must be a merch/download item
        Merch download = null;// = purchase.MerchRecord;
        if(download == null || (!download.IsActive))
            throw new System.Data.ObjectNotFoundException("downloadable item no longer exists or is active");
        
        
        
        
        string mappedFile = @"D:\willcallresources.zip";
        //get file from merch item
        
        
        //ensure file exists
        if(!File.Exists(mappedFile))
            throw new System.IO.FileNotFoundException("download file is cannot be found");
    
        string notes = purchase.Notes;
        int times = 0;
        
        if(notes != null)
        {
            string[] downloadDates = notes.Split(',');
            times = downloadDates.Length;
        }
        
        //if config and the product allows multiple downloads
        if(_Config._DownloadMax == -1 || download.DownloadMax > times)
        //if (_Config._DownloadMax == -1 || 3 > times)
        {
            //
            //use stored data
            context.Response.Clear();
            
            //todo method to determine content type by file ext
            //we need to force this to download
            string contentType = "application/x-download";// "audio/mpeg";// "application/octet-stream";// "mp3";
            context.Response.ContentType = contentType;

            //string friendlyName = download.DisplayNameWithAttribs;
            string filename = Path.GetFileNameWithoutExtension(mappedFile);
            string ext = Path.GetExtension(mappedFile);
            
            context.Response.AddHeader("Content-disposition",
                string.Format("attachment;filename={0}{1}", filename, ext));

            FileStream fs = null;
            long dataToRead = -1;

            string creator = purchase.InvoiceRecord.Creator;
            Guid userId = purchase.InvoiceRecord.UserId;
            string merchId = (purchase.TMerchId.HasValue) ? purchase.TMerchId.ToString() : "no merch id";
            string ip = string.Format("IP: {0}", context.Request.UserHostAddress);

            //log events! - start of download
            EventQ.LogEvent(DateTime.Now, DateTime.Now, _Enums.EventQStatus.Success, creator, userId, creator, 
                _Enums.EventQContext.Merch, _Enums.EventQVerb.StartingDownload, merchId, string.Empty, ip);
            
            //TODO log as an invoice event

            //TODO increment downloads attempted
            
            try
            {   
                using (fs = new FileStream(mappedFile, FileMode.Open))
                {
                    int length;
                    dataToRead = fs.Length;
                    byte[] buffer = new Byte[10000];

                    while (dataToRead > 0)
                    {
                        if (context.Response.IsClientConnected)
                        {
                            length = fs.Read(buffer, 0, 10000);
                            context.Response.OutputStream.Write(buffer, 0, length);
                            context.Response.Flush();
                            buffer = new Byte[10000];
                            dataToRead = dataToRead - length;
                        }
                        else
                            dataToRead = -1;
                    }

                }
            }
            catch (Exception) { }
            finally 
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }

                if (dataToRead == 0)
                {
                    //log events - file was downloaded
                    EventQ.LogEvent(DateTime.Now, DateTime.Now, _Enums.EventQStatus.Success, creator, userId, creator, 
                        _Enums.EventQContext.Merch, _Enums.EventQVerb.SuccessfulDownload, merchId, string.Empty, ip);
                    
                    //increment notes with another date
                    //update invoice item
                    purchase.DateShipped = DateTime.Now;
                    string newNotes = (purchase.Notes == null || purchase.Notes.Trim().Length == 0) ?
                        DateTime.Now.ToString("MM/dd/yyyy hh:mmtt") :
                        string.Format(", {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mmtt"));

                    purchase.Notes += newNotes;

                    purchase.Save();

                    //TODO log as an invoice event

                    //TODO increment downloads successful
                }
            }            
        }
        
        //purchase must have allowable downloads - try to deter from downloading too many times
    }
}