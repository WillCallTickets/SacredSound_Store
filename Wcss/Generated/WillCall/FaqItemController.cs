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
    /// Controller class for FaqItem
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class FaqItemController
    {
        // Preload our schema..
        FaqItem thisSchemaLoad = new FaqItem();
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
        public FaqItemCollection FetchAll()
        {
            FaqItemCollection coll = new FaqItemCollection();
            Query qry = new Query(FaqItem.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public FaqItemCollection FetchByID(object Id)
        {
            FaqItemCollection coll = new FaqItemCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public FaqItemCollection FetchByQuery(Query qry)
        {
            FaqItemCollection coll = new FaqItemCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (FaqItem.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (FaqItem.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtStamp,bool BActive,string Question,string Answer,int IDisplayOrder,int TFaqCategorieId)
	    {
		    FaqItem item = new FaqItem();
		    
            item.DtStamp = DtStamp;
            
            item.BActive = BActive;
            
            item.Question = Question;
            
            item.Answer = Answer;
            
            item.IDisplayOrder = IDisplayOrder;
            
            item.TFaqCategorieId = TFaqCategorieId;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtStamp,bool BActive,string Question,string Answer,int IDisplayOrder,int TFaqCategorieId)
	    {
		    FaqItem item = new FaqItem();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.BActive = BActive;
				
			item.Question = Question;
				
			item.Answer = Answer;
				
			item.IDisplayOrder = IDisplayOrder;
				
			item.TFaqCategorieId = TFaqCategorieId;
				
	        item.Save(UserName);
	    }
    }
}
