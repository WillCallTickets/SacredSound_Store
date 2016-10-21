using System;

using Wcss;

namespace WillCallWeb.Admin.AdminControls
{
    public partial class Merch_Copier : BaseControl
    {
        #region Page Overhead

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        #endregion

        protected void btnCreateCopy_Click(object sender, EventArgs e)
        {
            if (this.Page.IsValid && Atx.CurrentMerchRecord != null)
            {
                //determine if name is unique
                Merch newMerch = new Merch();
                newMerch.ApplicationId = Atx.CurrentMerchRecord.ApplicationId;
                newMerch.Name = txtName.Text.Trim();
                newMerch.Style = Atx.CurrentMerchRecord.Style;
                newMerch.Color = Atx.CurrentMerchRecord.Color;
                newMerch.Size = Atx.CurrentMerchRecord.Size;
                newMerch.TParentListing = Atx.CurrentMerchRecord.TParentListing;
                newMerch.BSoldOut = false;
                newMerch.BActive = false;
                newMerch.BUseSalePrice = false;
                newMerch.BInternalOnly = false;
                newMerch.BTaxable = false;
                newMerch.BFeaturedItem = false;
                newMerch.ShortText = Atx.CurrentMerchRecord.ShortText;
                newMerch.DisplayTemplate = (Atx.CurrentMerchRecord.IsDisplayRichText) ? _Enums.MerchDisplayTemplate.ControlsAboveRichText : _Enums.MerchDisplayTemplate.Legacy;
                newMerch.Description = Atx.CurrentMerchRecord.Description;
                newMerch.BUnlockActive = false;
                newMerch.Price = Atx.CurrentMerchRecord.Price;
                newMerch.DeliveryType = Atx.CurrentMerchRecord.DeliveryType;
                newMerch.Weight = Atx.CurrentMerchRecord.Weight;
                newMerch.FlatShip = Atx.CurrentMerchRecord.FlatShip;
                newMerch.VcFlatMethod = Atx.CurrentMerchRecord.VcFlatMethod;
                newMerch.BackorderDate = Atx.CurrentMerchRecord.BackorderDate;
                newMerch.IsShipSeparate = Atx.CurrentMerchRecord.IsShipSeparate;
                newMerch.MaxQuantityPerOrder = Atx.CurrentMerchRecord.MaxQuantityPerOrder;
                newMerch.DtStamp = DateTime.Now;
                newMerch.DtUnlockDate = Atx.CurrentMerchRecord.DtUnlockDate;
                newMerch.DtUnlockEndDate = Atx.CurrentMerchRecord.DtUnlockEndDate;
                newMerch.DtStartDate = Atx.CurrentMerchRecord.DtStartDate;
                newMerch.DtEndDate = Atx.CurrentMerchRecord.DtEndDate;

                newMerch.MSalePrice = Atx.CurrentMerchRecord.MSalePrice;
                newMerch.BLowRateQualified = Atx.CurrentMerchRecord.BLowRateQualified;

                //dont copy required records as they are so unique to the situation....

                try
                {
                    newMerch.Save();
                    int newMerchIdx = newMerch.Id;

                    if (chkImages.Checked && Atx.CurrentMerchRecord.ItemImageRecords().Count > 0)
                    {
                        foreach(ItemImage img in Atx.CurrentMerchRecord.ItemImageRecords())
                        {
                            try
                            {
                                ItemImage newImage = new ItemImage();
                                newImage.TMerchId = newMerchIdx;
                                newImage.TFutureId = null;
                                newImage.BItemImage = img.BItemImage;
                                newImage.BDetailImage = img.BDetailImage;
                                newImage.BOverrideThumbnail = img.BOverrideThumbnail;
                                newImage.DetailDescription = img.DetailDescription;

                                newImage.StorageRemote = img.StorageRemote;
                                newImage.Path = img.Path;
                                newImage.ImageHeight = img.ImageHeight;
                                newImage.ImageWidth = img.ImageWidth;
                                newImage.ThumbClass = img.ThumbClass;
                                newImage.IDisplayOrder = img.IDisplayOrder;
                                newImage.DtStamp = DateTime.Now;


                                //create a new name
                                //save/copy a new image with the new name
                                string newImageName = string.Format("{0}_{1}{2}", 
                                    System.IO.Path.GetFileNameWithoutExtension(img.ImageName), 
                                    newMerchIdx.ToString(),
                                    System.IO.Path.GetExtension(img.ImageName));

                                newImage.ImageName = newImageName;
                                newImage.Save();

                                //copy the new image
                                string dir = System.IO.Path.GetDirectoryName(img.ImageManager.OriginalUrl);
                                string newDest = Server.MapPath(string.Format("{0}\\{1}", dir, newImageName));
                                System.IO.File.Copy(Server.MapPath(img.ImageManager.OriginalUrl), newDest);
                            }
                            catch (Exception ex)
                            {
                                _Error.LogException(ex);
                            }
                        }
                    }

                    if (chkCopyChildren.Checked)
                    {
                        foreach(Merch child in Atx.CurrentMerchRecord.ChildMerchRecords())
                        {
                            Merch newChild = new Merch();
                            newChild.ApplicationId = child.ApplicationId;
                            newChild.Color = child.Color;
                            newChild.Description = child.Description;
                            newChild.DtStamp = DateTime.Now;
                            newChild.FlatShip = child.FlatShip;
                            newChild.FlatMethod = child.FlatMethod;
                            newChild.BackorderDate = child.BackorderDate;
                            newChild.IsShipSeparate = child.IsShipSeparate;
                            newChild.MaxQuantityPerOrder = child.MaxQuantityPerOrder;
                            newChild.Name = child.Name;
                            newChild.Price = child.Price;
                            //newChild.ShortText = child.ShortText;
                            newChild.Size = child.Size;
                            newChild.Style = child.Style;
                            newChild.TParentListing = newMerchIdx;
                            newChild.DeliveryType = child.DeliveryType;
                            newChild.Weight = child.Weight;

                            newChild.Save();
                        }
                    }

                    if (chkBundled.Checked)
                    {
                        foreach (MerchBundle bundle in Atx.CurrentMerchRecord.MerchBundleRecords())
                        {
                            //create a new bundle
                            MerchBundle newBundle = new MerchBundle();
                            newBundle.DtStamp = DateTime.Now;
                            newBundle.BActive = bundle.IsActive;
                            newBundle.DisplayOrder = bundle.DisplayOrder;
                            newBundle.TMerchId = newMerchIdx;
                            newBundle.TShowTicketId = bundle.TShowTicketId;
                            newBundle.Title = bundle.Title;
                            newBundle.Comment = bundle.Comment;
                            newBundle.RequiredParentQty = bundle.RequiredParentQty;
                            newBundle.MaxSelections = bundle.MaxSelections;
                            newBundle.Price = bundle.Price;
                            newBundle.IncludeWeight = bundle.IncludeWeight;
                            newBundle.Save();

                            //get the id
                            int newBundleId = newBundle.Id;

                            //copy the items
                            foreach (MerchBundleItem mbi in bundle.MerchBundleItemRecords())
                            {
                                MerchBundleItem newMbi = new MerchBundleItem();
                                newMbi.DtStamp = DateTime.Now;
                                newMbi.BActive = mbi.IsActive;
                                newMbi.TMerchBundleId = newBundleId;
                                newMbi.TMerchId = mbi.TMerchId;
                                newMbi.DisplayOrder = mbi.DisplayOrder;
                                newMbi.Save();
                            }
                        }
                    }

                    foreach (MerchJoinCat joinCat in Atx.CurrentMerchRecord.MerchJoinCatRecords())
                    {
                        //add to category for ordering
                        joinCat.MerchCategorieRecord.MerchJoinCatRecords().AddMerchToCollection(joinCat.TMerchCategorieId, newMerchIdx);
                    }

                    //refresh collections
                    Atx.Clear_CurrentMerchListing();
                    _Lookits.RefreshLookup(_Enums.LookupTableNames.MerchDivisions.ToString());
                    _Lookits.RefreshLookup(_Enums.LookupTableNames.MerchCategories.ToString());

                    base.Redirect(string.Format("/Admin/MerchEditor.aspx?p=ItemEdit&merchitem={0}", newMerch.Id));
                }
                catch (System.Threading.ThreadAbortException) { }
                catch (Exception ex)
                {
                    _Error.LogException(ex);

                    RequiredName.IsValid = false;
                    RequiredName.ErrorMessage = ex.Message;
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            
        }
}
}