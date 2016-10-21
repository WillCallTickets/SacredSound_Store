using System;
using System.Web.UI;

using Wcss;
using WillCallWeb;
using WillCallWeb.StoreObjects;

public partial class Store_ChooseTicket : WillCallWeb.BasePage, IPostBackEventHandler
{
    #region Handle Commands
    void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
    {
        string[] args = eventArgument.Split('~');
        string command = args[0];
        int idx = (args.Length > 1 && Utils.Validation.IsInteger((string)args[1])) ? int.Parse(args[1]) : 0;

        switch (command.ToLower())
        {
            case "add":
                if (idx > 0)
                {
                    //get qty
                    string qtySelectionName = string.Format("{0}{1}", Globals._selectTicketQtyPreamble, idx);
                    string req = Request[qtySelectionName];
                    if (req != null && Utils.Validation.IsInteger(req))
                    {
                        int qty = int.Parse(req);

                        if (Ctx.Cart != null && qty > 0)
                        {
                            string result = Ctx.Cart.SaleItem_AddUpdate(_Enums.InvoiceItemContext.ticket, idx, qty, this.Profile);

                            if (result.Length > 0)
                            {
                                _Error.LogException(new Exception(string.Format("Error adding ticket - {0}", result)));

                                return;
                            }
                        }
                    }
                }
                
                break;
            case "rmv":
                if (idx > 0 && Ctx.Cart != null)
                {
                    string result = Ctx.Cart.SaleItem_Remove(_Enums.InvoiceItemContext.ticket, idx);
                    if (result.Length > 0)
                    {
                        _Error.LogException(new Exception(string.Format("Error removing ticket - {0}", result)));
                        
                        return;
                    }
                }
                break;
        }
    }
    #endregion

    protected override void OnPreInit(EventArgs e)
    {
        QualifySsl(false);
        base.OnPreInit(e);        
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Form.Attributes.Add("class", "cstx");

        //avoid double clicks! always reset
        Ctx.OrderProcessingVariables = null;

        if (WillCallWeb.Globals.ShowId == 0)
        {
            string controlToLoad = string.Empty;

            //if (this.MasterPageFile.IndexOf("Alt1") != -1)
            //{   
            //    ShowDateCollection dates = Ctx.OrderedDisplayable_ShowDates; 
            //    ShowDate display = dates.GetDisplayableShowDate();

            //    int idx = (display != null) ? display.ShowRecord.Id : 0;
            //    Server.Transfer(string.Format("/Store/ChooseTicket.aspx?sid={0}", idx));

            //}
            //else
            controlToLoad = @"\Listing_Show";

            panelShow.Controls.Add(LoadControl(string.Format(@"..\Controls{0}.ascx", controlToLoad)));
        }
        else
        {   
            string controlToLoad = @"\Listing_Ticket";

            panelTicket.Controls.Add(LoadControl(string.Format(@"..\Controls{0}.ascx", controlToLoad)));
        }

        this.Page.Title = _Config._PageTitle_Header;
    }
}
