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
    /// Controller class for CharitableContribution
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class CharitableContributionController
    {
        // Preload our schema..
        CharitableContribution thisSchemaLoad = new CharitableContribution();
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
        public CharitableContributionCollection FetchAll()
        {
            CharitableContributionCollection coll = new CharitableContributionCollection();
            Query qry = new Query(CharitableContribution.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public CharitableContributionCollection FetchByID(object Id)
        {
            CharitableContributionCollection coll = new CharitableContributionCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public CharitableContributionCollection FetchByQuery(Query qry)
        {
            CharitableContributionCollection coll = new CharitableContributionCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (CharitableContribution.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (CharitableContribution.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtStamp,int TInvoiceItemId,int TCharitableOrgId)
	    {
		    CharitableContribution item = new CharitableContribution();
		    
            item.DtStamp = DtStamp;
            
            item.TInvoiceItemId = TInvoiceItemId;
            
            item.TCharitableOrgId = TCharitableOrgId;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtStamp,int TInvoiceItemId,int TCharitableOrgId)
	    {
		    CharitableContribution item = new CharitableContribution();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.TInvoiceItemId = TInvoiceItemId;
				
			item.TCharitableOrgId = TCharitableOrgId;
				
	        item.Save(UserName);
	    }
    }
}
