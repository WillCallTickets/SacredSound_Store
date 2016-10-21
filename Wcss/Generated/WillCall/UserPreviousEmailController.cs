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
    /// Controller class for User_PreviousEmail
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class UserPreviousEmailController
    {
        // Preload our schema..
        UserPreviousEmail thisSchemaLoad = new UserPreviousEmail();
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
        public UserPreviousEmailCollection FetchAll()
        {
            UserPreviousEmailCollection coll = new UserPreviousEmailCollection();
            Query qry = new Query(UserPreviousEmail.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public UserPreviousEmailCollection FetchByID(object Id)
        {
            UserPreviousEmailCollection coll = new UserPreviousEmailCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public UserPreviousEmailCollection FetchByQuery(Query qry)
        {
            UserPreviousEmailCollection coll = new UserPreviousEmailCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (UserPreviousEmail.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (UserPreviousEmail.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(Guid UserId,string EmailAddress,DateTime DtStamp)
	    {
		    UserPreviousEmail item = new UserPreviousEmail();
		    
            item.UserId = UserId;
            
            item.EmailAddress = EmailAddress;
            
            item.DtStamp = DtStamp;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,Guid UserId,string EmailAddress,DateTime DtStamp)
	    {
		    UserPreviousEmail item = new UserPreviousEmail();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.UserId = UserId;
				
			item.EmailAddress = EmailAddress;
				
			item.DtStamp = DtStamp;
				
	        item.Save(UserName);
	    }
    }
}
