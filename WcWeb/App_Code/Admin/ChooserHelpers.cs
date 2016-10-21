using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Wcss;

namespace WillCallWeb.Admin
{
    public class ChooserHelpers
    {
        public static void SetPanelAttribs(UpdatePanel panelToHoldControls, Merch merchandise, int listWidth)
        {
            if (merchandise != null && merchandise.IsParent)
            {
                //STYLES
                DropDownList styleList = null;
                AjaxControlToolkit.CascadingDropDown cddStyle = null;
                DropDownList colorList = null;
                AjaxControlToolkit.CascadingDropDown cddColor = null;
                DropDownList sizeList = null;
                AjaxControlToolkit.CascadingDropDown cddSize = null;

                if (merchandise.HasChildStyles)
                    CreateDropDown(merchandise, ref styleList, ref cddStyle, "Style", listWidth);

                if (merchandise.HasChildColors)
                {
                    CreateDropDown(merchandise, ref colorList, ref cddColor, "Color", listWidth);

                    if (styleList != null)
                        cddColor.ParentControlID = styleList.ID;
                }

                if (merchandise.HasChildSizes)
                {
                    CreateDropDown(merchandise, ref sizeList, ref cddSize, "Size", listWidth);

                    if (colorList != null)
                        cddSize.ParentControlID = colorList.ID;
                    else if (styleList != null)
                        cddSize.ParentControlID = styleList.ID;
                    else
                        cddSize.ParentControlID = string.Empty;
                }

                //add to panel in correct order
                if (sizeList != null && cddSize != null)
                {
                    panelToHoldControls.ContentTemplateContainer.Controls.AddAt(0, sizeList);
                    panelToHoldControls.ContentTemplateContainer.Controls.Add(cddSize);
                }
                if (colorList != null && cddColor != null)
                {
                    panelToHoldControls.ContentTemplateContainer.Controls.AddAt(0, colorList);
                    panelToHoldControls.ContentTemplateContainer.Controls.Add(cddColor);
                }
                if (styleList != null && cddStyle != null)
                {
                    panelToHoldControls.ContentTemplateContainer.Controls.AddAt(0, styleList);
                    panelToHoldControls.ContentTemplateContainer.Controls.Add(cddStyle);
                }
            }
        }

        public static void CreateDropDown(Merch merchandise, ref DropDownList listToCreate,
            ref AjaxControlToolkit.CascadingDropDown cascadingCounterpart, string categoryName, int listWidth)
        {
            listToCreate = new DropDownList();
            cascadingCounterpart = new AjaxControlToolkit.CascadingDropDown();

            listToCreate.ID = string.Format("ddl{0}", categoryName);
            listToCreate.Width = Unit.Pixel(listWidth);
            listToCreate.CssClass = "itemattrib";
            listToCreate.EnableViewState = false;

            cascadingCounterpart.ID = string.Format("Cascading{0}", categoryName);
            cascadingCounterpart.Category = categoryName;
            cascadingCounterpart.UseContextKey = true;
            cascadingCounterpart.ContextKey = merchandise.Id.ToString();
            cascadingCounterpart.LoadingText = string.Format("[ ... Loading {0} ... ]", categoryName);

            if (categoryName.ToLower() != "style" || (!merchandise.IsGiftCertificateDelivery))
                cascadingCounterpart.PromptText = string.Format("[ Please Select A {0} ]", categoryName);
            else
                cascadingCounterpart.PromptText = "[ Please Select An Amount ]";

            cascadingCounterpart.TargetControlID = listToCreate.ID;
            cascadingCounterpart.ServicePath = "/Services/MerchAttribute_Svc.asmx";
            cascadingCounterpart.ServiceMethod = string.Format("Get{0}s", categoryName);
            cascadingCounterpart.BehaviorID = string.Format("the{0}", categoryName);
        }
    }
}