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
    /// Controller class for CharitableListing
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class CharitableListingController
    {
        // Preload our schema..
        CharitableListing thisSchemaLoad = new CharitableListing();
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
        public CharitableListingCollection FetchAll()
        {
            CharitableListingCollection coll = new CharitableListingCollection();
            Query qry = new Query(CharitableListing.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public CharitableListingCollection FetchByID(object Id)
        {
            CharitableListingCollection coll = new CharitableListingCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public CharitableListingCollection FetchByQuery(Query qry)
        {
            CharitableListingCollection coll = new CharitableListingCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (CharitableListing.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (CharitableListing.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtStamp,int TCharitableOrgId,int IDisplayOrder,bool BAvailableForContribution,bool BTopBilling,Guid ApplicationId)
	    {
		    CharitableListing item = new CharitableListing();
		    
            item.DtStamp = DtStamp;
            
            item.TCharitableOrgId = TCharitableOrgId;
            
            item.IDisplayOrder = IDisplayOrder;
            
            item.BAvailableForContribution = BAvailableForContribution;
            
            item.BTopBilling = BTopBilling;
            
            item.ApplicationId = ApplicationId;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtStamp,int TCharitableOrgId,int IDisplayOrder,bool BAvailableForContribution,bool BTopBilling,Guid ApplicationId)
	    {
		    CharitableListing item = new CharitableListing();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.TCharitableOrgId = TCharitableOrgId;
				
			item.IDisplayOrder = IDisplayOrder;
				
			item.BAvailableForContribution = BAvailableForContribution;
				
			item.BTopBilling = BTopBilling;
				
			item.ApplicationId = ApplicationId;
				
	        item.Save(UserName);
	    }
    }
}
