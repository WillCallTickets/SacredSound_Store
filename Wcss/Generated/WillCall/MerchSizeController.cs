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
    /// Controller class for MerchSize
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class MerchSizeController
    {
        // Preload our schema..
        MerchSize thisSchemaLoad = new MerchSize();
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
        public MerchSizeCollection FetchAll()
        {
            MerchSizeCollection coll = new MerchSizeCollection();
            Query qry = new Query(MerchSize.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MerchSizeCollection FetchByID(object Id)
        {
            MerchSizeCollection coll = new MerchSizeCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public MerchSizeCollection FetchByQuery(Query qry)
        {
            MerchSizeCollection coll = new MerchSizeCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (MerchSize.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (MerchSize.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string Name,int IDisplayOrder,string Code,DateTime DtStamp,Guid ApplicationId)
	    {
		    MerchSize item = new MerchSize();
		    
            item.Name = Name;
            
            item.IDisplayOrder = IDisplayOrder;
            
            item.Code = Code;
            
            item.DtStamp = DtStamp;
            
            item.ApplicationId = ApplicationId;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,string Name,int IDisplayOrder,string Code,DateTime DtStamp,Guid ApplicationId)
	    {
		    MerchSize item = new MerchSize();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.Name = Name;
				
			item.IDisplayOrder = IDisplayOrder;
				
			item.Code = Code;
				
			item.DtStamp = DtStamp;
				
			item.ApplicationId = ApplicationId;
				
	        item.Save(UserName);
	    }
    }
}
