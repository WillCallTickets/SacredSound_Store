using System; 
using System.Text; 
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration; 
using System.Xml; 
using System.Xml.Serialization;
using SubSonic; 
using SubSonic.Utilities;
namespace Wcss
{
    /// <summary>
    /// Controller class for SubscriptionEmail
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class SubscriptionEmailController
    {
        // Preload our schema..
        SubscriptionEmail thisSchemaLoad = new SubscriptionEmail();
        private string userName = String.Empty;
        protected string UserName
        {
            get
            {
				if (userName.Length == 0) 
				{
    				if (System.Web.HttpContext.Current != null)
    				{
						userName=System.Web.HttpContext.Current.User.Identity.Name;
					}
					else
					{
						userName=System.Threading.Thread.CurrentPrincipal.Identity.Name;
					}
				}
				return userName;
            }
        }
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public SubscriptionEmailCollection FetchAll()
        {
            SubscriptionEmailCollection coll = new SubscriptionEmailCollection();
            Query qry = new Query(SubscriptionEmail.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public SubscriptionEmailCollection FetchByID(object Id)
        {
            SubscriptionEmailCollection coll = new SubscriptionEmailCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public SubscriptionEmailCollection FetchByQuery(Query qry)
        {
            SubscriptionEmailCollection coll = new SubscriptionEmailCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (SubscriptionEmail.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (SubscriptionEmail.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtStamp,int TSubscriptionId,int TEmailLetterId,string PostedFileName,string CssFile,DateTime? DtLastSent,string ConstructedHtml,string ConstructedText)
	    {
		    SubscriptionEmail item = new SubscriptionEmail();
		    
            item.DtStamp = DtStamp;
            
            item.TSubscriptionId = TSubscriptionId;
            
            item.TEmailLetterId = TEmailLetterId;
            
            item.PostedFileName = PostedFileName;
            
            item.CssFile = CssFile;
            
            item.DtLastSent = DtLastSent;
            
            item.ConstructedHtml = ConstructedHtml;
            
            item.ConstructedText = ConstructedText;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtStamp,int TSubscriptionId,int TEmailLetterId,string PostedFileName,string CssFile,DateTime? DtLastSent,string ConstructedHtml,string ConstructedText)
	    {
		    SubscriptionEmail item = new SubscriptionEmail();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.TSubscriptionId = TSubscriptionId;
				
			item.TEmailLetterId = TEmailLetterId;
				
			item.PostedFileName = PostedFileName;
				
			item.CssFile = CssFile;
				
			item.DtLastSent = DtLastSent;
				
			item.ConstructedHtml = ConstructedHtml;
				
			item.ConstructedText = ConstructedText;
				
	        item.Save(UserName);
	    }
    }
}
