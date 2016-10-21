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
    /// Controller class for SaleRule
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class SaleRuleController
    {
        // Preload our schema..
        SaleRule thisSchemaLoad = new SaleRule();
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
        public SaleRuleCollection FetchAll()
        {
            SaleRuleCollection coll = new SaleRuleCollection();
            Query qry = new Query(SaleRule.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public SaleRuleCollection FetchByID(object Id)
        {
            SaleRuleCollection coll = new SaleRuleCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public SaleRuleCollection FetchByQuery(Query qry)
        {
            SaleRuleCollection coll = new SaleRuleCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (SaleRule.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (SaleRule.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string Name,string DisplayText,string VcContext,bool BActive,int IDisplayOrder,DateTime DtStamp,Guid ApplicationId)
	    {
		    SaleRule item = new SaleRule();
		    
            item.Name = Name;
            
            item.DisplayText = DisplayText;
            
            item.VcContext = VcContext;
            
            item.BActive = BActive;
            
            item.IDisplayOrder = IDisplayOrder;
            
            item.DtStamp = DtStamp;
            
            item.ApplicationId = ApplicationId;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,string Name,string DisplayText,string VcContext,bool BActive,int IDisplayOrder,DateTime DtStamp,Guid ApplicationId)
	    {
		    SaleRule item = new SaleRule();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.Name = Name;
				
			item.DisplayText = DisplayText;
				
			item.VcContext = VcContext;
				
			item.BActive = BActive;
				
			item.IDisplayOrder = IDisplayOrder;
				
			item.DtStamp = DtStamp;
				
			item.ApplicationId = ApplicationId;
				
	        item.Save(UserName);
	    }
    }
}
