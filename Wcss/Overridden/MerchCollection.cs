using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Wcss
{
    public partial class MerchCollection
    {
        #region Retrieve Attribs

        //TODO: change to stored procs

        public ListItemCollection GetStyleList(int merchId)
        {
            ListItemCollection _list = new ListItemCollection();
     
            Merch entity = (Merch)this.Find(merchId);
     
            //if found - return appropriate styles
            if (entity != null && entity.Id > 0 && entity.HasChildStyles)
            {
                MerchCollection contextMerch = new MerchCollection();
                contextMerch.AddRange(entity.ChildMerchRecords().GetList().FindAll(
                    delegate(Merch match) { return (match.Available > 0 && match.Style != null && match.Style.Trim().Length > 0); } ));

                List<string> coll = new List<string>();

                foreach (Merch m in contextMerch)
                    if (!coll.Contains(m.Style))
                        coll.Add(m.Style);

                if (coll.Count > 1)
                    coll.Sort();

                foreach (string s in coll)
                    _list.Add(new ListItem(s));
            }      

            return _list;
        }


        /// <summary>
        /// if no style is specified, then it is treated as a top level list
        /// </summary>
        public ListItemCollection GetColorList(int merchId, string style)
        {
            ListItemCollection _list = new ListItemCollection();

            Merch entity = (Merch)this.Find(merchId);

            if (entity != null && entity.HasChildColors)
            {
                //establish base collection
                MerchCollection contextMerch = new MerchCollection();
                contextMerch.AddRange(entity.ChildMerchRecords().GetList().FindAll(
                        delegate(Merch match) { return (match.Color != null && match.Color.Trim().Length > 0 && match.Available > 0); }));

                //remove non-matching styles
                string stile = (style == null) ? string.Empty : style.Trim().ToLower();

                if (stile.Length > 0)
                    contextMerch.GetList().RemoveAll(delegate(Merch match)
                    { 
                        return (match.Style == null || (match.Style.Trim().ToLower() != style)); });

                //use a list to get sorting and contain method to work correctly
                List<string> coll = new List<string>();

                foreach (Merch m in contextMerch)
                    if (!coll.Contains(m.Color))
                        coll.Add(m.Color);

                if (coll.Count > 1)
                    coll.Sort();

                foreach (string s in coll)
                    _list.Add(new ListItem(s));
            }

            return _list;
        }

        /// <summary>
        /// if no color is specified, then it is treated as a top level list
        /// </summary>
        public ListItemCollection GetSizeList(int merchId, string style, string color)
        {
            ListItemCollection _list = new ListItemCollection();

            Merch entity = (Merch)this.Find(merchId);

            if (entity != null && entity.HasChildSizes)
            {
                //establish base collection
                MerchCollection contextMerch = new MerchCollection();
                contextMerch.AddRange(entity.ChildMerchRecords().GetList().FindAll(
                    delegate(Merch match) { return (match.Available > 0 &&
                        match.Size != null && match.Size.Trim().Length > 0); }));

                //remove non-matching styles
                string stile = (style == null) ? string.Empty : style.Trim().ToLower();

                if (stile.Length > 0)
                    contextMerch.GetList().RemoveAll(delegate(Merch match)
                    { return (match.Style == null || (match.Style.Trim().ToLower() != style)); });

                //remove non-matching colors
                string clr = (color == null) ? string.Empty : color.Trim().ToLower();

                if (clr.Length > 0)
                    contextMerch.GetList().RemoveAll(delegate(Merch match)
                    { return (match.Style == null || (match.Style.Trim().ToLower() != style)); });

                //use a list to get sorting and contain method to work correctly
                List<string> coll = new List<string>();

                foreach (Merch m in contextMerch)
                    if (!coll.Contains(m.Size))
                        coll.Add(m.Size);

                if (coll.Count > 1)
                    coll.Sort();

                foreach (string s in coll)
                    _list.Add(new ListItem(s));
            }

            return _list;
        }

        #endregion
    }
}
