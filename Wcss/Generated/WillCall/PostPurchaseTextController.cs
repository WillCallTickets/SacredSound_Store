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
    /// Controller class for PostPurchaseText
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class PostPurchaseTextController
    {
        // Preload our schema..
        PostPurchaseText thisSchemaLoad = new PostPurchaseText();
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
        public PostPurchaseTextCollection FetchAll()
        {
            PostPurchaseTextCollection coll = new PostPurchaseTextCollection();
            Query qry = new Query(PostPurchaseText.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public PostPurchaseTextCollection FetchByID(object Id)
        {
            PostPurchaseTextCollection coll = new PostPurchaseTextCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public PostPurchaseTextCollection FetchByQuery(Query qry)
        {
            PostPurchaseTextCollection coll = new PostPurchaseTextCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (PostPurchaseText.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (PostPurchaseText.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtStamp,int? TMerchId,int? TShowTicketId,bool BActive,int IDisplayOrder,string InProcessDescription,string PostText)
	    {
		    PostPurchaseText item = new PostPurchaseText();
		    
            item.DtStamp = DtStamp;
            
            item.TMerchId = TMerchId;
            
            item.TShowTicketId = TShowTicketId;
            
            item.BActive = BActive;
            
            item.IDisplayOrder = IDisplayOrder;
            
            item.InProcessDescription = InProcessDescription;
            
            item.PostText = PostText;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtStamp,int? TMerchId,int? TShowTicketId,bool BActive,int IDisplayOrder,string InProcessDescription,string PostText)
	    {
		    PostPurchaseText item = new PostPurchaseText();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.TMerchId = TMerchId;
				
			item.TShowTicketId = TShowTicketId;
				
			item.BActive = BActive;
				
			item.IDisplayOrder = IDisplayOrder;
				
			item.InProcessDescription = InProcessDescription;
				
			item.PostText = PostText;
				
	        item.Save(UserName);
	    }
    }
}
