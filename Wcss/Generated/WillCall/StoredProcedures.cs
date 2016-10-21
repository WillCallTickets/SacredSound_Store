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
namespace Wcss{
    public partial class SPs{
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_AnyDataInTables Procedure
        /// </summary>
        public static StoredProcedure AspnetAnyDataInTables(int? TablesToCheck)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_AnyDataInTables", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@TablesToCheck", TablesToCheck, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Applications_CreateApplication Procedure
        /// </summary>
        public static StoredProcedure AspnetApplicationsCreateApplication(string ApplicationName, Guid? ApplicationId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Applications_CreateApplication", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddOutputParameter("@ApplicationId", DbType.Guid, null, null);
            
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_CheckSchemaVersion Procedure
        /// </summary>
        public static StoredProcedure AspnetCheckSchemaVersion(string Feature, string CompatibleSchemaVersion)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_CheckSchemaVersion", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@Feature", Feature, DbType.String, null, null);
        	
            sp.Command.AddParameter("@CompatibleSchemaVersion", CompatibleSchemaVersion, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Membership_ChangePasswordQuestionAndAnswer Procedure
        /// </summary>
        public static StoredProcedure AspnetMembershipChangePasswordQuestionAndAnswer(string ApplicationName, string UserName, string NewPasswordQuestion, string NewPasswordAnswer)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Membership_ChangePasswordQuestionAndAnswer", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@NewPasswordQuestion", NewPasswordQuestion, DbType.String, null, null);
        	
