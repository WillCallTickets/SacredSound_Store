using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using Wcss;

/// <summary>
/// Summary description for AdminServices
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class AdminServices : System.Web.Services.WebService {

    //private WillCallWeb.AdminContext _atx;
    //protected WillCallWeb.AdminContext Atx { get { return _atx; } }

    public AdminServices () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
        //_atx = new WillCallWeb.AdminContext();
    }
        
    [WebMethod]
    public void PublishButton()
    {
        WillCallWeb.AdminContext atx = new WillCallWeb.AdminContext();
        atx.PublishSite();
    }

    #region Merch Ordinal Collections

    [WebMethod]
    public bool AddNewMerchDivision(string name, string description)
    {
        if(name == null || name.Trim().Length == 0)
            throw new Exception("Name is required.");

        MerchDivisionCollection coll = new MerchDivisionCollection().Load();
       
        if (coll.GetList().FindIndex(delegate(MerchDivision match) { return (match.Name.Equals(name, StringComparison.OrdinalIgnoreCase)); }) != -1)
            throw new Exception(string.Format("[{0}] is an existing Merch Division", name));

        try
        {
            coll.AddToCollection(name, description);
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        return true;
    }

    [WebMethod]
    public void AddNewMerchCategorie(int merchDivisionId, string name, string description)
    {
        //throw new Exception("new categorie: Avoiding db calls until we get the params correct.");

        if (name == null || name.Trim().Length == 0)
            throw new Exception("Name is required.");

        MerchCategorieCollection coll = new MerchCategorieCollection().Where(MerchCategorie.Columns.TMerchDivisionId, merchDivisionId).Load();

        if (coll.GetList().FindIndex(delegate(MerchCategorie match) { return (match.Name.Equals(name, StringComparison.OrdinalIgnoreCase)); }) != -1)
        {
            MerchDivision division = MerchDivision.FetchByID(merchDivisionId);
            throw new Exception(string.Format("[{0}] is an existing Merch Categorie within the [{1}] Merch Division", name, division.Name));
        }

        try
        {
            coll.AddToCollection(name, merchDivisionId, description);
        }
        catch (Exception ex)
        {
            throw (ex);
        }
    }

    [WebMethod]
    public void EditMerchDivision(int idx, string name, string description, bool isInternal)
    {
        //throw new Exception("edit mech div: Avoiding db calls until we get the params correct.");

        if (name == null || name.Trim().Length == 0)
            throw new Exception("Name is required.");

        MerchDivision ent = MerchDivision.FetchByID(idx);

        if (ent == null)
            throw new Exception( string.Format("Division ID {0} not found", idx.ToString()));

        ent.Name = name;
        ent.Description = description;
        ent.IsInternalOnly = isInternal;

        if (ent.IsDirty)
        {
            try
            {
                ent.Save();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }

    [WebMethod]
    public void EditMerchCategorie(int idx, string name, string description, bool isInternal)
    {
        //throw new Exception("edit mech categorie: Avoiding db calls until we get the params correct.");

        if (name == null || name.Trim().Length == 0)
            throw new Exception("Name is required.");

        MerchCategorie ent = MerchCategorie.FetchByID(idx);

        if (ent == null)
            throw new Exception( string.Format("Categorie ID {0} not found", idx.ToString()));

        ent.Name = name;
        ent.Description = description;
        ent.IsInternalOnly = isInternal;

        if (ent.IsDirty)
        {
            try
            {
                ent.Save();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }

    [WebMethod]
    public bool UpdateOrdinal_Merch(string ordinalContext, int itemIdx, int newOrdinal)
    {
        if (itemIdx > 0)
        {
            _Enums.OrdinalContext octx = (_Enums.OrdinalContext)Enum.Parse(typeof(_Enums.OrdinalContext), ordinalContext, true);

            switch (octx)
            {
                case _Enums.OrdinalContext.merchdivision:
                    MerchDivisionCollection coll = new MerchDivisionCollection().Load();
                    coll.InsertAtOrdinalInExistingCollection(itemIdx, newOrdinal);
                    break;

                case _Enums.OrdinalContext.merchcategorie:
                    MerchCategorie ent = MerchCategorie.FetchByID(itemIdx);
                    if (ent != null)
                    {
                        MerchCategorieCollection moll = new MerchCategorieCollection()
                            .Where(MerchCategorie.Columns.TMerchDivisionId, ent.TMerchDivisionId).Load();
                        moll.InsertAtOrdinalInExistingCollection(itemIdx, newOrdinal);
                    }
                    break;

                case _Enums.OrdinalContext.merchjoincat:
                    MerchJoinCat mjc = MerchJoinCat.FetchByID(itemIdx);

                    if (mjc != null)
                    {
                        MerchJoinCatCollection joll = new MerchJoinCatCollection()
                            .Where(MerchJoinCat.Columns.TMerchCategorieId, mjc.TMerchCategorieId).Load();
                        joll.InsertAtOrdinalInExistingCollection(itemIdx, newOrdinal);
                    }
                    break;
            }

            return true;
        }

        return false;
    }

    #endregion

    public static bool RemoveMerchChoiceFromSalePromotion(int removeId)
    {
        WillCallWeb.AdminContext Atx = new WillCallWeb.AdminContext();
        if (Atx.CurrentSalePromotion != null)
        {
            Atx.CurrentSalePromotion.RequiredMerchListing.Remove(removeId);
            
            //execute script on db
            string sql = string.Format("SET NOCOUNT ON; UPDATE [SalePromotion] SET vcTriggerList_Merch = @newListing WHERE [Id] = @promoId; SELECT 0; RETURN");
            _DatabaseCommandHelper helper = new _DatabaseCommandHelper(sql);
            helper.AddCmdParameter("newListing", Atx.CurrentSalePromotion.ReCalculate_RequiredMerchListing_String(), System.Data.DbType.String);
            helper.AddCmdParameter("promoId", Atx.CurrentSalePromotion.Id, System.Data.DbType.Int32);
            object o = helper.PerformQuery("Promo_RemoveMerchTrigger");
            
            //let client handle this - keep here as a reminder
            //atx.CurrentSalePromotion = null;

            return true;
        }

        return false;
    }
}
