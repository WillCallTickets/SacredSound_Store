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
    /// Controller class for MerchCategorie
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class MerchCategorieController
    {
        // Preload our schema..
        MerchCategorie thisSchemaLoad = new MerchCategorie();
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
        public MerchCategorieCollection FetchAll()
        {
            MerchCategorieCollection coll = new MerchCategorieCollection();
            Query qry = new Query(MerchCategorie.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MerchCategorieCollection FetchByID(object Id)
        {
            MerchCategorieCollection coll = new MerchCategorieCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public MerchCategorieCollection FetchByQuery(Query qry)
        {
            MerchCategorieCollection coll = new MerchCategorieCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (MerchCategorie.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (MerchCategorie.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string Name,int TMerchDivisionId,bool BInternalOnly,int IDisplayOrder,string Description,DateTime DtStamp)
	    {
		    MerchCategorie item = new MerchCategorie();
		    
            item.Name = Name;
            
            item.TMerchDivisionId = TMerchDivisionId;
            
            item.BInternalOnly = BInternalOnly;
            
            item.IDisplayOrder = IDisplayOrder;
            
            item.Description = Description;
            
            item.DtStamp = DtStamp;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,string Name,int TMerchDivisionId,bool BInternalOnly,int IDisplayOrder,string Description,DateTime DtStamp)
	    {
		    MerchCategorie item = new MerchCategorie();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.Name = Name;
				
			item.TMerchDivisionId = TMerchDivisionId;
				
			item.BInternalOnly = BInternalOnly;
				
			item.IDisplayOrder = IDisplayOrder;
				
			item.Description = Description;
				
			item.DtStamp = DtStamp;
				
	        item.Save(UserName);
	    }
    }
}