            sp.Command.AddParameter("@NewPasswordAnswer", NewPasswordAnswer, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Membership_CreateUser Procedure
        /// </summary>
        public static StoredProcedure AspnetMembershipCreateUser(string ApplicationName, string UserName, string Password, string PasswordSalt, string Email, string PasswordQuestion, string PasswordAnswer, bool? IsApproved, DateTime? CurrentTimeUtc, DateTime? CreateDate, int? UniqueEmail, int? PasswordFormat, Guid? UserId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Membership_CreateUser", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Password", Password, DbType.String, null, null);
        	
            sp.Command.AddParameter("@PasswordSalt", PasswordSalt, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Email", Email, DbType.String, null, null);
        	
            sp.Command.AddParameter("@PasswordQuestion", PasswordQuestion, DbType.String, null, null);
        	
            sp.Command.AddParameter("@PasswordAnswer", PasswordAnswer, DbType.String, null, null);
        	
            sp.Command.AddParameter("@IsApproved", IsApproved, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@CreateDate", CreateDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@UniqueEmail", UniqueEmail, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PasswordFormat", PasswordFormat, DbType.Int32, 0, 10);
        	
            sp.Command.AddOutputParameter("@UserId", DbType.Guid, null, null);
            
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Membership_FindUsers_LIKE_ProfileParameter Procedure
        /// </summary>
        public static StoredProcedure AspnetMembershipFindUsersLikeProfileParameter(string ApplicationName, string ParamName, string ParamValue, int? PageIndex, int? PageSize)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Membership_FindUsers_LIKE_ProfileParameter", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@ParamName", ParamName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@ParamValue", ParamValue, DbType.String, null, null);
        	
            sp.Command.AddParameter("@PageIndex", PageIndex, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PageSize", PageSize, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Membership_FindUsersByEmail Procedure
        /// </summary>
        public static StoredProcedure AspnetMembershipFindUsersByEmail(string ApplicationName, string EmailToMatch, int? PageIndex, int? PageSize)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Membership_FindUsersByEmail", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@EmailToMatch", EmailToMatch, DbType.String, null, null);
        	
            sp.Command.AddParameter("@PageIndex", PageIndex, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PageSize", PageSize, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Membership_FindUsersByName Procedure
        /// </summary>
        public static StoredProcedure AspnetMembershipFindUsersByName(string ApplicationName, string UserNameToMatch, int? PageIndex, int? PageSize)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Membership_FindUsersByName", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserNameToMatch", UserNameToMatch, DbType.String, null, null);
        	
            sp.Command.AddParameter("@PageIndex", PageIndex, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PageSize", PageSize, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Membership_FindUsersByProfileParameter Procedure
        /// </summary>
        public static StoredProcedure AspnetMembershipFindUsersByProfileParameter(string ApplicationName, string ParamName, string ParamValue, int? PageIndex, int? PageSize)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Membership_FindUsersByProfileParameter", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@ParamName", ParamName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@ParamValue", ParamValue, DbType.String, null, null);
        	
            sp.Command.AddParameter("@PageIndex", PageIndex, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PageSize", PageSize, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Membership_GetAllUsers Procedure
        /// </summary>
        public static StoredProcedure AspnetMembershipGetAllUsers(string ApplicationName, int? PageIndex, int? PageSize)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Membership_GetAllUsers", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@PageIndex", PageIndex, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PageSize", PageSize, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Membership_GetNumberOfUsersOnline Procedure
        /// </summary>
        public static StoredProcedure AspnetMembershipGetNumberOfUsersOnline(string ApplicationName, int? MinutesSinceLastInActive, DateTime? CurrentTimeUtc)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Membership_GetNumberOfUsersOnline", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@MinutesSinceLastInActive", MinutesSinceLastInActive, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc, DbType.DateTime, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Membership_GetPassword Procedure
        /// </summary>
        public static StoredProcedure AspnetMembershipGetPassword(string ApplicationName, string UserName, int? MaxInvalidPasswordAttempts, int? PasswordAttemptWindow, DateTime? CurrentTimeUtc, string PasswordAnswer)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Membership_GetPassword", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@MaxInvalidPasswordAttempts", MaxInvalidPasswordAttempts, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PasswordAttemptWindow", PasswordAttemptWindow, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@PasswordAnswer", PasswordAnswer, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Membership_GetPasswordWithFormat Procedure
        /// </summary>
        public static StoredProcedure AspnetMembershipGetPasswordWithFormat(string ApplicationName, string UserName, bool? UpdateLastLoginActivityDate, DateTime? CurrentTimeUtc)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Membership_GetPasswordWithFormat", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UpdateLastLoginActivityDate", UpdateLastLoginActivityDate, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc, DbType.DateTime, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Membership_GetUserByEmail Procedure
        /// </summary>
        public static StoredProcedure AspnetMembershipGetUserByEmail(string ApplicationName, string Email)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Membership_GetUserByEmail", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Email", Email, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Membership_GetUserByName Procedure
        /// </summary>
        public static StoredProcedure AspnetMembershipGetUserByName(string ApplicationName, string UserName, DateTime? CurrentTimeUtc, bool? UpdateLastActivity)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Membership_GetUserByName", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@UpdateLastActivity", UpdateLastActivity, DbType.Boolean, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Membership_GetUserByUserId Procedure
        /// </summary>
        public static StoredProcedure AspnetMembershipGetUserByUserId(Guid? UserId, DateTime? CurrentTimeUtc, bool? UpdateLastActivity)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Membership_GetUserByUserId", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@UserId", UserId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@UpdateLastActivity", UpdateLastActivity, DbType.Boolean, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Membership_ResetPassword Procedure
        /// </summary>
        public static StoredProcedure AspnetMembershipResetPassword(string ApplicationName, string UserName, string NewPassword, int? MaxInvalidPasswordAttempts, int? PasswordAttemptWindow, string PasswordSalt, DateTime? CurrentTimeUtc, int? PasswordFormat, string PasswordAnswer)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Membership_ResetPassword", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@NewPassword", NewPassword, DbType.String, null, null);
        	
            sp.Command.AddParameter("@MaxInvalidPasswordAttempts", MaxInvalidPasswordAttempts, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PasswordAttemptWindow", PasswordAttemptWindow, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PasswordSalt", PasswordSalt, DbType.String, null, null);
        	
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@PasswordFormat", PasswordFormat, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PasswordAnswer", PasswordAnswer, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Membership_SetPassword Procedure
        /// </summary>
        public static StoredProcedure AspnetMembershipSetPassword(string ApplicationName, string UserName, string NewPassword, string PasswordSalt, DateTime? CurrentTimeUtc, int? PasswordFormat)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Membership_SetPassword", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@NewPassword", NewPassword, DbType.String, null, null);
        	
            sp.Command.AddParameter("@PasswordSalt", PasswordSalt, DbType.String, null, null);
        	
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@PasswordFormat", PasswordFormat, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Membership_UnlockUser Procedure
        /// </summary>
        public static StoredProcedure AspnetMembershipUnlockUser(string ApplicationName, string UserName)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Membership_UnlockUser", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Membership_UpdateUser Procedure
        /// </summary>
        public static StoredProcedure AspnetMembershipUpdateUser(string ApplicationName, string UserName, string Email, string Comment, bool? IsApproved, DateTime? LastLoginDate, DateTime? LastActivityDate, int? UniqueEmail, DateTime? CurrentTimeUtc)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Membership_UpdateUser", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Email", Email, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Comment", Comment, DbType.String, null, null);
        	
            sp.Command.AddParameter("@IsApproved", IsApproved, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@LastLoginDate", LastLoginDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@LastActivityDate", LastActivityDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@UniqueEmail", UniqueEmail, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc, DbType.DateTime, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Membership_UpdateUserInfo Procedure
        /// </summary>
        public static StoredProcedure AspnetMembershipUpdateUserInfo(string ApplicationName, string UserName, bool? IsPasswordCorrect, bool? UpdateLastLoginActivityDate, int? MaxInvalidPasswordAttempts, int? PasswordAttemptWindow, DateTime? CurrentTimeUtc, DateTime? LastLoginDate, DateTime? LastActivityDate)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Membership_UpdateUserInfo", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@IsPasswordCorrect", IsPasswordCorrect, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@UpdateLastLoginActivityDate", UpdateLastLoginActivityDate, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@MaxInvalidPasswordAttempts", MaxInvalidPasswordAttempts, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PasswordAttemptWindow", PasswordAttemptWindow, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@LastLoginDate", LastLoginDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@LastActivityDate", LastActivityDate, DbType.DateTime, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Paths_CreatePath Procedure
        /// </summary>
        public static StoredProcedure AspnetPathsCreatePath(Guid? ApplicationId, string Path, Guid? PathId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Paths_CreatePath", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationId", ApplicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@Path", Path, DbType.String, null, null);
        	
            sp.Command.AddOutputParameter("@PathId", DbType.Guid, null, null);
            
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Personalization_GetApplicationId Procedure
        /// </summary>
        public static StoredProcedure AspnetPersonalizationGetApplicationId(string ApplicationName, Guid? ApplicationId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Personalization_GetApplicationId", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddOutputParameter("@ApplicationId", DbType.Guid, null, null);
            
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_PersonalizationAdministration_DeleteAllState Procedure
        /// </summary>
        public static StoredProcedure AspnetPersonalizationAdministrationDeleteAllState(bool? AllUsersScope, string ApplicationName, int? Count)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_PersonalizationAdministration_DeleteAllState", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@AllUsersScope", AllUsersScope, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddOutputParameter("@Count", DbType.Int32, 0, 10);
            
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_PersonalizationAdministration_FindState Procedure
        /// </summary>
        public static StoredProcedure AspnetPersonalizationAdministrationFindState(bool? AllUsersScope, string ApplicationName, int? PageIndex, int? PageSize, string Path, string UserName, DateTime? InactiveSinceDate)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_PersonalizationAdministration_FindState", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@AllUsersScope", AllUsersScope, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@PageIndex", PageIndex, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PageSize", PageSize, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@Path", Path, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@InactiveSinceDate", InactiveSinceDate, DbType.DateTime, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_PersonalizationAdministration_GetCountOfState Procedure
        /// </summary>
        public static StoredProcedure AspnetPersonalizationAdministrationGetCountOfState(int? Count, bool? AllUsersScope, string ApplicationName, string Path, string UserName, DateTime? InactiveSinceDate)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_PersonalizationAdministration_GetCountOfState", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddOutputParameter("@Count", DbType.Int32, 0, 10);
            
            sp.Command.AddParameter("@AllUsersScope", AllUsersScope, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Path", Path, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@InactiveSinceDate", InactiveSinceDate, DbType.DateTime, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_PersonalizationAdministration_ResetSharedState Procedure
        /// </summary>
        public static StoredProcedure AspnetPersonalizationAdministrationResetSharedState(int? Count, string ApplicationName, string Path)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_PersonalizationAdministration_ResetSharedState", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddOutputParameter("@Count", DbType.Int32, 0, 10);
            
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Path", Path, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_PersonalizationAdministration_ResetUserState Procedure
        /// </summary>
        public static StoredProcedure AspnetPersonalizationAdministrationResetUserState(int? Count, string ApplicationName, DateTime? InactiveSinceDate, string UserName, string Path)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_PersonalizationAdministration_ResetUserState", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddOutputParameter("@Count", DbType.Int32, 0, 10);
            
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@InactiveSinceDate", InactiveSinceDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Path", Path, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_PersonalizationAllUsers_GetPageSettings Procedure
        /// </summary>
        public static StoredProcedure AspnetPersonalizationAllUsersGetPageSettings(string ApplicationName, string Path)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_PersonalizationAllUsers_GetPageSettings", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Path", Path, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_PersonalizationAllUsers_ResetPageSettings Procedure
        /// </summary>
        public static StoredProcedure AspnetPersonalizationAllUsersResetPageSettings(string ApplicationName, string Path)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_PersonalizationAllUsers_ResetPageSettings", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Path", Path, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_PersonalizationAllUsers_SetPageSettings Procedure
        /// </summary>
        public static StoredProcedure AspnetPersonalizationAllUsersSetPageSettings(string ApplicationName, string Path, byte[] PageSettings, DateTime? CurrentTimeUtc)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_PersonalizationAllUsers_SetPageSettings", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Path", Path, DbType.String, null, null);
        	
            sp.Command.AddParameter("@PageSettings", PageSettings, DbType.Binary, null, null);
        	
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc, DbType.DateTime, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_PersonalizationPerUser_GetPageSettings Procedure
        /// </summary>
        public static StoredProcedure AspnetPersonalizationPerUserGetPageSettings(string ApplicationName, string UserName, string Path, DateTime? CurrentTimeUtc)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_PersonalizationPerUser_GetPageSettings", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Path", Path, DbType.String, null, null);
        	
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc, DbType.DateTime, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_PersonalizationPerUser_ResetPageSettings Procedure
        /// </summary>
        public static StoredProcedure AspnetPersonalizationPerUserResetPageSettings(string ApplicationName, string UserName, string Path, DateTime? CurrentTimeUtc)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_PersonalizationPerUser_ResetPageSettings", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Path", Path, DbType.String, null, null);
        	
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc, DbType.DateTime, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_PersonalizationPerUser_SetPageSettings Procedure
        /// </summary>
        public static StoredProcedure AspnetPersonalizationPerUserSetPageSettings(string ApplicationName, string UserName, string Path, byte[] PageSettings, DateTime? CurrentTimeUtc)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_PersonalizationPerUser_SetPageSettings", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Path", Path, DbType.String, null, null);
        	
            sp.Command.AddParameter("@PageSettings", PageSettings, DbType.Binary, null, null);
        	
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc, DbType.DateTime, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Profile_DeleteInactiveProfiles Procedure
        /// </summary>
        public static StoredProcedure AspnetProfileDeleteInactiveProfiles(string ApplicationName, int? ProfileAuthOptions, DateTime? InactiveSinceDate)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Profile_DeleteInactiveProfiles", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@ProfileAuthOptions", ProfileAuthOptions, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@InactiveSinceDate", InactiveSinceDate, DbType.DateTime, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Profile_DeleteProfiles Procedure
        /// </summary>
        public static StoredProcedure AspnetProfileDeleteProfiles(string ApplicationName, string UserNames)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Profile_DeleteProfiles", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserNames", UserNames, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Profile_GetNumberOfInactiveProfiles Procedure
        /// </summary>
        public static StoredProcedure AspnetProfileGetNumberOfInactiveProfiles(string ApplicationName, int? ProfileAuthOptions, DateTime? InactiveSinceDate)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Profile_GetNumberOfInactiveProfiles", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@ProfileAuthOptions", ProfileAuthOptions, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@InactiveSinceDate", InactiveSinceDate, DbType.DateTime, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Profile_GetProfiles Procedure
        /// </summary>
        public static StoredProcedure AspnetProfileGetProfiles(string ApplicationName, int? ProfileAuthOptions, int? PageIndex, int? PageSize, string UserNameToMatch, DateTime? InactiveSinceDate)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Profile_GetProfiles", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@ProfileAuthOptions", ProfileAuthOptions, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PageIndex", PageIndex, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PageSize", PageSize, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@UserNameToMatch", UserNameToMatch, DbType.String, null, null);
        	
            sp.Command.AddParameter("@InactiveSinceDate", InactiveSinceDate, DbType.DateTime, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Profile_GetProperties Procedure
        /// </summary>
        public static StoredProcedure AspnetProfileGetProperties(string ApplicationName, string UserName, DateTime? CurrentTimeUtc)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Profile_GetProperties", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc, DbType.DateTime, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Profile_SetProperties Procedure
        /// </summary>
        public static StoredProcedure AspnetProfileSetProperties(string ApplicationName, string PropertyNames, string PropertyValuesString, byte[] PropertyValuesBinary, string UserName, bool? IsUserAnonymous, DateTime? CurrentTimeUtc)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Profile_SetProperties", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@PropertyNames", PropertyNames, DbType.String, null, null);
        	
            sp.Command.AddParameter("@PropertyValuesString", PropertyValuesString, DbType.String, null, null);
        	
            sp.Command.AddParameter("@PropertyValuesBinary", PropertyValuesBinary, DbType.Binary, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@IsUserAnonymous", IsUserAnonymous, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc, DbType.DateTime, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_RegisterSchemaVersion Procedure
        /// </summary>
        public static StoredProcedure AspnetRegisterSchemaVersion(string Feature, string CompatibleSchemaVersion, bool? IsCurrentVersion, bool? RemoveIncompatibleSchema)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_RegisterSchemaVersion", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@Feature", Feature, DbType.String, null, null);
        	
            sp.Command.AddParameter("@CompatibleSchemaVersion", CompatibleSchemaVersion, DbType.String, null, null);
        	
            sp.Command.AddParameter("@IsCurrentVersion", IsCurrentVersion, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@RemoveIncompatibleSchema", RemoveIncompatibleSchema, DbType.Boolean, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Roles_CreateRole Procedure
        /// </summary>
        public static StoredProcedure AspnetRolesCreateRole(string ApplicationName, string RoleName)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Roles_CreateRole", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@RoleName", RoleName, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Roles_DeleteRole Procedure
        /// </summary>
        public static StoredProcedure AspnetRolesDeleteRole(string ApplicationName, string RoleName, bool? DeleteOnlyIfRoleIsEmpty)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Roles_DeleteRole", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@RoleName", RoleName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@DeleteOnlyIfRoleIsEmpty", DeleteOnlyIfRoleIsEmpty, DbType.Boolean, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Roles_GetAllRoles Procedure
        /// </summary>
        public static StoredProcedure AspnetRolesGetAllRoles(string ApplicationName)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Roles_GetAllRoles", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Roles_RoleExists Procedure
        /// </summary>
        public static StoredProcedure AspnetRolesRoleExists(string ApplicationName, string RoleName)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Roles_RoleExists", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@RoleName", RoleName, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Setup_RemoveAllRoleMembers Procedure
        /// </summary>
        public static StoredProcedure AspnetSetupRemoveAllRoleMembers(string name)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Setup_RemoveAllRoleMembers", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@name", name, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Setup_RestorePermissions Procedure
        /// </summary>
        public static StoredProcedure AspnetSetupRestorePermissions(string name)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Setup_RestorePermissions", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@name", name, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_UnRegisterSchemaVersion Procedure
        /// </summary>
        public static StoredProcedure AspnetUnRegisterSchemaVersion(string Feature, string CompatibleSchemaVersion)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_UnRegisterSchemaVersion", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@Feature", Feature, DbType.String, null, null);
        	
            sp.Command.AddParameter("@CompatibleSchemaVersion", CompatibleSchemaVersion, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Users_CreateUser Procedure
        /// </summary>
        public static StoredProcedure AspnetUsersCreateUser(Guid? ApplicationId, string UserName, bool? IsUserAnonymous, DateTime? LastActivityDate, Guid? UserId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Users_CreateUser", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationId", ApplicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@IsUserAnonymous", IsUserAnonymous, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@LastActivityDate", LastActivityDate, DbType.DateTime, null, null);
        	
            sp.Command.AddOutputParameter("@UserId", DbType.Guid, null, null);
            
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Users_DeleteUser Procedure
        /// </summary>
        public static StoredProcedure AspnetUsersDeleteUser(string ApplicationName, string UserName, int? TablesToDeleteFrom, int? NumTablesDeletedFrom)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Users_DeleteUser", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@TablesToDeleteFrom", TablesToDeleteFrom, DbType.Int32, 0, 10);
        	
            sp.Command.AddOutputParameter("@NumTablesDeletedFrom", DbType.Int32, 0, 10);
            
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_UsersInRoles_AddUsersToRoles Procedure
        /// </summary>
        public static StoredProcedure AspnetUsersInRolesAddUsersToRoles(string ApplicationName, string UserNames, string RoleNames, DateTime? CurrentTimeUtc)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_UsersInRoles_AddUsersToRoles", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserNames", UserNames, DbType.String, null, null);
        	
            sp.Command.AddParameter("@RoleNames", RoleNames, DbType.String, null, null);
        	
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc, DbType.DateTime, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_UsersInRoles_FindUsersInRole Procedure
        /// </summary>
        public static StoredProcedure AspnetUsersInRolesFindUsersInRole(string ApplicationName, string RoleName, string UserNameToMatch)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_UsersInRoles_FindUsersInRole", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@RoleName", RoleName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserNameToMatch", UserNameToMatch, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_UsersInRoles_GetRolesForUser Procedure
        /// </summary>
        public static StoredProcedure AspnetUsersInRolesGetRolesForUser(string ApplicationName, string UserName)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_UsersInRoles_GetRolesForUser", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_UsersInRoles_GetUsersInRoles Procedure
        /// </summary>
        public static StoredProcedure AspnetUsersInRolesGetUsersInRoles(string ApplicationName, string RoleName)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_UsersInRoles_GetUsersInRoles", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@RoleName", RoleName, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_UsersInRoles_IsUserInRole Procedure
        /// </summary>
        public static StoredProcedure AspnetUsersInRolesIsUserInRole(string ApplicationName, string UserName, string RoleName)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_UsersInRoles_IsUserInRole", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@RoleName", RoleName, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_UsersInRoles_RemoveUsersFromRoles Procedure
        /// </summary>
        public static StoredProcedure AspnetUsersInRolesRemoveUsersFromRoles(string ApplicationName, string UserNames, string RoleNames)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_UsersInRoles_RemoveUsersFromRoles", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserNames", UserNames, DbType.String, null, null);
        	
            sp.Command.AddParameter("@RoleNames", RoleNames, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_WebEvent_LogEvent Procedure
        /// </summary>
        public static StoredProcedure AspnetWebEventLogEvent(string EventId, DateTime? EventTimeUtc, DateTime? EventTime, string EventType, decimal? EventSequence, decimal? EventOccurrence, int? EventCode, int? EventDetailCode, string Message, string ApplicationPath, string ApplicationVirtualPath, string MachineName, string RequestUrl, string ExceptionType, string Details)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_WebEvent_LogEvent", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@EventId", EventId, DbType.AnsiStringFixedLength, null, null);
        	
            sp.Command.AddParameter("@EventTimeUtc", EventTimeUtc, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@EventTime", EventTime, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@EventType", EventType, DbType.String, null, null);
        	
            sp.Command.AddParameter("@EventSequence", EventSequence, DbType.Decimal, 0, 19);
        	
            sp.Command.AddParameter("@EventOccurrence", EventOccurrence, DbType.Decimal, 0, 19);
        	
            sp.Command.AddParameter("@EventCode", EventCode, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@EventDetailCode", EventDetailCode, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@Message", Message, DbType.String, null, null);
        	
            sp.Command.AddParameter("@ApplicationPath", ApplicationPath, DbType.String, null, null);
        	
            sp.Command.AddParameter("@ApplicationVirtualPath", ApplicationVirtualPath, DbType.String, null, null);
        	
            sp.Command.AddParameter("@MachineName", MachineName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@RequestUrl", RequestUrl, DbType.String, null, null);
        	
            sp.Command.AddParameter("@ExceptionType", ExceptionType, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Details", Details, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the dt_addtosourcecontrol Procedure
        /// </summary>
        public static StoredProcedure DtAddtosourcecontrol(string vchSourceSafeINI, string vchProjectName, string vchComment, string vchLoginName, string vchPassword)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("dt_addtosourcecontrol", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@vchSourceSafeINI", vchSourceSafeINI, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@vchProjectName", vchProjectName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@vchComment", vchComment, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@vchLoginName", vchLoginName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@vchPassword", vchPassword, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the dt_addtosourcecontrol_u Procedure
        /// </summary>
        public static StoredProcedure DtAddtosourcecontrolU(string vchSourceSafeINI, string vchProjectName, string vchComment, string vchLoginName, string vchPassword)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("dt_addtosourcecontrol_u", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@vchSourceSafeINI", vchSourceSafeINI, DbType.String, null, null);
        	
            sp.Command.AddParameter("@vchProjectName", vchProjectName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@vchComment", vchComment, DbType.String, null, null);
        	
            sp.Command.AddParameter("@vchLoginName", vchLoginName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@vchPassword", vchPassword, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the dt_adduserobject Procedure
        /// </summary>
        public static StoredProcedure DtAdduserobject()
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("dt_adduserobject", DataService.GetInstance("WillCall"), "");
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the dt_adduserobject_vcs Procedure
        /// </summary>
        public static StoredProcedure DtAdduserobjectVcs(string vchProperty)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("dt_adduserobject_vcs", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@vchProperty", vchProperty, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the dt_checkinobject Procedure
        /// </summary>
        public static StoredProcedure DtCheckinobject(string chObjectType, string vchObjectName, string vchComment, string vchLoginName, string vchPassword, int? iVCSFlags, int? iActionFlag, string txStream1, string txStream2, string txStream3)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("dt_checkinobject", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@chObjectType", chObjectType, DbType.AnsiStringFixedLength, null, null);
        	
            sp.Command.AddParameter("@vchObjectName", vchObjectName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@vchComment", vchComment, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@vchLoginName", vchLoginName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@vchPassword", vchPassword, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@iVCSFlags", iVCSFlags, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@iActionFlag", iActionFlag, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@txStream1", txStream1, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@txStream2", txStream2, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@txStream3", txStream3, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the dt_checkinobject_u Procedure
        /// </summary>
        public static StoredProcedure DtCheckinobjectU(string chObjectType, string vchObjectName, string vchComment, string vchLoginName, string vchPassword, int? iVCSFlags, int? iActionFlag, string txStream1, string txStream2, string txStream3)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("dt_checkinobject_u", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@chObjectType", chObjectType, DbType.AnsiStringFixedLength, null, null);
        	
            sp.Command.AddParameter("@vchObjectName", vchObjectName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@vchComment", vchComment, DbType.String, null, null);
        	
            sp.Command.AddParameter("@vchLoginName", vchLoginName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@vchPassword", vchPassword, DbType.String, null, null);
        	
            sp.Command.AddParameter("@iVCSFlags", iVCSFlags, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@iActionFlag", iActionFlag, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@txStream1", txStream1, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@txStream2", txStream2, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@txStream3", txStream3, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the dt_checkoutobject Procedure
        /// </summary>
        public static StoredProcedure DtCheckoutobject(string chObjectType, string vchObjectName, string vchComment, string vchLoginName, string vchPassword, int? iVCSFlags, int? iActionFlag)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("dt_checkoutobject", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@chObjectType", chObjectType, DbType.AnsiStringFixedLength, null, null);
        	
            sp.Command.AddParameter("@vchObjectName", vchObjectName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@vchComment", vchComment, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@vchLoginName", vchLoginName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@vchPassword", vchPassword, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@iVCSFlags", iVCSFlags, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@iActionFlag", iActionFlag, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the dt_checkoutobject_u Procedure
        /// </summary>
        public static StoredProcedure DtCheckoutobjectU(string chObjectType, string vchObjectName, string vchComment, string vchLoginName, string vchPassword, int? iVCSFlags, int? iActionFlag)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("dt_checkoutobject_u", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@chObjectType", chObjectType, DbType.AnsiStringFixedLength, null, null);
        	
            sp.Command.AddParameter("@vchObjectName", vchObjectName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@vchComment", vchComment, DbType.String, null, null);
        	
            sp.Command.AddParameter("@vchLoginName", vchLoginName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@vchPassword", vchPassword, DbType.String, null, null);
        	
            sp.Command.AddParameter("@iVCSFlags", iVCSFlags, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@iActionFlag", iActionFlag, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the dt_displayoaerror Procedure
        /// </summary>
        public static StoredProcedure DtDisplayoaerror(int? iObject, int? iresult)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("dt_displayoaerror", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@iObject", iObject, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@iresult", iresult, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the dt_displayoaerror_u Procedure
        /// </summary>
        public static StoredProcedure DtDisplayoaerrorU(int? iObject, int? iresult)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("dt_displayoaerror_u", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@iObject", iObject, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@iresult", iresult, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the dt_droppropertiesbyid Procedure
        /// </summary>
        public static StoredProcedure DtDroppropertiesbyid(int? id, string propertyX)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("dt_droppropertiesbyid", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@id", id, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@property", propertyX, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the dt_dropuserobjectbyid Procedure
        /// </summary>
        public static StoredProcedure DtDropuserobjectbyid(int? id)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("dt_dropuserobjectbyid", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@id", id, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the dt_generateansiname Procedure
        /// </summary>
        public static StoredProcedure DtGenerateansiname(string name)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("dt_generateansiname", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddOutputParameter("@name", DbType.AnsiString, null, null);
            
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the dt_getobjwithprop Procedure
        /// </summary>
        public static StoredProcedure DtGetobjwithprop(string propertyX, string valueX)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("dt_getobjwithprop", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@property", propertyX, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@value", valueX, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the dt_getobjwithprop_u Procedure
        /// </summary>
        public static StoredProcedure DtGetobjwithpropU(string propertyX, string uvalue)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("dt_getobjwithprop_u", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@property", propertyX, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@uvalue", uvalue, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the dt_getpropertiesbyid Procedure
        /// </summary>
        public static StoredProcedure DtGetpropertiesbyid(int? id, string propertyX)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("dt_getpropertiesbyid", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@id", id, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@property", propertyX, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the dt_getpropertiesbyid_u Procedure
        /// </summary>
        public static StoredProcedure DtGetpropertiesbyidU(int? id, string propertyX)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("dt_getpropertiesbyid_u", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@id", id, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@property", propertyX, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the dt_getpropertiesbyid_vcs Procedure
        /// </summary>
        public static StoredProcedure DtGetpropertiesbyidVcs(int? id, string propertyX, string valueX)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("dt_getpropertiesbyid_vcs", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@id", id, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@property", propertyX, DbType.AnsiString, null, null);
        	
            sp.Command.AddOutputParameter("@value", DbType.AnsiString, null, null);
            
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the dt_getpropertiesbyid_vcs_u Procedure
        /// </summary>
        public static StoredProcedure DtGetpropertiesbyidVcsU(int? id, string propertyX, string valueX)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("dt_getpropertiesbyid_vcs_u", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@id", id, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@property", propertyX, DbType.AnsiString, null, null);
        	
            sp.Command.AddOutputParameter("@value", DbType.String, null, null);
            
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the dt_isundersourcecontrol Procedure
        /// </summary>
        public static StoredProcedure DtIsundersourcecontrol(string vchLoginName, string vchPassword, int? iWhoToo)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("dt_isundersourcecontrol", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@vchLoginName", vchLoginName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@vchPassword", vchPassword, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@iWhoToo", iWhoToo, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the dt_isundersourcecontrol_u Procedure
        /// </summary>
        public static StoredProcedure DtIsundersourcecontrolU(string vchLoginName, string vchPassword, int? iWhoToo)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("dt_isundersourcecontrol_u", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@vchLoginName", vchLoginName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@vchPassword", vchPassword, DbType.String, null, null);
        	
            sp.Command.AddParameter("@iWhoToo", iWhoToo, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the dt_removefromsourcecontrol Procedure
        /// </summary>
        public static StoredProcedure DtRemovefromsourcecontrol()
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("dt_removefromsourcecontrol", DataService.GetInstance("WillCall"), "");
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the dt_setpropertybyid Procedure
        /// </summary>
        public static StoredProcedure DtSetpropertybyid(int? id, string propertyX, string valueX, byte[] lvalue)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("dt_setpropertybyid", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@id", id, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@property", propertyX, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@value", valueX, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@lvalue", lvalue, DbType.Binary, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the dt_setpropertybyid_u Procedure
        /// </summary>
        public static StoredProcedure DtSetpropertybyidU(int? id, string propertyX, string uvalue, byte[] lvalue)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("dt_setpropertybyid_u", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@id", id, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@property", propertyX, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@uvalue", uvalue, DbType.String, null, null);
        	
            sp.Command.AddParameter("@lvalue", lvalue, DbType.Binary, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the dt_validateloginparams Procedure
        /// </summary>
        public static StoredProcedure DtValidateloginparams(string vchLoginName, string vchPassword)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("dt_validateloginparams", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@vchLoginName", vchLoginName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@vchPassword", vchPassword, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the dt_validateloginparams_u Procedure
        /// </summary>
        public static StoredProcedure DtValidateloginparamsU(string vchLoginName, string vchPassword)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("dt_validateloginparams_u", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@vchLoginName", vchLoginName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@vchPassword", vchPassword, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the dt_vcsenabled Procedure
        /// </summary>
        public static StoredProcedure DtVcsenabled()
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("dt_vcsenabled", DataService.GetInstance("WillCall"), "");
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the dt_verstamp006 Procedure
        /// </summary>
        public static StoredProcedure DtVerstamp006()
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("dt_verstamp006", DataService.GetInstance("WillCall"), "");
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the dt_whocheckedout Procedure
        /// </summary>
        public static StoredProcedure DtWhocheckedout(string chObjectType, string vchObjectName, string vchLoginName, string vchPassword)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("dt_whocheckedout", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@chObjectType", chObjectType, DbType.AnsiStringFixedLength, null, null);
        	
            sp.Command.AddParameter("@vchObjectName", vchObjectName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@vchLoginName", vchLoginName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@vchPassword", vchPassword, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the dt_whocheckedout_u Procedure
        /// </summary>
        public static StoredProcedure DtWhocheckedoutU(string chObjectType, string vchObjectName, string vchLoginName, string vchPassword)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("dt_whocheckedout_u", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@chObjectType", chObjectType, DbType.AnsiStringFixedLength, null, null);
        	
            sp.Command.AddParameter("@vchObjectName", vchObjectName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@vchLoginName", vchLoginName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@vchPassword", vchPassword, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_BulkInsert_InventoryCodes Procedure
        /// </summary>
        public static StoredProcedure TxBulkInsertInventoryCodes(string InventoryHeaders)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_BulkInsert_InventoryCodes", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@InventoryHeaders", InventoryHeaders, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_ChargeStatement_Edit Procedure
        /// </summary>
        public static StoredProcedure TxChargeStatementEdit(string appName, int? month, int? year, decimal? perSales, decimal? perRefund, decimal? grossThreshhold, decimal? grossPct, decimal? ticketInvoicePct, decimal? ticketUnitPct, decimal? ticketSalesPct, decimal? merchInvoicePct, decimal? merchUnitPct, decimal? merchSalesPct, decimal? perTktShip, decimal? pctTktShipSales, decimal? perSubscription, decimal? perMailSent, bool? useCurrentValues)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_ChargeStatement_Edit", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@appName", appName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@month", month, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@year", year, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@perSales", perSales, DbType.Currency, 4, 19);
        	
            sp.Command.AddParameter("@perRefund", perRefund, DbType.Currency, 4, 19);
        	
            sp.Command.AddParameter("@grossThreshhold", grossThreshhold, DbType.Currency, 4, 19);
        	
            sp.Command.AddParameter("@grossPct", grossPct, DbType.Currency, 4, 19);
        	
            sp.Command.AddParameter("@ticketInvoicePct", ticketInvoicePct, DbType.Currency, 4, 19);
        	
            sp.Command.AddParameter("@ticketUnitPct", ticketUnitPct, DbType.Currency, 4, 19);
        	
            sp.Command.AddParameter("@ticketSalesPct", ticketSalesPct, DbType.Currency, 4, 19);
        	
            sp.Command.AddParameter("@merchInvoicePct", merchInvoicePct, DbType.Currency, 4, 19);
        	
            sp.Command.AddParameter("@merchUnitPct", merchUnitPct, DbType.Currency, 4, 19);
        	
            sp.Command.AddParameter("@merchSalesPct", merchSalesPct, DbType.Currency, 4, 19);
        	
            sp.Command.AddParameter("@perTktShip", perTktShip, DbType.Currency, 4, 19);
        	
            sp.Command.AddParameter("@pctTktShipSales", pctTktShipSales, DbType.Currency, 4, 19);
        	
            sp.Command.AddParameter("@perSubscription", perSubscription, DbType.Currency, 4, 19);
        	
            sp.Command.AddParameter("@perMailSent", perMailSent, DbType.Decimal, 0, 18);
        	
            sp.Command.AddParameter("@useCurrentValues", useCurrentValues, DbType.Boolean, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_ChargeStatement_View Procedure
        /// </summary>
        public static StoredProcedure TxChargeStatementView(string appName, int? year)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_ChargeStatement_View", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@appName", appName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@year", year, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_CheckoutCashew Procedure
        /// </summary>
        public static StoredProcedure TxCheckoutCashew(string aspnetUserId, string eCardName, string eCardNumber, string eCardMonth, string eCardYear)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_CheckoutCashew", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@aspnetUserId", aspnetUserId, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@eCardName", eCardName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@eCardNumber", eCardNumber, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@eCardMonth", eCardMonth, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@eCardYear", eCardYear, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_CSV_MerchByDivCat Procedure
        /// </summary>
        public static StoredProcedure TxCsvMerchByDivCat(Guid? applicationId, string deliveryDefault, string DeliveryType, int? DivId, int? CatId, string ActiveStatus)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_CSV_MerchByDivCat", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationId", applicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@deliveryDefault", deliveryDefault, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@DeliveryType", DeliveryType, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@DivId", DivId, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@CatId", CatId, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@ActiveStatus", ActiveStatus, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_CustomerSearch_ByProfileParam Procedure
        /// </summary>
        public static StoredProcedure TxCustomerSearchByProfileParam(string applicationName, string ParamName, string ParamValue)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_CustomerSearch_ByProfileParam", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationName", applicationName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@ParamName", ParamName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@ParamValue", ParamValue, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_CustomerSearch_InvoiceNumber Procedure
        /// </summary>
        public static StoredProcedure TxCustomerSearchInvoiceNumber(string applicationName, string invoiceNumber)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_CustomerSearch_InvoiceNumber", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationName", applicationName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@invoiceNumber", invoiceNumber, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_CustomerSearch_LikeProfileParam Procedure
        /// </summary>
        public static StoredProcedure TxCustomerSearchLikeProfileParam(string applicationName, string ParamName, string ParamValue)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_CustomerSearch_LikeProfileParam", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationName", applicationName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@ParamName", ParamName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@ParamValue", ParamValue, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_CustomerSearch_LikeUserName Procedure
        /// </summary>
        public static StoredProcedure TxCustomerSearchLikeUserName(string applicationName, string nameToFind)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_CustomerSearch_LikeUserName", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationName", applicationName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@nameToFind", nameToFind, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetBillShipsOfMerchItem Procedure
        /// </summary>
        public static StoredProcedure TxGetBillShipsOfMerchItem(Guid? applicationId, int? merchId, bool? exclusive, int? minQty, int? StartRowIndex, int? PageSize, DateTime? dtStart, DateTime? dtEnd)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetBillShipsOfMerchItem", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationId", applicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@merchId", merchId, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@exclusive", exclusive, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@minQty", minQty, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@StartRowIndex", StartRowIndex, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PageSize", PageSize, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@dtStart", dtStart, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@dtEnd", dtEnd, DbType.DateTime, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetBillShipsOfMerchItemCount Procedure
        /// </summary>
        public static StoredProcedure TxGetBillShipsOfMerchItemCount(Guid? applicationId, int? merchId, bool? exclusive, int? minQty, DateTime? dtStart, DateTime? dtEnd)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetBillShipsOfMerchItemCount", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationId", applicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@merchId", merchId, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@exclusive", exclusive, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@minQty", minQty, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@dtStart", dtStart, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@dtEnd", dtEnd, DbType.DateTime, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetCurrentObjects Procedure
        /// </summary>
        public static StoredProcedure TxGetCurrentObjects(string appName, string context, string granularcontext, DateTime? nowDate, int? showId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetCurrentObjects", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@appName", appName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@context", context, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@granularcontext", granularcontext, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@nowDate", nowDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@showId", showId, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetCustomerSalesHistory Procedure
        /// </summary>
        public static StoredProcedure TxGetCustomerSalesHistory(string ApplicationName, string UserName, int? StartRowIndex, int? PageSize)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetCustomerSalesHistory", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@StartRowIndex", StartRowIndex, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PageSize", PageSize, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetCustomerSalesHistoryCount Procedure
        /// </summary>
        public static StoredProcedure TxGetCustomerSalesHistoryCount(string ApplicationName, string UserName)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetCustomerSalesHistoryCount", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetInventoryTickets_Discrepancies Procedure
        /// </summary>
        public static StoredProcedure TxGetInventoryTicketsDiscrepancies(Guid? applicationId, int? StartRowIndex, int? PageSize, DateTime? StartDate, DateTime? EndDate)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetInventoryTickets_Discrepancies", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationId", applicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@StartRowIndex", StartRowIndex, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PageSize", PageSize, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@StartDate", StartDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@EndDate", EndDate, DbType.DateTime, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetInventoryTickets_DiscrepanciesCount Procedure
        /// </summary>
        public static StoredProcedure TxGetInventoryTicketsDiscrepanciesCount(Guid? applicationId, DateTime? StartDate, DateTime? EndDate)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetInventoryTickets_DiscrepanciesCount", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationId", applicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@StartDate", StartDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@EndDate", EndDate, DbType.DateTime, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetMerchCodesInRange Procedure
        /// </summary>
        public static StoredProcedure TxGetMerchCodesInRange(Guid? applicationId, int? ParentId, string Style, string Color, string Size, string ActiveStatus, string StartDate, string EndDate)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetMerchCodesInRange", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationId", applicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@ParentId", ParentId, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@Style", Style, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@Color", Color, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@Size", Size, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@ActiveStatus", ActiveStatus, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@StartDate", StartDate, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@EndDate", EndDate, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetMerchInventoryInRange Procedure
        /// </summary>
        public static StoredProcedure TxGetMerchInventoryInRange(Guid? applicationId, int? parentId, string Style, string Color, string Size, string ActiveStatus, int? StartRowIndex, int? PageSize)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetMerchInventoryInRange", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationId", applicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@parentId", parentId, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@Style", Style, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@Color", Color, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@Size", Size, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@ActiveStatus", ActiveStatus, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@StartRowIndex", StartRowIndex, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PageSize", PageSize, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetMerchInventoryInRange_Count Procedure
        /// </summary>
        public static StoredProcedure TxGetMerchInventoryInRangeCount(Guid? applicationId, int? parentId, string Style, string Color, string Size, string ActiveStatus)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetMerchInventoryInRange_Count", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationId", applicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@parentId", parentId, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@Style", Style, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@Color", Color, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@Size", Size, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@ActiveStatus", ActiveStatus, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetMerchParentsByDivCat Procedure
        /// </summary>
        public static StoredProcedure TxGetMerchParentsByDivCat(Guid? applicationId, string deliveryDefault, string DeliveryType, int? DivId, int? CatId, string ActiveStatus, int? StartRowIndex, int? PageSize)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetMerchParentsByDivCat", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationId", applicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@deliveryDefault", deliveryDefault, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@DeliveryType", DeliveryType, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@DivId", DivId, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@CatId", CatId, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@ActiveStatus", ActiveStatus, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@StartRowIndex", StartRowIndex, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PageSize", PageSize, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetMerchParentsByDivCat_Count Procedure
        /// </summary>
        public static StoredProcedure TxGetMerchParentsByDivCatCount(Guid? applicationId, string deliveryDefault, string DeliveryType, int? DivId, int? CatId, string ActiveStatus)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetMerchParentsByDivCat_Count", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationId", applicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@deliveryDefault", deliveryDefault, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@DeliveryType", DeliveryType, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@DivId", DivId, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@CatId", CatId, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@ActiveStatus", ActiveStatus, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetMerchSalesInRange Procedure
        /// </summary>
        public static StoredProcedure TxGetMerchSalesInRange(Guid? applicationId, int? ParentId, string Style, string Color, string Size, string ActiveStatus, bool? EmailOnly, bool? IncludeInvoiceIdWithEmail, string StartDate, string EndDate, int? StartRowIndex, int? PageSize)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetMerchSalesInRange", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationId", applicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@ParentId", ParentId, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@Style", Style, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@Color", Color, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@Size", Size, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@ActiveStatus", ActiveStatus, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@EmailOnly", EmailOnly, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@IncludeInvoiceIdWithEmail", IncludeInvoiceIdWithEmail, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@StartDate", StartDate, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@EndDate", EndDate, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@StartRowIndex", StartRowIndex, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PageSize", PageSize, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetMerchSalesInRange_Aggregates Procedure
        /// </summary>
        public static StoredProcedure TxGetMerchSalesInRangeAggregates(Guid? applicationId, int? ParentId, string Style, string Color, string Size, string ActiveStatus, string StartDate, string EndDate)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetMerchSalesInRange_Aggregates", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationId", applicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@ParentId", ParentId, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@Style", Style, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@Color", Color, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@Size", Size, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@ActiveStatus", ActiveStatus, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@StartDate", StartDate, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@EndDate", EndDate, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetMerchSalesInRange_Count Procedure
        /// </summary>
        public static StoredProcedure TxGetMerchSalesInRangeCount(Guid? applicationId, int? ParentId, string Style, string Color, string Size, string ActiveStatus, string StartDate, string EndDate)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetMerchSalesInRange_Count", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationId", applicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@ParentId", ParentId, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@Style", Style, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@Color", Color, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@Size", Size, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@ActiveStatus", ActiveStatus, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@StartDate", StartDate, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@EndDate", EndDate, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetOrdersInRange Procedure
        /// </summary>
        public static StoredProcedure TxGetOrdersInRange(Guid? applicationId, string Context, string StartDate, string EndDate, int? StartRowIndex, int? PageSize)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetOrdersInRange", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationId", applicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@Context", Context, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@StartDate", StartDate, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@EndDate", EndDate, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@StartRowIndex", StartRowIndex, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PageSize", PageSize, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetOrdersInRangeCount Procedure
        /// </summary>
        public static StoredProcedure TxGetOrdersInRangeCount(Guid? applicationId, string Context, string StartDate, string EndDate)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetOrdersInRangeCount", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationId", applicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@Context", Context, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@StartDate", StartDate, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@EndDate", EndDate, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetSalePromotions Procedure
        /// </summary>
        public static StoredProcedure TxGetSalePromotions(Guid? ApplicationId, string BannerContext, int? StartRowIndex, int? PageSize)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetSalePromotions", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationId", ApplicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@BannerContext", BannerContext, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@StartRowIndex", StartRowIndex, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PageSize", PageSize, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetSalePromotionsCount Procedure
        /// </summary>
        public static StoredProcedure TxGetSalePromotionsCount(Guid? applicationId, string BannerContext)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetSalePromotionsCount", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationId", applicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@BannerContext", BannerContext, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetSaleShowDateInventory Procedure
        /// </summary>
        public static StoredProcedure TxGetSaleShowDateInventory(Guid? applicationId, string nowName)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetSaleShowDateInventory", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationId", applicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@nowName", nowName, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetSaleShowDates Procedure
        /// </summary>
        public static StoredProcedure TxGetSaleShowDates(Guid? applicationId, string nowName)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetSaleShowDates", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationId", applicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@nowName", nowName, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetShipmentsInRange Procedure
        /// </summary>
        public static StoredProcedure TxGetShipmentsInRange(Guid? applicationId, string Context, string StartDate, string EndDate, int? StartRowIndex, int? PageSize)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetShipmentsInRange", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationId", applicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@Context", Context, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@StartDate", StartDate, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@EndDate", EndDate, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@StartRowIndex", StartRowIndex, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PageSize", PageSize, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetShipmentsInRangeCount Procedure
        /// </summary>
        public static StoredProcedure TxGetShipmentsInRangeCount(Guid? applicationId, string Context, string StartDate, string EndDate)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetShipmentsInRangeCount", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationId", applicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@Context", Context, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@StartDate", StartDate, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@EndDate", EndDate, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetShowDatesInRange Procedure
        /// </summary>
        public static StoredProcedure TxGetShowDatesInRange(Guid? applicationId, string defaultVenue, int? selectedVenueId, string startDate, string endDate, int? startRowIndex, int? pageSize, bool? returnSimpleRows, bool? returnShowDateRows, bool? returnTicketRows)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetShowDatesInRange", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationId", applicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@defaultVenue", defaultVenue, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@selectedVenueId", selectedVenueId, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@startDate", startDate, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@endDate", endDate, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@startRowIndex", startRowIndex, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@pageSize", pageSize, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@returnSimpleRows", returnSimpleRows, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@returnShowDateRows", returnShowDateRows, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@returnTicketRows", returnTicketRows, DbType.Boolean, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetShowDatesInRange_Count Procedure
        /// </summary>
        public static StoredProcedure TxGetShowDatesInRangeCount(Guid? applicationId, int? selectedVenueId, string startDate, string endDate)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetShowDatesInRange_Count", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationId", applicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@selectedVenueId", selectedVenueId, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@startDate", startDate, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@endDate", endDate, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetTicketCounts Procedure
        /// </summary>
        public static StoredProcedure TxGetTicketCounts(Guid? applicationId, string StartDate, string EndDate, int? StartRowIndex, int? PageSize)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetTicketCounts", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationId", applicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@StartDate", StartDate, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@EndDate", EndDate, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@StartRowIndex", StartRowIndex, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PageSize", PageSize, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetTicketCountsCount Procedure
        /// </summary>
        public static StoredProcedure TxGetTicketCountsCount(Guid? applicationId, string StartDate, string EndDate)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetTicketCountsCount", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationId", applicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@StartDate", StartDate, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@EndDate", EndDate, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetTicketSales Procedure
        /// </summary>
        public static StoredProcedure TxGetTicketSales(int? ShowDateId, string ShowTicketIds, string willCallText, string sortContext, string ShipContext, string PurchaseContext, bool? EmailOnly, int? StartRowIndex, int? PageSize)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetTicketSales", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ShowDateId", ShowDateId, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@ShowTicketIds", ShowTicketIds, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@willCallText", willCallText, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@sortContext", sortContext, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@ShipContext", ShipContext, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@PurchaseContext", PurchaseContext, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@EmailOnly", EmailOnly, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@StartRowIndex", StartRowIndex, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PageSize", PageSize, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetTicketSales_WorldshipExport Procedure
        /// </summary>
        public static StoredProcedure TxGetTicketSalesWorldshipExport(string ShowDateIdListUseCommas)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetTicketSales_WorldshipExport", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ShowDateIdList_UseCommas", ShowDateIdListUseCommas, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetTicketSales_WorldshipExportForBatch Procedure
        /// </summary>
        public static StoredProcedure TxGetTicketSalesWorldshipExportForBatch(int? batchId, string filter)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetTicketSales_WorldshipExportForBatch", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@batchId", batchId, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@filter", filter, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetTicketSalesCount Procedure
        /// </summary>
        public static StoredProcedure TxGetTicketSalesCount(int? ShowDateId, string ShowTicketIds, string willCallText, string ShipContext, string PurchaseContext)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetTicketSalesCount", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ShowDateId", ShowDateId, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@ShowTicketIds", ShowTicketIds, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@willCallText", willCallText, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@ShipContext", ShipContext, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@PurchaseContext", PurchaseContext, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_Inventory_AddUpdate Procedure
        /// </summary>
        public static StoredProcedure TxInventoryAddUpdate(Guid? guid, string sessId, string userName, int? idx, int? qty, DateTime? ttl, string context)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_Inventory_AddUpdate", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@guid", guid, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@sessId", sessId, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@userName", userName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@idx", idx, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@qty", qty, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@ttl", ttl, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@context", context, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_Inventory_Bundle_GetChildItemCount Procedure
        /// </summary>
        public static StoredProcedure TxInventoryBundleGetChildItemCount(int? BundleId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_Inventory_Bundle_GetChildItemCount", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@BundleId", BundleId, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_Inventory_ByContextOnId Procedure
        /// </summary>
        public static StoredProcedure TxInventoryByContextOnId(string Context, int? Idx)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_Inventory_ByContextOnId", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@Context", Context, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@Idx", Idx, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_Inventory_CleanupJob Procedure
        /// </summary>
        public static StoredProcedure TxInventoryCleanupJob(string appName, int? pastDueMinutes, bool? logToRemoveTable, int? dateOffsetMinutes)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_Inventory_CleanupJob", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@appName", appName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@pastDueMinutes", pastDueMinutes, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@logToRemoveTable", logToRemoveTable, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@dateOffsetMinutes", dateOffsetMinutes, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_Inventory_ClearCart_ReturnItemNotification Procedure
        /// </summary>
        public static StoredProcedure TxInventoryClearCartReturnItemNotification(string context, int? notifyThreshold, string guids, bool? incSales)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_Inventory_ClearCart_ReturnItemNotification", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@context", context, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@notifyThreshold", notifyThreshold, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@guids", guids, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@incSales", incSales, DbType.Boolean, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_Inventory_PendingTimeUpdate Procedure
        /// </summary>
        public static StoredProcedure TxInventoryPendingTimeUpdate(Guid? guid, string context, DateTime? newTime)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_Inventory_PendingTimeUpdate", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@guid", guid, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@context", context, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@newTime", newTime, DbType.DateTime, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_Inventory_RealTimeAvailability Procedure
        /// </summary>
        public static StoredProcedure TxInventoryRealTimeAvailability(int? idx, string context)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_Inventory_RealTimeAvailability", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@idx", idx, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@context", context, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_Inventory_SyncSold Procedure
        /// </summary>
        public static StoredProcedure TxInventorySyncSold(int? idx, bool? performUpdate, string context)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_Inventory_SyncSold", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@idx", idx, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@performUpdate", performUpdate, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@context", context, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_Inventory_Transfer_Ticket Procedure
        /// </summary>
        public static StoredProcedure TxInventoryTransferTicket(int? parentId, int? transferToId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_Inventory_Transfer_Ticket", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@parentId", parentId, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@transferToId", transferToId, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_JOB_CleanupCashew Procedure
        /// </summary>
        public static StoredProcedure TxJobCleanupCashew()
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_JOB_CleanupCashew", DataService.GetInstance("WillCall"), "");
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_JOB_GetBatch_EventData Procedure
        /// </summary>
        public static StoredProcedure TxJobGetBatchEventData(Guid? applicationId, Guid? threadGuid, int? batchSize, int? retrySeconds, int? archiveAfterDays)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_JOB_GetBatch_EventData", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationId", applicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@threadGuid", threadGuid, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@batchSize", batchSize, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@retrySeconds", retrySeconds, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@archiveAfterDays", archiveAfterDays, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_JOB_GetBatch_MailData Procedure
        /// </summary>
        public static StoredProcedure TxJobGetBatchMailData(Guid? applicationId, Guid? threadGuid, int? batchSize, int? retrySeconds, int? archiveAfterDays)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_JOB_GetBatch_MailData", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationId", applicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@threadGuid", threadGuid, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@batchSize", batchSize, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@retrySeconds", retrySeconds, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@archiveAfterDays", archiveAfterDays, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_JOB_MailReport Procedure
        /// </summary>
        public static StoredProcedure TxJobMailReport(string applicationName, bool? isDaily)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_JOB_MailReport", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationName", applicationName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@isDaily", isDaily, DbType.Boolean, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_Mailer_LetterStats Procedure
        /// </summary>
        public static StoredProcedure TxMailerLetterStats(Guid? appId, string StartDate, string EndDate, int? letterId, int? queued, int? sent, int? total)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_Mailer_LetterStats", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@appId", appId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@StartDate", StartDate, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@EndDate", EndDate, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@letterId", letterId, DbType.Int32, 0, 10);
        	
            sp.Command.AddOutputParameter("@queued", DbType.Int32, 0, 10);
            
            sp.Command.AddOutputParameter("@sent", DbType.Int32, 0, 10);
            
            sp.Command.AddOutputParameter("@total", DbType.Int32, 0, 10);
            
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_Merch_Update_AvoidRealTimeVars Procedure
        /// </summary>
        public static StoredProcedure TxMerchUpdateAvoidRealTimeVars(int? Id, string Name, string Style, string Color, string Size, bool? bActive, bool? bInternalOnly, bool? bSoldOut, bool? bTaxable, bool? bFeaturedItem, string ShortText, string vcDisplayTemplate, string Description, bool? bUnlockActive, string UnlockCode, DateTime? dtUnlockDate, DateTime? dtUnlockEndDate, DateTime? dtStartDate, DateTime? dtEndDate, decimal? mPrice, bool? bUseSalePrice, decimal? mSalePrice, string vcDeliveryType, bool? bLowRateQualified, decimal? mWeight, decimal? mFlatShip, string FlatMethod, DateTime? dtBackorder, bool? bShipSeparate, int? iMaxQtyPerOrder, int? iAllotment, int? iDamaged)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_Merch_Update_AvoidRealTimeVars", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@Id", Id, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@Name", Name, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@Style", Style, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@Color", Color, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@Size", Size, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@bActive", bActive, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@bInternalOnly", bInternalOnly, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@bSoldOut", bSoldOut, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@bTaxable", bTaxable, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@bFeaturedItem", bFeaturedItem, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@ShortText", ShortText, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@vcDisplayTemplate", vcDisplayTemplate, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@Description", Description, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@bUnlockActive", bUnlockActive, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@UnlockCode", UnlockCode, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@dtUnlockDate", dtUnlockDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@dtUnlockEndDate", dtUnlockEndDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@dtStartDate", dtStartDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@dtEndDate", dtEndDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@mPrice", mPrice, DbType.Currency, 4, 19);
        	
            sp.Command.AddParameter("@bUseSalePrice", bUseSalePrice, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@mSalePrice", mSalePrice, DbType.Currency, 4, 19);
        	
            sp.Command.AddParameter("@vcDeliveryType", vcDeliveryType, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@bLowRateQualified", bLowRateQualified, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@mWeight", mWeight, DbType.Currency, 4, 19);
        	
            sp.Command.AddParameter("@mFlatShip", mFlatShip, DbType.Currency, 4, 19);
        	
            sp.Command.AddParameter("@FlatMethod", FlatMethod, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@dtBackorder", dtBackorder, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@bShipSeparate", bShipSeparate, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@iMaxQtyPerOrder", iMaxQtyPerOrder, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@iAllotment", iAllotment, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@iDamaged", iDamaged, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_MerchChildrenSorted Procedure
        /// </summary>
        public static StoredProcedure TxMerchChildrenSorted(int? tParentListing, string sortDirection)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_MerchChildrenSorted", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@tParentListing", tParentListing, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@sortDirection", sortDirection, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_Picture_Update Procedure
        /// </summary>
        public static StoredProcedure TxPictureUpdate(int? Idx, string Context, int? Width, int? Height)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_Picture_Update", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@Idx", Idx, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@Context", Context, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@Width", Width, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@Height", Height, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_ProcessInventoryCode Procedure
        /// </summary>
        public static StoredProcedure TxProcessInventoryCode(int? invoiceItemId, string productContext, int? productId, DateTime? dateSold, string defaultCode, string deliveryType, string useInventoryCodeList, bool? reportToInvoiceItem)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_ProcessInventoryCode", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@invoiceItemId", invoiceItemId, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@productContext", productContext, DbType.AnsiStringFixedLength, null, null);
        	
            sp.Command.AddParameter("@productId", productId, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@dateSold", dateSold, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@defaultCode", defaultCode, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@deliveryType", deliveryType, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@useInventoryCodeList", useInventoryCodeList, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@reportToInvoiceItem", reportToInvoiceItem, DbType.Boolean, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_ProductAccess_LookupData Procedure
        /// </summary>
        public static StoredProcedure TxProductAccessLookupData(Guid? appId, int? hourStartOffset)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_ProductAccess_LookupData", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@appId", appId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@hourStartOffset", hourStartOffset, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_Report_DailySalesInfo Procedure
        /// </summary>
        public static StoredProcedure TxReportDailySalesInfo(Guid? applicationId, DateTime? dateOfSales, bool? reportVenue, bool? reportAct)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_Report_DailySalesInfo", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationId", applicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@dateOfSales", dateOfSales, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@reportVenue", reportVenue, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@reportAct", reportAct, DbType.Boolean, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_Report_MerchBundleDetail_InPeriod Procedure
        /// </summary>
        public static StoredProcedure TxReportMerchBundleDetailInPeriod(Guid? appId, string category, string activeStatus, string StartDate, string EndDate, int? StartRowIndex, int? PageSize, string merchBundleIdConstant)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_Report_MerchBundleDetail_InPeriod", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@appId", appId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@category", category, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@activeStatus", activeStatus, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@StartDate", StartDate, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@EndDate", EndDate, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@StartRowIndex", StartRowIndex, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PageSize", PageSize, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@merchBundleIdConstant", merchBundleIdConstant, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_Report_MerchBundleDetail_InPeriod_Count Procedure
        /// </summary>
        public static StoredProcedure TxReportMerchBundleDetailInPeriodCount(Guid? appId, string category, string activeStatus)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_Report_MerchBundleDetail_InPeriod_Count", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@appId", appId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@category", category, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@activeStatus", activeStatus, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_Report_MerchSalesDetail_InPeriod Procedure
        /// </summary>
        public static StoredProcedure TxReportMerchSalesDetailInPeriod(Guid? appId, string StartDate, string EndDate)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_Report_MerchSalesDetail_InPeriod", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@appId", appId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@StartDate", StartDate, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@EndDate", EndDate, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_Report_NumberOfTicketsInPeriodForShowsInPeriod Procedure
        /// </summary>
        public static StoredProcedure TxReportNumberOfTicketsInPeriodForShowsInPeriod(Guid? appId, string StartDate, string EndDate)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_Report_NumberOfTicketsInPeriodForShowsInPeriod", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@appId", appId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@StartDate", StartDate, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@EndDate", EndDate, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_Report_Sales Procedure
        /// </summary>
        public static StoredProcedure TxReportSales(Guid? applicationId, string StartDate, string EndDate)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_Report_Sales", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationId", applicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@StartDate", StartDate, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@EndDate", EndDate, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_Report_Sales_All_InRange_Data_Aggs Procedure
        /// </summary>
        public static StoredProcedure TxReportSalesAllInRangeDataAggs(Guid? applicationId, string StartDate, string EndDate, int? StartRowIndex, int? PageSize)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_Report_Sales_All_InRange_Data_Aggs", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationId", applicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@StartDate", StartDate, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@EndDate", EndDate, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@StartRowIndex", StartRowIndex, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PageSize", PageSize, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_Report_Sales_Gifts_InRange Procedure
        /// </summary>
        public static StoredProcedure TxReportSalesGiftsInRange(Guid? applicationId, string StartDate, string EndDate, int? StartRowIndex, int? PageSize)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_Report_Sales_Gifts_InRange", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationId", applicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@StartDate", StartDate, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@EndDate", EndDate, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@StartRowIndex", StartRowIndex, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PageSize", PageSize, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_Report_ServiceFeeBreakdownInPeriod Procedure
        /// </summary>
        public static StoredProcedure TxReportServiceFeeBreakdownInPeriod(Guid? appId, string StartDate, string EndDate)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_Report_ServiceFeeBreakdownInPeriod", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@appId", appId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@StartDate", StartDate, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@EndDate", EndDate, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_SendEmailTemplate Procedure
        /// </summary>
        public static StoredProcedure TxSendEmailTemplate(Guid? applicationId, string emailTemplate, string sendDate, string fromName, string fromAddress, string toAddress, string paramNames, string paramValues, string bccEmail, int? priority, string result)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_SendEmailTemplate", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationId", applicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@emailTemplate", emailTemplate, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@sendDate", sendDate, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@fromName", fromName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@fromAddress", fromAddress, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@toAddress", toAddress, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@paramNames", paramNames, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@paramValues", paramValues, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@bccEmail", bccEmail, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@priority", priority, DbType.Int32, 0, 10);
        	
            sp.Command.AddOutputParameter("@result", DbType.AnsiString, null, null);
            
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_Shipping_BatchListing Procedure
        /// </summary>
        public static StoredProcedure TxShippingBatchListing(int? batchId, string sortMethod, int? StartRowIndex, int? PageSize)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_Shipping_BatchListing", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@batchId", batchId, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@sortMethod", sortMethod, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@StartRowIndex", StartRowIndex, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PageSize", PageSize, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_Shipping_BatchListingCount Procedure
        /// </summary>
        public static StoredProcedure TxShippingBatchListingCount(int? batchId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_Shipping_BatchListingCount", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@batchId", batchId, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_Shipping_BatchUndo Procedure
        /// </summary>
        public static StoredProcedure TxShippingBatchUndo(int? delbatchId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_Shipping_BatchUndo", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@delbatchId", delbatchId, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_Shipping_FulfillmentItems Procedure
        /// </summary>
        public static StoredProcedure TxShippingFulfillmentItems(string ticketIdList, string sortMethod, string filterMethod, string willCallMethodText, int? StartRowIndex, int? PageSize)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_Shipping_FulfillmentItems", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ticketIdList", ticketIdList, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@sortMethod", sortMethod, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@filterMethod", filterMethod, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@willCallMethodText", willCallMethodText, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@StartRowIndex", StartRowIndex, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PageSize", PageSize, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_Shipping_FulfillmentItems_Count Procedure
        /// </summary>
        public static StoredProcedure TxShippingFulfillmentItemsCount(string ticketIdList, string filterMethod, string willCallMethodText)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_Shipping_FulfillmentItems_Count", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ticketIdList", ticketIdList, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@filterMethod", filterMethod, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@willCallMethodText", willCallMethodText, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_Shipping_UpdateBatchListing Procedure
        /// </summary>
        public static StoredProcedure TxShippingUpdateBatchListing(int? batchId, DateTime? newDate, decimal? actualShipping)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_Shipping_UpdateBatchListing", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@batchId", batchId, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@newDate", newDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@actualShipping", actualShipping, DbType.Currency, 4, 19);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_ShowTicket_Update_AvoidRealTimeVars Procedure
        /// </summary>
        public static StoredProcedure TxShowTicketUpdateAvoidRealTimeVars(int? Id, DateTime? dtDateOfShow, string CriteriaText, string SalesDescription, int? TAgeId, bool? bActive, bool? bSoldOut, string Status, bool? bDosTicket, string PriceText, decimal? mPrice, string DosText, decimal? mDosPrice, decimal? mServiceCharge, bool? bAllowShipping, bool? bAllowWillCall, bool? bHideShipMethod, DateTime? dtShipCutoff, bool? bOverrideSellout, bool? bUnlockActive, string UnlockCode, DateTime? dtUnlockDate, DateTime? dtUnlockEndDate, DateTime? dtPublicOnsale, DateTime? dtEndDate, int? iMaxQtyPerOrder, int? iAllotment, decimal? mFlatShip, string FlatMethod, DateTime? dtBackorder, bool? bShipSeparate)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_ShowTicket_Update_AvoidRealTimeVars", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@Id", Id, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@dtDateOfShow", dtDateOfShow, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@CriteriaText", CriteriaText, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@SalesDescription", SalesDescription, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@TAgeId", TAgeId, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@bActive", bActive, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@bSoldOut", bSoldOut, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@Status", Status, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@bDosTicket", bDosTicket, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@PriceText", PriceText, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@mPrice", mPrice, DbType.Currency, 4, 19);
        	
            sp.Command.AddParameter("@DosText", DosText, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@mDosPrice", mDosPrice, DbType.Currency, 4, 19);
        	
            sp.Command.AddParameter("@mServiceCharge", mServiceCharge, DbType.Currency, 4, 19);
        	
            sp.Command.AddParameter("@bAllowShipping", bAllowShipping, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@bAllowWillCall", bAllowWillCall, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@bHideShipMethod", bHideShipMethod, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@dtShipCutoff", dtShipCutoff, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@bOverrideSellout", bOverrideSellout, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@bUnlockActive", bUnlockActive, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@UnlockCode", UnlockCode, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@dtUnlockDate", dtUnlockDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@dtUnlockEndDate", dtUnlockEndDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@dtPublicOnsale", dtPublicOnsale, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@dtEndDate", dtEndDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@iMaxQtyPerOrder", iMaxQtyPerOrder, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@iAllotment", iAllotment, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@mFlatShip", mFlatShip, DbType.Currency, 4, 19);
        	
            sp.Command.AddParameter("@FlatMethod", FlatMethod, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@dtBackorder", dtBackorder, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@bShipSeparate", bShipSeparate, DbType.Boolean, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_ShowTicket_Update_DisplayOrder Procedure
        /// </summary>
        public static StoredProcedure TxShowTicketUpdateDisplayOrder(int? Id, int? iDisplayOrder)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_ShowTicket_Update_DisplayOrder", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@Id", Id, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@iDisplayOrder", iDisplayOrder, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_StoreCredit_ValidateGiftCertificateRedemption Procedure
        /// </summary>
        public static StoredProcedure TxStoreCreditValidateGiftCertificateRedemption(Guid? applicationId, Guid? code)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_StoreCredit_ValidateGiftCertificateRedemption", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationId", applicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@code", code, DbType.Guid, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_Subscription_GetSubsForUser Procedure
        /// </summary>
        public static StoredProcedure TxSubscriptionGetSubsForUser(Guid? appId, string userName)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_Subscription_GetSubsForUser", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@appId", appId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@userName", userName, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_Subscription_InsertMailerIntoQueue Procedure
        /// </summary>
        public static StoredProcedure TxSubscriptionInsertMailerIntoQueue(Guid? applicationId, int? subscriptionEmailId, string sendDate, string fromName, string fromAddress, int? priority)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_Subscription_InsertMailerIntoQueue", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationId", applicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@subscriptionEmailId", subscriptionEmailId, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@sendDate", sendDate, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@fromName", fromName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@fromAddress", fromAddress, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@priority", priority, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_Subscription_PauseMailerInQueue Procedure
        /// </summary>
        public static StoredProcedure TxSubscriptionPauseMailerInQueue(int? subscriptionEmailId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_Subscription_PauseMailerInQueue", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@subscriptionEmailId", subscriptionEmailId, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_Subscription_RemoveMailerFromQueue Procedure
        /// </summary>
        public static StoredProcedure TxSubscriptionRemoveMailerFromQueue(int? subscriptionEmailId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_Subscription_RemoveMailerFromQueue", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@subscriptionEmailId", subscriptionEmailId, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_Subscription_RemoveUserFromUnauthorizedSubs Procedure
        /// </summary>
        public static StoredProcedure TxSubscriptionRemoveUserFromUnauthorizedSubs(Guid? appId, string userName)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_Subscription_RemoveUserFromUnauthorizedSubs", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@appId", appId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@userName", userName, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_Subscription_RestartMailerInQueue Procedure
        /// </summary>
        public static StoredProcedure TxSubscriptionRestartMailerInQueue(int? subscriptionEmailId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_Subscription_RestartMailerInQueue", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@subscriptionEmailId", subscriptionEmailId, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_User_HasMembership Procedure
        /// </summary>
        public static StoredProcedure TxUserHasMembership(Guid? applicationId, string userName)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_User_HasMembership", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationId", applicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@userName", userName, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
    }
    
}
