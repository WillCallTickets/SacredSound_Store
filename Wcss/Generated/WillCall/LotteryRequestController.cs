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
    /// Controller class for LotteryRequest
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class LotteryRequestController
    {
        // Preload our schema..
        LotteryRequest thisSchemaLoad = new LotteryRequest();
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
        public LotteryRequestCollection FetchAll()
        {
            LotteryRequestCollection coll = new LotteryRequestCollection();
            Query qry = new Query(LotteryRequest.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public LotteryRequestCollection FetchByID(object Id)
        {
            LotteryRequestCollection coll = new LotteryRequestCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public LotteryRequestCollection FetchByQuery(Query qry)
        {
            LotteryRequestCollection coll = new LotteryRequestCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (LotteryRequest.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (LotteryRequest.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(Guid Guid,DateTime? DtStamp,int TLotteryId,Guid UserId,string UserName,string VcStatus,DateTime? DtStatus,string StatusBy,string StatusNotes,DateTime? DtFulfilled,int IRequested,int IPurchased,string StatusIP,string RequestIP,string FulfillIP)
	    {
		    LotteryRequest item = new LotteryRequest();
		    
            item.Guid = Guid;
            
            item.DtStamp = DtStamp;
            
            item.TLotteryId = TLotteryId;
            
            item.UserId = UserId;
            
            item.UserName = UserName;
            
            item.VcStatus = VcStatus;
            
            item.DtStatus = DtStatus;
            
            item.StatusBy = StatusBy;
            
            item.StatusNotes = StatusNotes;
            
            item.DtFulfilled = DtFulfilled;
            
            item.IRequested = IRequested;
            
            item.IPurchased = IPurchased;
            
            item.StatusIP = StatusIP;
            
            item.RequestIP = RequestIP;
            
            item.FulfillIP = FulfillIP;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,Guid Guid,DateTime? DtStamp,int TLotteryId,Guid UserId,string UserName,string VcStatus,DateTime? DtStatus,string StatusBy,string StatusNotes,DateTime? DtFulfilled,int IRequested,int IPurchased,string StatusIP,string RequestIP,string FulfillIP)
	    {
		    LotteryRequest item = new LotteryRequest();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.Guid = Guid;
				
			item.DtStamp = DtStamp;
				
			item.TLotteryId = TLotteryId;
				
			item.UserId = UserId;
				
			item.UserName = UserName;
				
			item.VcStatus = VcStatus;
				
			item.DtStatus = DtStatus;
				
			item.StatusBy = StatusBy;
				
			item.StatusNotes = StatusNotes;
				
			item.DtFulfilled = DtFulfilled;
				
			item.IRequested = IRequested;
				
			item.IPurchased = IPurchased;
				
			item.StatusIP = StatusIP;
				
			item.RequestIP = RequestIP;
				
			item.FulfillIP = FulfillIP;
				
	        item.Save(UserName);
	    }
    }
}
