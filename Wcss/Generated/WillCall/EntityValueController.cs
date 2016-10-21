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
    /// Controller class for EntityValue
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class EntityValueController
    {
        // Preload our schema..
        EntityValue thisSchemaLoad = new EntityValue();
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
        public EntityValueCollection FetchAll()
        {
            EntityValueCollection coll = new EntityValueCollection();
            Query qry = new Query(EntityValue.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public EntityValueCollection FetchByID(object Id)
        {
            EntityValueCollection coll = new EntityValueCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public EntityValueCollection FetchByQuery(Query qry)
        {
            EntityValueCollection coll = new EntityValueCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (EntityValue.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (EntityValue.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtCreated,DateTime DtModified,Guid? UserId,int IDisplayOrder,string VcContext,string VcTableRelation,int? TParentId,string VcValueContext,string VcValueType,string VcValue)
	    {
		    EntityValue item = new EntityValue();
		    
            item.DtCreated = DtCreated;
            
            item.DtModified = DtModified;
            
            item.UserId = UserId;
            
            item.IDisplayOrder = IDisplayOrder;
            
            item.VcContext = VcContext;
            
            item.VcTableRelation = VcTableRelation;
            
            item.TParentId = TParentId;
            
            item.VcValueContext = VcValueContext;
            
            item.VcValueType = VcValueType;
            
            item.VcValue = VcValue;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtCreated,DateTime DtModified,Guid? UserId,int IDisplayOrder,string VcContext,string VcTableRelation,int? TParentId,string VcValueContext,string VcValueType,string VcValue)
	    {
		    EntityValue item = new EntityValue();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtCreated = DtCreated;
				
			item.DtModified = DtModified;
				
			item.UserId = UserId;
				
			item.IDisplayOrder = IDisplayOrder;
				
			item.VcContext = VcContext;
				
			item.VcTableRelation = VcTableRelation;
				
			item.TParentId = TParentId;
				
			item.VcValueContext = VcValueContext;
				
			item.VcValueType = VcValueType;
				
			item.VcValue = VcValue;
				
	        item.Save(UserName);
	    }
    }
}
