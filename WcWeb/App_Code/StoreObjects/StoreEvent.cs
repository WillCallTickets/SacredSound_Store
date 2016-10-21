using System;

/// <summary>
/// Summary description for AdminEvent
/// </summary>
namespace WillCallWeb.StoreObjects
{
    public class StoreEvent
    {
        public StoreEvent() {}

        public delegate void NoMerchShipMethodChosenEvent(object sender, EventArgs e);
        public static event NoMerchShipMethodChosenEvent NoMerchShipMethodChosen;
        public static void OnNoMerchShipMethodChosen(object sender)
        {
            if (NoMerchShipMethodChosen != null)
            {
                NoMerchShipMethodChosen(sender, EventArgs.Empty);
            }
        }

        public delegate void RebindControlEvent();
        public static event RebindControlEvent RebindControl;
        public static void OnRebindControl()
        {
            if (RebindControl != null)
            {
                RebindControl();
            }
        }

      
    }
}
