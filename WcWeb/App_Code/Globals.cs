using System;

using Wcss;

namespace WillCallWeb
{
    public class Globals
    {
        public static string ThemesSelectorId = "";

        private static int _showId = 0;
        public static int ShowId { get { return _showId; } set { _showId = value; } }
        //private static int _showDateId = 0;
        //public static int ShowDateId { get { return _showDateId; } set { _showDateId = value; } }
        private static int _ticketId = 0;
        public static int TicketId { get { return _ticketId; } set { _ticketId = value; } }

        private static DateTime _monthSelected;
        public static DateTime MonthSelected { get { return _monthSelected; } set { _monthSelected = value; } }
        
        private static MerchCategorie _merCat = null;
        public static MerchCategorie MerCat 
        { 
            get 
            {
                if (_merCat != null && _merCat.MerchDivisionRecord.ApplicationId == _Config.APPLICATION_ID)
                    return _merCat;

                return null; 
            } 
            set 
            { _merCat = value; } 
        }

        private static int _merchId = 0;
        public static int MerchId { get { return _merchId; } set { _merchId = value; } }
        private static Merch _merch = null;
        public static Merch MerchItem 
        { 
            get 
            { 
                if (_merch != null && _merch.ApplicationId == _Config.APPLICATION_ID) 
                    return _merch; 
                
                _merchId = 0; 
                return null; 
            } 
            set { _merch = value; } 
        }

        public static string _selectMerchParamPreamble = "SelMrc";
        public static string _selectTicketQtyPreamble = "TktQty";
        public static string _selectMerchQtyPreamble = "MrcQty";
    }
}
