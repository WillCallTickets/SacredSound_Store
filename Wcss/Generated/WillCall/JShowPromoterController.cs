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
    /// Controller class for JShowPromoter
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class JShowPromoterController
    {
        // Preload our schema..
        JShowPromoter thisSchemaLoad = new JShowPromoter();
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
        public JShowPromoterCollection FetchAll()
        {
            JShowPromoterCollection coll = new JShowPromoterCollection();
            Query qry = new Query(JShowPromoter.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public JShowPromoterCollection FetchByID(object Id)
        {
            JShowPromoterCollection coll = new JShowPromoterCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public JShowPromoterCollection FetchByQuery(Query qry)
        {
            JShowPromoterCollection coll = new JShowPromoterCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (JShowPromoter.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (JShowPromoter.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int TPromoterId,int TShowId,string PreText,string PromoterText,string PostText,int IDisplayOrder,DateTime DtStamp)
	    {
		    JShowPromoter item = new JShowPromoter();
		    
            item.TPromoterId = TPromoterId;
            
            item.TShowId = TShowId;
            
            item.PreText = PreText;
            
            item.PromoterText = PromoterText;
            
            item.PostText = PostText;
            
            item.IDisplayOrder = IDisplayOrder;
            
            item.DtStamp = DtStamp;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,int TPromoterId,int TShowId,string PreText,string PromoterText,string PostText,int IDisplayOrder,DateTime DtStamp)
	    {
		    JShowPromoter item = new JShowPromoter();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.TPromoterId = TPromoterId;
				
			item.TShowId = TShowId;
				
			item.PreText = PreText;
				
			item.PromoterText = PromoterText;
				
			item.PostText = PostText;
				
			item.IDisplayOrder = IDisplayOrder;
				
			item.DtStamp = DtStamp;
				
	        item.Save(UserName);
	    }
    }
}
