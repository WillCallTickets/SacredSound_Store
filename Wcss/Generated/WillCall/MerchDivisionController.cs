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
    /// Controller class for MerchDivision
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class MerchDivisionController
    {
        // Preload our schema..
        MerchDivision thisSchemaLoad = new MerchDivision();
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
        public MerchDivisionCollection FetchAll()
        {
            MerchDivisionCollection coll = new MerchDivisionCollection();
            Query qry = new Query(MerchDivision.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MerchDivisionCollection FetchByID(object Id)
        {
            MerchDivisionCollection coll = new MerchDivisionCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public MerchDivisionCollection FetchByQuery(Query qry)
        {
            MerchDivisionCollection coll = new MerchDivisionCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (MerchDivision.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (MerchDivision.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string Name,bool BInternalOnly,int IDisplayOrder,string Description,DateTime DtStamp,Guid ApplicationId)
	    {
		    MerchDivision item = new MerchDivision();
		    
            item.Name = Name;
            
            item.BInternalOnly = BInternalOnly;
            
            item.IDisplayOrder = IDisplayOrder;
            
            item.Description = Description;
            
            item.DtStamp = DtStamp;
            
            item.ApplicationId = ApplicationId;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,string Name,bool BInternalOnly,int IDisplayOrder,string Description,DateTime DtStamp,Guid ApplicationId)
	    {
		    MerchDivision item = new MerchDivision();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.Name = Name;
				
			item.BInternalOnly = BInternalOnly;
				
			item.IDisplayOrder = IDisplayOrder;
				
			item.Description = Description;
				
			item.DtStamp = DtStamp;
				
			item.ApplicationId = ApplicationId;
				
	        item.Save(UserName);
	    }
    }
}
