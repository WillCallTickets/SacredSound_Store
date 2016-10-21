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
    /// Controller class for Required
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class RequiredController
    {
        // Preload our schema..
        Required thisSchemaLoad = new Required();
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
        public RequiredCollection FetchAll()
        {
            RequiredCollection coll = new RequiredCollection();
            Query qry = new Query(Required.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public RequiredCollection FetchByID(object Id)
        {
            RequiredCollection coll = new RequiredCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public RequiredCollection FetchByQuery(Query qry)
        {
            RequiredCollection coll = new RequiredCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (Required.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (Required.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtStamp,bool BActive,bool BExclusive,DateTime? DtStart,DateTime? DtEnd,string VcRequiredContext,string VcIdx,int IRequiredQty,decimal MMinAmount,string Description)
	    {
		    Required item = new Required();
		    
            item.DtStamp = DtStamp;
            
            item.BActive = BActive;
            
            item.BExclusive = BExclusive;
            
            item.DtStart = DtStart;
            
            item.DtEnd = DtEnd;
            
            item.VcRequiredContext = VcRequiredContext;
            
            item.VcIdx = VcIdx;
            
            item.IRequiredQty = IRequiredQty;
            
            item.MMinAmount = MMinAmount;
            
            item.Description = Description;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtStamp,bool BActive,bool BExclusive,DateTime? DtStart,DateTime? DtEnd,string VcRequiredContext,string VcIdx,int IRequiredQty,decimal MMinAmount,string Description)
	    {
		    Required item = new Required();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.BActive = BActive;
				
			item.BExclusive = BExclusive;
				
			item.DtStart = DtStart;
				
			item.DtEnd = DtEnd;
				
			item.VcRequiredContext = VcRequiredContext;
				
			item.VcIdx = VcIdx;
				
			item.IRequiredQty = IRequiredQty;
				
			item.MMinAmount = MMinAmount;
				
			item.Description = Description;
				
	        item.Save(UserName);
	    }
    }
}
