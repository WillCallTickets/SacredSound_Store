using System;

/// <summary>
/// Summary description for AdminEvent
/// </summary>
namespace WillCallWeb.Admin
{
    public class AdminEvent
    {
        public AdminEvent()
        {
        }

        


        //private static readonly object OrdinalListChangedEventKey = new object();
        //public delegate void OrdinalListChangedEventHandler(object sender, OrdinalListChangedEventArgs e);

        //public event OrdinalListChangedEventHandler SelectedDateChanged
        //{
        //    add { Events.AddHandler(OrdinalListChangedEventKey, value); }
        //    remove { Events.RemoveHandler(OrdinalListChangedEventKey, value); }
        //}
        //public virtual void OnOrdinalListChanged(OrdinalListChangedEventArgs e)
        //{
        //    OrdinalListChangedEventHandler handler = (OrdinalListChangedEventHandler)Events[OrdinalListChangedEventKey];

        //    if (handler != null)
        //        handler(this, e);
        //}




        public class FormModeChangedEventArgs : EventArgs
        {
            protected System.Web.UI.WebControls.FormViewMode _newMode;

            //Alt Constructor
            public FormModeChangedEventArgs(System.Web.UI.WebControls.FormViewMode newMode)
            {
                _newMode = newMode;
            }

            public System.Web.UI.WebControls.FormViewMode NewMode { get { return _newMode; } }
        }

        public class EditorEntityChangedEventArgs : EventArgs
        {
            protected int _idx;
            protected string _name;

            //Alt Constructor
            public EditorEntityChangedEventArgs(int idx, string name)
            {
                _idx = idx;
                _name = name;
            }

            public int Idx { get { return _idx; } }
            public string Name { get { return _name; } }
        }

        public delegate void MailerContent_ContentChangedEvent(object sender, EventArgs e);
        public static event MailerContent_ContentChangedEvent MailerContent_ContentChanged;
        public static void OnMailerContent_ContentChanged(object sender)
        {
            if (MailerContent_ContentChanged != null)
            {
                MailerContent_ContentChanged(sender, new EventArgs());
            }
        }

        public class ItemImageChosenEventArgs : EventArgs
        {
            protected int _idx;

            //Default Constructor
            public ItemImageChosenEventArgs()
            {
                _idx = 0;
            }

            //Alt Constructor
            public ItemImageChosenEventArgs(int idx)
            {
                _idx = idx;
            }

            public int ChosenId { get { return _idx; } }
        }
        public delegate void ItemImageChosenEvent(object sender, AdminEvent.ItemImageChosenEventArgs e);
        public static event ItemImageChosenEvent ItemImageChosen;
        public static void OnItemImageChosen(object sender, int idx)
        {
            if (ItemImageChosen != null)
            {
                ItemImageChosen(sender, new AdminEvent.ItemImageChosenEventArgs(idx));
            }
        }

        public class ShowChosenEventArgs : EventArgs
        {
            protected int _idx;

            //Default Constructor
            public ShowChosenEventArgs()
            {
                _idx = 0;
            }

            //Alt Constructor
            public ShowChosenEventArgs(int idx)
            {
                _idx = idx;
            }

            public int ChosenId { get { return _idx; } }
        }
        public delegate void ShowChosenEvent(object sender, AdminEvent.ShowChosenEventArgs e);
        public static event ShowChosenEvent ShowChosen;
        public static void OnShowChosen(object sender, int idx)
        {
            if (ShowChosen != null)
            {
                //ensures that the context variable is updated as well
                BindNewShowSelectionToContext(idx);
                ShowChosen(sender, new AdminEvent.ShowChosenEventArgs(idx));
            }
        }

        private static void BindNewShowSelectionToContext(int idx)
        {
            AdminContext atx = new AdminContext();

            atx.SetCurrentShowRecord(idx);
        }

        public class ActChosenEventArgs : EventArgs
        {
            protected int _idx;
            protected string _name;

            //Default Constructor
            public ActChosenEventArgs() 
            {
                _idx = 0;
                _name = string.Empty;
            }

            //Alt Constructor
            public ActChosenEventArgs(int idx, string name)
            {
                _idx = idx;
                _name = name;
            }

            public int ChosenId { get { return _idx; } }
            public string ChosenName { get { return _name; } }
        }
        public delegate void ActChosenEvent(object sender, ActChosenEventArgs e);
        public static event ActChosenEvent ActChosen;
        public static void OnActChosen(object sender, int id, string name)
        {
            if (ActChosen != null) { ActChosen(sender, new ActChosenEventArgs(id, name)); }
        }

        public class UpdateParentGridEventArgs : EventArgs
        {
            protected bool _isInsert = false;

            //Default Constructor
            public UpdateParentGridEventArgs()
            {
                _isInsert = false;
            }

            //Alt Constructor
            public UpdateParentGridEventArgs(bool IsCompletedInsert)
            {
                _isInsert = IsCompletedInsert;
            }

            public bool IsCompletedInsert { get { return _isInsert; } }
        }
        public delegate void UpdateParentGridEvent(object sender, UpdateParentGridEventArgs e);
        public static event UpdateParentGridEvent UpdateParentGrid;
        public static void OnUpdateParentGrid(object sender, bool isCompletedInsert)
        {
            if (UpdateParentGrid != null) { UpdateParentGrid(sender, new UpdateParentGridEventArgs(isCompletedInsert)); }
        }

        public class GridSelectionChangedEventArgs : EventArgs
        {
            protected int _selectedId = 0;

            //Default Constructor
            public GridSelectionChangedEventArgs()
            {
            }

            //Alt Constructor
            public GridSelectionChangedEventArgs(int selectedId)
            {
                _selectedId = selectedId;
            }

            public int SelectedId { get { return _selectedId; } }
        }
        public delegate void GridSelectionChangedEvent(object sender, GridSelectionChangedEventArgs e);
        public static event GridSelectionChangedEvent GridSelectionChanged;
        public static void OnGridSelectionChanged(object sender, int SelectedId)
        {
            if (GridSelectionChanged != null) { GridSelectionChanged(sender, new GridSelectionChangedEventArgs(SelectedId)); }
        }

        public delegate void UpdateSalePromotionEvent(object sender, EventArgs e);
        public static event UpdateSalePromotionEvent UpdateSalePromotion;
        public static void OnUpdateSalePromotion(object sender)
        {
            if (UpdateSalePromotion != null) { UpdateSalePromotion(sender, new EventArgs()); }
        }
        public delegate void ShowNameChangeEvent(object sender, EventArgs e);
        public static event ShowNameChangeEvent ShowNameChanged;
        public static void OnShowNameChanged(object sender)
        {
            if (ShowNameChanged != null) { ShowNameChanged(sender, new EventArgs()); }
        }
    }
}
